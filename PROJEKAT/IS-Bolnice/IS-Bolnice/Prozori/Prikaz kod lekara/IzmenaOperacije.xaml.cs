using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.IO;
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
using IS_Bolnice.Servisi;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for IzmenaOperacije.xaml
    /// </summary>
    public partial class IzmenaOperacije : Window
    {
        private OperacijaKontroler operacijaKontroler = new OperacijaKontroler();
        private LekarKontroler lekarKontroler = new LekarKontroler();
        private BolnicaKontroler bolnicaKontroler = new BolnicaKontroler();

        private List<Lekar> lekariSpecijalisti = new List<Lekar>();
        private List<Operacija> operacije = new List<Operacija>();

        public DateTime StariDatum  { get; set; }
        public string StariSat { get; set; }
        public string StariMinut { get; set; }
        public IzmenaOperacije()
        {
            InitializeComponent();
            List<Lekar> sviLekari = lekarKontroler.GetSviLekariSpecijalisti();
            foreach (Lekar p in sviLekari)
            {
                    string podaci = p.Ime + " " + p.Prezime + " " + p.Jmbg + " " + p.Oblast.Naziv;
                    listaLekara.Items.Add(podaci);
                    lekariSpecijalisti.Add(p);
            }
            foreach (Soba s in bolnicaKontroler.GetSveOperacioneSale()) 
            {
                comboBoxSale.Items.Add(s.Id + " " + s.Kvadratura + "m^2" + " " + s.Tip.ToString());
            }
            
        }

        private void Button_ClickIzmeni(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da ste dobro uneli sve podatke za izmenu?", "Izmena operacije", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Operacija novaOperacija = new Operacija();
                string idLekara = listaLekara.SelectedItem.ToString().Split(' ')[2];
                string idSale = comboBoxSale.SelectedItem.ToString().Split(' ')[0];
                Operacija operacijaSelektovana = operacije.ElementAt(terminiList.SelectedIndex);

                DateTime pocetak = new DateTime(operacijaSelektovana.VremePocetkaOperacije.Year, operacijaSelektovana.VremePocetkaOperacije.Month,
                    operacijaSelektovana.VremePocetkaOperacije.Day, operacijaSelektovana.VremePocetkaOperacije.Hour, operacijaSelektovana.VremePocetkaOperacije.Minute, 0);
                DateTime kraj = new DateTime(operacijaSelektovana.VremeKrajaOperacije.Year, operacijaSelektovana.VremeKrajaOperacije.Month,
                    operacijaSelektovana.VremeKrajaOperacije.Day, operacijaSelektovana.VremeKrajaOperacije.Hour, operacijaSelektovana.VremeKrajaOperacije.Minute, 0);
                kraj = kraj.AddMinutes(45); //Predpostavka da ce operacija trajati 45 minuta 
                //TODO: UBACI POLJE ZA DUZINU OPERACIJE
                novaOperacija.Lekar.Jmbg = idLekara;
                novaOperacija.Pacijent.Jmbg = txtOperJmbg.Text;
                novaOperacija.Soba.Id = idSale;
                novaOperacija.VremePocetkaOperacije = pocetak;
                novaOperacija.VremeKrajaOperacije = kraj;
                novaOperacija.Hitna = (bool)boxHitno.IsChecked;

                operacijaKontroler.IzmeniOperaciju(StariDatum, StariSat, StariMinut, novaOperacija);

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da napustite izmenu operacije, premene se neće sačuvati!", "Izmena operacije", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void lekariList_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
