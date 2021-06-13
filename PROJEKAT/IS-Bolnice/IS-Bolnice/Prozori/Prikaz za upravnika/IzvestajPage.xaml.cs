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
    /// Interaction logic for IzvestajPage.xaml
    /// </summary>
    public partial class IzvestajPage : Page
    {
        List<Lekar> sviLekari;
        LekarKontroler kontoler = new LekarKontroler();
        public IzvestajPage()
        {
            InitializeComponent();
            sviLekari = kontoler.GetSviLekari();
            listBox.ItemsSource = sviLekari;
        }

        private void Potvrdi_btn_Click(object sender, RoutedEventArgs e)
        {
            List<Lekar> selektovaniLekari = new List<Lekar>();
            foreach (Lekar lekar in listBox.SelectedItems) {

                selektovaniLekari.Add(lekar);
            }
            Page izvestaj = new IzvestajPage2(selektovaniLekari);
            this.NavigationService.Navigate(izvestaj);
        }
    }
}
