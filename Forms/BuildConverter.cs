using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using GLToolsGUI.Model;
using GLToolsGUI.Utils;
using ImageMagick;

namespace GLToolsGUI.Forms
{
    public partial class BuildConverter : Form
    {
        private const string KleiBinTypeFileDialogDescription = "Klei Build (*.bin)|*.bin";
        private GLBuild _currentlyLoadedBuild;

        public BuildConverter()
        {
            InitializeComponent();
            Popup.SetErrorLabel(errorText);
        }
        
        private void SetPreviewImage(GLTexture texture)
        {
            var previewedImage = texture.Image.Clone();
            previewedImage.Resize(previewImage.Width, previewImage.Height);
            previewImage.Image = Image.FromStream(new MemoryStream(previewedImage.ToByteArray(MagickFormat.Png)));
        }

        private void openBuild_Click(object sender, EventArgs e)
        {
            buildName.Text = "No build loaded";
            using var dialog = new OpenFileDialog
            {
                Filter = KleiBinTypeFileDialogDescription, 
                RestoreDirectory = true
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
                
            var buildFile = File.OpenRead(dialog.FileName); // reader will dispose of file automatically
            using (var reader = new GLReader(buildFile))
            {
                try
                {
                    _currentlyLoadedBuild = new GLBuild(reader);
                    SetPreviewImage(_currentlyLoadedBuild.RootTexture);
                }
                catch (Exception exception)
                {
                    Popup.Error("Error loading build!", true, dialog.FileName, exception);
                    return;
                }
            }

            buildName.Text = _currentlyLoadedBuild.Root;
            
            GLAnimationSet anim;
            string AnimPath = Path.GetFullPath(Path.Combine(dialog.FileName, "..", "anim.bin"));
            var animFile = File.OpenRead(AnimPath);
            using (var reader = new GLReader(animFile))
            {
                try
                {
                    anim = new GLAnimationSet(reader);
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error loading anim: " + exception.Message);
                    return;
                }
            }

            SCML.createFile("anim.scml", _currentlyLoadedBuild, anim);
        }

        private void saveBuild_Click(object sender, EventArgs e)
        {
            if (_currentlyLoadedBuild == null)
            {
                Popup.Error("No build loaded!");
                return;
            }

            using var dialog = new OpenFileDialog
            {
                Filter = KleiBinTypeFileDialogDescription, 
                RestoreDirectory = true
            };


            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var output = File.OpenWrite(dialog.FileName);
            using var writer = new GLWriter(output);
            try
            {
                _currentlyLoadedBuild.Write(writer, true, true);
                Popup.Success("Successfully saved to: " + dialog.FileName);
            }
            catch (Exception exception)
            {
                Popup.Error("Error saving build!", true, dialog.FileName, exception);
            }
        }

        private void loadFramesFromFolders_Click(object sender, EventArgs e)
        {
            if (_currentlyLoadedBuild == null)
            {
                Popup.Error("No build loaded!");
                return;
            }

            // TODO: make folder browser
            using var dialog = new OpenFileDialog
            {
                Filter = "any|*.*", 
                RestoreDirectory = true
            };


            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            
            _currentlyLoadedBuild.UpdatePartsFromDirectory(Path.GetDirectoryName(dialog.FileName) ?? throw new Exception($"Invalid Path: {dialog.FileName}"));
            _currentlyLoadedBuild.UpdateRootTextureFromParts();
            SetPreviewImage(_currentlyLoadedBuild.RootTexture);
        }

        private void saveFramesToFolders_Click(object sender, EventArgs e)
        {
            if (_currentlyLoadedBuild == null)
            {
                Popup.Error("No build loaded!");
                return;
            }

            // TODO: make folder browser
            using var dialog = new OpenFileDialog
            {
                Filter = "any|*.*", 
                RestoreDirectory = true
            };


            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string baseDir = Path.GetDirectoryName(dialog.FileName) ?? throw new Exception("Invalid path: " + dialog.FileName);
            foreach ((string name, var frames) in _currentlyLoadedBuild.Parts)
            {
                string dirName = Path.Combine(baseDir, name);
                Directory.CreateDirectory(dirName);
                foreach ((string index, var image) in frames)
                {
                    image.Write(Path.Combine(dirName, index + ".png"));
                }
            }
        }
    }
}