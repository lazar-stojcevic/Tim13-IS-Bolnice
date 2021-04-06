using System;
using System.Collections.Generic;

public class Pacijent : Korisnik
{
    public Lekar IzabraniLekar { get; set; }
    public bool Guest { get; set; }
    public List<string> Alergeni { get; set; }

    public Pacijent() 
    {
        Alergeni = new List<string>();    
    }
}