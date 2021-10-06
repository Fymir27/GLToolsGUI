
namespace GLToolsGUI.Forms
{
    partial class BuildConverter
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openBuild = new System.Windows.Forms.Button();
            this.buildName = new System.Windows.Forms.Label();
            this.saveBuild = new System.Windows.Forms.Button();
            this.previewImage = new System.Windows.Forms.PictureBox();
            this.errorText = new System.Windows.Forms.Label();
            this.loadFramesFromFolders = new System.Windows.Forms.Button();
            this.saveFramesToFolders = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize) (this.previewImage)).BeginInit();
            this.SuspendLayout();
            // 
            // openBuild
            // 
            this.openBuild.Location = new System.Drawing.Point(12, 12);
            this.openBuild.Name = "openBuild";
            this.openBuild.Size = new System.Drawing.Size(75, 23);
            this.openBuild.TabIndex = 0;
            this.openBuild.Text = "Open Build";
            this.openBuild.UseVisualStyleBackColor = true;
            this.openBuild.Click += new System.EventHandler(this.openBuild_Click);
            // 
            // buildName
            // 
            this.buildName.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.buildName.Location = new System.Drawing.Point(93, 12);
            this.buildName.Name = "buildName";
            this.buildName.Size = new System.Drawing.Size(303, 23);
            this.buildName.TabIndex = 1;
            this.buildName.Text = "No build loaded";
            this.buildName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // saveBuild
            // 
            this.saveBuild.Location = new System.Drawing.Point(1123, 12);
            this.saveBuild.Name = "saveBuild";
            this.saveBuild.Size = new System.Drawing.Size(129, 23);
            this.saveBuild.TabIndex = 2;
            this.saveBuild.Text = "Save Build From Frames";
            this.saveBuild.UseVisualStyleBackColor = true;
            this.saveBuild.Click += new System.EventHandler(this.saveBuild_Click);
            // 
            // previewImage
            // 
            this.previewImage.Location = new System.Drawing.Point(12, 41);
            this.previewImage.Name = "previewImage";
            this.previewImage.Size = new System.Drawing.Size(1240, 604);
            this.previewImage.TabIndex = 3;
            this.previewImage.TabStop = false;
            // 
            // errorText
            // 
            this.errorText.ForeColor = System.Drawing.Color.Red;
            this.errorText.Location = new System.Drawing.Point(12, 655);
            this.errorText.Name = "errorText";
            this.errorText.Size = new System.Drawing.Size(1240, 17);
            this.errorText.TabIndex = 4;
            this.errorText.Text = "Error";
            // 
            // loadFramesFromFolders
            // 
            this.loadFramesFromFolders.Location = new System.Drawing.Point(445, 12);
            this.loadFramesFromFolders.Name = "loadFramesFromFolders";
            this.loadFramesFromFolders.Size = new System.Drawing.Size(157, 23);
            this.loadFramesFromFolders.TabIndex = 5;
            this.loadFramesFromFolders.Text = "Load Frames From Folders";
            this.loadFramesFromFolders.UseVisualStyleBackColor = true;
            this.loadFramesFromFolders.Click += new System.EventHandler(this.loadFramesFromFolders_Click);
            // 
            // saveFramesToFolders
            // 
            this.saveFramesToFolders.Location = new System.Drawing.Point(608, 12);
            this.saveFramesToFolders.Name = "saveFramesToFolders";
            this.saveFramesToFolders.Size = new System.Drawing.Size(157, 23);
            this.saveFramesToFolders.TabIndex = 6;
            this.saveFramesToFolders.Text = "Save Frames To Folders";
            this.saveFramesToFolders.UseVisualStyleBackColor = true;
            this.saveFramesToFolders.Click += new System.EventHandler(this.saveFramesToFolders_Click);
            // 
            // BuildConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.saveFramesToFolders);
            this.Controls.Add(this.loadFramesFromFolders);
            this.Controls.Add(this.errorText);
            this.Controls.Add(this.previewImage);
            this.Controls.Add(this.saveBuild);
            this.Controls.Add(this.buildName);
            this.Controls.Add(this.openBuild);
            this.Name = "BuildConverter";
            this.Text = "Build Converter";
            ((System.ComponentModel.ISupportInitialize) (this.previewImage)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button saveFramesToFolders;

        private System.Windows.Forms.Button button1;

        private System.Windows.Forms.PictureBox previewImage;

        private System.Windows.Forms.Button saveBuild;

        private System.Windows.Forms.Button loadFramesFromFolders;

        private System.Windows.Forms.Label errorText;

        private System.Windows.Forms.Label buildName;
        private System.Windows.Forms.Button openBuild;

        #endregion
    }
}