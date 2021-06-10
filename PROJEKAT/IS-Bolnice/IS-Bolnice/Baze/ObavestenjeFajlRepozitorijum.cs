using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Baze.Klase;
using IS_Bolnice.Model;

public class ObavestenjeFajlRepozitorijum : GenerickiFajlRepozitorijum<Obavestenje>, IObavestenjaRepozitorijum
{
    IPacijentRepozitorijum pacijentRepo = new PacijentFajlRepozitorijum();
    private readonly string vremenskiFormatPisanje = "M/d/yyyy h:mm:ss tt";
    private readonly string[] vremenskiFormatiCitanje = new[]
    {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
    };
    private readonly string[] listSeparator = new string[] { ":@:" };

    public ObavestenjeFajlRepozitorijum() : base(@"..\..\Datoteke\obavestenja.txt")
    {
    }

    public override Obavestenje KreirajEntitet(string[] podaciEntiteta)
    {
        Obavestenje o = new Obavestenje();
        o.Id = podaciEntiteta[0];
        o.Naslov = podaciEntiteta[1];
        o.Sadrzaj = podaciEntiteta[2];
        o.VremeKreiranja = DateTime.ParseExact(podaciEntiteta[3], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        o.Uloge = SveUlogeObavestenja(podaciEntiteta[4]);
        o.OdredjeniPacijenti = SviPacijentiObavestenja(podaciEntiteta[5]);

        return o;
    }

    public override string KreirajTextZaUpis(Obavestenje obavestenje)
    {
        string o = obavestenje.Id + "#" + obavestenje.Naslov + "#" + obavestenje.Sadrzaj + "#"
                   + obavestenje.VremeKreiranja.ToString(vremenskiFormatPisanje) + "#";

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

        o += "#";
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

    public List<Obavestenje> SvaObavestenjaLekara()
    {
        List<Obavestenje> svaObavestenjaLekara = new List<Obavestenje>();

        foreach (Obavestenje obavestenje in GetSve())
        {
            if (obavestenje.Uloge.Contains(Uloge.Lekari))
            {
                svaObavestenjaLekara.Add(obavestenje);
            }
        }

        return svaObavestenjaLekara;
    }

    public List<Obavestenje> SvaObavestenjaPacijenata()
    {
        List<Obavestenje> svaObavestenjaPacijenata = new List<Obavestenje>();

        foreach (Obavestenje obavestenje in GetSve())
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
        List<Obavestenje> svaObavestenjaPacijenta = SvaObavestenjaPacijenata();

        foreach (Obavestenje obavestenje in GetSve())
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

    public List<Obavestenje> SvaObavestenjaSekretara()
    {
        List<Obavestenje> svaObavestenjaSekretara = new List<Obavestenje>();

        foreach (Obavestenje obavestenje in GetSve())
        {
            if (obavestenje.Uloge.Contains(Uloge.Sekretari))
            {
                svaObavestenjaSekretara.Add(obavestenje);
            }
        }

        return svaObavestenjaSekretara;
    }

    public List<Obavestenje> SvaObavestenjaUpravnika()
    {
        List<Obavestenje> svaObavestenjaUpravnika = new List<Obavestenje>();

        foreach (Obavestenje obavestenje in GetSve())
        {
            if (obavestenje.Uloge.Contains(Uloge.Upravnici))
            {
                svaObavestenjaUpravnika.Add(obavestenje);
            }
        }

        return svaObavestenjaUpravnika;
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
        List<Pacijent> sviPacijentiObavestenja = new List<Pacijent>();

        string[] odredjeniPacijenti = delovi.Split(listSeparator, StringSplitOptions.None);
        if (!odredjeniPacijenti[0].Equals(""))
        {
            foreach (string pacijentJMBG in odredjeniPacijenti)
            {
                sviPacijentiObavestenja.Add(pacijentRepo.GetPoJmbg(pacijentJMBG));
            }
        }
        return sviPacijentiObavestenja;
    }
}