using System;
using System.Collections.Generic;
using System.IO;

public class BazaIzvestaja
{
   public List<Izvestaj> SviIzvestaji()
   {
      throw new NotImplementedException();
   }
   
   public void KreirajIzvestaj(string izvestaj)
   {
        File.AppendAllText(@"..\..\Datoteke\izvestaji.txt", izvestaj);
    }
   
   public void IzmeniIzvestaj(Izvestaj izvestaj)
   {
      throw new NotImplementedException();
   }
   
   public void ObrisiIzvestaj(Izvestaj izvestaj)
   {
      throw new NotImplementedException();
   }
   
   public string fileLocation;

}