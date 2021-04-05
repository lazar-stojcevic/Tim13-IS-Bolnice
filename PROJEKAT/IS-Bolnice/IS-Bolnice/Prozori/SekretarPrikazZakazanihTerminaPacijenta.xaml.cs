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

namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for SekretarPrikazZakazanihTerminaPacijenta.xaml
    /// </summary>
    public partial class SekretarPrikazZakazanihTerminaPacijenta : Window
    {
        private Pacijent pacijent;
        private BazaPregleda bp;

        public ObservableCollection<Pregled> PreglediPacijenta
        {
            get;
            set;
        }

        public SekretarPrikazZakazanihTerminaPacijenta(Pacijent p)
        {
            InitializeComponent();
            bp = new BazaPregleda();

            pacijent = p;
            this.DataContext = this;

            pacijentTxt.Text = p.Ime + " " + p.Prezime;

            PreglediPacijenta = new ObservableCollection<Pregled>(bp.PreglediOdredjenogPacijenta(p.Jmbg));
        }

        private void Button_Click_Otkazi(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Izmeni(object sender, RoutedEventArgs e)
        {

        }
    }
}
