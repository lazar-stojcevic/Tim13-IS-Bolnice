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
        public OdgovoriNaZahteveLekoviPage()
        {
            InitializeComponent();
            listBox.ItemsSource = ParseToString();
        }

        public List<string> ParseToString() {
            OdgovorNaZahtevFajlRepozitorijum baza = new OdgovorNaZahtevFajlRepozitorijum();
            List<OdgovorNaZahtevZaValidaciju> odgovoriNaZahteve = baza.DobaviSve();
            List<string> linije = new List<string>();
            foreach (OdgovorNaZahtevZaValidaciju odgovor in odgovoriNaZahteve) {
                string linija = "ID: " + odgovor.Lek.Id + " Naziv: " + odgovor.Lek.Ime;
                linije.Add(linija);
            }
            return linije;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] redTabele = listBox.SelectedItem.ToString().Split(' ');
            Page odgovorNaZahtev = new OdgovorNaZahtevLekPage(redTabele[1]);
            this.NavigationService.Navigate(odgovorNaZahtev);
        }
    }
}
