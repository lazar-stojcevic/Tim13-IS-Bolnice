using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using IS_Bolnice.Baze;
using IS_Bolnice.Model;

namespace IS_Bolnice.Prozori.Sekretar
{
    public partial class RadnoVremeLekara : Window
    {
        private Lekar selektovaniLekar;
        private readonly int INTERVAL_MINUTA_RADNOG_VREMENA = 10;
        private bool napravljenaIzmena = false;
        private BazaRadnogVremena bazaRadnogVremena = new BazaRadnogVremena();

        public RadnoVremeLekara(Lekar lekar)
        {
            InitializeComponent();

            selektovaniLekar = lekar;
            tbLekar.Text = lekar.Ime + " " + lekar.Prezime;

            PopuniComboBoxSati();
            PopuniComboBoxMinuta();
            PodesavanjeRedovnogRadnogVremena();
            PodesavanjeComboBoxevaSlobodnihDana();
        }

        private void PodesavanjeRedovnogRadnogVremena()
        {
            DateTime pocetak = selektovaniLekar.RadnoVreme.StandardnoRadnoVreme.Pocetak;
            DateTime kraj = selektovaniLekar.RadnoVreme.StandardnoRadnoVreme.Kraj;

            int satiPocetkaRadnogVremena = pocetak.Hour;
            cbSatiPocetak.SelectedValue = satiPocetkaRadnogVremena;

            int minutiPocetkaRadnogVremena = pocetak.Minute;
            if (minutiPocetkaRadnogVremena % INTERVAL_MINUTA_RADNOG_VREMENA == 0)
            {
                cbMinutiPocetak.SelectedValue = minutiPocetkaRadnogVremena;
            }

            TimeSpan interval = kraj.Subtract(pocetak);
            int trajanjeRadnogVremena = interval.Hours;
            cbSatiTrajanje.SelectedValue = trajanjeRadnogVremena;
        }

        private void PodesavanjeComboBoxevaSlobodnihDana()
        {
            foreach (DayOfWeek slobodanDan in selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji)
            {
                switch (slobodanDan)
                {
                    case DayOfWeek.Monday:
                        boxPonedeljak.IsChecked = true;
                        break;
                    case DayOfWeek.Tuesday:
                        boxUtorak.IsChecked = true;
                        break;
                    case DayOfWeek.Wednesday:
                        boxSreda.IsChecked = true;
                        break;
                    case DayOfWeek.Thursday:
                        boxCetvrtak.IsChecked = true;
                        break;
                    case DayOfWeek.Friday:
                        boxPetak.IsChecked = true;
                        break;
                    case DayOfWeek.Saturday:
                        boxSubota.IsChecked = true;
                        break;
                    case DayOfWeek.Sunday:
                        boxNedelja.IsChecked = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private void PopuniComboBoxSati()
        {
            List<int> trajanja = new List<int>();
            for (int i = 0; i <= 24; i += 1)
            {
                trajanja.Add(i);
            }

            cbSatiPocetak.ItemsSource = trajanja.GetRange(0, 24);
            cbSatiTrajanje.ItemsSource = trajanja.GetRange(1, 24);
        }

        private void PopuniComboBoxMinuta()
        {
            List<int> trajanja = new List<int>();
            for (int i = 0; i <= 60; i += 10)
            {
                trajanja.Add(i);
            }

            cbMinutiPocetak.ItemsSource = trajanja;
            cbMinutiPocetak.SelectedIndex = 0;
        }

        private void Button_Click_Slobodni_Dani(object sender, RoutedEventArgs e)
        {
            SlobodniDaniLekara sdl = new SlobodniDaniLekara(selektovaniLekar);
            sdl.ShowDialog();
        }

        private void Button_Click_Vanredni_Rad(object sender, RoutedEventArgs e)
        {
            VanredniRadLekara vrl = new VanredniRadLekara(selektovaniLekar);
            vrl.ShowDialog();
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            if (napravljenaIzmena)
            {
                AzurirajRadnoVremeLekara();
            }

            AzurirajSlobodneDaneLekara();
            bazaRadnogVremena.IzmeniRadnoVreme(selektovaniLekar.RadnoVreme);
            Close();
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AzurirajRadnoVremeLekara()
        {
            int satiPocetkaRadnogVremena = (int) cbSatiPocetak.SelectedValue;
            int minutiPocetkaRadnogVremena = (int) cbMinutiPocetak.SelectedValue;
            int satiTrajanjaRadnogVremena = (int) cbSatiTrajanje.SelectedValue;

            DateTime pocetakRadnogVremena = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                satiPocetkaRadnogVremena, minutiPocetkaRadnogVremena, 0, 0);
            DateTime krajRadnogVremena = pocetakRadnogVremena.AddHours(satiTrajanjaRadnogVremena);

            VremenskiInterval radnoVreme = new VremenskiInterval(pocetakRadnogVremena, krajRadnogVremena);
            selektovaniLekar.RadnoVreme.StandardnoRadnoVreme = radnoVreme;
        }

        private void AzurirajSlobodneDaneLekara()
        {
            if ((bool) boxPonedeljak.IsChecked)
            {
                if (!selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Contains(DayOfWeek.Monday))
                    selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Add(DayOfWeek.Monday);
            }
            else
            {
                selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Remove(DayOfWeek.Monday);
            }

            if ((bool) boxUtorak.IsChecked)
            {
                if (!selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Contains(DayOfWeek.Tuesday))
                    selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Add(DayOfWeek.Tuesday);
            }
            else
            {
                selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Remove(DayOfWeek.Tuesday);
            }

            if ((bool) boxSreda.IsChecked)
            {
                if (!selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Contains(DayOfWeek.Wednesday))
                    selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Add(DayOfWeek.Wednesday);
            }
            else
            {
                selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Remove(DayOfWeek.Wednesday);
            }

            if ((bool) boxCetvrtak.IsChecked)
            {
                if (!selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Contains(DayOfWeek.Thursday))
                    selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Add(DayOfWeek.Thursday);
            }
            else
            {
                selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Remove(DayOfWeek.Thursday);
            }

            if ((bool) boxPetak.IsChecked)
            {
                if (!selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Contains(DayOfWeek.Friday))
                    selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Add(DayOfWeek.Friday);
            }
            else
            {
                selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Remove(DayOfWeek.Friday);
            }

            if ((bool) boxSubota.IsChecked)
            {
                if (!selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Contains(DayOfWeek.Saturday))
                    selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Add(DayOfWeek.Saturday);
            }
            else
            {
                selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Remove(DayOfWeek.Saturday);
            }

            if ((bool) boxNedelja.IsChecked)
            {
                if (!selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Contains(DayOfWeek.Sunday))
                    selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Add(DayOfWeek.Sunday);
            }
            else
            {
                selektovaniLekar.RadnoVreme.SlobodniDaniUNedelji.Remove(DayOfWeek.Sunday);
            }
        }

        private void cbSatiPocetak_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            napravljenaIzmena = true;
        }

        private void cbMinutiPocetak_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            napravljenaIzmena = true;
        }

        private void cbSatiTrajanje_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            napravljenaIzmena = true;
        }
    }
}
