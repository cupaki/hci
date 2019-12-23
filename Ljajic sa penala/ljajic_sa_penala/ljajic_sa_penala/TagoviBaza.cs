using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ljajic_sa_penala
{
    class TagoviBaza
    {
        private static List<Tag> tagovi = new List<Tag>();
        private readonly string datoteka;
        private static TagoviBaza instance;

        private TagoviBaza()
        {
            datoteka = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tagovi.podaci");
            UcitajDatoteku();
        }

        public static TagoviBaza getInstance() {
            if (instance == null)
                instance = new TagoviBaza();
            return instance;
        }

        public List<Tag> getTagovi() {
            return tagovi;
        }

        public void UcitajDatoteku() {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = null;

            if (File.Exists(datoteka))
            {
                try
                {
                    stream = File.Open(datoteka, FileMode.Open);
                    tagovi = (List<Tag>)formatter.Deserialize(stream);
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
                tagovi = new List<Tag>();
        }

        public void MemorisiDatoteku() {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = null;

            try
            {
                stream = File.Open(datoteka, FileMode.OpenOrCreate);
                formatter.Serialize(stream, tagovi);
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
