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
using IS_Bolnice.Kontroleri;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarUvidUOdobreneLekove.xaml
    /// </summary>
    public partial class LekarUvidUOdobreneLekove : Page
    {
        private LekKontroler lekKontroler = new LekKontroler();
        ObservableCollection<Lek> lekovi = new ObservableCollection<Lek>();
        public LekarUvidUOdobreneLekove()
        {
            InitializeComponent();
            foreach (Lek lek in lekKontroler.GetSviLekovi())
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

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 1;
        }

        private void ToggleButton_OnUnchecked_UnChecked(object sender, RoutedEventArgs e)
        {
            help.Opacity = 0;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
