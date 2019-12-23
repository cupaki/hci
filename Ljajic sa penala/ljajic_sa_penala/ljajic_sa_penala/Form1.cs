using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ljajic_sa_penala
{
    public partial class Form1 : Form
    {
        private TreeNode cvor_za_prevlacenje = null;
        private Rectangle mouseDownSelekcioniProzor;
        private Point ofsetEkrana;

        private PictureBox pbZum = null;
        private Spomenik aktivniSpomenik = null;
        private PictureBox pb = null;

        Dictionary<object, bool> errorRepeat = new Dictionary<object, bool>();
        private Regex rx_imeS = null;
        private Regex rx_oznaka = null;
        private bool unosCheck = false;
        Tip tipSpomenika = null;
        List<Tag> tags = new List<Tag>();
        List<Tag> tagoviSpomenika = new List<Tag>();

        private Regex rx_tipNaziv = null;
        private Regex rx_tipOznaka = null;
        //private bool tipCheck = false;

        Image defaultIkonica = null;
        Image ikonica = null;

        public Form1()
        {
            InitializeComponent();

            errorRepeat.Add(comboBox1, false);
         //   errorRepeat.Add(comboBox2, false);
         //   errorRepeat.Add(comboBox3, false);
            errorRepeat.Add(comboBox4, false);

            rx_imeS = new Regex("^[A-Za-z]");
            rx_oznaka = new Regex("^.+");
            errorRepeat.Add(textBox2, false);
            errorRepeat.Add(textBox1, false);
            errorRepeat.Add(textBox3, false);
            errorRepeat.Add(textBox4, false);

            rx_tipOznaka = new Regex("^.+");
            rx_tipNaziv = new Regex("^[A-Za-z]");

            defaultIkonica = pictureBox2.BackgroundImage;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) {
                toolStripStatusLabel1.Text = "Niste označili spomenik za izmenu.";
                toolStripStatusLabel1.Visible = true;
                statusStrip1.Visible = true;
                return;
            }

            statusStrip1.Visible = false;

            Spomenik sp = (Spomenik)dataGridView1.SelectedRows[0].Tag;
            dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
            tabControl1.SelectTab(tabPage2);
            tabPage2.Text = "Izmena";
            textBox1.Text = sp.oznaka;
            textBox2.Text = sp.ime;
            comboBox1.SelectedItem = sp.eraPorekla;
            if (sp.unesco.Equals("DA"))
            {
                radioButton1.Checked = true;
            }
            else
                radioButton2.Checked = true;
            comboBox4.SelectedItem = sp.tustickiStatus;
            if (sp.arhObradjen.Equals("DA"))
                radioButton3.Checked = true;
            else
                radioButton4.Checked = false;
            if (sp.naseljen.Equals("DA"))
                radioButton5.Checked = true;
            else
                radioButton6.Checked = true;
            numericUpDown1.Value = sp.prihod;
            dateTimePicker1.Value = sp.datum;
            tipSpomenika = sp.tip;
            textBox3.Text = sp.tip.oznakaTipa;
            textBox4.Text = sp.tip.imeTipa;
            richTextBox1.Text = sp.tip.opisTipa;
            pictureBox2.BackgroundImage = sp.tip.ikonicaTipa;

            SpomeniciBaza.getInstance().getSpomenici().Remove(sp);

        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            //Ovo je događaj validiranja koji se okida kada polje _izgubi_ fokus. 
            if (rx_oznaka.Match(textBox1.Text).Success)
            {
                greskaUnosa.SetError(textBox1, ""); //Ovako se postavlja da se greška isključi
                errorRepeat[sender] = false; // Ovo resetuje brojanje ponavljanje greške
            }
            else
            {
                unosCheck = false;
                //Ovim se podešava da se ispisuje greška.
                greskaUnosa.SetError(textBox1, "Oznaka ne sme ostati prazna.");
                //  formIsValid = false;
                if (!errorRepeat[sender]) //Ovo je način da zabranimo korisnku da izađe iz kontrole prvi put, ali ne drugi put
                {
                   // e.Cancel = true; //Prelazak iz kontrole je zabranjen
                }
                errorRepeat[sender] = !errorRepeat[sender]; //Promenimo stanje vođenja računa o preskakanju iz kontrole u kontrolu
            }
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            //Ovo je događaj validiranja koji se okida kada polje _izgubi_ fokus. 
            if (rx_imeS.Match(textBox2.Text).Success)
            {
                greskaUnosa.SetError(textBox2, ""); //Ovako se postavlja da se greška isključi
                errorRepeat[sender] = false; // Ovo resetuje brojanje ponavljanje greške
            }
            else
            {
                unosCheck = false;
                //Ovim se podešava da se ispisuje greška.
                greskaUnosa.SetError(textBox2, "Ime moze sadržati samo slova.");
                //  formIsValid = false;
                if (!errorRepeat[sender]) //Ovo je način da zabranimo korisnku da izađe iz kontrole prvi put, ali ne drugi put
                {
                   // e.Cancel = true; //Prelazak iz kontrole je zabranjen
                }
                errorRepeat[sender] = !errorRepeat[sender]; //Promenimo stanje vođenja računa o preskakanju iz kontrole u kontrolu
            }
        }

        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            //Ovo je događaj validiranja koji se okida kada polje _izgubi_ fokus. 
            if (rx_tipOznaka.Match(textBox3.Text).Success)
            {
                greskaUnosa.SetError(textBox3, ""); //Ovako se postavlja da se greška isključi
                errorRepeat[sender] = false; // Ovo resetuje brojanje ponavljanje greške
            }
            else
            {
                unosCheck = false;
                //Ovim se podešava da se ispisuje greška.
                greskaUnosa.SetError(textBox3, "Oznaka tipa ne sme ostati prazna.");
                //  formIsValid = false;
                if (!errorRepeat[sender]) //Ovo je način da zabranimo korisnku da izađe iz kontrole prvi put, ali ne drugi put
                {
                    //e.Cancel = true; //Prelazak iz kontrole je zabranjen
                }
                errorRepeat[sender] = !errorRepeat[sender]; //Promenimo stanje vođenja računa o preskakanju iz kontrole u kontrolu
            }
        }

        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            //Ovo je događaj validiranja koji se okida kada polje _izgubi_ fokus. 
            if (rx_tipNaziv.Match(textBox4.Text).Success)
            {
                greskaUnosa.SetError(textBox4, ""); //Ovako se postavlja da se greška isključi
                errorRepeat[sender] = false; // Ovo resetuje brojanje ponavljanje greške
            }
            else
            {
                unosCheck = false;
                //Ovim se podešava da se ispisuje greška.
                greskaUnosa.SetError(textBox4, "U nazivu tipa mogu biti samo slova.");
                //  formIsValid = false;
                if (!errorRepeat[sender]) //Ovo je način da zabranimo korisnku da izađe iz kontrole prvi put, ali ne drugi put
                {
                    //e.Cancel = true; //Prelazak iz kontrole je zabranjen
                }
                errorRepeat[sender] = !errorRepeat[sender]; //Promenimo stanje vođenja računa o preskakanju iz kontrole u kontrolu
            }
        }

        private void comboBox1_Validating(object sender, CancelEventArgs e)
        {
            if (!comboBox1.Items.Contains(comboBox1.Text))
            {
                unosCheck = false;
                greskaUnosa.SetError(comboBox1, "Era porekla ne sme ostati prazna");
                // formIsValid = false;
                if (!errorRepeat[sender]) //Ovo je način da zabranimo korisnku da izađe iz kontrole prvi put, ali ne drugi put
                {
                   // e.Cancel = true; //Prelazak iz kontrole je zabranjen
                }
                errorRepeat[sender] = !errorRepeat[sender];
            }
            else
            {
                if (comboBox1.Items.Contains(comboBox1.Text))
                {
                    greskaUnosa.SetError(comboBox1, ""); //Ovako se postavlja da se greška isključi
                    errorRepeat[sender] = false;
                }
            }
        }

  /*      private void comboBox2_Validating(object sender, CancelEventArgs e)
        {
            if (!comboBox2.Items.Contains(comboBox2.Text))
            {
                unosCheck = false;
                greskaUnosa.SetError(comboBox2, "UNESCO polje ne sme ostati prazno.");
                // formIsValid = false;
                if (!errorRepeat[sender]) //Ovo je način da zabranimo korisnku da izađe iz kontrole prvi put, ali ne drugi put
                {
                    //e.Cancel = true; //Prelazak iz kontrole je zabranjen
                }
                errorRepeat[sender] = !errorRepeat[sender];
            }
            else
            {
                if (comboBox2.Items.Contains(comboBox2.Text))
                {
                    greskaUnosa.SetError(comboBox2, ""); //Ovako se postavlja da se greška isključi
                    errorRepeat[sender] = false;
                }
            }
        } */

        private void comboBox4_Validating(object sender, CancelEventArgs e)
        {
            if (!comboBox4.Items.Contains(comboBox4.Text))
            {
                unosCheck = false;
                greskaUnosa.SetError(comboBox4, "Turistički status ne sme ostati prazan.");
                // formIsValid = false;
                if (!errorRepeat[sender]) //Ovo je način da zabranimo korisnku da izađe iz kontrole prvi put, ali ne drugi put
                {
                    //e.Cancel = true; //Prelazak iz kontrole je zabranjen
                }
                errorRepeat[sender] = !errorRepeat[sender];
            }
            else
            {
                if (comboBox4.Items.Contains(comboBox4.Text))
                {
                    greskaUnosa.SetError(comboBox4, ""); //Ovako se postavlja da se greška isključi
                    errorRepeat[sender] = false;
                }
            }
        }

   /*     private void comboBox3_Validating(object sender, CancelEventArgs e)
        {
            if (!comboBox3.Items.Contains(comboBox3.Text))
            {
                unosCheck = false;
                greskaUnosa.SetError(comboBox3, "Polje 'Arheološki obrađen' ne sme ostati prazno.");
                // formIsValid = false;
                if (!errorRepeat[sender]) //Ovo je način da zabranimo korisnku da izađe iz kontrole prvi put, ali ne drugi put
                {
                    //e.Cancel = true; //Prelazak iz kontrole je zabranjen
                }
                errorRepeat[sender] = !errorRepeat[sender];
            }
            else
            {
                if (comboBox3.Items.Contains(comboBox3.Text))
                {
                    greskaUnosa.SetError(comboBox3, ""); //Ovako se postavlja da se greška isključi
                    errorRepeat[sender] = false;
                }
            }
        } */

    /*    private void comboBox5_Validating(object sender, CancelEventArgs e)
        {
            if (!comboBox5.Items.Contains(comboBox5.Text))
            {
                unosCheck = false;
                greskaUnosa.SetError(comboBox5, "Polje 'Naseljeni region' ne sme ostati prazno.");
                // formIsValid = false;
                if (!errorRepeat[sender]) //Ovo je način da zabranimo korisnku da izađe iz kontrole prvi put, ali ne drugi put
                {
                    //e.Cancel = true; //Prelazak iz kontrole je zabranjen
                }
                errorRepeat[sender] = !errorRepeat[sender];
            }
            else
            {
                if (comboBox5.Items.Contains(comboBox5.Text))
                {
                    greskaUnosa.SetError(comboBox5, ""); //Ovako se postavlja da se greška isključi
                    errorRepeat[sender] = false;
                }
            }
        } */

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            comboBox1.Text = "";
          //  comboBox2.Text = "";
          //  comboBox3.Text = "";
            comboBox4.Text = "";
          //  comboBox5.Text = "";
            numericUpDown1.Value = 0;
            richTextBox1.Text = "";
            dateTimePicker1.Value = DateTime.Today;

            pictureBox2.Image = null;
            pictureBox2.BackgroundImage = null;
            pictureBox2.BackgroundImage = defaultIkonica;
            greskaUnosa.Clear();
            label13.Visible = false;
            listView2.Items.Clear();


            List<Tag> tagici = TagoviBaza.getInstance().getTagovi();
            foreach (Tag t in tagici) {
                Console.WriteLine(t.oznakaTaga);
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files (*.jpg; *.png)| *.jpg; *.png";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                defaultIkonica = pictureBox2.BackgroundImage;
                pictureBox2.BackgroundImage = null;
                ikonica = Image.FromFile(ofd.FileName);
                Image slicica = (Image)(new Bitmap(ikonica, new Size(64, 64)));
                //pictureBox2.Image = (Image)(new Bitmap(ikonica, new Size(64, 64)));
                pictureBox2.BackgroundImage = slicica;
                // pbIkonica.Load(ofd.FileName);


                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox2.BackColor = Color.Transparent;
                //slikaValid = true;
                // epTip.SetError(pbIkonica, "");

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            unosCheck = true;
            this.ValidateChildren();
            if (!unosCheck)
            {
                label13.Visible = true;
                timer1.Start();
            }
            else {
                //OVDE CE ICI CUVANJE PODATAKA U OBJEKAT SPOMENIKA
                //PRE CISCENJA TEXT I COMBO BOXOVA

                String oznaka = textBox1.Text;
                String ime = textBox2.Text;
                String era = comboBox1.SelectedItem.ToString();
                String unesco;
                if (radioButton1.Checked)
                    unesco = radioButton1.Text;
                else
                    unesco = radioButton2.Text;
                String turisticki = comboBox4.SelectedItem.ToString();
                String arhObradjen;
                if (radioButton3.Checked)
                    arhObradjen = radioButton3.Text;
                else
                    arhObradjen = radioButton4.Text;
                String naseljen;
                if (radioButton5.Checked)
                    naseljen = radioButton5.Text;
                else
                    naseljen = radioButton6.Text;
                decimal prihod = numericUpDown1.Value;
                DateTime datum = dateTimePicker1.Value.Date;
            /*    foreach (Tag t in tags) {
                    
                        ListViewItem item = listView2.FindItemWithText(t.oznakaTaga);
                        // if (item == null)
                        //  item.Text = "";
                        if (item.Text.Equals(t.oznakaTaga))
                            tagoviSpomenika.Add(t);
                    
                } 
                //Image ikonica = tipSpomenika.ikonicaTipa;
                foreach(Tag tt in tagoviSpomenika) {
                    Console.WriteLine(tt.oznakaTaga);
                } */

                List<Spomenik> spomenici = SpomeniciBaza.getInstance().getSpomenici();
                Spomenik s = new Spomenik(oznaka, ime, era, unesco, turisticki, arhObradjen, naseljen, prihod, datum, tipSpomenika, tipSpomenika.ikonicaTipa, tagoviSpomenika);
                SpomeniciBaza.getInstance().getSpomenici().Add(s);
                osveziTabeluSpomenika();

                label26.Visible = true;
                timer1.Start();


                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                comboBox1.Text = "";
               // comboBox2.Text = "";
               // comboBox3.Text = "";
                comboBox4.Text = "";
              //  comboBox5.Text = "";
                numericUpDown1.Value = 0;
                richTextBox1.Text = "";
                dateTimePicker1.Value = DateTime.Today;

                pictureBox2.Image = null;
                pictureBox2.BackgroundImage = defaultIkonica;
                listView2.Items.Clear();

                formTree();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            label13.Visible = false;
            label25.Visible = false;
            label26.Visible = false;
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            //osveziTabeluSpomenika();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            tabPage2.Text = "Novi spomenik";
            greskaUnosa.Clear();
            pictureBox2.Image = null;
            pictureBox2.BackgroundImage = defaultIkonica;
            osveziTabeluSpomenika();
            statusStrip1.Visible = false;
            toolStripStatusLabel1.Visible = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK) {
               // List<Tag> tag22 = TagoviBaza.getInstance().getTagovi();
                Tag tag = (Tag)f.Tag;
               // tag22.Add(tag);
                TagoviBaza.getInstance().getTagovi().Add(tag);

                ListViewItem li = new ListViewItem();
                li.ForeColor = tag.bojaTaga;
                li.Text = tag.oznakaTaga;
                li.Tag = tag;
                listView1.Items.Add(li);

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmTipovi ft = new frmTipovi();
            ft.Tag = "odaberi";
            ft.osveziTabelu();
            ft.ShowDialog();
            if (ft.DialogResult == DialogResult.OK) {
                Tip tip = (Tip)ft.Tag;
                tipSpomenika = tip;
                textBox3.Text = tip.oznakaTipa;
                textBox4.Text = tip.imeTipa;
                richTextBox1.Text = tip.opisTipa;
                pictureBox2.BackgroundImage = tip.ikonicaTipa;
                
            } 
        }

        private void button8_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                
                listView2.Items.Add((ListViewItem)item.Clone());
                Tag t = new Tag();
                t.oznakaTaga = item.Text;
                t.bojaTaga = item.ForeColor;
                tagoviSpomenika.Add(t);
            }
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView2.SelectedItems) {
                listView2.Items.Remove(item);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            unosCheck = true;
            this.ValidateChildren();
            if (!unosCheck)
            {
                label13.Visible = true;
                timer1.Start();
            }
            else {
                List<Tip> tipovi = TipoviBaza.getInstance().getTipovi();

                String oznakaTipa = textBox3.Text;
                String imeTipa = textBox4.Text;
                String opisTipa = richTextBox1.Text;
                Image ikonaTipa = pictureBox2.BackgroundImage;

               // noviTip.oznakaTipa = textBox3.Text;
               // noviTip.imeTipa = textBox4.Text;
               // noviTip.opisTipa = richTextBox1.Text;
               // noviTip.ikonicaTipa = pictureBox2.BackgroundImage;
                Tip tip = new Tip(oznakaTipa, imeTipa, opisTipa, ikonaTipa);
                TipoviBaza.getInstance().getTipovi().Add(tip);
                tipSpomenika = tip;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                richTextBox1.ReadOnly = true;
                richTextBox1.BackColor = Color.LightGray;
                label25.Visible = true;
                timer1.Start();
            }
        }

        public void osveziTabeluSpomenika() {
            dataGridView1.Rows.Clear();
            List<Spomenik> spomenici = SpomeniciBaza.getInstance().getSpomenici();

            foreach (Spomenik s in spomenici)
            {
                String tagovi = "";
                foreach (Tag t in s.tagovi)
                {
                    tagovi = tagovi + " #" + t.oznakaTaga;
                    
                }

                dataGridView1.Rows.Add(new object[] { s.oznaka, s.ime, s.eraPorekla, s.unesco, s.tustickiStatus, s.arhObradjen, s.naseljen, s.prihod, s.datum, s.tip.imeTipa, new Bitmap(s.tip.ikonicaTipa, new Size(30, 30)), tagovi });
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = s;
            }
            dataGridView1.ClearSelection();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                toolStripStatusLabel1.Text = "Niste oznacili spomenik za brisanje.";
                statusStrip1.Visible = true;
                toolStripStatusLabel1.Visible = true;
                return;
            }
            else
                statusStrip1.Visible = false;
            //statusStrip1.Visible = false;


            Spomenik sp = (Spomenik)dataGridView1.SelectedRows[0].Tag;
            if (sp == null)
            {
                return;
            }
            SpomeniciBaza.getInstance().getSpomenici().Remove(sp);
            dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
            formTree();

            
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) { 
                //OVDE CE SE POLJA ANULIRATI
                return;
            }

            Spomenik sp = (Spomenik)dataGridView1.SelectedRows[0].Tag;
            if (sp == null)
            {
                return;
            }

            //POLJIMA DAJEMO VREDNOSTI SPOMENIKA sp
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SpomeniciBaza.getInstance().UcitajDatoteku();
            TipoviBaza.getInstance().UcitajDatoteku();
            TagoviBaza.getInstance().UcitajDatoteku();

            List<Tip> tipovi = TipoviBaza.getInstance().getTipovi();
            List<Spomenik> spomenici = SpomeniciBaza.getInstance().getSpomenici();
            List<Tag> tagovi = TagoviBaza.getInstance().getTagovi();

            //vezivanje tipa spomenika sa tipom
            foreach (Tip t in tipovi)
            {

                foreach (Spomenik sp in spomenici)
                {

                    if (sp.tip.imeTipa.Equals(t.imeTipa))
                    {
                        sp.tip = t;
                    }
                }
            }

            // povezivanje taga spomenika sa tagom
            foreach (Spomenik sp in spomenici)
            {
                List<Tag> tagovi_spomenika = new List<Tag>();


                foreach (Tag Tagsp in sp.tagovi)
                {

                    foreach (Tag tag in tagovi)
                    {
                        if (tag.oznakaTaga.Equals(Tagsp.oznakaTaga))
                        {
                            tagovi_spomenika.Add(tag);
                        }
                    }

                }
                sp.tagovi = tagovi_spomenika;
            }

            List<Tag> tagLista = TagoviBaza.getInstance().getTagovi();
            foreach (Tag tt in tagLista) {
                ListViewItem li = new ListViewItem();
                li.ForeColor = tt.bojaTaga;
                li.Text = tt.oznakaTaga;
                li.Tag = tt;
                listView1.Items.Add(li);
            }
            
            osveziTabeluSpomenika();

            formTree();
            pictureBox1.AllowDrop = true;
            foreach (ToolStripMenuItem item in contextMenuStrip1.Items)
            {
                if (item.Name.Equals("ukloniToolStripMenuItem"))
                {
                    item.Click += new EventHandler(Ukloni_spomenik_Click);
                }
            }

           // treeView1.GiveFeedback += tr
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SpomeniciBaza.getInstance().MemorisiDatoteku();
            TipoviBaza.getInstance().MemorisiDatoteku();
            TagoviBaza.getInstance().MemorisiDatoteku();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox3.ReadOnly = false;
            textBox4.ReadOnly = false;
            richTextBox1.ReadOnly = false;
            richTextBox1.BackColor = Color.White;
        }

        private void formTree() {
            List<Tip> tipovi = TipoviBaza.getInstance().getTipovi();
            List<Spomenik> spomenici = SpomeniciBaza.getInstance().getSpomenici();
            List<Spomenik> spomenici_tipa;
            treeView1.Nodes.Find("Tipovi", true)[0].Nodes.Clear();
            treeView1.Nodes.Find("Tipovi", true)[0].ForeColor = Color.Black;
            Image ikonica_tipa = imageList1.Images[0];
            imageList1.Images.Clear();
            imageList1.Images.Add(ikonica_tipa);
            pictureBox1.Controls.Clear();
            int i = 0;
            if (tipovi.Count > 0) {
                foreach (Tip t in tipovi) {
                    spomenici_tipa = new List<Spomenik>();
                    foreach (Spomenik sp in spomenici) {
                        if (sp.tip.imeTipa.Equals(t.imeTipa)) {
                            spomenici_tipa.Add(sp);
                        }
                    }

                    TreeNode[] cvorovi_spomenici = new TreeNode[spomenici_tipa.Count];
                    if (spomenici_tipa.Count > 0) {
                        for (int j = 0; j < spomenici_tipa.Count; j++) {
                            imageList1.Images.Add(spomenici_tipa[j].tip.ikonicaTipa);
                            i++;
                            cvorovi_spomenici[j] = new TreeNode(spomenici_tipa[j].ime, i, i);
                            cvorovi_spomenici[j].Tag = spomenici_tipa[j];
                            cvorovi_spomenici[j].ToolTipText = spomenici_tipa[j].ime;

                            if (spomenici_tipa[j].prevucen) {
                                cvorovi_spomenici[j].ForeColor = Color.Gray;

                                PictureBox pbSpomenik = new PictureBox();

                                pbSpomenik.Size = new System.Drawing.Size(30, 30);
                                pbSpomenik.Location = spomenici_tipa[j].mapa_pozicija;
                                pbSpomenik.Image = new Bitmap(spomenici_tipa[j].tip.ikonicaTipa, new Size(30, 30));

                                String tagovi_za_detalje = "";
                                foreach (Tag tag_detalji in spomenici_tipa[j].tagovi) {
                                    tagovi_za_detalje += " #" + tag_detalji.oznakaTaga;
                                }
                                String detalji = "Naziv:              " + spomenici_tipa[j].ime + Environment.NewLine +
                                                 "Tip:                " + spomenici_tipa[j].tip.imeTipa + Environment.NewLine +
                                                 "Era porekla:        " + spomenici_tipa[j].eraPorekla + Environment.NewLine +
                                                 "Unesco:             " + spomenici_tipa[j].unesco + Environment.NewLine +
                                                 "Turistički status:  " + spomenici_tipa[j].tustickiStatus + Environment.NewLine +
                                                 "Arheološki obrađen: " + spomenici_tipa[j].arhObradjen + Environment.NewLine +
                                                 "Naseljeni region:   " + spomenici_tipa[j].naseljen + Environment.NewLine +
                                                 "Godišnji prihod:    " + spomenici_tipa[j].prihod.ToString() + " $" + Environment.NewLine +
                                                 "Datum otkrivanja:   " + spomenici_tipa[j].datum.ToString("dd.MM.yyyy.") + Environment.NewLine +
                                                 "Tagovi:             " + tagovi_za_detalje;

                                pbSpomenik.MouseHover += new EventHandler(zumiraj_ikonicu_MouseHover);
                                pbSpomenik.DragOver += new DragEventHandler(pbSpomenik_DragOver);
                                pbSpomenik.AllowDrop = true;

                                toolTip1.SetToolTip(pbSpomenik, detalji);
                                pbSpomenik.Tag = spomenici_tipa[j];
                                pictureBox1.Controls.Add(pbSpomenik);
                            }
                        }
                    }

                    imageList1.Images.Add(t.ikonicaTipa);
                    i++;
                    TreeNode cvor_tip = new TreeNode(t.imeTipa, i, i, cvorovi_spomenici);
                    cvor_tip.ToolTipText = "Tip: " + t.imeTipa;
                    cvor_tip.Expand();
                    treeView1.Nodes.Find("Tipovi", true)[0].Nodes.Add(cvor_tip);
                    treeView1.Nodes.Find("Tipovi", true)[0].Expand();
                }
            }
        }


        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            cvor_za_prevlacenje = treeView1.GetNodeAt(e.Location);
            if (cvor_za_prevlacenje != null)
            {
                treeView1.SelectedNode = cvor_za_prevlacenje;

                if (cvor_za_prevlacenje.GetNodeCount(true) == 0)
                {
                    if (cvor_za_prevlacenje.Tag != null)
                    {
                        Spomenik sp = (Spomenik)cvor_za_prevlacenje.Tag;
                        if (sp.prevucen == false)
                        {
                            Size dragVelicina = SystemInformation.DragSize;
                            mouseDownSelekcioniProzor = new Rectangle(new Point(e.X - dragVelicina.Width / 2, e.Y - dragVelicina.Height / 2), dragVelicina);
                        }
                        else
                        {
                            mouseDownSelekcioniProzor = Rectangle.Empty;
                        }
                    }
                }
                //////////////////////////////////
                else
                {
                    cvor_za_prevlacenje = null;
                }
                ////////////////////////////////
            }
        }

        private void treeView1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (mouseDownSelekcioniProzor != Rectangle.Empty && (!mouseDownSelekcioniProzor.Contains(e.X, e.Y)))
                {
                    ofsetEkrana = SystemInformation.WorkingArea.Location;
                    if (cvor_za_prevlacenje != null)
                    {
                        DragDropEffects dropEffect = this.DoDragDrop(cvor_za_prevlacenje, DragDropEffects.Copy);
                    }
                }

            }
        }

        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            Type testTip = new TreeNode().GetType();

            if (e.Data.GetDataPresent(testTip))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            Type testTip = new TreeNode().GetType();
            PictureBox mapa = (PictureBox)sender;
            Spomenik spomenik;
            mouseDownSelekcioniProzor = Rectangle.Empty;
            Point p = pictureBox1.PointToClient(Cursor.Position);

            PictureBox pb = (PictureBox)pictureBox1.GetChildAtPoint(p);

            if (e.Data.GetDataPresent(testTip) && pb == null) {
                TreeNode prevuceni_cvor = (TreeNode)e.Data.GetData(testTip);
                spomenik = (Spomenik)prevuceni_cvor.Tag;
                PictureBox pbSpomenik = new PictureBox();

                pbSpomenik.Size = new System.Drawing.Size(30, 30);
                pbSpomenik.Location = new Point(p.X - 15, p.Y - 15);
                pbSpomenik.Image = new Bitmap(spomenik.tip.ikonicaTipa, new Size(30, 30));
                prevuceni_cvor.ForeColor = Color.Gray;
                pbSpomenik.Tag = spomenik;

                String tagovi_za_detalje = "";
                foreach (Tag tag_detalji in spomenik.tagovi)
                {
                    tagovi_za_detalje += " #" + tag_detalji.oznakaTaga;
                }
                String detalji = "Naziv:              " + spomenik.ime + Environment.NewLine +
                                 "Tip:                " + spomenik.tip.imeTipa + Environment.NewLine +
                                 "Era porekla:        " + spomenik.eraPorekla + Environment.NewLine +
                                 "Unesco:             " + spomenik.unesco + Environment.NewLine +
                                 "Turistički status:  " + spomenik.tustickiStatus + Environment.NewLine +
                                 "Arheološki obrađen: " + spomenik.arhObradjen + Environment.NewLine +
                                 "Naseljeni region:   " + spomenik.naseljen + Environment.NewLine +
                                 "Godišnji prihod:    " + spomenik.prihod.ToString() + " $" + Environment.NewLine +
                                 "Datum otkrivanja:   " + spomenik.datum.ToString("dd.MM.yyyy.") + Environment.NewLine +
                                 "Tagovi:             " + tagovi_za_detalje;

                pbSpomenik.MouseHover += new EventHandler(zumiraj_ikonicu_MouseHover);
                pbSpomenik.DragOver += new DragEventHandler(pbSpomenik_DragOver);
                pbSpomenik.AllowDrop = true;
                pictureBox1.Controls.Add(pbSpomenik);
                spomenik.prevucen = true;

                spomenik.mapa_pozicija = pbSpomenik.Location;
            }
        }

        private void zumiraj_ikonicu_MouseHover(object sender, EventArgs e)
        {
            if (pbZum != null)
            {
                int x1 = pbZum.Location.X;
                int y1 = pbZum.Location.Y;

                for (int i = 120; i > 30; i -= 11)
                {

                    pbZum.Size = new Size(i, i);
                    pbZum.Location = new Point(x1 + (120 - i) / 2, y1 + (120 - i) / 2);
                    pbZum.Image = new Bitmap(aktivniSpomenik.tip.ikonicaTipa, new Size(i, i));

                    pictureBox1.Refresh();
                }
                pbZum.Dispose();
            }
            Point p = pictureBox1.PointToClient(Cursor.Position);
            pb = (PictureBox)pictureBox1.GetChildAtPoint(p);

            if (pb != null) {
                Spomenik spomenik = (Spomenik)pb.Tag;
                aktivniSpomenik = spomenik;
                pbZum = new PictureBox();
                pbZum.ContextMenuStrip = contextMenuStrip1;
                //pbZum.ContextMenuStrip = cmspbIkonica;

                int x = pb.Location.X + 15;
                int y = pb.Location.Y + 15;
                pictureBox1.Controls.Add(pbZum);
                pbZum.Show();
                pbZum.BringToFront();

                for (int i = 0; i < 90; i += 5)
                {

                    pbZum.Size = new Size(30 + i, 30 + i);
                    pbZum.Location = new Point(x - (30 + i) / 2, y - (30 + i) / 2);
                    pbZum.Image = new Bitmap(spomenik.tip.ikonicaTipa, new Size(30 + i, 30 + i));
                    pbZum.BringToFront();
                    pictureBox1.Refresh();
                }

                String tagovi_za_detalje = "";
                foreach (Tag tag_detalji in spomenik.tagovi)
                {
                    tagovi_za_detalje += " #" + tag_detalji.oznakaTaga;
                }

                String detalji = "Naziv: " + spomenik.ime + Environment.NewLine +
                                 "Tip: " + spomenik.tip.imeTipa + Environment.NewLine +
                                 "Era porekla: " + spomenik.eraPorekla + Environment.NewLine +
                                 "Unesco: " + spomenik.unesco + Environment.NewLine +
                                 "Turistički status: " + spomenik.tustickiStatus + Environment.NewLine +
                                 "Arheološki obrađen: " + spomenik.arhObradjen + Environment.NewLine +
                                 "Naseljeni region: " + spomenik.naseljen + Environment.NewLine +
                                 "Godišnji prihod: " + spomenik.prihod.ToString() + " $" + Environment.NewLine +
                                 "Datum otkrivanja: " + spomenik.datum.ToString("dd.MM.yyyy.") + Environment.NewLine +
                                 "Tagovi: " + tagovi_za_detalje;
                toolTip1.SetToolTip(pbZum, detalji);
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            if (pbZum != null)
            {
                int x = pbZum.Location.X;
                int y = pbZum.Location.Y;

                for (int i = 120; i > 30; i -= 11)
                {

                    pbZum.Size = new Size(i, i);
                    pbZum.Location = new Point(x + (120 - i) / 2, y + (120 - i) / 2);
                    pbZum.Image = new Bitmap(aktivniSpomenik.tip.ikonicaTipa, new Size(i, i));
                    pictureBox1.Refresh();
                }
                pbZum.Dispose();
            }
        }

        private void pbSpomenik_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
        }

        private void pictureBox1_DragOver(object sender, DragEventArgs e)
        {
            Type testTip = new TreeNode().GetType();


            //Console.WriteLine("pbMapa_dragover");
            if (e.Data.GetDataPresent(testTip))
            {
                e.Effect = DragDropEffects.Copy;
                //e.UseDefaultCursors = false;
                //Cursor.Current = Cursors.Hand;
                return;
            }

            e.Effect = DragDropEffects.All;
        }

        private void Ukloni_spomenik_Click(object sender, EventArgs e)
        {
            Spomenik sp = aktivniSpomenik;
            sp.prevucen = false;
            TreeNode[] cvorovi = treeView1.Nodes.Find(sp.ime, true);
            if (cvorovi.Length > 0)
            {
                cvorovi[0].ForeColor = Color.Black;
            }
            pb.Dispose();
            formTree();
        }

    }
}
