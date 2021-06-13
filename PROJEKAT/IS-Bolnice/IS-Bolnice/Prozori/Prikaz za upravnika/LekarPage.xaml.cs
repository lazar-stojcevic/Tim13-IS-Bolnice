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
    /// Interaction logic for LekarPage.xaml
    /// </summary>
    public partial class LekarPage : Page
    {
        LekarKontroler lekarKontroler = new LekarKontroler();
        string id;
        public LekarPage(string idLekara)
        {
            InitializeComponent();
            this.frame.Content = new BrojPrijemaGraf(idLekara);
            this.frame1.Content = new SlobodniZauzetiTerminiGraf(idLekara);
            label.Content = lekarKontroler.GetLekar(idLekara).Ime + " "+ lekarKontroler.GetLekar(idLekara).Prezime;
            id = idLekara;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Page recenzija = new RecenzijePagexaml(id);
            this.NavigationService.Navigate(recenzija);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
