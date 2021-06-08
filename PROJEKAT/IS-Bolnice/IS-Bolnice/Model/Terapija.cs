using System;
using System.ComponentModel;

public class Terapija : INotifyPropertyChanged
{
   private Double ucestanostKonzumiranja;
   private DateTime vremePocetka;
   private DateTime vremeKraja;
   private int daniIzmedjuKonzumiranja;
   private Lek lek;
   private string opis;

   public Terapija()
   {
       lek = new Lek();
   }

    public Double UcestanostKonzumiranja
    {
        get { return ucestanostKonzumiranja; }
        set
        {
            if (ucestanostKonzumiranja == value) return;
            ucestanostKonzumiranja = value;
            RaisePropertyChanged("ucestanostKonzumiranja");
        }
    }

    public DateTime VremePocetka
    {
        get { return vremePocetka; }
        set
        {
            if (vremePocetka == value) return;
            vremePocetka = value;
            RaisePropertyChanged("vremePocetka");
        }
    }

    public DateTime VremeKraja
    {
        get { return vremeKraja; }
        set
        {
            if (vremeKraja == value) return;
            vremeKraja = value;
            RaisePropertyChanged("vremeKraja");
        }
    }

    public int RazlikaNaKolikoSeDanaUzimaLek
    {
        get { return daniIzmedjuKonzumiranja; }
        set
        {
            if (daniIzmedjuKonzumiranja == value) return;
            daniIzmedjuKonzumiranja = value;
            RaisePropertyChanged("daniIzmedjuKonzumiranja");
        }
    }

    public Lek Lek
    {
        get { return lek; }
        set
        {
            if (lek == value) return;
            lek = value;
            RaisePropertyChanged("Lek");
        }
    }

    public string Opis
    {
        get { return opis; }
        set
        {
            if (opis == value) return;
            opis = value;
            RaisePropertyChanged("opis");
        }
    }

   

    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string propName)
    {
        PropertyChangedEventHandler eh = PropertyChanged;
        if (eh != null)
        {
            eh(this, new PropertyChangedEventArgs(propName));
        }
    }

}