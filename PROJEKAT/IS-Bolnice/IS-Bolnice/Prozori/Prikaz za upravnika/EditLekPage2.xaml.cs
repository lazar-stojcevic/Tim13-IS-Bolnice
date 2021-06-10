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
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Kontroleri;

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for EditLekPage2.xaml
    /// </summary>
    public partial class EditLekPage2 : Page
    {
        Lek noviLek;
        bool kreiranjeIzmena;
        List<Lek> lekovi;
        LekKontroler lekoviKontroler = new LekKontroler();
        public EditLekPage2(Lek lek, bool izmenaKreiranje)
        {
            InitializeComponent();
            noviLek = lek;
            kreiranjeIzmena = izmenaKreiranje;
            lekovi = lekoviKontroler.GetSviLekovi();
            listBox_sastojci.ItemsSource = ParseSastojakToString();
            listBox.ItemsSource = lekovi;
            SelectSastojke();
            SelectZamenskeLekove();

        }

        public void SelectZamenskeLekove() {
            foreach (Lek zamenskiLek in noviLek.ZamenskiLekovi) {
                SelectZamenskiLek(zamenskiLek.Id);
            }
        }

        public void SelectZamenskiLek(string sifraLeka) {
            foreach (Lek lek in listBox.Items) {
                if (lek.Id.Equals(sifraLeka)) {
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
            ISastojakRepozitorijum sastojakRepo = new SastojakFajlRepozitorijum();
            List<Sastojak> sastojci = sastojakRepo.GetSve();
            List<string> tekst = new List<string>();
            foreach (Sastojak sastojak in sastojci)
            {
                tekst.Add(sastojak.Ime);
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
            LekKontroler kontroler = new LekKontroler();
            OdgovoriNaZahtevZaValidacijeKontroler odgovorKontroler = new OdgovoriNaZahtevZaValidacijeKontroler();
            if (kreiranjeIzmena)
            {
                kontroler.IzmeniLek(noviLek);
            }
            else
            {
                OdgovorNaZahtevZaValidaciju odgovorNaZahtev = new OdgovorNaZahtevZaValidaciju(noviLek, null);
                odgovorKontroler.ObrisiOdgovorNaZahtevZaValidaciju(odgovorNaZahtev);
                AddLekPage3 addLekareZaZahtev = new AddLekPage3(noviLek);
                this.NavigationService.Navigate(addLekareZaZahtev);
            }
            if (kreiranjeIzmena)
            {
                Page lekovi = new LekoviPage();
                this.NavigationService.Navigate(lekovi);
            }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
