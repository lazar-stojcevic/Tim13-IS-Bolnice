// File:    BazaHospitalizacija.cs
// Author:  Zola
// Created: Monday, May 17, 2021 7:14:56 PM
// Purpose: Definition of Class BazaHospitalizacija

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Repozitorijumi
{
   public class BazaHospitalizacija
   {
      private String fileLocation = @"..\..\Datoteke\hospitalizacija.txt";
      private static string vremenskiFormatPisanje = "M/d/yyyy";
      private static string[] vremenskiFormatiCitanje = new[]
      {
          "M/d/yyyy",
          "M-d-yyyy"
      };

        public List<Hospitalizacija> SveHospitalizacije()
      {
          List<Hospitalizacija> ret = new List<Hospitalizacija>();
          if (File.Exists(@"..\..\Datoteke\hospitalizacija.txt"))
          {
              string[] lines = File.ReadAllLines(@"..\..\Datoteke\hospitalizacija.txt");
              foreach (string line in lines)
              {
                  Hospitalizacija hospitalizacija = new Hospitalizacija();
                  string[] delovi = line.Split('#');
                  hospitalizacija.PocetakHospitalizacije = DateTime.ParseExact(delovi[0], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                      DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                  hospitalizacija.KrajHospitalizacije = DateTime.ParseExact(delovi[1], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                      DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);


                  BazaBolnica bazaBolnica = new BazaBolnica();
                  BazaPacijenata bazaPacijenata = new BazaPacijenata();
                  hospitalizacija.Soba = bazaBolnica.GetSobaById(delovi[2]);
                  hospitalizacija.Pacijent = bazaPacijenata.PacijentSaOvimJMBG(delovi[3]);

                  ret.Add(hospitalizacija);
              }
          }
          return ret;
        }
      
      public bool KreirajHospitalizaciju(Hospitalizacija hospitalizacija)
      {
          List<string> linije = new List<string>();
          string novaLinija = hospitalizacija.PocetakHospitalizacije.ToString(vremenskiFormatPisanje) + "#" + hospitalizacija.KrajHospitalizacije.ToString(vremenskiFormatPisanje) + "#" + hospitalizacija.Soba.Id + "#" + hospitalizacija.Pacijent.Jmbg;
          linije.Add(novaLinija);
          List<Hospitalizacija> sveHospitalizacije = SveHospitalizacije();
          foreach (Hospitalizacija hostIter in sveHospitalizacije)
          {
              if (hostIter.Pacijent.Jmbg.Equals(hospitalizacija.Pacijent.Jmbg) &&
                  hostIter.KrajHospitalizacije > DateTime.Now)
              {
                  return false;
              }
          }
          File.AppendAllLines(fileLocation, linije);
          return true;
      }
      
      public void IzmeniHospitalizaciju(Hospitalizacija hospitalizacijaIzmenjeno)
      {
         throw new NotImplementedException();
      }
      
      public void ObrisiHospitalizaciju(Hospitalizacija hospitalizacija)
      {
          List<Hospitalizacija> sveHospitalizacije = SveHospitalizacije();
          List<string> listaStringova = new List<string>();

          foreach (Hospitalizacija hospIter in sveHospitalizacije)
          {
              if (!hospitalizacija.Pacijent.Jmbg.Equals(hospIter.Pacijent.Jmbg) && hospIter.KrajHospitalizacije > DateTime.Now)
              {
                  string novaLinija = hospIter.PocetakHospitalizacije.ToString(vremenskiFormatPisanje) + "#" + hospIter.KrajHospitalizacije.ToString(vremenskiFormatPisanje) + "#" + hospIter.Soba.Id + "#" + hospIter.Pacijent.Jmbg;
                    listaStringova.Add(novaLinija);
              }
          }
          File.WriteAllLines(fileLocation, listaStringova);
      }
   
   }
}