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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for EditLekPage2.xaml
    /// </summary>
    public partial class EditLekPage2 : Page
    {
        Lek noviLek;
        bool kreiranjeIzmena;
        public EditLekPage2(Lek lek, bool izmenaKreiranje)
        {
            InitializeComponent();
            noviLek = lek;
            kreiranjeIzmena = izmenaKreiranje;
            listBox_sastojci.ItemsSource = ParseSastojakToString();
            listBox.ItemsSource = ParseLekToString();
            SelectSastojke();
            SelectZamenskeLekove();

        }

        public void SelectZamenskeLekove() {
            foreach (Lek zamenskiLek in noviLek.ZamenskiLekovi) {
                SelectZamenskiLek(zamenskiLek.Id);
            }
        }

        public void SelectZamenskiLek(string sifraLeka) {
            MessageBox.Show("Radi");
            foreach (string lek in listBox.Items) {
                string[] podaciOLeku = lek.Split(' ');
                if (podaciOLeku[1].Equals(sifraLeka)) {
                    listBox.SelectedItems.Add(lek);
                }
            }
        }

        public void SelectSastojke() {
            foreach (Sastojak sastojak in noviLek.Alergeni) {
                SelectSastojak(sastojak.Ime);
            }
        }

        public void SelectSastojak(string imeSastojka) {

            foreach (string ime in listBox_sastojci.Items) {
                if (ime.Equals(imeSastojka)) {
                    listBox_sastojci.SelectedItems.Add(ime);
                }
            }
        }

        public List<string> ParseSastojakToString()
        {
            BazaSastojaka bazaSastojaka = new BazaSastojaka();
            List<Sastojak> sastojci = bazaSastojaka.DobaviSve();
            List<string> tekst = new List<string>();
            foreach (Sastojak sastojak in sastojci)
            {
                tekst.Add(sastojak.Ime);
            }
            return tekst;
        }

        public List<string> ParseLekToString()
        {
            BazaLekova bazaLekova = new BazaLekova();
            List<string> tekst = new List<string>();
            List<Lek> lekovi = bazaLekova.DobaviSve();
            foreach (Lek lek in lekovi)
            {
                string linija = "ID: " + lek.Id + " Naziv: " + lek.Ime;
                tekst.Add(linija);
            }
            return tekst;
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            if (kreiranjeIzmena)
            {
                Page lekovi = new LekoviPage();
                this.NavigationService.Navigate(lekovi);
            }
            else
            {
                Page odgovoriNaZahteve = new OdgovoriNaZahteveLekoviPage();
                this.NavigationService.Navigate(odgovoriNaZahteve);
            }
        }

        private void Sastojci_btn_Click(object sender, RoutedEventArgs e)
        {
            Page sastojci = new SastojciPage();
            this.NavigationService.Navigate(sastojci);
        }

        private void Potvrdi_btn_Click(object sender, RoutedEventArgs e)
        {
            BazaLekova bazaLekova = new BazaLekova();
            BazaOdgovoraNaZahteveZaValidacijuLekova bazaOdgovora = new BazaOdgovoraNaZahteveZaValidacijuLekova();
            if (kreiranjeIzmena)
            {
                bazaLekova.Izmeni(noviLek);
            }
            else {
                OdgovorNaZahtevZaValidaciju odgovorNaZahtev = new OdgovorNaZahtevZaValidaciju(noviLek, null);
                bazaOdgovora.Obrisi(odgovorNaZahtev.Lek.Id);
                AddLekPage3 addLekareZaZahtev = new AddLekPage3(noviLek);
                this.NavigationService.Navigate(addLekareZaZahtev);
            }
            if (kreiranjeIzmena)
            {
                Page lekovi = new LekoviPage();
                this.NavigationService.Navigate(lekovi);
            }
            /*else
            {
                Page odgovoriNaZahteve = new OdgovoriNaZahteveLekoviPage();
                this.NavigationService.Navigate(odgovoriNaZahteve);
            }*/
        }

        private void listBox_sastojci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Sastojak> sastojci = new List<Sastojak>();
            foreach (string selectedIdem in listBox_sastojci.SelectedItems) {
                Sastojak sastojak = new Sastojak(selectedIdem);
                sastojci.Add(sastojak);
            }
            noviLek.Alergeni = sastojci;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Lek> zamenskiLekovi = new List<Lek>();
            foreach (string selectedItem in listBox.SelectedItems) {
                string[] podaciOLeku = selectedItem.Split(' ');
                Lek lek = new Lek(podaciOLeku[1]);
                zamenskiLekovi.Add(lek);
            }
            noviLek.ZamenskiLekovi = zamenskiLekovi;
        }
    }
}
