using IS_Bolnice.Model;

public class OdgovorNaZahtevZaValidaciju: Entitet
{
    public string Obrazlozenje { get; set; }

    public Lek Lek { get; set; }

    public OdgovorNaZahtevZaValidaciju(string idLeka):base(idLeka)
    {
    }

    public OdgovorNaZahtevZaValidaciju(Lek lek, string obrazlozenje):base(lek.Id)
    {
        Lek = lek;
        Obrazlozenje = obrazlozenje;
    }

}