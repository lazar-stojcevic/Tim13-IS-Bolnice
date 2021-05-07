using IS_Bolnice.Prozori.Prikaz_za_upravnika;
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

namespace IS_Bolnice.Prozori.UpravnikPages
{
    /// <summary>
    /// Interaction logic for OpremaPage.xaml
    /// </summary>
    public partial class OpremaPage : Page
    {
        public OpremaPage()
        {
            InitializeComponent();
        }

        private void UpravljanjeButton_Click(object sender, RoutedEventArgs e)
        {
            Page upravljanje = new UpravljanjeOpremomPage();
            this.NavigationService.Navigate(upravljanje);
        }

        private void NabavkaButton_Click(object sender, RoutedEventArgs e)
        {
            Page nabavka = new NabavkaOpremePage();
            this.NavigationService.Navigate(nabavka);
        }


        private void PreraspodelaButton_Click(object sender, RoutedEventArgs e)
        {
            Page preraspodela = new PrepaspodelaOpremePage();
            this.NavigationService.Navigate(preraspodela);
        }
    }
}
