using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

public class BazaSastojaka
{
    private static string fileLocation = @"..\..\Datoteke\sastojci.txt";

    public List<Sastojak> SviSastojci()
   {
        List<Sastojak> ret = new List<Sastojak>();
        if (File.Exists(fileLocation))
        {
            string[] lines = File.ReadAllLines(fileLocation);
            foreach (string line in lines)
            {
                Sastojak sastojak = new Sastojak(line);
                ret.Add(sastojak);
            }
        }
        else
        {

            MessageBox.Show("Nista");
        }
        return ret;
    }
   
   public void KreirajSastojak(Sastojak sastojak)
   {
        if (File.Exists(fileLocation))
        {
            List<string> nazivSastojka = new List<string>();
            nazivSastojka.Add(sastojak.Ime);
            File.AppendAllLines(fileLocation, nazivSastojka);
        }
        else
        {

            MessageBox.Show("Nista");
        }
    }
   
   public void IzmeniSastojak(Sastojak sastojakIzmenjeno)
   {
      throw new NotImplementedException();
   }
   
   public void ObrisiSastojak(Sastojak sastojak)
   {
      throw new NotImplementedException();
   }

}