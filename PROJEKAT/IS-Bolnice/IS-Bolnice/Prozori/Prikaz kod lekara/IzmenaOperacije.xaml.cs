using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IS_Bolnice.DTOs;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Kontroleri.Ustanova;
using IS_Bolnice.Kontroleri.Korisnicki;
using IS_Bolnice.Kontroleri.Termini;
using IS_Bolnice.Servisi;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for IzmenaOperacije.xaml
    /// </summary>
    public partial class IzmenaOperacije : Page
    {
        private OperacijaKontroler operacijaKontroler = new OperacijaKontroler();
        private LekarKontroler lekarKontroler = new LekarKontroler();
        private BolnicaKontroler bolnicaKontroler = new BolnicaKontroler();
        private List<Operacija> operacije = new List<Operacija>();
        public string IdStareOperacije { get; set; }

        public IzmenaOperacije(string idStareOperacijeOperacije)
        {
            InitializeComponent();

            IdStareOperacije = idStareOperacijeOperacije;
            
            List<Lekar> lekariSpecijalisti = lekarKontroler.GetSviLekariSpecijalisti();
            listaLekara.ItemsSource = lekariSpecijalisti;
           

            List<Soba> sveOperacioneSale = bolnicaKontroler.GetSveOperacioneSale();
            comboBoxSale.ItemsSource = sveOperacioneSale;

        }

        private void Button_ClickIzmeni(object sender, RoutedEventArgs e)
        {
            if (txtDuzina.Text.Equals(""))
            {
                MessageBox.Show("Niste uneli dužinu trajanje operacije", "Dužina trajanja operacije",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da ste dobro uneli sve podatke za izmenu?", "Izmena operacije", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Operacija novaOperacija = new Operacija(IdStareOperacije);
                Lekar lekar = (Lekar)listaLekara.SelectedItem;
                string idLekara = lekar.Jmbg;

                Soba soba = (Soba)comboBoxSale.SelectionBoxItem;
                string idSale = soba.Id;
                Operacija operacijaSelektovana = operacije.ElementAt(terminiList.SelectedIndex);

                DateTime pocetak = new DateTime(operacijaSelektovana.VremePocetkaOperacije.Year, operacijaSelektovana.VremePocetkaOperacije.Month,
                    operacijaSelektovana.VremePocetkaOperacije.Day, operacijaSelektovana.VremePocetkaOperacije.Hour, operacijaSelektovana.VremePocetkaOperacije.Minute, 0);
                DateTime kraj = new DateTime(operacijaSelektovana.VremeKrajaOperacije.Year, operacijaSelektovana.VremeKrajaOperacije.Month,
                    operacijaSelektovana.VremeKrajaOperacije.Day, operacijaSelektovana.VremeKrajaOperacije.Hour, operacijaSelektovana.VremeKrajaOperacije.Minute, 0);
                try
                {
                    kraj = kraj.AddMinutes(Int32.Parse(txtDuzina.Text));
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Niste validno uneli dužinu trajanje operacije", "Dužina trajanja operacije",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                kraj = kraj.AddMinutes(Int32.Parse(txtDuzina.Text));

                novaOperacija.Lekar.Jmbg = idLekara;
                novaOperacija.Pacijent.Jmbg = txtOperJmbg.Text;
                novaOperacija.Soba.Id = idSale;
                novaOperacija.VremePocetkaOperacije = pocetak;
                novaOperacija.VremeKrajaOperacije = kraj;
                novaOperacija.Hitna = (bool)boxHitno.IsChecked;

                operacijaKontroler.IzmeniOperaciju(novaOperacija);

                NavigationService.GoBack();

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da napustite izmenu operacije, premene se neće sačuvati!", "Izmena operacije", "Da", "Ne", MessageBoxImage.Question);
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

                Lekar lekar = (Lekar) listaLekara.SelectedItem;
                string jmbgLekara = lekar.Jmbg;

                Soba soba = (Soba) comboBoxSale.SelectionBoxItem;
                string idSale = soba.Id;
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

                operacije = operacijaKontroler.GetDostuptniTerminiLekaraZaDatuProstoriju(operacijaDto);



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

        private void ValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[#]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }


    }
}
