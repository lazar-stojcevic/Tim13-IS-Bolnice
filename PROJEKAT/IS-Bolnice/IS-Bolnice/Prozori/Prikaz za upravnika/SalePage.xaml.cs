using IS_Bolnice.Kontroleri;
using IS_Bolnice.ViewModel.Upravnik;
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
using IS_Bolnice.Kontroleri.Ustanova;

namespace IS_Bolnice.Prozori.UpravnikPages
{
    /// <summary>
    /// Interaction logic for SalePage.xaml
    /// </summary>
    public partial class SalePage : Page
    {
        private List<Soba> sveSobeZaPrikaz = new List<Soba>();
        private BolnicaKontroler kontroler = new BolnicaKontroler();
        public SalePage()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            this.DataContext = new SalePageViewModel(this.NavigationService);
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Soba selectovanaSoba = (Soba)listBox.SelectedItem;
            Page editSale = new EditSalePage(selectovanaSoba.Id);
            this.NavigationService.Navigate(editSale);
        }

        private void tip_opreme_txt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
