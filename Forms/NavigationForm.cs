using System;
using System.Windows.Forms;

namespace GLToolsGUI.Forms
{
    public partial class NavigationForm : Form
    {
        public NavigationForm()
        {
            InitializeComponent();
        }

        private void btnConvertTexture_Click(object sender, EventArgs e)
        {
            var formTexture = new TextureConverter();
            formTexture.Show();
        }

        private void btnConvertPlax_Click(object sender, EventArgs e)
        {
            var formPlax = new PlaxConverter();
            formPlax.Show();
        }

        private void btnConvertBuild_Click(object sender, EventArgs e)
        {
            var formBuild = new BuildConverter();
            formBuild.Show();
        }
    }
}