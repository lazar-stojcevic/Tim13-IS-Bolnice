using System;
using System.Collections.Generic;

public class Pacijent : Korisnik
{
    private Lekar izabraniLekar;
    private bool guest = false;
    private List<string> alergeni = new List<string>();
    
    public Pacijent() { }

    public Lekar IzabraniLekar { get; set; }
    public bool Guest { get; set; }
    public List<string> Alergeni { get; set; }
}