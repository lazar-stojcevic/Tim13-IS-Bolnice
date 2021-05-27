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
using IS_Bolnice.Model;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarObavestenja.xaml
    /// </summary>
    public partial class LekarObavestenja : Page
    {
        public LekarObavestenja()
        {
            InitializeComponent();
            ObavestenjeKontroler obavestenjeKontroler = new ObavestenjeKontroler();
            List<Obavestenje> listaObavestenja = obavestenjeKontroler.GetSvaSortiranaObavestenja();

            listaObaveštenja.ItemsSource = listaObavestenja;

        }

        private void Open_OnClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Obavestenje recenzija = button.DataContext as Obavestenje;
            MessageBox.Show(recenzija.Sadrzaj, "Obaveštenje");
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
