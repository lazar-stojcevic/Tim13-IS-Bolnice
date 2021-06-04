using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IS_Bolnice.DTOs;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.ViewModel.Lekar;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    public partial class ZakazivanjeOperacije : Page
    {
        private string jmbgPac = "";
        public ZakazivanjeOperacije(string jmbgPacijenta)
        {
            InitializeComponent();
            jmbgPac = jmbgPacijenta;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ZakazivanjeOperacijeViewModel(this.NavigationService, jmbgPac);
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 1;
        }

        private void ToggleButton_OnUnchecked_UnChecked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 0;
        }
        /*
        private void Button_ClickZakazi(object sender, RoutedEventArgs e)
        {
            if (txtDuzina.Text.Equals(""))
            {
                MessageBox.Show("Niste uneli dužinu trajanje operacije", "Dužina trajanja operacije",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da ste dobro uneli sve vrednosti?", "Zakazivanje operacije", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Operacija operacija = KreirajNovuOperaciju();
                operacijaKontroler.ZakaziOperaciju(operacija);
                MessageBox.Show("Operacijacija uspešno kreirana", "Kreirana operacija", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                NavigationService.GoBack();
            }
        }

        private Operacija KreirajNovuOperaciju()
        {
            Operacija operacija = operacije.ElementAt(terminiList.SelectedIndex);
            Lekar lekar = (Lekar)listaLekara.SelectedItem;
            string idLekara = lekar.Jmbg;

            Soba soba = (Soba)comboBoxSale.SelectionBoxItem;
            string idSale = soba.Id;
            //TODO: OVAJ DEO MORA DA SE VALIDIRA ALI ZA SAD JE OK

            DateTime pocetak = new DateTime(operacija.VremePocetkaOperacije.Year, operacija.VremePocetkaOperacije.Month,
                operacija.VremePocetkaOperacije.Day, operacija.VremePocetkaOperacije.Hour,
                operacija.VremePocetkaOperacije.Minute, 0);
            DateTime kraj = new DateTime(operacija.VremePocetkaOperacije.Year, operacija.VremePocetkaOperacije.Month,
                operacija.VremePocetkaOperacije.Day, operacija.VremePocetkaOperacije.Hour, operacija.VremePocetkaOperacije.Minute, 0);
            kraj = kraj.AddMinutes(Int16.Parse(txtDuzina.Text));
            operacija.Lekar.Jmbg = idLekara;
            operacija.Pacijent.Jmbg = txtOperJmbg.Text;
            operacija.Soba.Id = idSale;
            operacija.VremePocetkaOperacije = pocetak;
            operacija.VremeKrajaOperacije = kraj;
            operacija.Hitna = (bool) boxHitno.IsChecked;
            operacija.Lekar.RadnoVreme = new RadnoVremeKontroler().DobaviRadnoVremeLekara(idLekara);
            return operacija;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da želite da odustanete od zakazivanje operacije?", "Zakazivanje operacije", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                NavigationService.GoBack();
            }
        }

        private void liste_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PostavljanjeParametara();
        }

        private void TxtDuzina_LostFocus(object sender, RoutedEventArgs e)
        {
            PostavljanjeParametara();
        }

        private void PostavljanjeParametara()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (listaLekara.SelectedIndex == -1 || txtDuzina.Text.Equals(""))
                {
                    return;
                }

                if (terminiList.SelectedIndex == -1 || listaLekara.SelectedIndex == -1 || txtDuzina.Text.Equals(""))
                {
                    potvrdi.IsEnabled = false;
                }
                else
                {
                    potvrdi.IsEnabled = true;
                }

                int trajanjeOperacije;
                try
                {
                    trajanjeOperacije = Int32.Parse(txtDuzina.Text);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Niste validno uneli dužinu trajanja operacije", "Dužina trajanja operacije",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }



                OperacijaDTO operacijaDto = new OperacijaDTO()
                {
                    Lekar = (Lekar) listaLekara.SelectedItem,
                    Soba = (Soba) comboBoxSale.SelectionBoxItem,
                    TrajanjeOperacijeUMinutima = trajanjeOperacije
                };

                operacije = operacijaKontroler.DostuptniTerminiLekaraZaDatuProstoriju(operacijaDto);


                terminiList.Items.Clear();

                foreach (Operacija operacija in operacije)
                {
                    terminiList.Items.Add(operacija.VremePocetkaOperacije);
                }

                if (terminiList.Items.Count != 0)
                {
                    terminiList.SelectedIndex = 0;
                }

                if (terminiList.SelectedIndex == -1 || listaLekara.SelectedIndex == -1)
                {
                    potvrdi.IsEnabled = false;
                }
                else
                {
                    potvrdi.IsEnabled = true;
                }
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }


        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 1;
        }

        private void ToggleButton_OnUnchecked_UnChecked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 0;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_ClickNazad(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
        */
    }
        
}
