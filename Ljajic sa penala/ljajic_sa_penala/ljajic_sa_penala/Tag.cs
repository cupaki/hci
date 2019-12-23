using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ljajic_sa_penala
{
    [Serializable]
   public  class Tag
    {
        public String oznakaTaga { get; set; }
        public Color bojaTaga { get; set; }
        public String opisTaga { get; set; }

        public Tag(String oznaka, string opis, Color boja)
        {
            this.oznakaTaga = oznaka;
            this.opisTaga = opis;
            this.bojaTaga = boja;
        }

        public Tag() {
            this.oznakaTaga = "";
            this.bojaTaga = Color.White;
            this.opisTaga = "";
        }
    }
}
