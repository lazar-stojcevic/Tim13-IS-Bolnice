using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using IS_Bolnice.ViewModel.Sekretar;

namespace IS_Bolnice.Prozori.Sekretar
{
    public partial class SekretarAzuriranjeAlergena : Window
    {
        private Pacijent pacijentRef;

        public SekretarAzuriranjeAlergena(Pacijent pacijent)
        {
            InitializeComponent();
            pacijentRef = pacijent;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            this.DataContext = new AzuriranjeAlergenaViewModel(this, pacijentRef);
        }
    }
}
