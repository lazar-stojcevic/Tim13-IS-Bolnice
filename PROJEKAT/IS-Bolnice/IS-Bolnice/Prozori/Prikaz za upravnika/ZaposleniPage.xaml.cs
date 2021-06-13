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

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for ZaposleniPage.xaml
    /// </summary>
    public partial class ZaposleniPage : Page
    {
        List<Lekar> lekari = new List<Lekar>();
        LekarKontroler kontroler = new LekarKontroler();
        public ZaposleniPage()
        {
            InitializeComponent();
            lekari = kontroler.GetSviLekari();
            listBox.ItemsSource = lekari;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Lekar selektovaniLekar = (Lekar) listBox.SelectedItem;
            Page lekarPage = new LekarPage(selektovaniLekar.Id);
            this.NavigationService.Navigate(lekarPage);
        }
    }
}
