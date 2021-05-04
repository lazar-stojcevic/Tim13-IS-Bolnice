// File:    BazaOdgovoraNaZahteveZaValidacijuLekova.cs
// Author:  Zola
// Created: Monday, May 3, 2021 9:32:38 PM
// Purpose: Definition of Class BazaOdgovoraNaZahteveZaValidacijuLekova

using System;
using System.Collections.Generic;
using System.IO;

public class BazaOdgovoraNaZahteveZaValidacijuLekova
{
   private String fileLocation = @"..\..\Datoteke\odgovorLekovi.txt";
   
   public List<OdgovorNaZahtevZaValidaciju> SviOdgovoriNaZahteve()
   {
      throw new NotImplementedException();
   }
   
   public void KreirajOdgovorNaZahtev(OdgovorNaZahtevZaValidaciju odgovor, ZahtevZaValidacijuLeka zahtev)
   {
        string novaLinija = zahtev.Lek.Sifra + "#" + zahtev.Lek.Ime + "#" + zahtev.Lek.Opis + "#" + zahtev.Lek.PotrebanRecept + "#";
        if (zahtev.Lek.Alergeni.Count != 0)
        {
            foreach (Sastojak sastojak in zahtev.Lek.Alergeni)
            {
                novaLinija += sastojak.Ime + ",";
            }
        }

        novaLinija += "#";

        //JOVANA DODAJ KAD UBACIS ZAMENSKE LEKOVI
        File.AppendAllText(fileLocation, novaLinija);
   }
   
   public void IzmeniOdgovorNaZahtev(OdgovorNaZahtevZaValidaciju odgovorIzmenjen)
   {
      throw new NotImplementedException();
   }
   
   public void ObrisiOdgovorNaZahtev(OdgovorNaZahtevZaValidaciju odgovor)
   {
      throw new NotImplementedException();
   }

}