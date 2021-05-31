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

        private Pregled stariPregled = new Pregled();
        private PregledKontroler pregledKontroler = new PregledKontroler();

        List<Pregled> pregledi = new List<Pregled>();
        public LekarIzmenaPregleda(Pregled stariPr)
        {
            stariPregled = stariPr;
            InitializeComponent();
            LekarKontroler lekarKontroler = new LekarKontroler();
            listaLekara.ItemsSource = lekarKontroler.GetSviLekari();

        }

        private void Button_ClickIzmeni(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da ste dobro uneli sve podatke za izmenu?", "Izmena pregleda", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Pregled noviPregled = KreirajNoviPregled();

                pregledKontroler.IzmeniPregled(noviPregled);
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
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (listaLekara.SelectedIndex == -1)
                {
                    return;
                }

                if (terminiList.SelectedIndex == -1 || listaLekara.SelectedIndex == -1)
                {
                    potvrdi.IsEnabled = false;
                }
                else
                {
                    potvrdi.IsEnabled = true;
                }

                Lekar lekar = (Lekar) listaLekara.SelectedItem;

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

        private Pregled KreirajNoviPregled()
        {
            Pregled noviPregled = new Pregled();
            Pregled selektovaniTermin = pregledi.ElementAt(terminiList.SelectedIndex);
            Lekar lekar = (Lekar)listaLekara.SelectedItem;
            string idLekara = lekar.Jmbg;
            noviPregled.Id = stariPregled.Id;
            noviPregled.Lekar.Ordinacija = lekar.Ordinacija;

            DateTime pocetak = new DateTime(selektovaniTermin.VremePocetkaPregleda.Year, selektovaniTermin.VremePocetkaPregleda.Month,
                selektovaniTermin.VremePocetkaPregleda.Day, selektovaniTermin.VremePocetkaPregleda.Hour,
                selektovaniTermin.VremePocetkaPregleda.Minute, 0);
            DateTime kraj = new DateTime(selektovaniTermin.VremeKrajaPregleda.Year, selektovaniTermin.VremeKrajaPregleda.Month,
                selektovaniTermin.VremeKrajaPregleda.Day, selektovaniTermin.VremeKrajaPregleda.Hour,
                selektovaniTermin.VremeKrajaPregleda.Minute,
                0);
            kraj = kraj.AddMinutes(45); //Predpostavka da ce pregled trajati 45 minuta 

            noviPregled.Lekar.Jmbg = idLekara;
            noviPregled.Pacijent.Jmbg = txtOperJmbg.Text;
            noviPregled.VremePocetkaPregleda = pocetak;
            noviPregled.VremeKrajaPregleda = kraj;
            return noviPregled;
        }

    }
}
