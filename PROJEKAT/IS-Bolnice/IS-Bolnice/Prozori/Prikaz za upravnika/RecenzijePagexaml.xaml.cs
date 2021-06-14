using IS_Bolnice.Model;
using IS_Bolnice.Repozitorijumi;
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
using IS_Bolnice.Repozitorijumi.Interfejsi;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Kontroleri.Informativni;

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for RecenzijePagexaml.xaml
    /// </summary>
    public partial class RecenzijePagexaml : Page
    {
        List<Anketa> sveAnketeLekara;
        AnketaKontroler kontroler = new AnketaKontroler();

        public RecenzijePagexaml(string idLekara)
        {
            InitializeComponent();
            sveAnketeLekara = kontroler.GetSveAnketeLekara();
            listBox.ItemsSource = sveAnketeLekara;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Anketa anketa = (Anketa)listBox.SelectedItem;
            Window window = new PregledRecenzijeWindow(anketa.Opis);
            window.Show();
        }
    }
}
