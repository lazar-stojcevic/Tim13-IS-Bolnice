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
    public partial class OdabirZakazivanja : Window
    {
        private string jmbgPac;
        public OdabirZakazivanja(string jmbgPacijenta)
        {
            InitializeComponent();
            jmbgPac = jmbgPacijenta;
        }

        private void kodLekaraBtn_Click(object sender, RoutedEventArgs e)
        {
            ZakazivanjeKodOdredjenogLekara zkod = new ZakazivanjeKodOdredjenogLekara(jmbgPac);
            zkod.ShowDialog();
        }

        private void uTerminuBtn_Click(object sender, RoutedEventArgs e)
        {
            ZakazivanjeUOdredjenomTerminu zuot = new ZakazivanjeUOdredjenomTerminu(jmbgPac);
            zuot.ShowDialog();
        }

        private void izadjiBtn_Click(object sender, RoutedEventArgs e)
        {
            // dodati da / ne
            this.Close();
        }
    }
}
