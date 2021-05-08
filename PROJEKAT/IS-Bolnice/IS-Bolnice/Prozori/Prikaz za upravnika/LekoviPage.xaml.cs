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
    /// Interaction logic for LekoviPage.xaml
    /// </summary>
    public partial class LekoviPage : Page
    {
        public LekoviPage()
        {
            InitializeComponent();
            listBox.ItemsSource = ParseLekToString();
        }

        public List<string> ParseLekToString()
        {
            BazaLekova bazaLekova = new BazaLekova();
            List<string> tekst = new List<string>();
            List<Lek> lekovi = bazaLekova.SviLekovi();
            foreach (Lek lek in lekovi)
            {
                string linija = "ID: " + lek.Sifra + " Naziv: " + lek.Ime;
                tekst.Add(linija);
            }
            return tekst;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Page addLek = new AddLekPage();
            this.NavigationService.Navigate(addLek);
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BazaLekova bazaLekova = new BazaLekova();
            string[] podaciOLeku = listBox.SelectedItem.ToString().Split(' ');
            Page editPage = new EditBrisanjePage(bazaLekova.GetLek(podaciOLeku[1]));
            this.NavigationService.Navigate(editPage);
        }

        private void OdgovoriButton_Click(object sender, RoutedEventArgs e)
        {
            Page odgovoriNaZahteve = new OdgovoriNaZahteveLekoviPage();
            this.NavigationService.Navigate(odgovoriNaZahteve);
        }
    }
}
