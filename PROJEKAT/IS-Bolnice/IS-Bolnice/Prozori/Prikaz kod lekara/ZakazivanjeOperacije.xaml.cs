using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IS_Bolnice.Kontroleri;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    public partial class ZakazivanjeOperacije : Window
    {
        List<Lekar> lekariSpecijalisti = new List<Lekar>();
        OperacijaKontroler operacijaKontroler = new OperacijaKontroler();
        private LekarKontroler lekarKontroler = new LekarKontroler();
        List<Operacija> operacije = new List<Operacija>();
        public ZakazivanjeOperacije()
        {
            InitializeComponent();
            List<Lekar> sviLekari = lekarKontroler.GetSviLekari();
            foreach (Lekar p in sviLekari)
            {
                // svi lekari specijalisti
                if (!p.JelLekarOpstePrakse())
                {
                    string podaci = p.Ime + " " + p.Prezime + " " + p.Jmbg + " " + p.Oblast.Naziv;
                    listaLekara.Items.Add(podaci);
                    lekariSpecijalisti.Add(p);
                }
                
            }

            foreach(Soba s in new BolnicaKontroler().GetSveOperacioneSale())
            {
                    comboBoxSale.Items.Add(s.Id + " " + s.Kvadratura+ "m^2"+" "+ s.Tip.ToString());
            }
        }

        private void Button_ClickZakazi(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da ste dobro uneli sve vrednosti?", "Zakazivanje operacije", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Operacija operacija = KreirajNovuOperaciju();
                operacijaKontroler.ZakaziOperaciju(operacija);
                MessageBox.Show("Operacijacija uspešno kreirana", "Kreirana operacija", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                this.Close();
            }
        }

        private Operacija KreirajNovuOperaciju()
        {
            Operacija operacija = operacije.ElementAt(terminiList.SelectedIndex);
            string idLekara = listaLekara.SelectedItem.ToString().Split(' ')[2];
            string idSale = comboBoxSale.SelectedItem.ToString().Split(' ')[0];
            //TODO: OVAJ DEO MORA DA SE VALIDIRA ALI ZA SAD JE OK

            DateTime pocetak = new DateTime(operacija.VremePocetkaOperacije.Year, operacija.VremePocetkaOperacije.Month,
                operacija.VremePocetkaOperacije.Day, operacija.VremePocetkaOperacije.Hour,
                operacija.VremePocetkaOperacije.Minute, 0);
            DateTime kraj = new DateTime(operacija.VremeKrajaOperacije.Year, operacija.VremeKrajaOperacije.Month,
                operacija.VremeKrajaOperacije.Day, operacija.VremeKrajaOperacije.Hour, operacija.VremeKrajaOperacije.Minute, 0);
            kraj = kraj.AddMinutes(45);
            operacija.Lekar.Jmbg = idLekara;
            operacija.Pacijent.Jmbg = txtOperJmbg.Text;
            operacija.Soba.Id = idSale;
            operacija.VremePocetkaOperacije = pocetak;
            operacija.VremeKrajaOperacije = kraj;
            operacija.Hitna = (bool) boxHitno.IsChecked;
            return operacija;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da želite da odustanete od zakazivanje operacije?", "Zakazivanje operacije", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void liste_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaLekara.SelectedIndex == -1) { return; } 
            if (terminiList.SelectedIndex == -1 || listaLekara.SelectedIndex == -1)
            {
                potvrdi.IsEnabled = false;
            }
            else
            {
                potvrdi.IsEnabled = true;
            }

            string jmbgLekara = lekariSpecijalisti.ElementAt(listaLekara.SelectedIndex).Jmbg;
            string idSale = comboBoxSale.SelectedItem.ToString().Split(' ')[0];
            operacije = lekarKontroler.GetDostupniTerminiZaLekaraIDatuProstoriju(jmbgLekara, idSale);

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

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 1;
        }

        private void ToggleButton_OnUnchecked_UnChecked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 0;
        }
    }

}
