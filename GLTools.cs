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
        private string[] allowedImageTypes = new[]
        {
            "png",
            "jpg",
            "jpeg",
            "gif"
        };

        MagickImage currentlyLoadedImage;
        MagickImage currentlyLoadedKTEX;
        const int magicBytesCount = 5;
        byte[] kleiMagicBytes = null;
        byte[] kleiFileMagic = { (byte)'K', (byte)'T', (byte)'E', (byte)'X' };

        public GLTools()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            kleiMagicBytesPreview.Text = "No original texture loaded";
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

        private string bytesToString(byte[] bytes)
        {
            return bytes.Aggregate("", (carry, currentByte) => carry += (char)currentByte);
        }

        private void OpenKTEX_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                var fileTypeString = string.Join(";", allowedImageTypes.Select(type => $"*.{type}"));
                dialog.InitialDirectory = Directory.GetCurrentDirectory();
                dialog.Filter = "Klei Texture (*.tex)|*.tex";                
                dialog.RestoreDirectory = true;
                if(dialog.ShowDialog() != DialogResult.OK)
                {
                    errorText.Text = "Error selecting file";
                    return;
                }

                var fs = dialog.OpenFile();
                int magicLength = 4;
                var magic = new byte[magicLength];                
                if(fs.Read(magic, 0, magicLength) < magicLength)
                {
                    errorText.Text = "Failed to read magic!";
                    return;
                }

                var magicString = bytesToString(magic);

                if(magicString == "KTEX")
                {                    
                    kleiMagicBytes = new byte[magicBytesCount];
                    if (fs.Read(kleiMagicBytes, 0, magicBytesCount) < magicBytesCount)
                    {
                        errorText.Text = "Failed to read KLEI disrcriminator!";
                        return;
                    }
                    kleiMagicBytesPreview.Text = "";
                    foreach (var b in kleiMagicBytes)
                    {
                        kleiMagicBytesPreview.Text += (int)b;
                    }
                } 
                else if(magicString == "DDS")
                {
                    fs.Position = 0;
                } 
                else
                {
                    errorText.Text = "Invalid file magic";
                    return;
                }

                long bytesLeft = fs.Length - fs.Position;
                var ddsImageBytes = new byte[bytesLeft];                
                fs.Read(ddsImageBytes, 0, (int)bytesLeft);
                fs.Close();

                SetLoadedKTEX(new MagickImage(ddsImageBytes));
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

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
                dialog.Filter = "PNG Image (*.png)|*.png";
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    errorText.Text = "Error selecting file";
                    return;
                }

                currentlyLoadedKTEX.Write(dialog.FileName);
            }   
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void openImage_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                var fileTypeString = string.Join(";", allowedImageTypes.Select(type => $"*.{type}"));
                dialog.InitialDirectory = Directory.GetCurrentDirectory();
                dialog.Filter = "Image (*.png,*.jpeg)|*.png;*jpeg";
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    errorText.Text = "Error selecting file";
                    return;
                }

                SetLoadedImage(new MagickImage(dialog.FileName));
            }
        }

        private void kleiMagicBytesPreview_Click(object sender, EventArgs e)
        {

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
                errorText.Text = "Please load original texture first";
                return;
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.InitialDirectory = Directory.GetCurrentDirectory();
                dialog.Filter = "Klei Texture (*.tex)|*.tex";
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    errorText.Text = "Error selecting file";
                    return;
                }

                currentlyLoadedImage.Format = MagickFormat.Dds;
                currentlyLoadedImage.Settings.SetDefine(MagickFormat.Dds, "compression", "dxt5");
                var rawDDS = currentlyLoadedImage.ToByteArray();

                var outputFile = File.OpenWrite(dialog.FileName);

                outputFile.Write(kleiFileMagic, 0, kleiFileMagic.Length);
                outputFile.Write(kleiMagicBytes, 0, magicBytesCount);
                outputFile.Write(rawDDS, 0, rawDDS.Length);
                outputFile.Close();
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
