// File:    HospitalizacijaFajlRepozitorijum.cs
// Author:  Zola
// Created: Monday, May 17, 2021 7:14:56 PM
// Purpose: Definition of Class HospitalizacijaFajlRepozitorijum

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Baze.Klase;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Baze
{
   public class HospitalizacijaFajlRepozitorijum:GenerickiFajlRepozitorijum<Hospitalizacija>, IHospitalizacijaRepozitorijum
   {
      private String fileLocation = @"..\..\Datoteke\hospitalizacija.txt";
      private static string vremenskiFormatPisanje = "M/d/yyyy";
      private static string[] vremenskiFormatiCitanje = new[]
      {
          "M/d/yyyy",
          "M-d-yyyy"
      };

      public HospitalizacijaFajlRepozitorijum():base(@"..\..\Datoteke\hospitalizacija.txt"){}
      public List<Hospitalizacija> GetSveHospitalizacijeZaSobu(string sobaID) {

            List<Hospitalizacija> hospitalizacije = new List<Hospitalizacija>();
            foreach (Hospitalizacija hospIter in GetSve()) {
                if (hospIter.Soba.Id.Equals(sobaID)) {
                    hospitalizacije.Add(hospIter);
                }
            }
            return hospitalizacije;
        }

      //OVO MORA DA SE KORISTI RADI LOGIKE OKO ZAUZIMANJA
      public bool KreirajHospitalizaciju(Hospitalizacija hospitalizacija)
      {
          List<string> linije = new List<string>();
          string novaLinija = hospitalizacija.PocetakHospitalizacije.ToString(vremenskiFormatPisanje) + "#" + hospitalizacija.KrajHospitalizacije.ToString(vremenskiFormatPisanje) + "#" + hospitalizacija.Soba.Id + "#" + hospitalizacija.Pacijent.Jmbg;
          linije.Add(novaLinija);
          List<Hospitalizacija> sveHospitalizacije = GetSve();
          int brojUSobi = 0;
          foreach (Hospitalizacija hostIter in sveHospitalizacije)
          {
              if (hostIter.Pacijent.Jmbg.Equals(hospitalizacija.Pacijent.Jmbg) &&
                  hostIter.KrajHospitalizacije > DateTime.Now)
              {
                  return false;
              }

              if (hospitalizacija.Soba.Id.Equals(hostIter.Soba.Id) && hostIter.KrajHospitalizacije > DateTime.Now)
              {
                  ++brojUSobi;
                  if (brojUSobi >= new SadrzajSobeServis().BrojKrevetaUSobi(hospitalizacija.Soba.Id))
                  {
                      return false;
                  }
              }
          }
          File.AppendAllLines(fileLocation, linije);
          return true;
      }

        public override Hospitalizacija KreirajEntitet(string[] delovi)
        {
            Hospitalizacija hospitalizacija = new Hospitalizacija();
            hospitalizacija.PocetakHospitalizacije = DateTime.ParseExact(delovi[0], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            hospitalizacija.KrajHospitalizacije = DateTime.ParseExact(delovi[1], vremenskiFormatiCitanje, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

            hospitalizacija.Soba = new BolnicaFajlRepozitorijum().GetSobaById(delovi[2]);
            hospitalizacija.Pacijent = new PacijentFajlRepozitorijum().GetPoJmbg(delovi[3]);
            hospitalizacija.Id = hospitalizacija.Pacijent.Id + "+" + hospitalizacija.Soba.Id;

            return hospitalizacija;
        }

        public override string KreirajTextZaUpis(Hospitalizacija hospitalizacija)
        {
            string novaLinija = hospitalizacija.PocetakHospitalizacije.ToString(vremenskiFormatPisanje) + "#" + hospitalizacija.KrajHospitalizacije.ToString(vremenskiFormatPisanje) + "#" + hospitalizacija.Soba.Id + "#" + hospitalizacija.Pacijent.Jmbg;
            return novaLinija;
        }
    }
}