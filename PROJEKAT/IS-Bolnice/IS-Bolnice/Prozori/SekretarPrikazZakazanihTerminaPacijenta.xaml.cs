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
        private BazaOperacija bo;

        public ObservableCollection<Pregled> PreglediPacijenta
        {
            get;
            set;
        }

        public ObservableCollection<Operacija> OperacijePacijenta
        {
            get;
            set;
        }

        public SekretarPrikazZakazanihTerminaPacijenta(Pacijent p)
        {
            InitializeComponent();
            bp = new BazaPregleda();
            bo = new BazaOperacija();

            pacijent = p;
            this.DataContext = this;

            pacijentTxt.Text = p.Ime + " " + p.Prezime;

            PreglediPacijenta = new ObservableCollection<Pregled>(bp.PreglediOdredjenogPacijenta(p.Jmbg));
            OperacijePacijenta = new ObservableCollection<Operacija>(bo.OperacijeOdredjenogPacijenta(p));
        }

        private void Button_Click_Otkazi_Pregled(object sender, RoutedEventArgs e)
        {
            Pregled pregled = PreglediPacijenta[lvPregledi.SelectedIndex];
            if (pregled != null)
            {
                string sMessageBoxText = "Da li ste sigurni da želite da otkažete termin?";
                string sCaption = "Otkazivanje termina";

                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxImage icnMessageBox = MessageBoxImage.Question;

                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        bp.OtkaziPregled(pregled);
                        PreglediPacijenta.Remove(pregled);
                        break;

                    case MessageBoxResult.No:
                        /* ... */
                        break;
                }
            }
        }

        private void Button_Click_Izmeni_Pregled(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Izmeni_Operaciju(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Otkazi_Operaciju(object sender, RoutedEventArgs e)
        {

        }
    }
}
