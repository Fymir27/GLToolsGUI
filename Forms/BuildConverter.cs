using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using GLToolsGUI.Model;
using GLToolsGUI.Utils;
using ImageMagick;

namespace GLToolsGUI.Forms
{
    public partial class BuildConverter : Form
    {
        private const string KleiBinTypeFileDialogDescription = "Klei Animation (*.bin)|*.bin";
        private GLBuild _currentlyLoadedBuild;
        private GLAnimationSet _currentlyLoadedAnimationSet;

        public BuildConverter()
        {
            InitializeComponent();
            Popup.SetErrorLabel(errorText);
            errorText.Text = "";
        }
        
        private void SetPreviewImage(GLTexture texture)
        {
            var previewedImage = texture.Image.Clone();
            previewedImage.Resize(previewImage.Width, previewImage.Height);
            previewImage.Image = Image.FromStream(new MemoryStream(previewedImage.ToByteArray(MagickFormat.Png)));
        }

        private void openBuild_Click(object sender, EventArgs e)
        {
            buildName.Text = "No animation loaded";
            errorText.Text = "";
            using var dialog = new OpenFileDialog
            {
                Filter = KleiBinTypeFileDialogDescription, 
                RestoreDirectory = true
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string filename = dialog.FileName;
            bool isAnimation = filename.EndsWith("anim.bin");
            
            string buildFilename = isAnimation  
                ? filename.Replace("anim.bin", "build.bin")
                : filename;

            var buildFile = File.OpenRead(buildFilename); // reader will dispose of file automatically
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

            if (isAnimation)
            {
                var animFile = File.OpenRead(filename);
                using var reader = new GLReader(animFile);
                try
                {
                    _currentlyLoadedAnimationSet = new GLAnimationSet(reader);
                }
                catch (Exception exception)
                {
                    Popup.Error("Error loading anim: ", true, "Error", exception);
                    return;
                }
            }


            #region debug

            // Klei Build Format as readable JSON; TODO: remove
            File.WriteAllText("build.json", JsonSerializer.Serialize(_currentlyLoadedBuild, new JsonSerializerOptions
            {
                IncludeFields = true,
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
                WriteIndented = true
            }));
            
            // symbol refs as JSON; TODO: remove
            File.WriteAllText("symbol_refs.json", JsonSerializer.Serialize(
                _currentlyLoadedBuild.Symbols.Select(symbol => new
                {
                    symbol.Ref1,
                    Ref1Resolved = _currentlyLoadedBuild.Refs[symbol.Ref1],
                    symbol.Ref2,
                    Ref2Resolved = _currentlyLoadedBuild.Refs[symbol.Ref2],
                }), 
                new JsonSerializerOptions
                {
                    IncludeFields = true,
                    NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
                    WriteIndented = true
                })
            );

            // Klei Animation Format as readable JSON; TODO: remove
            File.WriteAllText("anim.json", JsonSerializer.Serialize(_currentlyLoadedAnimationSet, new JsonSerializerOptions()
            {
                IncludeFields = true,
                WriteIndented = true,
            }));

            #endregion
           
        }

        private void saveBuild_Click(object sender, EventArgs e)
        {
            if (_currentlyLoadedBuild == null)
            {
                Popup.Error("No build loaded!");
                return;
            }

            using var dialog = new SaveFileDialog
            {
                Filter = KleiBinTypeFileDialogDescription, 
                RestoreDirectory = true
            };


            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var filename = dialog.FileName;
            SaveBuildToFile(filename);
        }

        private void SaveBuildToFile(string filename)
        {
            var output = File.OpenWrite(filename);
            using var writer = new GLWriter(output);
            try
            {
                _currentlyLoadedBuild.Write(writer, true, true);
                Popup.Success("Successfully saved to: " + filename);
            }
            catch (Exception exception)
            {
                Popup.Error("Error saving build!", true, filename, exception);
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
            using var dialog = new SaveFileDialog
            {
                Filter = "any|*.*", 
                RestoreDirectory = true
            };


            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string filename = dialog.FileName;
            SaveImagesToDir(filename);
        }

        private void SaveImagesToDir(string filename)
        {
            string baseDir = Path.GetDirectoryName(filename) ?? throw new Exception("Invalid path: " + filename);
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

        private void saveAnimation_Click(object sender, EventArgs e)
        {
            if (_currentlyLoadedAnimationSet == null)
            {
                Popup.Error("No animation loaded!");
                return;
            }

            using var dialog = new SaveFileDialog
            {
                FileName = "anim",
                Filter = "Spriter File|*.scml",
                RestoreDirectory = true
            };


            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            
            string animFilename = dialog.FileName;
            SaveImagesToDir(animFilename);

            if (!SCMLWriter.WriteGLAnimation(dialog.FileName, _currentlyLoadedBuild, _currentlyLoadedAnimationSet))
            {
                Popup.Error("Failed to write SCML file");
            }
        }

        private void saveGLAnimation_Click(object sender, EventArgs e)
        {
            Popup.Error("TODO"); // TODO
        }

        private void loadAnimation_Click(object sender, EventArgs e)
        {
            Popup.Error("TODO"); // TODO
        }
    }
}