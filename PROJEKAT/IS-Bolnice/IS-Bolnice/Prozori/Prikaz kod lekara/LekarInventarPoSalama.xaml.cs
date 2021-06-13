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
using IS_Bolnice.Kontroleri.Ustanova;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarInventarPoSalama.xaml
    /// </summary>
    public partial class LekarInventarPoSalama : Page
    {
        public LekarInventarPoSalama()
        {
            InitializeComponent();
            List<Soba> listaSvihSoba = new BolnicaKontroler().GetSveSobe();
            listaSoba.ItemsSource = listaSvihSoba;
            /*
            List<SadrzajSobe> sadrzajiSobe = new List<SadrzajSobe>();
            inventar.ItemsSource = sadrzajiSobe;
            */
        }

        private void PromenaSelekcije(object sender, SelectionChangedEventArgs e)
        {
            Soba soba = (Soba) listaSoba.SelectedItem;
            List<SadrzajSobe> sadrzaj = new SadrzajSobeKontroler().GetSadrzajSobe(soba.Id);
            inventar.ItemsSource = sadrzaj;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
