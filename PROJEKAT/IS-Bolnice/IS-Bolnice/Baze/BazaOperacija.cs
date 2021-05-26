using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class BazaOperacija
{
    public string fileLocation;
    private static string vremenskiFormatPisanje = "M/d/yyyy h:mm:ss tt";
    private static string[] vremenskiFormatiCitanje = new[]
    {
        "M/d/yyyy h:mm:ss tt",
        "M-d-yyyy h:mm:ss tt"
    };
    
    public List<Operacija> SveOperacijeUOdabranojSali(string idSale)
    {
        List<Operacija> sveOperacije = SveSledeceOperacije();
        List<Operacija> operacijeUOdabranojSali = new List<Operacija>();
        foreach (Operacija operacija in sveOperacije)
        {
            if (operacija.Soba.Id.Equals(idSale))
            {
                operacijeUOdabranojSali.Add(operacija);
            }
        }
        return operacijeUOdabranojSali;
    }


    public List<Operacija> SveSledeceOperacijeDatogLekara(string jmbgLekara)
    {
        List<Operacija> povratnaVrednost = new List<Operacija>();
        List<Operacija> sveNaredneOperacijeSvihLekara = SveSledeceOperacije();
        foreach (Operacija iterOperacija in sveNaredneOperacijeSvihLekara)
        {
            if (iterOperacija.Lekar.Jmbg.Equals(jmbgLekara)){
                povratnaVrednost.Add(iterOperacija);
            }
        }
        return povratnaVrednost;
    }


    public List<Operacija> SveOperacije()
    {
        List<Operacija> ret = new List<Operacija>();
        BazaPacijenata bazaPacijenata = new BazaPacijenata();
        BazaBolnica bazaBolnica = new BazaBolnica();
        BazaLekara bazaLekara = new BazaLekara();
        List<Bolnica> bolnice = bazaBolnica.SveBolnice();
        List<Pacijent> pacijenti = bazaPacijenata.SviPacijenti();
        List<Lekar> lekari = bazaLekara.SviLekari();
        if (File.Exists(@"..\..\Datoteke\operacije.txt"))
        {
            string[] lines = File.ReadAllLines(@"..\..\Datoteke\operacije.txt");
            foreach (string line in lines)
            {
                Operacija o = new Operacija();
                string[] delovi = line.Split('#');
                o.VremePocetkaOperacije = DateTime.ParseExact(delovi[0], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                                                  DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                o.VremeKrajaOperacije = DateTime.ParseExact(delovi[1], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                                                  DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                o.Pacijent.Jmbg = delovi[2];
                foreach (Pacijent p in pacijenti)
                {
                    if (o.Pacijent.Jmbg.Equals(p.Jmbg))
                    {
                        o.Pacijent = p;
                        break;
                    }
                }

                foreach (Bolnica bolnica in bolnice)
                {
                    foreach (Soba s in bolnica.Soba)
                    {
                        if (delovi[4].Equals(s.Id))
                        {
                            o.Soba = s;
                            break;
                        }
                    }
                }
                o.Lekar.Jmbg = delovi[3];
                foreach (Lekar lekar in lekari)
                {
                    if (delovi[3].Equals(lekar.Jmbg))
                    {
                        o.Lekar = lekar;
                        break;
                    }
                }
                o.Soba.Id = delovi[4];

                if (delovi[5].Equals("True"))
                {
                    o.Hitna = true;
                }
                else o.Hitna = false;


                ret.Add(o);
            }
        }
        else
        {
            Console.WriteLine("Nista");
        }
        return ret;
    }

    public List<Operacija> SveSledeceOperacije()
    {
        List<Operacija> ret = new List<Operacija>();
        foreach (Operacija operacija in SveOperacije())
        {
            if (operacija.VremePocetkaOperacije > DateTime.Now.AddHours(-1))
            {
                ret.Add(operacija);
            }
        }

        return ret;
    }

    public void ZakaziOperaciju(Operacija novaOperacija)
    {
        string linija = novaOperacija.VremePocetkaOperacije.ToString(vremenskiFormatPisanje) + "#" + novaOperacija.VremeKrajaOperacije.ToString(vremenskiFormatPisanje) + "#" +
            novaOperacija.Pacijent.Jmbg + "#" + novaOperacija.Lekar.Jmbg + "#" + novaOperacija.Soba.Id + "#" + novaOperacija.Hitna + System.Environment.NewLine;
        File.AppendAllText(@"..\..\Datoteke\operacije.txt", linija);
    }

    public void OtkaziOperaciju(Operacija operacija)
   {
        string path = @"..\..\Datoteke\operacije.txt";

        List<Operacija> operacije = SveSledeceOperacije();
        List<string> lines = new List<string>();

        foreach (Operacija o in operacije)
        {
            if (!(o.Pacijent.Jmbg.Equals(operacija.Pacijent.Jmbg) && o.Soba.Id.Equals(operacija.Soba.Id) && o.VremePocetkaOperacije.Equals(operacija.VremePocetkaOperacije)))
            {
                string zakazivanje = o.VremePocetkaOperacije.ToString(vremenskiFormatPisanje) + "#" + o.VremeKrajaOperacije.ToString(vremenskiFormatPisanje) + "#" +
                    o.Pacijent.Jmbg + "#" + o.Lekar.Jmbg + "#" + o.Soba.Id + "#" + o.Hitna;
                lines.Add(zakazivanje);
            }

        }

        File.WriteAllLines(path, lines);
    }


    public void IzmeniOperaciju(Operacija novaOperacija, Operacija staraOperacija)
   {
        OtkaziOperaciju(staraOperacija);
        ZakaziOperaciju(novaOperacija);
   }

    public List<Operacija> SveSledeceOperacijeZaLekara(string jmbgLekara)
    {
        List<Operacija> sledeceOperacije = new List<Operacija>();

        foreach (Operacija operacija in SveSledeceOperacije())
        {
            if (operacija.Lekar.Jmbg.Equals(jmbgLekara) && operacija.VremeKrajaOperacije > DateTime.Now)
            {
                sledeceOperacije.Add(operacija);
            }
        }
        return sledeceOperacije;
    }
}