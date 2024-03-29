﻿using System.ComponentModel;

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
            this.btnConvertTexture.Location = new System.Drawing.Point(26, 35);
            this.btnConvertTexture.Name = "btnConvertTexture";
            this.btnConvertTexture.Size = new System.Drawing.Size(75, 23);
            this.btnConvertTexture.TabIndex = 0;
            this.btnConvertTexture.Text = "Texture";
            this.btnConvertTexture.UseVisualStyleBackColor = true;
            this.btnConvertTexture.Click += new System.EventHandler(this.btnConvertTexture_Click);
            // 
            // btnConvertPlax
            // 
            this.btnConvertPlax.Location = new System.Drawing.Point(107, 35);
            this.btnConvertPlax.Name = "btnConvertPlax";
            this.btnConvertPlax.Size = new System.Drawing.Size(75, 23);
            this.btnConvertPlax.TabIndex = 1;
            this.btnConvertPlax.Text = "Plax";
            this.btnConvertPlax.UseVisualStyleBackColor = true;
            this.btnConvertPlax.Click += new System.EventHandler(this.btnConvertPlax_Click);
            // 
            // btnConvertBuild
            // 
            this.btnConvertBuild.Location = new System.Drawing.Point(188, 35);
            this.btnConvertBuild.Name = "btnConvertBuild";
            this.btnConvertBuild.Size = new System.Drawing.Size(75, 23);
            this.btnConvertBuild.TabIndex = 2;
            this.btnConvertBuild.Text = "Build";
            this.btnConvertBuild.UseVisualStyleBackColor = true;
            this.btnConvertBuild.Click += new System.EventHandler(this.btnConvertBuild_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(26, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(237, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "What would you like to convert?";
            // 
            // NavigationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 69);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnConvertBuild);
            this.Controls.Add(this.btnConvertPlax);
            this.Controls.Add(this.btnConvertTexture);
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