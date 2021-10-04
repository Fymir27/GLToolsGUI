using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using GLToolsGUI.Model;
using GLToolsGUI.Utils;
using ImageMagick;

namespace GLToolsGUI.Forms
{
    public partial class PlaxConverter : Form
    {
        private const string ImageTypeFileDialogDescription = "PNG Image (*.png)|*.png";
        private const string PlaxTypeFileDialogDescription = "Klei PLAX (*.tex)|*.tex";

        private MagickImage _currentlyLoadedImage;
        private GLPlax _currentlyLoadedPlax;

        public PlaxConverter()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            plaxRootPreview.Text = "No original PLAX loaded";
            Popup.SetErrorLabel(errorText);
        }

        private void SetLoadedPlax(GLPlax plax)
        {
            _currentlyLoadedPlax = plax;
            plaxRootPreview.Text = plax.Root;
            var previewImage = plax.Combined.Clone();
            previewImage.Resize(loadedKTEXPreview.Width, loadedKTEXPreview.Height);
            loadedKTEXPreview.Image = Image.FromStream(new MemoryStream(previewImage.ToByteArray(MagickFormat.Png)));
        }

        private void SetLoadedImage(MagickImage image)
        {
            _currentlyLoadedImage = image;
            var previewImage = image.Clone();
            previewImage.Resize(loadedImagePreview.Width, loadedKTEXPreview.Height);
            loadedImagePreview.Image = Image.FromStream(new MemoryStream(previewImage.ToByteArray(MagickFormat.Png)));
        }

        private void OpenPLAX_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = PlaxTypeFileDialogDescription;
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var plaxFile = File.OpenRead(dialog.FileName); // reader will dispose of file automatically
                using (var reader = new GLReader(plaxFile))
                {
                    try
                    {
                        var plax = new GLPlax(reader);
                        SetLoadedPlax(plax);
                    }
                    catch (Exception exception)
                    {
                        Popup.Error("Error loading plax!", true, dialog.FileName, exception);
                    }
                }
            }
        }

        private void SavePNG_Click(object sender, EventArgs e)
        {
            if (_currentlyLoadedPlax == null)
            {
                Popup.Error("No texture loaded");
                return;
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = ImageTypeFileDialogDescription;
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    _currentlyLoadedPlax.Combined.Write(dialog.FileName);
                    Popup.Success("Successfully saved to: " + dialog.FileName);
                }
                catch (Exception exception)
                {
                    Popup.Error("Error saving Image!", true, dialog.FileName, exception);
                }
            }
        }

        private void openImage_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = ImageTypeFileDialogDescription;
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                SetLoadedImage(new MagickImage(dialog.FileName));
            }
        }

        private void savePLAX_click(object sender, EventArgs e)
        {
            if (_currentlyLoadedImage == null)
            {
                Popup.Error("No image loaded");
                return;
            }

            if (_currentlyLoadedPlax == null)
            {
                Popup.Error("Please load original PLAX first!");
                return;
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = PlaxTypeFileDialogDescription;
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var output = File.OpenWrite(dialog.FileName);
                using (var writer = new GLWriter(output))
                {
                    try
                    {
                        _currentlyLoadedPlax.Combined = _currentlyLoadedImage;
                        _currentlyLoadedPlax.Write(writer, true);
                        Popup.Success("Successfully saved to: " + dialog.FileName);
                    }
                    catch (Exception exception)
                    {
                        Popup.Error("Error saving texture!", true, dialog.FileName, exception);
                    }
                }
            }
        }
    }
}