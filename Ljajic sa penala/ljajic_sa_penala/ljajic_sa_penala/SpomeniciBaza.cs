using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ljajic_sa_penala
{
    class SpomeniciBaza
    {
        private static List<Spomenik> spomenici = new List<Spomenik>();
        private readonly string datoteka;
        private static SpomeniciBaza instance;

        private SpomeniciBaza() {
            datoteka = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "spmenici.podaci");
            UcitajDatoteku();
        }

        public static SpomeniciBaza getInstance() {
            if (instance == null)
                instance = new SpomeniciBaza();
            return instance;
        }

        public List<Spomenik> getSpomenici() {
            return spomenici;
        }

        public void UcitajDatoteku() {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = null;

            if (File.Exists(datoteka))
            {
                try
                {
                    stream = File.Open(datoteka, FileMode.Open);
                    spomenici = (List<Spomenik>)formatter.Deserialize(stream);
                }
                catch
                {
                    //
                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                }
            }
            else
                spomenici = new List<Spomenik>();
        }
        public void MemorisiDatoteku() {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = null;

            try
            {
                stream = File.Open(datoteka, FileMode.OpenOrCreate);
                formatter.Serialize(stream, spomenici);
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
