using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using IS_Bolnice.Baze;

public class BazaLekara
{
    private BazaRadnogVremena bazaRadnogVremena = new BazaRadnogVremena();
    private static string vremenskiFormatPisanje = "M/d/yyyy h:mm:ss tt";
    private static string[] vremenskiFormatiCitanje = new[]
    {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
    };

    // metoda izlistava samo lekare opste prakse
    public List<Lekar> LekariOpstePrakse()
    {
        List<Lekar> LekariOP = new List<Lekar>();
        List<Lekar> sviLekari = SviLekari();

        foreach(Lekar l in sviLekari)
        {
            if(l.JelLekarOpstePrakse())
            {
                LekariOP.Add(l);
            }
        }

        return LekariOP;
    }

    // metoda izlistava samo lekare specijaliste
    public List<Lekar> LekariSpecijalisti()
    {
        List<Lekar> LekariSpecijalisti = new List<Lekar>();
        List<Lekar> sviLekari = SviLekari();

        foreach (Lekar l in sviLekari)
        {
            if (!l.JelLekarOpstePrakse())
            {
                LekariSpecijalisti.Add(l);
            }
        }

        return LekariSpecijalisti;
    }

    public List<Lekar> LekariOdredjeneOblasti(string trazenaOblast)
    {
        List<Lekar> lekariOdredjeneOblasti = new List<Lekar>();
        List<Lekar> sviLekari = SviLekari();

        foreach (Lekar lekar in sviLekari)
        {
            if (lekar.Oblast.Naziv.Equals(trazenaOblast))
            {
                lekariOdredjeneOblasti.Add(lekar);
            }
        }
        return lekariOdredjeneOblasti;
    }

    public List<Lekar> SviLekari()
    {
        List<Lekar> ret = new List<Lekar>();
        if (File.Exists(@"..\..\Datoteke\lekari.txt"))
        {
            string[] lines = File.ReadAllLines(@"..\..\Datoteke\lekari.txt");
            foreach (string line in lines)
            {
                Lekar p = new Lekar();
                string[] delovi = line.Split('#');
                p.Jmbg = delovi[0];
                p.Ime = delovi[1];
                p.Prezime = delovi[2];
                p.Oblast = new OblastLekara(delovi[3]);
                p.KorisnickoIme = delovi[4];
                p.Sifra = delovi[5];
                p.RadnoVreme = bazaRadnogVremena.RadnoVremeOdredjenogLekara(delovi[0]);
                p.Ordinacija = new Soba(delovi[6]);
                //TREBA DODATI ORDINACIJU
                ret.Add(p);
            }
        }
        else
        {
            Console.WriteLine("Nista");
        }
        return ret;
    }

    public Lekar DobaviLekara(string jmbgLekara)
    {
        foreach (Lekar lekar in SviLekari())
        {
            if (lekar.Jmbg.Equals(jmbgLekara))
            {
                return lekar;
            }
        }

        return null;
    }

    public void KreirajLekara(Lekar noviLekar)
   {
      throw new NotImplementedException();
   }
   
   public void ObrisiLekara(Lekar lekar)
   {
      throw new NotImplementedException();
   }
   
   public void IzmeniLekara(Lekar lekar)
   {
      throw new NotImplementedException();
   }
   
   public string fileLocation;

}