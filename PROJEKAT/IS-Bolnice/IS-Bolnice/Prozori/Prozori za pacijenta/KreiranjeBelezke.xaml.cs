using IS_Bolnice.ViewModel.VMPacijent;
using System.Windows;
using System.Windows.Controls;

namespace IS_Bolnice.Prozori.Prozori_za_pacijenta
{
    public partial class KreiranjeBelezke : Window
    {
        private string jmbgPacijenta;

        ListView listaZaOsvezavanje;

        public KreiranjeBelezke(string jmbg, ListView listaObavestenja = null)
        {
            InitializeComponent();

            jmbgPacijenta = jmbg;
            listaZaOsvezavanje = listaObavestenja;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            this.DataContext = new KreiranjeBelezkeViewModel(this, jmbgPacijenta, listaZaOsvezavanje);
        }
    }
}
