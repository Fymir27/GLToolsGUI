﻿using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GLToolsGUI.Model;
using GLToolsGUI.Utils;
using ImageMagick;

namespace GLToolsGUI.Forms
{
    public partial class TextureConverter : Form
    {
        private const string ImageTypeFileDialogDescription = "PNG Image (*.png)|*.png";
        private const string KtexTypeFileDialogDescription = "Klei Texture(*.tex)|*.tex";

        private MagickImage _currentlyLoadedImage;
        private GLTexture _currentlyLoadedTexture;

        public TextureConverter()
        {
            InitializeComponent();
            Popup.SetErrorLabel(errorText);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            kleiMagicBytesPreview.Text = "No original texture loaded";
        }

        private static string BytesToHexString(byte[] bytes)
        {
            return string.Join(" ", bytes.Select(b => $"x{b,2:X2}"));
        }

        private void SetLoadedTexture(GLTexture texture)
        {
            _currentlyLoadedTexture = texture;
            var previewImage = texture.Image.Clone();
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

        private void OpenKTEX_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = KtexTypeFileDialogDescription;
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var textureFile = File.OpenRead(dialog.FileName); // reader will dispose of file automatically
                using (var reader = new GLReader(textureFile))
                {
                    try
                    {
                        var texture = new GLTexture(reader);
                        SetLoadedTexture(texture);
                        kleiMagicBytesPreview.Text = BytesToHexString(texture.MagicBytes);
                    }
                    catch (Exception exception)
                    {
                        Popup.Error("Error loading texture!", true, dialog.FileName, exception);
                    }
                }
            }
        }

        private void SavePNG_Click(object sender, EventArgs e)
        {
            if (_currentlyLoadedTexture == null)
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
                    _currentlyLoadedTexture.Image.Write(dialog.FileName);
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

        private void saveKTEX_click(object sender, EventArgs e)
        {
            if (_currentlyLoadedImage == null)
            {
                Popup.Error("No image loaded");
                return;
            }

            if (_currentlyLoadedTexture == null)
            {
                Popup.Error("Please load original texture first!");
                return;
            }

            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = KtexTypeFileDialogDescription;
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                using (var outputFile = dialog.OpenFile())
                {
                    try
                    {
                        _currentlyLoadedTexture.Image = _currentlyLoadedImage;
                        _currentlyLoadedTexture.Write(outputFile);
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