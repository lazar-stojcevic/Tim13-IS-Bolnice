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

namespace IS_Bolnice.Prozori.Sekretar
{
    /// <summary>
    /// Interaction logic for GodisniOdmorLekara.xaml
    /// </summary>
    public partial class GodisniOdmorLekara : Window
    {
        private Lekar selektovaniLekar;

        public GodisniOdmorLekara(Lekar lekar)
        {
            InitializeComponent();

            selektovaniLekar = lekar;
            tbLekar.Text = lekar.Ime + " " + lekar.Prezime;
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            DateTime pocetak = DateTime.Parse(datePicker_pocetak.SelectedDate.ToString());
            DateTime kraj = DateTime.Parse(datePicker_kraj.SelectedDate.ToString());
            if (!Validno(pocetak, kraj))
            {
                MessageBox.Show("Nevalidan unos datuma godišnjeg odmora.");
                return;
            }
            // TODO: implementirati analizu za slobodne dane i belezenje ako je to moguce i vracanje poruke
        }

        private bool Validno(DateTime pocetak, DateTime kraj)
        {
            return pocetak < kraj;
        }
    }
}
