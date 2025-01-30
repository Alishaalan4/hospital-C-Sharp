using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hospital
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        // FOR USER CONTROL
        private void button1_Click(object sender, EventArgs e)
        {
            new Form6().ShowDialog();
        }

        // FOR DOCTOR CONTROL
        private void button2_Click(object sender, EventArgs e)
        {
            new Form7().ShowDialog();
        }
        // FOR ADMIN CONTROL
        private void button3_Click(object sender, EventArgs e)
        {
            new Form8().ShowDialog();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            new Form7().ShowDialog();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            new Form8().ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Form11().ShowDialog();
        }
    }
}
