using System.Windows;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.ViewModel.Sekretar;


namespace IS_Bolnice.Prozori.Sekretar
{
    public partial class DodavanjeGuestNalogaWindow : Window
    {
        public DodavanjeGuestNalogaWindow()
        {
            InitializeComponent();
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            this.DataContext = new DodavanjeGuestNalogaViewModel(this);
        }
    }
}
