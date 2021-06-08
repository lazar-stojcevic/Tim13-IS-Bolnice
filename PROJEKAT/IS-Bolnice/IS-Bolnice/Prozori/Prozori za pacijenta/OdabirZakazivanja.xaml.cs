using IS_Bolnice.ViewModel.VMPacijent;
using System.Windows;

namespace IS_Bolnice.Prozori.Prozori_za_pacijenta
{
    public partial class OdabirZakazivanja : Window
    {
        private string jmbgPacijenta;

        public OdabirZakazivanja(string jmbgPacijenta)
        {
            InitializeComponent();
            this.jmbgPacijenta = jmbgPacijenta;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            this.DataContext = new OdabirZakazivanjaViewModel(this, jmbgPacijenta);
        }
    }
}
