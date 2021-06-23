
namespace GLToolsGUI.Forms
{
    partial class TextureConverter
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
            this.openKTEX = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.savePNG = new System.Windows.Forms.Button();
            this.loadedKTEXPreview = new System.Windows.Forms.PictureBox();
            this.saveKTEX = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.openImage = new System.Windows.Forms.Button();
            this.kleiMagicBytesPreview = new System.Windows.Forms.Label();
            this.loadedImagePreview = new System.Windows.Forms.PictureBox();
            this.errorText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.loadedKTEXPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.loadedImagePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // openKTEX
            // 
            this.openKTEX.Location = new System.Drawing.Point(82, 8);
            this.openKTEX.Name = "openKTEX";
            this.openKTEX.Size = new System.Drawing.Size(75, 23);
            this.openKTEX.TabIndex = 3;
            this.openKTEX.Text = "Choose File";
            this.openKTEX.UseVisualStyleBackColor = true;
            this.openKTEX.Click += new System.EventHandler(this.OpenKTEX_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Klei Texture";
            // 
            // savePNG
            // 
            this.savePNG.Location = new System.Drawing.Point(163, 8);
            this.savePNG.Name = "savePNG";
            this.savePNG.Size = new System.Drawing.Size(90, 23);
            this.savePNG.TabIndex = 5;
            this.savePNG.Text = "Save as PNG";
            this.savePNG.UseVisualStyleBackColor = true;
            this.savePNG.Click += new System.EventHandler(this.SavePNG_Click);
            // 
            // loadedKTEXPreview
            // 
            this.loadedKTEXPreview.Location = new System.Drawing.Point(12, 37);
            this.loadedKTEXPreview.Name = "loadedKTEXPreview";
            this.loadedKTEXPreview.Size = new System.Drawing.Size(615, 619);
            this.loadedKTEXPreview.TabIndex = 6;
            this.loadedKTEXPreview.TabStop = false;
            // 
            // saveKTEX
            // 
            this.saveKTEX.Location = new System.Drawing.Point(1122, 8);
            this.saveKTEX.Name = "saveKTEX";
            this.saveKTEX.Size = new System.Drawing.Size(130, 23);
            this.saveKTEX.TabIndex = 9;
            this.saveKTEX.Text = "Save as Klei Texture";
            this.saveKTEX.UseVisualStyleBackColor = true;
            this.saveKTEX.Click += new System.EventHandler(this.saveKTEX_click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(953, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Standard Image";
            // 
            // openImage
            // 
            this.openImage.Location = new System.Drawing.Point(1041, 8);
            this.openImage.Name = "openImage";
            this.openImage.Size = new System.Drawing.Size(75, 23);
            this.openImage.TabIndex = 7;
            this.openImage.Text = "Choose File";
            this.openImage.UseVisualStyleBackColor = true;
            this.openImage.Click += new System.EventHandler(this.openImage_Click);
            // 
            // kleiMagicBytesPreview
            // 
            this.kleiMagicBytesPreview.AutoSize = true;
            this.kleiMagicBytesPreview.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.kleiMagicBytesPreview.Location = new System.Drawing.Point(259, 13);
            this.kleiMagicBytesPreview.Name = "kleiMagicBytesPreview";
            this.kleiMagicBytesPreview.Size = new System.Drawing.Size(134, 13);
            this.kleiMagicBytesPreview.TabIndex = 10;
            this.kleiMagicBytesPreview.Text = "0x69 0x69 0x69 0x69 0x69";
            this.kleiMagicBytesPreview.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // loadedImagePreview
            // 
            this.loadedImagePreview.Location = new System.Drawing.Point(637, 37);
            this.loadedImagePreview.Name = "loadedImagePreview";
            this.loadedImagePreview.Size = new System.Drawing.Size(615, 619);
            this.loadedImagePreview.TabIndex = 11;
            this.loadedImagePreview.TabStop = false;
            // 
            // errorText
            // 
            this.errorText.AutoSize = true;
            this.errorText.ForeColor = System.Drawing.Color.Red;
            this.errorText.Location = new System.Drawing.Point(12, 659);
            this.errorText.Name = "errorText";
            this.errorText.Size = new System.Drawing.Size(74, 13);
            this.errorText.TabIndex = 12;
            this.errorText.Text = "No Errors (yet)";
            // 
            // TextureConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.errorText);
            this.Controls.Add(this.loadedImagePreview);
            this.Controls.Add(this.kleiMagicBytesPreview);
            this.Controls.Add(this.saveKTEX);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.openImage);
            this.Controls.Add(this.loadedKTEXPreview);
            this.Controls.Add(this.savePNG);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.openKTEX);
            this.Name = "TextureConverter";
            this.Text = "GLTools";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize) (this.loadedKTEXPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.loadedImagePreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button openKTEX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button savePNG;
        private System.Windows.Forms.PictureBox loadedKTEXPreview;
        private System.Windows.Forms.Button saveKTEX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button openImage;
        private System.Windows.Forms.Label kleiMagicBytesPreview;
        private System.Windows.Forms.PictureBox loadedImagePreview;
        private System.Windows.Forms.Label errorText;
    }
}

