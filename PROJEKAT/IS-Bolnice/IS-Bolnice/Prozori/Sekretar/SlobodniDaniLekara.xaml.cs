using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using IS_Bolnice.Repozitorijumi;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Kontroleri.Korisnicki;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Prozori.Sekretar
{
    public partial class SlobodniDaniLekara : Window
    {
        private Lekar selektovaniLekar;
        private RadnoVremeKontroler radnoVremeKontroler = new RadnoVremeKontroler();

        public ObservableCollection<DateTime> PrikazSlobodnihDanaLekara
        {
            get;
            set;
        }

        public SlobodniDaniLekara(Lekar lekar)
        {
            InitializeComponent();
            this.DataContext = this;

            selektovaniLekar = lekar;
            tbLekar.Text = lekar.Ime + " " + lekar.Prezime;
            labelPreostaliDani.Content = lekar.RadnoVreme.PreostaliSlobodniDaniUGodini.ToString();

            PrikazSlobodnihDanaLekara = new ObservableCollection<DateTime>();
            OsvezavanjePrikazaSlobodnihDana();
        }

        private void OsvezavanjePrikazaSlobodnihDana()
        {
            List<DateTime> slobodniDani = selektovaniLekar.RadnoVreme.SlobodniDani;
            slobodniDani.Sort();
            PrikazSlobodnihDanaLekara.Clear();
            foreach (DateTime datum in slobodniDani)
            {
                PrikazSlobodnihDanaLekara.Add(datum);
            }
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool ValidnoUnesenDatum(DateTime pocetak, DateTime kraj)
        {
            DateTime sada = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            return (pocetak <= kraj) && pocetak >= sada;
        }

        private void Button_Click_Kreiraj_Godisnji(object sender, RoutedEventArgs e)
        {
            DateTime pocetak = DateTime.Parse(datePicker_pocetak.SelectedDate.ToString());
            DateTime kraj = DateTime.Parse(datePicker_kraj.SelectedDate.ToString());
            if (!ValidnoUnesenDatum(pocetak, kraj))
            {
                InformativniProzor ip1 = new InformativniProzor("Nevalidan unos datuma godišnjeg odmora.");
                ip1.ShowDialog();
                return;
            }

            TimeSpan intervalGodisnjegOdmora = kraj.Subtract(pocetak);
            int daniGodisnjegOdmora = intervalGodisnjegOdmora.Days;
            if (daniGodisnjegOdmora > selektovaniLekar.RadnoVreme.PreostaliSlobodniDaniUGodini)
            {
                InformativniProzor ip2 = new InformativniProzor("Nema dovoljno preostalih slobodnih dana.");
                ip2.ShowDialog();
                return;
            }

            List<DateTime> potencijalniSlobodniDani = new List<DateTime>();
            for (int i = 0; i <= daniGodisnjegOdmora; i++)
            {
                potencijalniSlobodniDani.Add(pocetak.AddDays(i));
            }

            if (radnoVremeKontroler.PreklapanjeIntervalaGodisnjegOdmoraLekara(potencijalniSlobodniDani, selektovaniLekar.Jmbg))
            {
                InformativniProzor ip3 = new InformativniProzor("Zadati itnerval se već preklapa sa postojećim slobodnim danima.");
                ip3.ShowDialog();
                return;
            }

            if (radnoVremeKontroler.PreklapanjeIntervalaGodisnjegOdmoraSaObavezamaLekara(potencijalniSlobodniDani,
                selektovaniLekar.Jmbg))
            {
                InformativniProzor ip4 = new InformativniProzor("Lekar poseduje obaveze u zadatom itnervalu godišnjeg odmora.");
                ip4.ShowDialog();
                return;
            }

            foreach (var dan in potencijalniSlobodniDani)
            {
                selektovaniLekar.RadnoVreme.SlobodniDani.Add(dan);
            }
            selektovaniLekar.RadnoVreme.PreostaliSlobodniDaniUGodini -= radnoVremeKontroler.PreracunajBrojIskoriscenihSlobodnihDanaLekara(selektovaniLekar.Jmbg, potencijalniSlobodniDani);
            radnoVremeKontroler.IzmeniRadnoVreme(selektovaniLekar.RadnoVreme);
            InformativniProzor ip5 = new InformativniProzor("Uspešno kreiran godišnji odmor!");
            ip5.ShowDialog();
            OsvezavanjePrikazaSlobodnihDana();
            labelPreostaliDani.Content = selektovaniLekar.RadnoVreme.PreostaliSlobodniDaniUGodini;
        }
    }
}
