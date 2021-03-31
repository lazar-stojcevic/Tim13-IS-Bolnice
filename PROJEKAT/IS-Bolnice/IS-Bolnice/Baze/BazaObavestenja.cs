using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class BazaObavestenja
{
    private static string fileLocation = @"..\..\Datoteke\obavestenja.txt";

    public List<Obavestenje> SvaObavestenja()
    {
        List<string> linije = File.ReadAllLines(fileLocation).ToList();
        return NapraviObavestenja(linije);
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

        // po tome ce se splitovati string
        string[] stringSeparators = new string[] { "#!^" };

        foreach (string linija in linije)
        {
            string[] delovi = linija.Split(stringSeparators, StringSplitOptions.None);

            Obavestenje o = new Obavestenje();
            o.Sifra = delovi[0];
            o.Naslov = delovi[1];
            o.Sadrzaj = delovi[2];
            o.VremeKreiranja = DateTime.Parse(delovi[3]);

            obavestenja.Add(o);
        }
        return obavestenja;
    }

    private string ObavestenjeToString(Obavestenje obavestenje)
    {
        // polja unutar txt datoteke se razdvajaju sa "#!^" kako bi bilo skoro nemoguce da se u sadrzaju pojavi ta kombinacija
        string o = obavestenje.Sifra + "#!^" + obavestenje.Naslov + "#!^" + obavestenje.Sadrzaj + "#!^" + obavestenje.VremeKreiranja;
        return o;
    }
}