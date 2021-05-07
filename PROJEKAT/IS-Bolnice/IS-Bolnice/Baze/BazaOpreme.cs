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
                    if (delovi[3].Equals("0"))
                    {
                        p.Tip = TipOpreme.staticka;
                    }
                    else
                    {
                        p.Tip = TipOpreme.dinamicka;
                    }
                    if (delovi[2] == "False")
                    {
                        p.Obrisano = false;
                    }
                    else
                    {
                        p.Obrisano = true;
                    }

                    ret.Add(p);
                }
            }
            else
            {

                MessageBox.Show("Nista");
            }
            return ret;
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

        public void KreirajOpremu(List<Predmet> predmeti)
        {
            if (File.Exists(fileLocation))
            {
                List<string> tekst = new List<string>();
                string oprema;
                foreach (Predmet p in predmeti)
                {
                    oprema = p.Id + "#" + p.Naziv + "#" + p.Obrisano + "#";
                    if (p.Tip == TipOpreme.staticka)
                    {
                        oprema = oprema + "0#";
                    }
                    else
                    {
                        oprema = oprema + "1#";
                    }
                    tekst.Add(oprema);
                }

                File.WriteAllLines(fileLocation, tekst);
            }
            else
            {

                MessageBox.Show("Nista");
            }
        }

        public void IzmeniOpremu(Predmet predmet)
        {
            throw new NotImplementedException();
        }

        public void ObrisiOpremu(Predmet predmet)
        {
            throw new NotImplementedException();
        }


   public string fileLocation = @"..\..\Datoteke\oprema.txt";

}