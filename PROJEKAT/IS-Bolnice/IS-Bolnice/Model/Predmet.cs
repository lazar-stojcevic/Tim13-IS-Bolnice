using IS_Bolnice.Model;

public class Predmet : Entitet
{
    public Predmet(string id):base(id)
    {}
  
    public TipOpreme Tip { get; set; }

    public string Naziv { get; set; }

    public bool Obrisano { get; set; }
}