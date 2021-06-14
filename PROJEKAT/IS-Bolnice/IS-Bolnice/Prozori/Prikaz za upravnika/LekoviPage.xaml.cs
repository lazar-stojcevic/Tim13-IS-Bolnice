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
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Kontroleri.Lekovi;

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for LekoviPage.xaml
    /// </summary>
    public partial class LekoviPage : Page
    {
        LekKontroler kontroler = new LekKontroler();
        public LekoviPage()
        {
            InitializeComponent();
            listBox.ItemsSource = kontroler.GetSviLekovi();
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Page addLek = new AddLekPage();
            this.NavigationService.Navigate(addLek);
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Page editPage = new EditBrisanjePage((Lek)listBox.SelectedItem);
            this.NavigationService.Navigate(editPage);
        }

        private void OdgovoriButton_Click(object sender, RoutedEventArgs e)
        {
            Page odgovoriNaZahteve = new OdgovoriNaZahteveLekoviPage();
            this.NavigationService.Navigate(odgovoriNaZahteve);
        }
    }
}
