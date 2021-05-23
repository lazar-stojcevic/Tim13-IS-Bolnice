using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace IS_Bolnice.Prozori.Sekretar
{
    /// <summary>
    /// Interaction logic for SekretarZakazivanjeOperacije.xaml
    /// </summary>
    public partial class SekretarZakazivanjeOperacije : Window
    {
        private BazaLekara bazaLekara = new BazaLekara();
        private BazaBolnica bazaBolnica = new BazaBolnica();
        private BazaOperacija bazaOperacija = new BazaOperacija();
        private List<Lekar> lekari;
        private List<Soba> sobe;
        private Pacijent pacijent;
        private Operacija operacija;


        public SekretarZakazivanjeOperacije(Pacijent p)
        {
            InitializeComponent();

            lekari = bazaLekara.LekariSpecijalisti();
            sobe = bazaBolnica.SveOperacioneSaleOveBolnice();
            operacija = new Operacija();

            pacijent = p;
            operacija.Pacijent = p;
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

            List<string> sobeString = new List<string>();
            // formiranje stringa za svaku sobu
            foreach (Soba s in sobe)
            {
                string sobaString = s.Id + " " + s.Tip.ToString();
                sobeString.Add(sobaString);
            }
            comboSale.ItemsSource = sobeString;
            comboSale.SelectedIndex = -1;

            // popunjavanje ponudjenih trajanja
            List<double> trajanja = new List<double>();
            for (double i = 0.5; i <= 24; i += 0.5)
            {
                trajanja.Add(i);
            }
            comboTrajanja.ItemsSource = trajanja;
            comboTrajanja.IsEnabled = false;

            // inicijalno dugme za novi termin nije dostupno dok se ne odabere lekar
            odabirTermina.IsEnabled = false;
        }

        private void comboLekari_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            odabirTermina.IsEnabled = true;

            Lekar lekar = lekari[comboLekari.SelectedIndex];
            operacija.Lekar = lekar;
        }

        private void comboSala_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Soba sala = sobe[comboSale.SelectedIndex];
            operacija.Soba = sala;
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            if (comboLekari.SelectedIndex != -1 && comboSale.SelectedIndex != -1)
            {
                bazaOperacija.ZakaziOperaciju(operacija);
                this.Close();
            }
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_Odaberi_Termin(object sender, RoutedEventArgs e)
        {
            // na prvom mestu je pocetak termina a na drugom kraj termina
            List<DateTime> termin = new List<DateTime>();
            Lekar lekar = lekari[comboLekari.SelectedIndex];
            OdredjivanjeTermina ot = new OdredjivanjeTermina(termin, lekar);
            ot.ShowDialog();

            // unutar liste termin moze da se nalazi samo pocetak i samo kraj termina
            if (termin.Count == 2)
            {
                operacija.VremePocetkaOperacije = termin[0];
                operacija.VremeKrajaOperacije = termin[1];
                comboTrajanja.IsEnabled = true;
                txtTermin.Text = termin[0].ToString();
            }
        }

        private void comboTrajanje_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            double trajanje = (double)comboTrajanja.SelectedItem;
            operacija.VremeKrajaOperacije = operacija.VremePocetkaOperacije.AddHours(trajanje);
        }
    }
}
