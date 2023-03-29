
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
            this.saveAnimation = new System.Windows.Forms.Button();
            this.saveGLAnim = new System.Windows.Forms.Button();
            this.loadAnimation = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.previewImage)).BeginInit();
            this.SuspendLayout();
            // 
            // openBuild
            // 
            this.openBuild.Location = new System.Drawing.Point(14, 14);
            this.openBuild.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.openBuild.Name = "openBuild";
            this.openBuild.Size = new System.Drawing.Size(196, 27);
            this.openBuild.TabIndex = 0;
            this.openBuild.Text = "Open Griftlands Build/Animation";
            this.openBuild.UseVisualStyleBackColor = true;
            this.openBuild.Click += new System.EventHandler(this.openBuild_Click);
            // 
            // buildName
            // 
            this.buildName.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.buildName.Location = new System.Drawing.Point(211, 14);
            this.buildName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.buildName.Name = "buildName";
            this.buildName.Size = new System.Drawing.Size(271, 27);
            this.buildName.TabIndex = 1;
            this.buildName.Text = "No animation loaded";
            this.buildName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // saveBuild
            // 
            this.saveBuild.Location = new System.Drawing.Point(1357, 14);
            this.saveBuild.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.saveBuild.Name = "saveBuild";
            this.saveBuild.Size = new System.Drawing.Size(103, 27);
            this.saveBuild.TabIndex = 2;
            this.saveBuild.Text = "Save GL Build";
            this.saveBuild.UseVisualStyleBackColor = true;
            this.saveBuild.Click += new System.EventHandler(this.saveBuild_Click);
            // 
            // previewImage
            // 
            this.previewImage.Location = new System.Drawing.Point(14, 47);
            this.previewImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.previewImage.Name = "previewImage";
            this.previewImage.Size = new System.Drawing.Size(1447, 697);
            this.previewImage.TabIndex = 3;
            this.previewImage.TabStop = false;
            // 
            // errorText
            // 
            this.errorText.ForeColor = System.Drawing.Color.Red;
            this.errorText.Location = new System.Drawing.Point(14, 756);
            this.errorText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.errorText.Name = "errorText";
            this.errorText.Size = new System.Drawing.Size(1447, 20);
            this.errorText.TabIndex = 4;
            this.errorText.Text = "Error";
            // 
            // loadFramesFromFolders
            // 
            this.loadFramesFromFolders.Location = new System.Drawing.Point(540, 14);
            this.loadFramesFromFolders.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.loadFramesFromFolders.Name = "loadFramesFromFolders";
            this.loadFramesFromFolders.Size = new System.Drawing.Size(183, 27);
            this.loadFramesFromFolders.TabIndex = 5;
            this.loadFramesFromFolders.Text = "Load Frames From Folders";
            this.loadFramesFromFolders.UseVisualStyleBackColor = true;
            this.loadFramesFromFolders.Click += new System.EventHandler(this.loadFramesFromFolders_Click);
            // 
            // saveFramesToFolders
            // 
            this.saveFramesToFolders.Location = new System.Drawing.Point(834, 14);
            this.saveFramesToFolders.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.saveFramesToFolders.Name = "saveFramesToFolders";
            this.saveFramesToFolders.Size = new System.Drawing.Size(183, 27);
            this.saveFramesToFolders.TabIndex = 6;
            this.saveFramesToFolders.Text = "Save Frames To Folders";
            this.saveFramesToFolders.UseVisualStyleBackColor = true;
            this.saveFramesToFolders.Click += new System.EventHandler(this.saveFramesToFolders_Click);
            // 
            // saveAnimation
            // 
            this.saveAnimation.Location = new System.Drawing.Point(1025, 14);
            this.saveAnimation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.saveAnimation.Name = "saveAnimation";
            this.saveAnimation.Size = new System.Drawing.Size(199, 27);
            this.saveAnimation.TabIndex = 7;
            this.saveAnimation.Text = "Save Spriter (scml) Animation ";
            this.saveAnimation.UseVisualStyleBackColor = true;
            this.saveAnimation.Click += new System.EventHandler(this.saveAnimation_Click);
            // 
            // saveGLAnim
            // 
            this.saveGLAnim.Location = new System.Drawing.Point(1232, 14);
            this.saveGLAnim.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.saveGLAnim.Name = "saveGLAnim";
            this.saveGLAnim.Size = new System.Drawing.Size(117, 27);
            this.saveGLAnim.TabIndex = 8;
            this.saveGLAnim.Text = "Save GL Anim";
            this.saveGLAnim.UseVisualStyleBackColor = true;
            this.saveGLAnim.Click += new System.EventHandler(this.saveGLAnimation_Click);
            // 
            // loadAnimation
            // 
            this.loadAnimation.Location = new System.Drawing.Point(349, 14);
            this.loadAnimation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.loadAnimation.Name = "loadAnimation";
            this.loadAnimation.Size = new System.Drawing.Size(183, 27);
            this.loadAnimation.TabIndex = 9;
            this.loadAnimation.Text = "Load Spriter (scml) Animation";
            this.loadAnimation.UseVisualStyleBackColor = true;
            this.loadAnimation.Click += new System.EventHandler(this.loadAnimation_Click);
            // 
            // BuildConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1475, 786);
            this.Controls.Add(this.loadAnimation);
            this.Controls.Add(this.saveGLAnim);
            this.Controls.Add(this.saveAnimation);
            this.Controls.Add(this.saveFramesToFolders);
            this.Controls.Add(this.loadFramesFromFolders);
            this.Controls.Add(this.errorText);
            this.Controls.Add(this.previewImage);
            this.Controls.Add(this.saveBuild);
            this.Controls.Add(this.buildName);
            this.Controls.Add(this.openBuild);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "BuildConverter";
            this.Text = "Build Converter";
            ((System.ComponentModel.ISupportInitialize)(this.previewImage)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button saveFramesToFolders;

        private System.Windows.Forms.PictureBox previewImage;

        private System.Windows.Forms.Button saveBuild;

        private System.Windows.Forms.Button loadFramesFromFolders;

        private System.Windows.Forms.Label errorText;

        private System.Windows.Forms.Label buildName;
        private System.Windows.Forms.Button openBuild;

        #endregion

        private System.Windows.Forms.Button saveAnimation;
        private System.Windows.Forms.Button saveGLAnim;
        private System.Windows.Forms.Button loadAnimation;
    }
}