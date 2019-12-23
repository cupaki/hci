using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ljajic_sa_penala
{
    [Serializable]
    public class Tip
    {
        public String oznakaTipa { get; set; }
        public String imeTipa { get; set; }
        public String opisTipa { get; set; }
        public Image ikonicaTipa { get; set; }


        public Tip(String oz, String ime, String opis, Image ik)
        {
            this.oznakaTipa = oz;
            this.imeTipa = ime;
            this.opisTipa = opis;
            this.ikonicaTipa = ik;
            //Console.WriteLine("Tip kreiran");
        }

        public String ispisiTip()
        {
            String ispis = "Tip: " + this.oznakaTipa + " " + this.imeTipa + " " + this.opisTipa;
            return ispis;
        }
    }
}
