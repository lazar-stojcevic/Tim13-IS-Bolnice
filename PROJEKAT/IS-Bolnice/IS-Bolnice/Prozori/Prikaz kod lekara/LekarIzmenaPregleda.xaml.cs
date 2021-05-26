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
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarIzmenaPregleda.xaml
    /// </summary>
    public partial class LekarIzmenaPregleda : Window
    {
        public DateTime StariDatum { get; set; }
        public string StariSat { get; set; }
        public string StariMinut { get; set; }

        private PregledKontroler pregledKontroler = new PregledKontroler();

        List<Pregled> pregledi = new List<Pregled>();
        public LekarIzmenaPregleda()
        {
            InitializeComponent();
            LekarKontroler lekarKontroler = new LekarKontroler();
            listaLekara.ItemsSource = lekarKontroler.GetSviLekari();

        }

        private void Button_ClickIzmeni(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da ste dobro uneli sve podatke za izmenu?", "Izmena pregleda", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Lekar lekar = (Lekar)listaLekara.SelectedItem;
                string idLekara = lekar.Jmbg;

                Pregled noviPregled = new Pregled();
                noviPregled.Lekar.Ordinacija = lekar.Ordinacija;
                Pregled pregled = pregledi.ElementAt(terminiList.SelectedIndex);

                DateTime pocetak = new DateTime(pregled.VremePocetkaPregleda.Year, pregled.VremePocetkaPregleda.Month,
                    pregled.VremePocetkaPregleda.Day, pregled.VremePocetkaPregleda.Hour,
                    pregled.VremePocetkaPregleda.Minute, 0);
                DateTime kraj = new DateTime(pregled.VremeKrajaPregleda.Year, pregled.VremeKrajaPregleda.Month,
                    pregled.VremeKrajaPregleda.Day, pregled.VremeKrajaPregleda.Hour, pregled.VremeKrajaPregleda.Minute,
                    0);
                kraj = kraj.AddMinutes(45); //Predpostavka da ce pregled trajati 45 minuta 

                noviPregled.Lekar.Jmbg = idLekara;
                noviPregled.Pacijent.Jmbg = txtOperJmbg.Text;
                noviPregled.VremePocetkaPregleda = pocetak;
                noviPregled.VremeKrajaPregleda = kraj;
                pregledKontroler.IzmeniPregled(StariDatum, StariSat, StariMinut, noviPregled);
                this.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da napustite izmenu operacije, premene se neće sačuvati!", "Izmena pregleda", "Da", "Ne", MessageBoxImage.Question);
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

            Lekar lekar = (Lekar)listaLekara.SelectedItem;

            pregledi = pregledKontroler.GetDostupniTerminiPregledaLekaraUNarednomPeriodu(lekar);
            terminiList.Items.Clear();

            foreach (Pregled p in pregledi)
            {
                terminiList.Items.Add(p.VremePocetkaPregleda);
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
