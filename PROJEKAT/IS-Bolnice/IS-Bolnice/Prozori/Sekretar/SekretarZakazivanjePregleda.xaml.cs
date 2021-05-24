using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using IS_Bolnice.Kontroleri;

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
                pregledKontroler.ZakaziPregled(pregled);
                this.Close();
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
            if (checkBoxLekar.IsChecked == true && !lekar.Jmbg.Equals(pacijent.IzabraniLekar.Jmbg))
            {
                checkBoxLekar.IsChecked = false;
            }

            int i = 0;
            DateTime trenutnoVreme = DateTime.Now;
            while (i < 5)
            {
                Pregled pregled = new Pregled();
                pregled.VremePocetkaPregleda = trenutnoVreme.AddMinutes(5);
                pregled.VremeKrajaPregleda = pregled.VremePocetkaPregleda.AddMinutes(45);   // pretpostavka da termin traje 45min
                pregled.Lekar = lekar;
                trenutnoVreme = pregled.VremeKrajaPregleda; // potencijalno sledeci slobodan termin je 5min nakon zavrsetka ovog termina
                //TODO: provera da li se termin vec preklapa sa nekim terminom i koje je radno vreme lekara
                i++;
                PreglediLekara.Add(pregled);
            }
        }

        private void RadioButton_Termin_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Lekar_Checked(object sender, RoutedEventArgs e)
        {

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
                PreglediLekara.Add(p);
            }
        }
    }
}
