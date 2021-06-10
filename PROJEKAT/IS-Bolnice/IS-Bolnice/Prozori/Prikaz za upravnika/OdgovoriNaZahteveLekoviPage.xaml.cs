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
    /// Interaction logic for OdgovoriNaZahteveLekoviPage.xaml
    /// </summary>
    public partial class OdgovoriNaZahteveLekoviPage : Page
    {
        private List<OdgovorNaZahtevZaValidaciju> odgovori = new List<OdgovorNaZahtevZaValidaciju>();
        private OdgovoriNaZahtevZaValidacijeKontroler kontroler = new OdgovoriNaZahtevZaValidacijeKontroler();

        public OdgovoriNaZahteveLekoviPage()
        {
            InitializeComponent();
            odgovori = kontroler.GetSve();
            listBox.ItemsSource = odgovori;
        }

        public List<string> ParseToString() {
            OdgovorNaZahtevFajlRepozitorijum baza = new OdgovorNaZahtevFajlRepozitorijum();
            List<OdgovorNaZahtevZaValidaciju> odgovoriNaZahteve = baza.GetSve();
            List<string> linije = new List<string>();
            foreach (OdgovorNaZahtevZaValidaciju odgovor in odgovoriNaZahteve) {
                string linija = "ID: " + odgovor.Lek.Id + " Naziv: " + odgovor.Lek.Ime;
                linije.Add(linija);
            }
            return linije;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OdgovorNaZahtevZaValidaciju odgovorNaZahtevZaValidaciju = (OdgovorNaZahtevZaValidaciju)listBox.SelectedItem;
            Page odgovorNaZahtev = new OdgovorNaZahtevLekPage(odgovorNaZahtevZaValidaciju.Lek.Id);
            this.NavigationService.Navigate(odgovorNaZahtev);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
