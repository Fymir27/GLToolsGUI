
namespace GLToolsGUI.Forms
{
    partial class PlaxConverter
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
            this.openPLAX = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.savePNG = new System.Windows.Forms.Button();
            this.loadedKTEXPreview = new System.Windows.Forms.PictureBox();
            this.savePLAX = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.openImage = new System.Windows.Forms.Button();
            this.plaxRootPreview = new System.Windows.Forms.Label();
            this.loadedImagePreview = new System.Windows.Forms.PictureBox();
            this.errorText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.loadedKTEXPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.loadedImagePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // openPLAX
            // 
            this.openPLAX.Location = new System.Drawing.Point(82, 8);
            this.openPLAX.Name = "openPLAX";
            this.openPLAX.Size = new System.Drawing.Size(75, 23);
            this.openPLAX.TabIndex = 3;
            this.openPLAX.Text = "Choose File";
            this.openPLAX.UseVisualStyleBackColor = true;
            this.openPLAX.Click += new System.EventHandler(this.OpenPLAX_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Klei Plax";
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
            // savePLAX
            // 
            this.savePLAX.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.savePLAX.Location = new System.Drawing.Point(1122, 8);
            this.savePLAX.Name = "savePLAX";
            this.savePLAX.Size = new System.Drawing.Size(130, 23);
            this.savePLAX.TabIndex = 9;
            this.savePLAX.Text = "Save as PLAX";
            this.savePLAX.UseVisualStyleBackColor = true;
            this.savePLAX.Click += new System.EventHandler(this.savePLAX_click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(953, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Standard Image";
            // 
            // openImage
            // 
            this.openImage.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openImage.Location = new System.Drawing.Point(1041, 8);
            this.openImage.Name = "openImage";
            this.openImage.Size = new System.Drawing.Size(75, 23);
            this.openImage.TabIndex = 7;
            this.openImage.Text = "Choose File";
            this.openImage.UseVisualStyleBackColor = true;
            this.openImage.Click += new System.EventHandler(this.openImage_Click);
            // 
            // plaxRootPreview
            // 
            this.plaxRootPreview.AutoSize = true;
            this.plaxRootPreview.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.plaxRootPreview.Location = new System.Drawing.Point(259, 13);
            this.plaxRootPreview.Name = "plaxRootPreview";
            this.plaxRootPreview.Size = new System.Drawing.Size(134, 13);
            this.plaxRootPreview.TabIndex = 10;
            this.plaxRootPreview.Text = "0x69 0x69 0x69 0x69 0x69";
            this.plaxRootPreview.TextAlign = System.Drawing.ContentAlignment.TopCenter;
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
            this.errorText.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.errorText.AutoSize = true;
            this.errorText.ForeColor = System.Drawing.Color.Red;
            this.errorText.Location = new System.Drawing.Point(12, 659);
            this.errorText.Name = "errorText";
            this.errorText.Size = new System.Drawing.Size(74, 13);
            this.errorText.TabIndex = 12;
            this.errorText.Text = "No Errors (yet)";
            // 
            // PlaxConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.errorText);
            this.Controls.Add(this.loadedImagePreview);
            this.Controls.Add(this.plaxRootPreview);
            this.Controls.Add(this.savePLAX);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.openImage);
            this.Controls.Add(this.loadedKTEXPreview);
            this.Controls.Add(this.savePNG);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.openPLAX);
            this.Name = "PlaxConverter";
            this.Text = "Plax Converter";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize) (this.loadedKTEXPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.loadedImagePreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button openPLAX;

        private System.Windows.Forms.Button savePLAX;

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button savePNG;
        private System.Windows.Forms.PictureBox loadedKTEXPreview;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button openImage;
        private System.Windows.Forms.Label plaxRootPreview;
        private System.Windows.Forms.PictureBox loadedImagePreview;
        private System.Windows.Forms.Label errorText;
    }
}

