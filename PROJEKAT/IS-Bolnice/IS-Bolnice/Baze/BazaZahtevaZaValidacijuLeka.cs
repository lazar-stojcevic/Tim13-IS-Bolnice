// File:    BazaZahtevaZaValidacijuLeka.cs
// Author:  Zola
// Created: Monday, May 3, 2021 9:32:33 PM
// Purpose: Definition of Class BazaZahtevaZaValidacijuLeka

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class BazaZahtevaZaValidacijuLeka
{
   private String fileLocation = @"..\..\Datoteke\zahteviLekovi.txt";
   
   public List<ZahtevZaValidacijuLeka> SviZahtevi()
   {
        List<Lek> lekovi = new List<Lek>();
        BazaLekara bazaLekara = new BazaLekara();
        List<Lekar> sviLekari = bazaLekara.SviLekari();
        List<ZahtevZaValidacijuLeka> listaZahteva = new List<ZahtevZaValidacijuLeka>();

        if (File.Exists(@"..\..\Datoteke\zahteviLekovi.txt"))
        {
            string[] lines = File.ReadAllLines(@"..\..\Datoteke\zahteviLekovi.txt");
            foreach (string line in lines)
            {
                Lek p = new Lek();
                string[] delovi = line.Split('#');
                p.Sifra = delovi[0];
                p.Ime = delovi[1];
                p.Opis = delovi[2];
                if (delovi[3].Equals("1"))
                {
                    p.PotrebanRecept = true;
                }
                else
                {
                    p.PotrebanRecept = false;
                }

                string alergeniSvi = delovi[4];
                if (!alergeniSvi.Equals(""))
                {
                    string[] alergen = alergeniSvi.Split(',');
                    foreach (string a in alergen)
                    {
                        if (a.Equals("")) { break; }

                        if (!a.Equals(""))
                        {
                            Sastojak s = new Sastojak(a);
                            p.Alergeni.Add(s);
                        }
                    }
                }

                Console.WriteLine(line);
                ZahtevZaValidacijuLeka zahtev = new ZahtevZaValidacijuLeka();
                zahtev.Lek = p;

                string[] idLekara = delovi[5].Split('-');
                foreach (Lekar lekarIter in sviLekari)
                {
                    foreach (string id in idLekara)
                    {
                        if (idLekara.Equals("")) { break; }
                        if (lekarIter.Jmbg.Equals(id))
                        {
                            zahtev.lekariKomeIdeNaValidaciju.Add(lekarIter);
                        }
                    }
                }
                listaZahteva.Add(zahtev);
            }
        }
        else
        {
            Console.WriteLine("Nista");
        }
        return listaZahteva;
    }
   
   public void KreirajZahtev(ZahtevZaValidacijuLeka zahtev)
   {
      throw new NotImplementedException();
   }
   
   public void IzmeniZahtev(ZahtevZaValidacijuLeka zahtevZaValidaciju)
   {
      throw new NotImplementedException();
   }
   
   public void ObrisiZahtev(ZahtevZaValidacijuLeka zahtev)
   {
        List<ZahtevZaValidacijuLeka> sviZahtevi = SviZahtevi();
        List<string> zahtevi = new List<string>();

        foreach (ZahtevZaValidacijuLeka iterZahtev in sviZahtevi)
        {
            if (!iterZahtev.Lek.Sifra.Equals(zahtev.Lek.Sifra))
            {
                string linija = iterZahtev.Lek.Sifra + "#" + iterZahtev.Lek.Ime + "#" + iterZahtev.Lek.Opis + "#";
                if (iterZahtev.Lek.PotrebanRecept) { linija += "1#"; } else { linija += "0#"; }
                foreach (Sastojak sastojak in iterZahtev.Lek.Alergeni)
                {
                    linija += sastojak.Ime + ",";
                }
                linija.Remove(linija.LastIndexOf(','), 1);
                linija += "#";
                foreach (Lekar lekar in iterZahtev.lekariKomeIdeNaValidaciju)
                {
                    linija += lekar.Jmbg + "-";
                }
                linija.Remove(linija.LastIndexOf('-'), 1);
                zahtevi.Add(linija);
            }
        }
        File.WriteAllLines(fileLocation, zahtevi);
    }

}