using IS_Bolnice.Prozori.UpravnikPages;
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
using System.Windows.Shapes;
using IS_Bolnice.Kontroleri;

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for UpravnikWindow.xaml
    /// </summary>
    public partial class UpravnikWindow : Window
    {
        Upravnik upravnik = new Upravnik();
        private UpravnikKontroler kontroler = new UpravnikKontroler();
        public UpravnikWindow(string jmbg)
        {
            InitializeComponent();
            upravnik = kontroler.GetByJmbg(jmbg);
            this.frame.NavigationService.Navigate(new NalogPage(upravnik));
        }

        private void NalogMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Page nalog = new NalogPage(upravnik);
            this.frame.NavigationService.Navigate(nalog);
        }

        private void ProstorijeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Page sala = new SalePage();
            this.frame.NavigationService.Navigate(sala);
        }

        private void OpremaMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Page oprema = new OpremaPage();
            this.frame.NavigationService.Navigate(oprema);
        }

        private void LekoviMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Page lekovi = new LekoviPage();
            this.frame.NavigationService.Navigate(lekovi);
        }

        private void RenoviranjeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Page renoviranje = new RenoviranjeSpajanjePage();
            this.frame.NavigationService.Navigate(renoviranje);
        }

        private void CenovnikMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Page cenovnik = new CenovnikPage();
            this.frame.NavigationService.Navigate(cenovnik);
        }

        private void ZaposleniMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Page zaposleni = new ZaposleniPage();
            this.frame.NavigationService.Navigate(zaposleni);
        }

        private void IzvestajMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Page izvestaj = new IzvestajPage();
            this.frame.NavigationService.Navigate(izvestaj);
        }
    }
}
