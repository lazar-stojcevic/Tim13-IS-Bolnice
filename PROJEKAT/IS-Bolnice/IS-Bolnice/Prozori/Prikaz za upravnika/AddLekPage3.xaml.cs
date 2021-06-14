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
using IS_Bolnice.Kontroleri.Korisnicki;
using IS_Bolnice.Kontroleri.Lekovi;

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for AddLekPage3.xaml
    /// </summary>
    public partial class AddLekPage3 : Page
    {
        Lek noviLek;

        ZahtevZaValidacijuKontroler kontroler = new ZahtevZaValidacijuKontroler();
        LekarKontroler lekarKontroler = new LekarKontroler();
        List<Lekar> lekari;
        public AddLekPage3(Lek lek)
        {
            InitializeComponent();
            noviLek = lek;
            lekari = lekarKontroler.GetSviLekari();
            listBox.ItemsSource = lekari;
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            Page lekovi = new LekoviPage();
            this.NavigationService.Navigate(lekovi);
        }

        private void Potvrdi_btn_Click(object sender, RoutedEventArgs e)
        {
            List<Lekar> lekari = new List<Lekar>();
            foreach (Lekar lekar in listBox.SelectedItems) {
                lekari.Add(lekar);
            }
            ZahtevZaValidacijuLeka zahtevZaValidaciju = new ZahtevZaValidacijuLeka(noviLek, lekari);
            kontroler.KreirajZahtevZaValidaciju(zahtevZaValidaciju);
            Page lekovi = new LekoviPage();
            this.NavigationService.Navigate(lekovi);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
