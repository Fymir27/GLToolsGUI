using System;
using System.IO;
using System.Windows.Forms;
using GLToolsGUI.Model;
using GLToolsGUI.Utils;

namespace GLToolsGUI.Forms
{
    public partial class BuildConverter : Form
    {
        private const string KleiBinTypeFileDialogDescription = "Klei Build (*.bin)|*.bin";

        public BuildConverter()
        {
            InitializeComponent();
        }

        private void openBuild_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                //dialog.InitialDirectory = Directory.GetCurrentDirectory();
                dialog.Filter = KleiBinTypeFileDialogDescription;       
                dialog.RestoreDirectory = true;
                
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    Console.WriteLine("Failed selecting file");
                    return;
                }

                GLBuild build;
                var buildFile = File.OpenRead(dialog.FileName); // reader will dispose of file automatically
                using (var reader = new GLReader(buildFile))
                {
                    try
                    {
                        build = new GLBuild(reader);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Error loading build: " + exception.Message);
                        return;
                    }
                }
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

                buildName.Text = build.Root;
            }
        }
    }
}