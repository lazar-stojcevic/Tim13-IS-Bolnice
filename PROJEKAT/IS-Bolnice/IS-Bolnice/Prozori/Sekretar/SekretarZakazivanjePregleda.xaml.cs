using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Kontroleri.Korisnicki;
using IS_Bolnice.Kontroleri.Termini;

namespace IS_Bolnice.Prozori.Sekretar
{
    public partial class SekretarZakazivanjePregleda : Window
    {
        private PregledKontroler pregledKontroler = new PregledKontroler();
        private LekarKontroler lekarKontroler = new LekarKontroler();
        private List<Lekar> lekari;
        private Pacijent pacijent;

        public ObservableCollection<Pregled> PreglediLekara
        {
            get;
            set;
        }

        public SekretarZakazivanjePregleda(Pacijent p)
        {
            InitializeComponent();

            this.DataContext = this;
            lekari = lekarKontroler.GetSviLekari();
            PreglediLekara = new ObservableCollection<Pregled>();
            pacijent = p;
            PopunjavanjePoljaPrikaza();
        }

        private void OsvezavanjeListeSlobodnihPregledaPoTerminu()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                PreglediLekara.Clear();
                foreach (var pregled in pregledKontroler.GetSlobodniPreglediLekaraOpstePrakseUNarednomPeriodu())
                {
                    PreglediLekara.Add(pregled);
                }
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }
        private void PopunjavanjePoljaPrikaza()
        {
            txtIme.Text = pacijent.Ime;
            txtPrezime.Text = pacijent.Prezime;
            txtJmbg.Text = pacijent.Jmbg;

            List<string> lekariString = new List<string>();

            // formiranje stringa za svakog lekara
            foreach (Lekar l in lekari)
            {
                string lekarString = l.Ime + " " + l.Prezime + " (" + l.Oblast.Naziv + ")";
                lekariString.Add(lekarString);
            }

            comboLekari.ItemsSource = lekariString;
            comboLekari.SelectedIndex = -1;

            // inicijalno dugme za novi termin nije dostupno dok se ne odabere lekar
            odabirTermina.IsEnabled = false;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Lekar izabraniLekar = pacijent.IzabraniLekar;
            if (izabraniLekar != null)
            {
                int indeks = 0;
                foreach (Lekar l in lekari)
                {
                    if (l.Jmbg.Equals(izabraniLekar.Jmbg))
                    {
                        break;
                    }
                    indeks++;
                }
                comboLekari.SelectedIndex = indeks;
            }
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            int index = prikazTermina.SelectedIndex;
            if (index != -1)
            {
                Pregled pregled = PreglediLekara[index];
                pregled.Pacijent = pacijent;
                if (pregledKontroler.ZakaziPregled(pregled))
                {
                    InformativniProzor ip = new InformativniProzor("Uspešno zakazan pregled.");
                    ip.ShowDialog();
                    this.Close();
                }
                else
                {
                    InformativniProzor ip = new InformativniProzor("Termin ne može da se zakaže.");
                    ip.ShowDialog();
                }
            }
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void comboLekari_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            odabirTermina.IsEnabled = true;

            Lekar lekar = lekari[comboLekari.SelectedIndex];
            List<Pregled> pregledi = pregledKontroler.GetSviBuduciPreglediLekara(lekar.Jmbg);
            PreglediLekara.Clear();

            // provera za checkbox da li treba da ostane oznacen
            if (pacijent.IzabraniLekar == null)
            {
                checkBoxLekar.IsChecked = false;
            }
            else if (checkBoxLekar.IsChecked == true && !lekar.Jmbg.Equals(pacijent.IzabraniLekar.Jmbg))
            {
                checkBoxLekar.IsChecked = false;
            }

            if ((bool) rbLekar.IsChecked)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    foreach (var pregled in pregledKontroler.GetDostupniTerminiPregledaLekaraUNarednomPeriodu(lekar))
                    {
                        PreglediLekara.Add(pregled);
                    }
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            }
        }

        private void Button_Click_Novi_Termin(object sender, RoutedEventArgs e)
        {
            // na prvom mestu je pocetak termina a na drugom kraj termina
            List<DateTime> termin = new List<DateTime>();
            Lekar lekar = lekari[comboLekari.SelectedIndex];
            OdredjivanjeTermina ot = new OdredjivanjeTermina(termin, lekar);
            ot.ShowDialog();

            // unutar liste termin moze da se nalazi samo pocetak i samo kraj termina
            if (termin.Count == 2)
            {
                Pregled p = new Pregled
                {
                    VremePocetkaPregleda = termin[0],
                    VremeKrajaPregleda = termin[1],
                    Lekar = lekar
                };
                PreglediLekara.Clear();
                PreglediLekara.Add(p);
                prikazTermina.SelectedValue = PreglediLekara.Count - 1;
            }
        }

        private void RadioButton_Termin_Checked(object sender, RoutedEventArgs e)
        {
            checkBoxLekar.IsEnabled = false;
            comboLekari.IsEnabled = false;
            comboLekari.SelectedIndex = -1;
            if (PreglediLekara != null)
            {
                OsvezavanjeListeSlobodnihPregledaPoTerminu();
            }
        }

        private void RadioButton_Lekar_Checked(object sender, RoutedEventArgs e)
        {
            PreglediLekara.Clear();
            checkBoxLekar.IsEnabled = true;
            comboLekari.IsEnabled = true;
        }
    }
}
