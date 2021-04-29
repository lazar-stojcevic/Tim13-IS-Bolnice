using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class BazaObavestenja
{
    private readonly string fileLocation = @"..\..\Datoteke\obavestenja.txt";
    private readonly string vremenskiFormatPisanje = "M/d/yyyy h:mm:ss tt";
    private readonly string[] vremenskiFormatiCitanje = new[]
    {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
    };
    // po tome ce se splitovati string
    private readonly string[] lineSeparator = new string[] { "#!^" };
    private readonly string[] listSeparator = new string[] { ":@:" };

    public List<Obavestenje> SvaObavestenja()
    {
        List<string> linije = File.ReadAllLines(fileLocation).ToList();
        return NapraviObavestenja(linije);
    }

    public List<Obavestenje> SvaObavestenjaLekara()
    {
        List<Obavestenje> svaObavestenja = SvaObavestenja();
        List<Obavestenje> svaObavestenjaLekara = new List<Obavestenje>();

        foreach (Obavestenje obavestenje in svaObavestenja)
        {
            if (obavestenje.Uloge.Contains(Uloge.Lekari))
            {
                svaObavestenjaLekara.Add(obavestenje);
            }
        }

        return svaObavestenjaLekara;
    }

    public List<Obavestenje> SvaObavestenjaUpravnika()
    {
        List<Obavestenje> svaObavestenja = SvaObavestenja();
        List<Obavestenje> svaObavestenjaUpravnika = new List<Obavestenje>();

        foreach (Obavestenje obavestenje in svaObavestenja)
        {
            if (obavestenje.Uloge.Contains(Uloge.Upravnici))
            {
                svaObavestenjaUpravnika.Add(obavestenje);
            }
        }

        return svaObavestenjaUpravnika;
    }

    public List<Obavestenje> SvaObavestenjaSekretara()
    {
        List<Obavestenje> svaObavestenja = SvaObavestenja();
        List<Obavestenje> svaObavestenjaSekretara = new List<Obavestenje>();

        foreach (Obavestenje obavestenje in svaObavestenja)
        {
            if (obavestenje.Uloge.Contains(Uloge.Sekretari))
            {
                svaObavestenjaSekretara.Add(obavestenje);
            }
        }

        return svaObavestenjaSekretara;
    }

    public List<Obavestenje> SvaObavestenjaPacijenata()
    {
        List<Obavestenje> svaObavestenja = SvaObavestenja();
        List<Obavestenje> svaObavestenjaPacijenata = new List<Obavestenje>();

        foreach (Obavestenje obavestenje in svaObavestenja)
        {
            if (obavestenje.Uloge.Contains(Uloge.Pacijenti))
            {
                svaObavestenjaPacijenata.Add(obavestenje);
            }
        }

        return svaObavestenjaPacijenata;
    }

    public List<Obavestenje> SvaObavestenjaPacijenta(Pacijent pacijent)
    {
        List<Obavestenje> svaObavestenja = SvaObavestenja();
        List<Obavestenje> svaObavestenjaPacijenta = SvaObavestenjaPacijenata();

        foreach (Obavestenje obavestenje in svaObavestenja)
        {
            foreach (Pacijent p in obavestenje.OdredjeniPacijenti)
            {
                if (p.Jmbg.Equals(pacijent.Jmbg))
                {
                    svaObavestenjaPacijenta.Add(obavestenje);
                    break;
                }
            }
        }

        return svaObavestenjaPacijenta;
    }
   
    public void KreirajObavestenje(Obavestenje obavestenje)
    {
        // lista se koristi samo zato sto je to potrebno za metodu AppendAllLines
        List<string> pLista = new List<string>();
        pLista.Add(ObavestenjeToString(obavestenje));
        File.AppendAllLines(fileLocation, pLista);
    }
   
    public void IzmeniObavestenje(Obavestenje obavestenjeIzmenjeno)
    {
        List<string> linije = File.ReadAllLines(fileLocation).ToList();
        List<Obavestenje> obavestenja = NapraviObavestenja(linije);

        List<string> obavestenjaString = new List<string>();

        foreach (Obavestenje o in obavestenja)
        {
            if (obavestenjeIzmenjeno.Sifra.Equals(o.Sifra))
            {
                obavestenjaString.Add(ObavestenjeToString(obavestenjeIzmenjeno));
            }
            else
            {
                obavestenjaString.Add(ObavestenjeToString(o));
            }
        }

        File.WriteAllLines(fileLocation, obavestenjaString);
    }
   
    public void ObrisiObavestenje(Obavestenje obavestenje)
    {
        List<string> linije = File.ReadAllLines(fileLocation).ToList();
        List<Obavestenje> obavestenja = NapraviObavestenja(linije);

        List<string> obavestenjaString = new List<string>();

        foreach (Obavestenje o in obavestenja)
        {
            if (obavestenje.Sifra.Equals(o.Sifra))
            {
                // samo se preskoci upisivanje
            }
            else
            {
                obavestenjaString.Add(ObavestenjeToString(o));
            }
        }

        File.WriteAllLines(fileLocation, obavestenjaString);
    }

    private List<Obavestenje> NapraviObavestenja(List<string> linije)
    {
        List<Obavestenje> obavestenja = new List<Obavestenje>();

        foreach (string linija in linije)
        {
            string[] delovi = linija.Split(lineSeparator, StringSplitOptions.None);

            Obavestenje o = new Obavestenje();
            o.Sifra = delovi[0];
            o.Naslov = delovi[1];
            o.Sadrzaj = delovi[2];
            o.VremeKreiranja = DateTime.ParseExact(delovi[3], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                                                  DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            o.Uloge = SveUlogeObavestenja(delovi[4]);
            o.OdredjeniPacijenti = SviPacijentiObavestenja(delovi[5]);

            obavestenja.Add(o);
        }
        return obavestenja;
    }

    private List<Uloge> SveUlogeObavestenja(string delovi)
    {
        List<Uloge> sveUloge = new List<Uloge>();
        string[] uloge = delovi.Split(listSeparator, StringSplitOptions.None);
        if (!uloge[0].Equals(""))
        {
            foreach (string u in uloge)
            {
                Uloge uloga = (Uloge)Enum.Parse(typeof(Uloge), u);
                sveUloge.Add(uloga);
            }
        }   
        return sveUloge;
    }

    private List<Pacijent> SviPacijentiObavestenja(string delovi)
    {
        BazaPacijenata bp = new BazaPacijenata();
        List<Pacijent> sviPacijentiObavestenja = new List<Pacijent>();

        string[] odredjeniPacijenti = delovi.Split(listSeparator, StringSplitOptions.None);
        if (!odredjeniPacijenti[0].Equals(""))
        {
            foreach (string pacijentJMBG in odredjeniPacijenti)
            {
                sviPacijentiObavestenja.Add(bp.PacijentSaOvimJMBG(pacijentJMBG));
            }
        }
        return sviPacijentiObavestenja;
    }

    private string ObavestenjeToString(Obavestenje obavestenje)
    {
        // polja unutar txt datoteke se razdvajaju sa "#!^" kako bi bilo skoro nemoguce da se u sadrzaju pojavi ta kombinacija
        string o = obavestenje.Sifra + "#!^" + obavestenje.Naslov + "#!^" + obavestenje.Sadrzaj + "#!^" 
            + obavestenje.VremeKreiranja.ToString(vremenskiFormatPisanje) + "#!^";

        // sadrzaj liste se razdvaja sa ":@:" kako bi bilo skoro nemoguce da se u sadrzaju pojavi ta kombinacija
        foreach (Uloge uloga in obavestenje.Uloge)
        {
            o += uloga.ToString() + ":@:";
        }
        if (obavestenje.Uloge.Count != 0)
        {
            // brisanje poslednja 3 karaktera
            o = o.Remove(o.Length - 3, 3);
        }
        
        o += "#!^";
        foreach (Pacijent pacijent in obavestenje.OdredjeniPacijenti)
        {
            o += pacijent.Jmbg + ":@:";
        }
        if (obavestenje.OdredjeniPacijenti.Count != 0)
        {
            o = o.Remove(o.Length - 3, 3);
        }

        return o;
    }
}