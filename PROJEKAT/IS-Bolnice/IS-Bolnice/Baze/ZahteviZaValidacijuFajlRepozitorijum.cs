// File:    ZahteviZaValidacijuFajlRepozitorijum.cs
// Author:  Zola
// Created: Monday, May 3, 2021 9:32:33 PM
// Purpose: Definition of Class ZahteviZaValidacijuFajlRepozitorijum

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Baze.Klase;

public class ZahteviZaValidacijuFajlRepozitorijum : GenerickiFajlRepozitorijum<ZahtevZaValidacijuLeka>, IZahteviZaValidacijuRepozitorijum
{
    public ZahteviZaValidacijuFajlRepozitorijum():base(@"..\..\Datoteke\zahteviLekovi.txt")
    { }

    public override ZahtevZaValidacijuLeka KreirajEntitet(string[] podaciEntiteta)
    {
        
        Lek p = new Lek();
        p.Id = podaciEntiteta[0];
        p.Ime = podaciEntiteta[1];
        p.Opis = podaciEntiteta[2];

        Console.WriteLine(p.Opis + "     " + p.Opis);

        p.PotrebanRecept = PostaviDaLiJePotrebanRecept(podaciEntiteta[3]);

        p.Alergeni = DobaviAlergeneLeka(podaciEntiteta[4]);

        p.ZamenskiLekovi = DobaviZamenskeLekove(podaciEntiteta[5]);

        ZahtevZaValidacijuLeka zahtev = new ZahtevZaValidacijuLeka(p);

        string[] idLekara = podaciEntiteta[6].Split('-');
        foreach (Lekar lekarIter in new LekarFajlRepozitorijum().DobaviSve())
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

        return zahtev;
    }

    public override string KreirajTextZaUpis(ZahtevZaValidacijuLeka entitet)
    {
        string novaLinija = entitet.Lek.Id + "#" + entitet.Lek.Ime + "#" + entitet.Lek.Opis + "#";
        if (entitet.Lek.PotrebanRecept)
        {
            novaLinija += "1#";
        }
        else novaLinija += "0#";


        novaLinija = UbaciSastojkeLeka(entitet, novaLinija);

        novaLinija = UbaciZamenskeLekove(entitet, novaLinija);

        novaLinija = DodajLekareKomeLekIdenaValidaciju(entitet, novaLinija);

        return novaLinija;
    }

    private List<Lek> DobaviZamenskeLekove(string podaciEntiteta)
    {
        List<Lek> zamenskiLekovi = new List<Lek>();
        if (!podaciEntiteta.Equals(""))
        {
            string[] zameskiLek = podaciEntiteta.Split('/');
            foreach (string deo in zameskiLek)
            {
                Lek lek = new Lek(deo);
                foreach (Lek lekIter in new LekFajlRepozitorijum().DobaviSve())
                {
                    if (deo.Equals(lekIter.Id))
                    {
                        lek.Ime = lekIter.Ime;
                    }
                }

                zamenskiLekovi.Add(lek);
            }
        }

        return zamenskiLekovi;
    }

    private List<Sastojak> DobaviAlergeneLeka(string podaciEntiteta)
    {
        List<Sastojak> listaAlergena = new List<Sastojak>();
        if (!podaciEntiteta.Equals(""))
        {
            string[] alergen = podaciEntiteta.Split(',');
            foreach (string a in alergen)
            {
                if (a.Equals(""))
                {
                    break;
                }

                if (!a.Equals(""))
                {
                    Sastojak s = new Sastojak(a);
                    listaAlergena.Add(s);
                }
            }
        }

        return listaAlergena;
    }

    private static bool PostaviDaLiJePotrebanRecept(string podaciEntiteta)
    {
        if (podaciEntiteta.Equals("1"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private static string DodajLekareKomeLekIdenaValidaciju(ZahtevZaValidacijuLeka entitet, string novaLinija)
    {
        novaLinija = novaLinija + "#";
        foreach (Lekar lekar in entitet.lekariKomeIdeNaValidaciju)
        {
            novaLinija = novaLinija + lekar.Jmbg + "-";
        }

        novaLinija = novaLinija.Remove(novaLinija.Length - 1);
        return novaLinija;
    }

    private static string UbaciZamenskeLekove(ZahtevZaValidacijuLeka entitet, string novaLinija)
    {
        novaLinija = novaLinija + "#";
        if (entitet.Lek.ZamenskiLekovi.Count != 0)
        {
            foreach (Lek zamenskiLek in entitet.Lek.ZamenskiLekovi)
            {
                novaLinija = novaLinija + zamenskiLek.Id + "/";
            }

            novaLinija = novaLinija.Remove(novaLinija.Length - 1);
        }

        return novaLinija;
    }

    private static string UbaciSastojkeLeka(ZahtevZaValidacijuLeka entitet, string novaLinija)
    {
        if (entitet.Lek.Alergeni.Count != 0)
        {
            foreach (Sastojak sastojak in entitet.Lek.Alergeni)
            {
                novaLinija += sastojak.Ime + ",";
            }
        }
        else
        {
            novaLinija += "nema,";
        }

        novaLinija = novaLinija.Remove(novaLinija.Length - 1);
        return novaLinija;
    }

    public List<ZahtevZaValidacijuLeka> DobaviZahteveZaValidacijuZaLekara(string idLekara)
    {
        List<ZahtevZaValidacijuLeka> povratnaVrednost = new List<ZahtevZaValidacijuLeka>();

        foreach (ZahtevZaValidacijuLeka zahtev in DobaviSve())
        {
            foreach (Lekar lekar in zahtev.lekariKomeIdeNaValidaciju)
            {
                if (lekar.Jmbg.Equals(idLekara))
                {
                    povratnaVrednost.Add(zahtev);
                    break;
                }
            }
        }

        return povratnaVrednost;
    }
}