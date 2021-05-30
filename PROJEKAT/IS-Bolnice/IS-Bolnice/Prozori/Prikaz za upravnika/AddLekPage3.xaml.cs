using IS_Bolnice.Kontroleri;
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
    /// Interaction logic for AddLekPage3.xaml
    /// </summary>
    public partial class AddLekPage3 : Page
    {
        Lek noviLek;

        ZahtevZaValidacijuKontroler kontroler = new ZahtevZaValidacijuKontroler();

        public AddLekPage3(Lek lek)
        {
            InitializeComponent();
            noviLek = lek;
            listBox.ItemsSource = ParseLekarToString();
        }

        public List<string> ParseLekarToString() {
            LekarFajlRepozitorijum lekarFajlRepozitorijum = new LekarFajlRepozitorijum();
            List<Lekar> lekari = lekarFajlRepozitorijum.DobaviSve();
            List<string> tekst = new List<string>();
            foreach(Lekar lekar in lekari)
            {
                string linija = "Ime: " + lekar.Ime + " Prezime: " + lekar.Prezime + " JMBG: " + lekar.Jmbg;
                tekst.Add(linija);
            }
            return tekst;
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            Page lekovi = new LekoviPage();
            this.NavigationService.Navigate(lekovi);
        }

        private void Potvrdi_btn_Click(object sender, RoutedEventArgs e)
        {
            List<Lekar> lekari = new List<Lekar>();
            foreach (string linija in listBox.SelectedItems) {
                string[] deo = linija.Split(' ');
                Lekar lekar = new Lekar();
                lekar.Jmbg = deo[5];
                lekari.Add(lekar);
            }
            ZahtevZaValidacijuLeka zahtevZaValidaciju = new ZahtevZaValidacijuLeka(noviLek, lekari);
            kontroler.KreirajZahtevZaValidaciju(zahtevZaValidaciju);
            Page lekovi = new LekoviPage();
            this.NavigationService.Navigate(lekovi);
        }
    }
}
