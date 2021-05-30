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
using System.Windows.Shapes;

namespace IS_Bolnice.Prozori.Prozori_za_pacijenta
{
    public partial class PrikazTerminaOperacija : Window
    {
        private OperacijaKontroler operacijaKontroler = new OperacijaKontroler();
        public PrikazTerminaOperacija(string jmbgPacijenat)
        {
            InitializeComponent();
            listaOperacija.ItemsSource = operacijaKontroler.GetSveSledeveOperacijePacijenta(jmbgPacijenat);
        }

        private void izadji_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
