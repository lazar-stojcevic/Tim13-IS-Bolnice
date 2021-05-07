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

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarDodavanjeZamenskogLeka.xaml
    /// </summary>
    public partial class LekarDodavanjeZamenskogLeka : Window
    {
        private ObservableCollection<Lek> zamenskiLekovi = new ObservableCollection<Lek>();
        private BazaLekova bazaLekova = new BazaLekova();
        private List<Lek> lekoviZaPrikaz = new List<Lek>();
        public LekarDodavanjeZamenskogLeka(ObservableCollection<Lek> zamenski, string sifraTrenutnogLeka)
        {
            InitializeComponent();
            zamenskiLekovi = zamenski;
            lekoviZaPrikaz = bazaLekova.SviLekovi();
            int i = 0;
            foreach (Lek lek in bazaLekova.SviLekovi())
            {
                foreach (Lek zalenskiLek in zamenski)
                {
                    //Console.WriteLine(sifraTrenutnogLeka + "  +++++++       " + zalenskiLek.Sifra);
                    if (zalenskiLek.Sifra.Equals(lek.Sifra) || lek.Sifra.Equals(sifraTrenutnogLeka))
                    {
                        lekoviZaPrikaz.RemoveAt(i);
                        --i;
                        break;
                    }
                }

                ++i;
            }

            listaZamenskihLekova.ItemsSource = lekoviZaPrikaz;
        }

        private void Button_ClickDodaj(object sender, RoutedEventArgs e)
        {
            if (listaZamenskihLekova.SelectedIndex != -1)
            {
                zamenskiLekovi.Add((Lek)listaZamenskihLekova.SelectedItem);
            }

            MessageBox.Show("Uspešno je dodat zamenski lek", "Dodat zamesnki lek", MessageBoxButton.OK,
                MessageBoxImage.Information);
            this.Close();
        }

        private void Button_ClickKraj(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
