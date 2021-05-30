using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using IS_Bolnice.Baze.Interfejsi;

public class PacijentFajlRepozitorijum : IPacijentRepozitorijum
{
    private static string fileLocation = @"..\..\Datoteke\pacijenti.txt";
    private static string vremenskiFormatPisanje = "M/d/yyyy h:mm:ss tt";
    private static string[] vremenskiFormatiCitanje = new[]
    {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
    };

    public Pacijent DobaviPoJmbg(string jmbg)
    {
        foreach (Pacijent p in DobaviSve())
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

    public List<Pacijent> DobaviSve()
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

    public void Izmeni(Pacijent noviEntitet)
    {
        List<string> pacijentiString = new List<string>();

        foreach (Pacijent p in DobaviSve())
        {
            if (noviEntitet.Jmbg.Equals(p.Jmbg))
            {
                pacijentiString.Add(PacijentToString(noviEntitet));
            }
            else
            {
                pacijentiString.Add(PacijentToString(p));
            }
        }

        File.WriteAllLines(fileLocation, pacijentiString);
    }

    public void Obrisi(string jmbg)
    {
        List<string> pacijentiString = new List<string>();

        foreach (Pacijent p in DobaviSve())
        {
            if (p.Jmbg.Equals(jmbg))
            {
                p.Obrisan = true;
            }
            pacijentiString.Add(PacijentToString(p));
        }

        File.WriteAllLines(fileLocation, pacijentiString);
    }

    public void Sacuvaj(Pacijent noviEntitet)
    {
        // lista se koristi samo zato sto je to potrebno za metodu AppendAllLines
        List<string> pLista = new List<string>();
        pLista.Add(PacijentToString(noviEntitet));
        File.AppendAllLines(fileLocation, pLista);
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
}