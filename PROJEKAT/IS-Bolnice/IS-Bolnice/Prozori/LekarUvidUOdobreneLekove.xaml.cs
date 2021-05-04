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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarUvidUOdobreneLekove.xaml
    /// </summary>
    public partial class LekarUvidUOdobreneLekove : Page
    {
        BazaLekova bazaLekova = new BazaLekova();
        ObservableCollection<Lek> lekovi = new ObservableCollection<Lek>();
        public LekarUvidUOdobreneLekove()
        {
            InitializeComponent();
            foreach (Lek lek in bazaLekova.SviLekovi())
            {
                lekovi.Add(lek);
            }
            listaLekova.ItemsSource = lekovi;
        }

        private void Button_IzmeniClick(object sender, RoutedEventArgs e)
        {
            Page uvidULek = new LekarPodaciOLeku((Lek)listaLekova.SelectedItem, lekovi);
            this.NavigationService.Navigate(uvidULek);


        }

        private void Button_OdustaniClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
