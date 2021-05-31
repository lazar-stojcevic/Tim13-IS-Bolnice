using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Baze.Klase;

public class IzvestajFajlRepozitorijum: GenerickiFajlRepozitorijum<Izvestaj>, IIzvestajRepozitorijum
{
    private static string formatPisanjaDatuma = "M/d/yyyy h:mm:ss tt";
    private static string[] formatCitanjaDatuma = new[]
    {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
    };

    public IzvestajFajlRepozitorijum() : base(@"..\..\Datoteke\izvestaji.txt")
    {
    }

    public List<Izvestaj> SviIzvestajiPacijenta(string jmbgPacijenta)
    {
        List<Izvestaj> izvestajiPacijenta = new List<Izvestaj>();

        foreach (Izvestaj izvestaj in DobaviSve())
        {
            if (izvestaj.Pacijent.Jmbg == jmbgPacijenta)
            {
                izvestajiPacijenta.Add(izvestaj);
            }
        }

        return izvestajiPacijenta;
    }

    public override Izvestaj KreirajEntitet(string[] podaciEntiteta)
    {
        LekarFajlRepozitorijum lekarRepo = new LekarFajlRepozitorijum();
        PacijentFajlRepozitorijum pacijentRepo = new PacijentFajlRepozitorijum();
        Lekar lekar = lekarRepo.DobaviPoId(podaciEntiteta[1]);
        Pacijent pacijent = pacijentRepo.DobaviPoJmbg(podaciEntiteta[2]);

        List<Terapija> terapije = NapraviTerapijeOdLinije(podaciEntiteta[5]);

        Izvestaj izvestaj = new Izvestaj(lekar, pacijent, podaciEntiteta[3], FormirajDatumZaCitanje(podaciEntiteta[4]));
        izvestaj.Terapija = terapije;
        izvestaj.Id = podaciEntiteta[0];
        return izvestaj;
    }

    public override string KreirajTextZaUpis(Izvestaj entitet)
    {
        string textIzvestaja;
        textIzvestaja = entitet.Id +"#" + entitet.Lekar.Jmbg + "#" + entitet.Pacijent.Jmbg + "#" + entitet.Opis + "#" +
                        DateTime.Now.Date + "#";
        foreach (Terapija terapija in entitet.Terapija)
        {
            textIzvestaja += KreirajTextJedneTerapije(terapija);
        }

        return textIzvestaja;
    }

    private static string KreirajTextJedneTerapije(Terapija terapija)
    {
        string textIzvestaja = "";
        textIzvestaja += terapija.Lek.Id + "$" + terapija.Lek.Ime + "$" + terapija.Lek.Opis + "$" +
                         terapija.RazlikaNaKolikoSeDanaUzimaLek + "$"
                         + terapija.UcestanostKonzumiranja + "$" + terapija.VremePocetka + "$" +
                         terapija.VremeKraja + "$" + terapija.Opis + "&";
        return textIzvestaja;
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

        lek.Id = podaci[0];
        lek.Ime = podaci[1];
        lek.Opis = podaci[2];

        return lek;
    }

    private DateTime FormirajDatumZaCitanje(string datum)
    {
        return DateTime.ParseExact(datum, formatCitanjaDatuma, CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
    }
}