using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for LekarIzvestaj.xaml
    /// </summary>
    public partial class LekarIzvestaj : Window
    {

        ObservableCollection<Terapija> terapije = new ObservableCollection<Terapija>();

        public LekarIzvestaj()
        {
            InitializeComponent();
            listaLekova.ItemsSource = terapije;
        }

        private void Button_DodajLek(object sender, RoutedEventArgs e)
        {
            LekarDodavanjeLeka prozor = new LekarDodavanjeLeka(terapije);
            prozor.Show();

            
        }

        private void Button_ObrisiLek(object sender, RoutedEventArgs e)
        {
            terapije.Remove((Terapija)listaLekova.SelectedItem);
        }

        private void Button_ZavrsiPregled(object sender, RoutedEventArgs e)
        {
            //Ovde ce biti generisanje nekog izvestaja
        }
    }
}
