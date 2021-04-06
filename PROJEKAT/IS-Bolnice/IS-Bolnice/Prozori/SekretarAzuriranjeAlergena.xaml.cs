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
    /// Interaction logic for SekretarAzuriranjeAlergena.xaml
    /// </summary>
    public partial class SekretarAzuriranjeAlergena : Window
    {
        private Pacijent pacijent;
        private BazaPacijenata bp;

        public ObservableCollection<string> AlergeniPacijenta
        {
            get;
            set;
        }

        public SekretarAzuriranjeAlergena(Pacijent p)
        {
            InitializeComponent();
            bp = new BazaPacijenata();
            pacijent = p;

            this.DataContext = this;
            pacijentTxt.Text = p.Ime + " " + p.Prezime;

            AlergeniPacijenta = new ObservableCollection<string>(p.Alergeni);
            lvAlergeni.ItemsSource = AlergeniPacijenta;
        }

        private void Button_Click_Dodaj(object sender, RoutedEventArgs e)
        {
            AlergeniPacijenta.Add(alergenTxt.Text);
            alergenTxt.Text = "";
        }

        private void Button_Click_Obrisi(object sender, RoutedEventArgs e)
        {
            int index = lvAlergeni.SelectedIndex;
            if (index != -1)
            {
                AlergeniPacijenta.RemoveAt(index);
            }
        }

        private void alergenTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            if (alergenTxt.Text.Equals("Unos novog alergena"))
            {
                alergenTxt.Text = "";
            }  
        }

        private void alergenTxt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (alergenTxt.Text.Equals(""))
            {
                alergenTxt.Text = "Unos novog alergena";
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            pacijent.Alergeni = AlergeniPacijenta.ToList();
            // u ovom slucaju se nikada nece menjati jmbg pa je moguce staviti istu instancu za oba parametra
            bp.IzmeniPacijenta(pacijent, pacijent);
        }
    }
}
