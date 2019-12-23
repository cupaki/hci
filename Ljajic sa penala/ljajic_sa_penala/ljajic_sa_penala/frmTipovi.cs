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
    public partial class frmTipovi : Form
    {

        public static Tip privremeniTip;
        public frmTipovi()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public void osveziTabelu() {
            dataGridView1.Rows.Clear();
            List<Tip> tipovi = TipoviBaza.getInstance().getTipovi();

            foreach (Tip t in tipovi) {
                dataGridView1.Rows.Add(new object[] { t.oznakaTipa, t.imeTipa, new Bitmap(t.ikonicaTipa, new Size(30, 30)), t.opisTipa});
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = t;
            }
            dataGridView1.ClearSelection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Tip tip = (Tip)dataGridView1.SelectedRows[0].Tag;
            privremeniTip = tip;
            this.Tag = tip;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                richTextBox1.Text = "";
                return;
            }
            Tip tip = (Tip)dataGridView1.SelectedRows[0].Tag;
            if (tip == null) return;

            textBox1.Text = tip.oznakaTipa;
            textBox2.Text = tip.imeTipa;
            pictureBox1.Image = new Bitmap(tip.ikonicaTipa, new Size(64, 64));
           // pictureBox1.BackgroundImage = tip.ikonicaTipa;
            richTextBox1.Text = tip.opisTipa;
        }

        private void frmTipovi_Load(object sender, EventArgs e)
        {
            osveziTabelu();
            Tip tip = (Tip)this.Tag;
            for (int i = 0; i < dataGridView1.RowCount; i++) {
                Tip t = (Tip)dataGridView1.Rows[i].Tag;
                if (tip.oznakaTipa.Equals(t.oznakaTipa)) {
                    dataGridView1.Rows[i].Visible = false;
                    break;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                toolStripStatusLabel1.Text = "Niste odabrali tip za brisanje.";
                statusStrip1.Visible = true;
                toolStripStatusLabel1.Visible = true;
                return;
            }

            statusStrip1.Visible = false;
            Tip tip = (Tip)dataGridView1.SelectedRows[0].Tag;
            bool imaSpomenik = false;

            List<Spomenik> spomenici = SpomeniciBaza.getInstance().getSpomenici();
            foreach (Spomenik s in spomenici) {
                if (s.tip.imeTipa.Equals(tip.imeTipa)) {
                    imaSpomenik = true;
                    break;
                }
            }

            if (imaSpomenik)
            {
                brisanjeTipaYN btYN = new brisanjeTipaYN();
                btYN.ShowDialog();
                if (btYN.DialogResult == DialogResult.No)
                {
                    //nista se ne desava;
                }
                else if (btYN.DialogResult == DialogResult.OK)
                {
                    TipoviBaza.getInstance().getTipovi().Remove(tip);
                    List<Spomenik> spomeniciZaBrisanje = new List<Spomenik>();
                    spomenici = SpomeniciBaza.getInstance().getSpomenici();

                    foreach (Spomenik s in spomenici)
                    {
                        if (s.tip.oznakaTipa.Equals(tip.oznakaTipa))
                            spomeniciZaBrisanje.Add(s);
                    }

                    foreach (Spomenik spo in spomeniciZaBrisanje)
                    {
                        spomenici.Remove(spo);
                    }
                    osveziTabelu();
                }
            }
            else {
                TipoviBaza.getInstance().getTipovi().Remove(tip);
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                osveziTabelu();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
          /*  if (dataGridView1.SelectedRows.Count == 0) {
                toolStripStatusLabel1.Text = "Niste odabrali tip za izmenu.";
                statusStrip1.Visible = true;
                return;
            }

            int indexSelekt = dataGridView1.SelectedRows[0].Index;
            Tip tip = (Tip)dataGridView1.SelectedRows[0].Tag;
            */
        }
    }
}
