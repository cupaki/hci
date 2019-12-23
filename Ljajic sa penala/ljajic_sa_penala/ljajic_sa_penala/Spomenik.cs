using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ljajic_sa_penala
{
    [Serializable]
    public class Spomenik
    {
        public String oznaka {get ; set;}
        public String ime { get; set; }
        public String eraPorekla { get; set; }
        public String unesco { get; set; }
        public String tustickiStatus { get; set; }
        public String arhObradjen { get; set; }
        public String naseljen { get; set; }
        public decimal prihod { get; set; }
        public DateTime datum { get; set; }
        public Image ikona;
        public bool prevucen { get; set; }
        public Point mapa_pozicija { get; set; }

        public Tip tip { get; set; }

        public List<Tag> tagovi { get; set; }

        public Spomenik(String oz, String im, String ep, String un, String ts, String ao, String nas, decimal pr, DateTime dat, Tip t, Image ik, List<Tag> tags) {
            this.oznaka = oz;
            this.ime = im;
            this.eraPorekla = ep;
            this.unesco = un;
            this.tustickiStatus = ts;
            this.arhObradjen = ao;
            this.naseljen = nas;
            this.prihod = pr;
            this.datum = dat;
            this.tip = t;
            this.ikona = ik;
            this.tagovi = tags;
            this.prevucen = false;
        }

        public Spomenik() {
            this.oznaka = "";
            this.ime = "";
            this.eraPorekla = "";
            this.unesco = "";
            this.tustickiStatus = "";
            this.arhObradjen = "";
            this.naseljen = "";
            this.prihod = 0;
            this.datum = DateTime.Now;
            this.tip = null;
            this.tagovi = new List<Tag>();
            this.prevucen = false;
        }

        public String ispisiSpomenik() {
            String ispis = "Spomenik" + oznaka + ime + eraPorekla + unesco + tustickiStatus + arhObradjen + naseljen + prihod + datum.ToString() + tip.imeTipa;
            return ispis;
        }
    }
}
