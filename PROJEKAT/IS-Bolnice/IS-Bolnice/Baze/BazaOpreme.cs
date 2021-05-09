// File:    BazaOpreme.cs
// Author:  teddy
// Created: Monday, April 12, 2021 6:08:22 PM
// Purpose: Definition of Class BazaOpreme

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

public class BazaOpreme
{
        public List<Predmet> SvaOprema()
        {
            List<Predmet> ret = new List<Predmet>();
            if (File.Exists(fileLocation))
            {
                string[] lines = File.ReadAllLines(fileLocation);
                foreach (string line in lines)
                {
                    Predmet p = new Predmet();
                    string[] delovi = line.Split('#');
                    p.Id = delovi[0];
                    p.Naziv = delovi[1];
                    p.Tip = ParseStringToTip(delovi[3]);
                    p.Obrisano = ParseStringToBool(delovi[2]);
                    ret.Add(p);
                }
            }
            else
            {

                MessageBox.Show("Nista");
            }
            return ret;
        }

    public TipOpreme ParseStringToTip(string tekst) {
        if (tekst.Equals("0"))
        {
            return TipOpreme.staticka;
        }
        else
        {
            return TipOpreme.dinamicka;
        }
    }

    public bool ParseStringToBool(string tekst)
    {
        if (tekst.Equals("False"))
        { 
            return false;
        }
        else
        {
            return true;
        }
    }

        public Predmet GetPredmet(string idOpreme)
        {

            List<Predmet> predmeti = SvaOprema();
            foreach (Predmet p in predmeti)
            {
                if (p.Id.Equals(idOpreme))
                {
                    return p;
                }
            }
            Predmet predmet = new Predmet();
            return predmet;
        }

        public void KreirajOpremu(Predmet predmet)
        {
            if (File.Exists(fileLocation))
            {
                List<string> tekst = new List<string>();
                string oprema = ParseToString(predmet);
                tekst.Add(oprema);
                File.AppendAllLines(fileLocation, tekst);
            }
            else
            {

                MessageBox.Show("Nista");
            }
        }

        public string ParseToString(Predmet predmet) {
            string oprema = predmet.Id + "#" + predmet.Naziv + "#" + predmet.Obrisano + "#";
            if (predmet.Tip == TipOpreme.staticka)
            {
                oprema = oprema + "0#";
            }
            else
            {
                oprema = oprema + "1#";
            }
            return oprema;
        }

        public void IzmeniOpremu(Predmet predmet)
        {
            ObrisiOpremu(predmet);
            KreirajOpremu(predmet);
        }

        public void ObrisiOpremu(Predmet predmet)
        {
            List<Predmet> svaOprema = SvaOprema();
            List<string> tekst = new List<string>();
            foreach (Predmet oprema in svaOprema) {
                if (!oprema.Id.Equals(predmet.Id)) {
                    string linija = ParseToString(oprema);
                    tekst.Add(linija);
                }
            }
            File.WriteAllLines(fileLocation, tekst);
        }


   public string fileLocation = @"..\..\Datoteke\oprema.txt";

}