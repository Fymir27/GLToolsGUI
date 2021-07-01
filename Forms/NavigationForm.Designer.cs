using System.ComponentModel;

namespace GLToolsGUI.Forms
{
    partial class NavigationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.btnConvertTexture = new System.Windows.Forms.Button();
            this.btnConvertPlax = new System.Windows.Forms.Button();
            this.btnConvertBuild = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnConvertTexture
            // 
            this.btnConvertTexture.Location = new System.Drawing.Point(35, 54);
            this.btnConvertTexture.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnConvertTexture.Name = "btnConvertTexture";
            this.btnConvertTexture.Size = new System.Drawing.Size(100, 35);
            this.btnConvertTexture.TabIndex = 0;
            this.btnConvertTexture.Text = "Texture";
            this.btnConvertTexture.UseVisualStyleBackColor = true;
            this.btnConvertTexture.Click += new System.EventHandler(this.btnConvertTexture_Click);
            // 
            // btnConvertPlax
            // 
            this.btnConvertPlax.Location = new System.Drawing.Point(143, 54);
            this.btnConvertPlax.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnConvertPlax.Name = "btnConvertPlax";
            this.btnConvertPlax.Size = new System.Drawing.Size(100, 35);
            this.btnConvertPlax.TabIndex = 1;
            this.btnConvertPlax.Text = "Plax";
            this.btnConvertPlax.UseVisualStyleBackColor = true;
            this.btnConvertPlax.Click += new System.EventHandler(this.btnConvertPlax_Click);
            // 
            // btnConvertBuild
            // 
            this.btnConvertBuild.Location = new System.Drawing.Point(251, 54);
            this.btnConvertBuild.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnConvertBuild.Name = "btnConvertBuild";
            this.btnConvertBuild.Size = new System.Drawing.Size(100, 35);
            this.btnConvertBuild.TabIndex = 2;
            this.btnConvertBuild.Text = "Build";
            this.btnConvertBuild.UseVisualStyleBackColor = true;
            this.btnConvertBuild.Click += new System.EventHandler(this.btnConvertBuild_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(35, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(316, 35);
            this.label1.TabIndex = 3;
            this.label1.Text = "What would you like to convert?";
            // 
            // NavigationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 106);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnConvertBuild);
            this.Controls.Add(this.btnConvertPlax);
            this.Controls.Add(this.btnConvertTexture);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "NavigationForm";
            this.Text = "GLTools";
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button btnConvertTexture;
        private System.Windows.Forms.Button btnConvertPlax;
        private System.Windows.Forms.Button btnConvertBuild;
        private System.Windows.Forms.Label label1;

        #endregion
    }
}