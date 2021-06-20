﻿using System;
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
                var buildFile = dialog.OpenFile(); // reader will dispose of file automatically
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

                buildName.Text = build.Root;
            }
        }
    }
}