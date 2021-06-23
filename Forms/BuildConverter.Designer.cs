
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // openBuild
            // 
            this.openBuild.Location = new System.Drawing.Point(25, 27);
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
            this.buildName.Location = new System.Drawing.Point(106, 27);
            this.buildName.Name = "buildName";
            this.buildName.Size = new System.Drawing.Size(100, 23);
            this.buildName.TabIndex = 1;
            this.buildName.Text = "No build loaded";
            this.buildName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(258, 186);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 105);
            this.panel1.TabIndex = 2;
            // 
            // BuildConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buildName);
            this.Controls.Add(this.openBuild);
            this.Name = "BuildConverter";
            this.Text = "KleiBuild";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panel1;

        private System.Windows.Forms.Label buildName;

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.Button openBuild;

        #endregion

        private System.Windows.Forms.Button button1;
    }
}