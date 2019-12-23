using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ljajic_sa_penala
{
    class TipoviBaza
    {
        private static List<Tip> tipovi = new List<Tip>();

        private readonly string datoteka;

        private static TipoviBaza instance;

        private TipoviBaza() {
            datoteka = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tipovi.podaci");
            UcitajDatoteku();
        }

        public static TipoviBaza getInstance() {
            if (instance == null)
                instance = new TipoviBaza();
            return instance;
        }

        public List<Tip> getTipovi() {
            return tipovi;
        }

        public void UcitajDatoteku() {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = null;

            if (File.Exists(datoteka))
            {
                try
                {
                    stream = File.Open(datoteka, FileMode.Open);
                    tipovi = (List<Tip>)formatter.Deserialize(stream);
                }
                catch
                {
                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                }
            }
            else
                tipovi = new List<Tip>();
        }

        public void MemorisiDatoteku() {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = null;

            try
            {
                stream = File.Open(datoteka, FileMode.OpenOrCreate);
                formatter.Serialize(stream, tipovi);
            }
            catch
            {
                //
            }
            finally {
                if (stream != null)
                    stream.Dispose();
            }
        }
    }
}
