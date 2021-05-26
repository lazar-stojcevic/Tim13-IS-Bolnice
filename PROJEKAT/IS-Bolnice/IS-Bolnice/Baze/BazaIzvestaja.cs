using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class BazaIzvestaja
{
    private static string fileLocation = @"..\..\Datoteke\izvestaji.txt";
    private static string formatPisanjaDatuma = "M/d/yyyy h:mm:ss tt";
    private static string[] formatCitanjaDatuma = new[]
    {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
    };
    public List<Izvestaj> SviIzvestaji()
    {
        List<Izvestaj> izvestaji = new List<Izvestaj>();

        foreach (string linija in ProcitajDatoteku())
        {
            string[] deloviLinije = linija.Split('#');
            izvestaji.Add(FormirajIzvestajOdLinije(deloviLinije));
        }

        return izvestaji;
    }

    public void KreirajIzvestaj(string izvestaj)
    {
        File.AppendAllText(@"..\..\Datoteke\izvestaji.txt", izvestaj);
    }

    public void IzmeniIzvestaj(Izvestaj izvestaj)
    {
        throw new NotImplementedException();
    }

    public void ObrisiIzvestaj(Izvestaj izvestaj)
    {
        throw new NotImplementedException();
    }

    private Izvestaj FormirajIzvestajOdLinije(string[] linija)
    {
        Lekar lekar = new Lekar();
        lekar.Jmbg = linija[0];
        Pacijent pacijent = new Pacijent();
        pacijent.Jmbg = linija[1];

        List<Terapija> terapije = NapraviTerapijeOdLinije(linija[4]);

        Izvestaj izvestaj = new Izvestaj(lekar, pacijent, linija[2], FormirajDatumZaCitanje(linija[3]));
        izvestaj.Terapija = terapije;

        return izvestaj;
    }

    private List<Terapija> NapraviTerapijeOdLinije(string linija)
    {
        List<Terapija> terapije = new List<Terapija>();

        foreach (string deoLinije in linija.Split('&'))
        {
            if (deoLinije == "") break;

            Terapija terapija = NapraviTerapiju(deoLinije);
            terapije.Add(terapija);
        }

        return terapije;
    }

    private Terapija NapraviTerapiju(string deoLinije)
    {
        string[] podatak = deoLinije.Split('$');

        Terapija terapija = new Terapija();

        Lek lek = NapraviLek(podatak);

        terapija.Lek = lek;
        terapija.RazlikaNaKolikoSeDanaUzimaLek = Int32.Parse(podatak[3]);
        terapija.UcestanostKonzumiranja = Double.Parse(podatak[4]);
        terapija.VremePocetka = FormirajDatumZaCitanje(podatak[5]);
        terapija.VremeKraja = FormirajDatumZaCitanje(podatak[6]);
        terapija.Opis = podatak[7];

        return terapija;
    }

    private Lek NapraviLek(string[] podaci)
    {
        Lek lek = new Lek();

        lek.Sifra = podaci[0];
        lek.Ime = podaci[1];
        lek.Opis = podaci[2];

        return lek;
    }

    private List<string> ProcitajDatoteku()
    {
        return File.ReadAllLines(fileLocation).ToList();
    }

    private DateTime FormirajDatumZaCitanje(string datum)
    {
        return DateTime.ParseExact(datum, formatCitanjaDatuma, CultureInfo.InvariantCulture,
                                              DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
    }
}