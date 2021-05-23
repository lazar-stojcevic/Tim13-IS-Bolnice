using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class BazaPacijenata
{
    private static string fileLocation = @"..\..\Datoteke\pacijenti.txt";
    private static string vremenskiFormatPisanje = "M/d/yyyy h:mm:ss tt";
    private static string[] vremenskiFormatiCitanje = new[]
    {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
    };

    public List<Pacijent> SviPacijenti()
    {
        List<string> linije = File.ReadAllLines(fileLocation).ToList();

        // odabir i vracanje samo onih pacijenata koji nisu logicki obrisani
        List<Pacijent> sviPacijenti = NapraviPacijente(linije);
        List<Pacijent> aktuelniPacijenti = new List<Pacijent>();

        foreach (Pacijent p in sviPacijenti)
        {
            if (p.Obrisan == false)
            {
                aktuelniPacijenti.Add(p);
            }
        }
        return aktuelniPacijenti;
    }

    public Pacijent poslednjiDodat()
    {
        List<Pacijent> pacijenti = SviPacijenti();
        int index = pacijenti.Count() - 1;

        if (index != -1)
        {
            return pacijenti[index];
        }
        return null;
    }

    public Pacijent PacijentSaOvimJMBG(string jmbg)
    {
        foreach (Pacijent p in SviPacijenti())
        {
            if (p.Obrisan == false)
            {
                if (p.Jmbg.Equals(jmbg))
                {
                    return p;
                }
            }
        }
        return null;
    }

    public void KreirajPacijenta(Pacijent pacijent)
    {
        // lista se koristi samo zato sto je to potrebno za metodu AppendAllLines
        List<string> pLista = new List<string>();
        pLista.Add(PacijentToString(pacijent));
        File.AppendAllLines(fileLocation, pLista);
    }
   
    public void ObrisiPacijenta(Pacijent pacijent)
    {
        List<string> linije = new List<string>();
        linije = File.ReadAllLines(fileLocation).ToList();

        List<Pacijent> pacijenti = new List<Pacijent>();
        pacijenti = NapraviPacijente(linije);

        List<string> pacijentiString = new List<string>();

        foreach (Pacijent p in pacijenti)
        {
            if (pacijent.Jmbg.Equals(p.Jmbg))
            {
                p.Obrisan = true;
            }
            pacijentiString.Add(PacijentToString(p));
        }

        File.WriteAllLines(fileLocation, pacijentiString);
    }
   
    public void IzmeniPacijenta(Pacijent pacijentIzmenjen, Pacijent pacijentPocetni)
     {
        List<string> linije = new List<string>();
        linije = File.ReadAllLines(fileLocation).ToList();

        List<Pacijent> pacijenti = new List<Pacijent>();
        pacijenti = NapraviPacijente(linije);

        List<string> pacijentiString = new List<string>();

        foreach(Pacijent p in pacijenti)
        {
            if (pacijentPocetni.Jmbg.Equals(p.Jmbg))
            {
                pacijentiString.Add(PacijentToString(pacijentIzmenjen));
            }
            else
            {
                pacijentiString.Add(PacijentToString(p));
            }
        }

        File.WriteAllLines(fileLocation, pacijentiString);
     }

    private string PacijentToString(Pacijent pacijent)
    {
        string p = pacijent.Jmbg + "#" + pacijent.KorisnickoIme + "#" + pacijent.Sifra + "#" + pacijent.Ime + "#" +
            pacijent.Prezime + "#" + pacijent.BrojTelefona + "#" + pacijent.EMail + "#" + pacijent.Adresa + "#" +
            pacijent.Pol.ToString() + "#" + pacijent.Obrisan + "#" + pacijent.DatumRodjenja.ToString(vremenskiFormatPisanje) + "#";

        // upisivanje liste alergena
        foreach (Sastojak s in pacijent.Alergeni)
        {
            p += s.Ime + ",";
        }
        p = p.TrimEnd(',');

        // upisivanje izabranog lekara ako postoji
        if (pacijent.IzabraniLekar != null)
        {
            p += "#" + pacijent.IzabraniLekar.Jmbg;
        }

        return p;
    }

    private List<Pacijent> NapraviPacijente(List<string> linije)
    {
        List<Pacijent> pacijenti = new List<Pacijent>();

        foreach (string linija in linije)
        {
            string[] delovi = linija.Split('#');

            Pacijent p = new Pacijent();
            p.Jmbg = delovi[0];
            p.KorisnickoIme = delovi[1];
            p.Sifra = delovi[2];
            p.Ime = delovi[3];
            p.Prezime = delovi[4];
            p.BrojTelefona = delovi[5];
            p.EMail = delovi[6];
            p.Adresa = delovi[7];
            if (delovi[8].Equals("muski"))
            {
                p.Pol = Pol.muski;
            }
            else if (delovi[8].Equals("zenski"))
            {
                p.Pol = Pol.zenski;
            }
            else
            {
                p.Pol = Pol.drugo;
            }
            p.Obrisan = Boolean.Parse(delovi[9]);
            p.DatumRodjenja = DateTime.ParseExact(delovi[10], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                                                  DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

            // lista alergena
            string[] alergeni = delovi[11].Split(',');
            foreach (string a in alergeni)
            {
                if (!a.Equals(""))
                {
                    p.Alergeni.Add(new Sastojak(a));
                }
            }

            // ako postoji i izabrani lekar (bez provere bi puklo ukoliko korisnik nema izabranog lekara)
            if (delovi.Length > 12)
            {
                BazaLekara bl = new BazaLekara();
                List<Lekar> lekari = bl.LekariOpstePrakse();
                foreach (Lekar l in lekari)
                {
                    if (l.Jmbg.Equals(delovi[12]))
                    {
                        p.IzabraniLekar = l;
                        break;
                    }
                }
            } 

            pacijenti.Add(p);
        }
        return pacijenti;
    }

    public bool JedinstvenoKorisnickoIme(string korisnickoIme)
    {
        foreach (Pacijent p in SviPacijenti())
        {
            if (p.KorisnickoIme.Equals(korisnickoIme))
            {
                return false;
            }
        }

        return true;
    }

    public bool JedinstvenJmbgPacijenta(string jmbg)
    {
        foreach (Pacijent p in SviPacijenti())
        {
            if (p.Jmbg.Equals(jmbg))
            {
                return false;
            }
        }

        return true;
    }

}