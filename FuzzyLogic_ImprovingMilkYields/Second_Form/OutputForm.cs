using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FuzzyLogic
{
    public partial class OutputForm : Form
    {
        public OutputForm()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        public void ShowImage(Image img)
        {
            pictureBox1.Image = img;
        }
    }
}
