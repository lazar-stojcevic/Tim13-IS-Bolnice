using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Model;
using IS_Bolnice.Repozitorijumi.Interfejsi;

namespace IS_Bolnice.Repozitorijumi.Klase
{
   public abstract class GenerickiFajlRepozitorijum<T>: GenerickiRepozitorijum<T> where T: Entitet 
   {
       private string putanjaDoFajla;

       public GenerickiFajlRepozitorijum(string putanja)
       {
           putanjaDoFajla = putanja;
       }


        public List<T> GetSve()
        {
            List<T> povratnaVrednost = new List<T>();
            if (File.Exists(putanjaDoFajla))
            {
                string[] linije = File.ReadAllLines(putanjaDoFajla);
                foreach (string linija in linije)
                {
                    
                    string[] delovi = linija.Split('#');
                    T entitet = KreirajEntitet(delovi);
                    povratnaVrednost.Add(entitet);
                }
            }

            return povratnaVrednost;

        }


        public T GetPoId(string id)
        {
            List<T> sviEntiteti = GetSve();
            foreach (T entitet in sviEntiteti)
            {
                if (entitet.Id.Equals(id))
                {
                    return entitet;
                }
            }

            return null;

        }

        public void Sacuvaj(T noviEntitet)
        {
            if (File.Exists(putanjaDoFajla))
            {
                List<string> tekst = new List<string>();
                string ispis = KreirajTextZaUpis(noviEntitet);
                tekst.Add(ispis);
                File.AppendAllLines(putanjaDoFajla, tekst);
            }
        }

        public void Izmeni(T noviEntitet)
        {
            Obrisi(noviEntitet.Id);
            Sacuvaj(noviEntitet);
        }

        public void Obrisi(string id)
        {
            List<T> sviEntiteti = GetSve();
            List<string> tekst = new List<string>();
            foreach (T entitet in sviEntiteti)
            {
                if (!entitet.Id.Equals(id))
                {
                    string linija = KreirajTextZaUpis(entitet);
                    tekst.Add(linija);
                }
            }
            File.WriteAllLines(putanjaDoFajla, tekst);
        }

        public abstract T KreirajEntitet(string[] podaciEntiteta);
        public abstract string KreirajTextZaUpis(T entitet);
   }
}
