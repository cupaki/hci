using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ljajic_sa_penala
{
    public partial class Form2 : Form
    {
         //Tag tag;
         Color boja;
         bool formValidOznaka = false;
         bool formValidBoja = false;
         bool izmena = false;
        // Tag tagIzmena = null;

        public Form2()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            
            //txtOpis.Text +=  " " + txtOznaka.Text;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                boja = cd.Color;
                formValidBoja = true;
               // erUnos.SetError(button1, "");
                panel1.BackColor = cd.Color;               
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Tag tag = new Tag();
            tag.bojaTaga = panel1.BackColor;
            tag.opisTaga = richTextBox1.Text;
            tag.oznakaTaga = textBox1.Text;
            this.Tag = tag;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
