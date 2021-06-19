using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageMagick;

namespace GLToolsGUI
{
    public partial class GLTools : Form
    {
        private readonly string imageTypeFileDialogDescription;
        private readonly string ktexTypeFileDialogDescription;

        MagickImage currentlyLoadedImage;
        MagickImage currentlyLoadedKTEX;

        // Klei's Texture files are special .dds files with dxt5 compression
        // They usually start with "KTEX" followed by 5 more "magic" bytes
        // that I don't know the meaning of but are probably necessary
        const int magicBytesCount = 5;
        byte[] kleiMagicBytes = null;
        readonly byte[] kleiFileMagic = { (byte)'K', (byte)'T', (byte)'E', (byte)'X' };
        const string compression = "dxt5";

        public GLTools()
        {
            InitializeComponent();            
            imageTypeFileDialogDescription = $"PNG Image (*.png)|*.png";
            ktexTypeFileDialogDescription = "Klei Texture(*.tex)|*.tex";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            kleiMagicBytesPreview.Text = "No original texture loaded";
        }

        private string BytesToASCIIString(byte[] bytes)
        {
            return string.Join("", bytes.Select(b => (char)b));
        }

        private string BytesToHexString(byte[] bytes)
        {
            return string.Join(" ", bytes.Select(b => $"x{b,2:X2}"));
        }

        private void SetLoadedKTEX(MagickImage image)
        {
            currentlyLoadedKTEX = image;
            var previewImage = currentlyLoadedKTEX.Clone();
            previewImage.Resize(loadedKTEXPreview.Width, loadedKTEXPreview.Height);
            loadedKTEXPreview.Image = Image.FromStream(new MemoryStream(previewImage.ToByteArray(MagickFormat.Png)));
        } 
        
        private void SetLoadedImage(MagickImage image)
        {
            currentlyLoadedImage = image;
            var previewImage = currentlyLoadedImage.Clone();
            previewImage.Resize(loadedImagePreview.Width, loadedKTEXPreview.Height);
            loadedImagePreview.Image = Image.FromStream(new MemoryStream(previewImage.ToByteArray(MagickFormat.Png)));
        }

        private void OpenKTEX_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.InitialDirectory = Directory.GetCurrentDirectory();
                dialog.Filter = ktexTypeFileDialogDescription;                
                dialog.RestoreDirectory = true;

                if(dialog.ShowDialog() != DialogResult.OK)
                {
                    errorText.Text = "Error selecting file";
                    return;
                }
                var fs = dialog.OpenFile();               

                int expectedFileMagicLength = 4;
                var fileMagicBytes = new byte[expectedFileMagicLength];                
                if(fs.Read(fileMagicBytes, 0, expectedFileMagicLength) < expectedFileMagicLength)
                {
                    errorText.Text = "Failed to read magic!";
                    return;
                }
                                
                var fileMagic = BytesToASCIIString(fileMagicBytes);

                if(fileMagic == "KTEX")
                {                    
                    kleiMagicBytes = new byte[magicBytesCount];
                    if (fs.Read(kleiMagicBytes, 0, magicBytesCount) < magicBytesCount)
                    {
                        errorText.Text = "Failed to read KLEI magic bytes!";
                        return;
                    }
                    kleiMagicBytesPreview.Text = BytesToHexString(kleiMagicBytes);                    
                } 
                else if(fileMagic.StartsWith("DDS"))
                {                    
                    fs.Position = 0; // no special klei magic, just a simple .dds file
                } 
                else
                {
                    errorText.Text = "Invalid file format";
                    return;
                }

                long bytesLeft = fs.Length - fs.Position;
                var ddsImageBytes = new byte[bytesLeft];                
                fs.Read(ddsImageBytes, 0, (int)bytesLeft);
                fs.Close();

                SetLoadedKTEX(new MagickImage(ddsImageBytes));
            }
        }

        private void SavePNG_Click(object sender, EventArgs e)
        {
            if (currentlyLoadedKTEX == null)
            {
                errorText.Text = "No image loaded";
                return;
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.InitialDirectory = Directory.GetCurrentDirectory();
                dialog.Filter = imageTypeFileDialogDescription;
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    errorText.Text = "Error selecting file";
                    return;
                }

                currentlyLoadedKTEX.Write(dialog.FileName);
            }   
        }

        private void openImage_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {               
                dialog.InitialDirectory = Directory.GetCurrentDirectory();
                dialog.Filter = imageTypeFileDialogDescription;
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    errorText.Text = "Error selecting file";
                    return;
                }

                SetLoadedImage(new MagickImage(dialog.FileName));
            }
        }

        private void saveKTEX_click(object sender, EventArgs e)
        {
            if (currentlyLoadedImage == null)
            {
                errorText.Text = "No image loaded";
                return;
            }

            if (kleiMagicBytes == null)
            {
                errorText.Text = "Please load original texture first!";
                return;
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.InitialDirectory = Directory.GetCurrentDirectory();
                dialog.Filter = ktexTypeFileDialogDescription;
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    errorText.Text = "Error selecting file";
                    return;
                }

                // convert to dds
                currentlyLoadedImage.Format = MagickFormat.Dds;
                currentlyLoadedImage.Settings.SetDefine(MagickFormat.Dds, "compression", compression);
                var rawDDS = currentlyLoadedImage.ToByteArray();

                var outputFile = File.OpenWrite(dialog.FileName);

                // put everything back in its place
                // "KTEX" + 5 magic bytes from original klei texture + rest of DDS file
                outputFile.Write(kleiFileMagic, 0, kleiFileMagic.Length);
                outputFile.Write(kleiMagicBytes, 0, magicBytesCount);
                outputFile.Write(rawDDS, 0, rawDDS.Length);
                outputFile.Close();
            }
        }
    }
}
