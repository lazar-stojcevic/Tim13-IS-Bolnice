using IS_Bolnice.Kontroleri;
using IS_Bolnice.ViewModel.VMPacijent;
using System.Windows;

namespace IS_Bolnice.Prozori.Prozori_za_pacijenta
{
    public partial class PrikazTerminaOperacija : Window
    {
        private string jmbgPacijenta;
        //OperacijaKontroler operacijaKontroler = new OperacijaKontroler();
        public PrikazTerminaOperacija(string jmbgPacijenat)
        {
            InitializeComponent();
            this.jmbgPacijenta = jmbgPacijenat;
            //listaOperacija.ItemsSource = operacijaKontroler.GetSveBuduceOperacijePacijenta(jmbgPacijenat);
        }
        void OnLoad(object sender, RoutedEventArgs e) 
        {
            this.DataContext = new PrikazOperacijaViewModel(this, jmbgPacijenta);
        }
    }
}
