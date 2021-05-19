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
    public partial class RadnoVremeLekara : Window
    {
        private Lekar selektovaniLekar;

        public RadnoVremeLekara(Lekar lekar)
        {
            InitializeComponent();

            selektovaniLekar = lekar;
            tbLekar.Text = lekar.Ime + " " + lekar.Prezime;

            PopuniComboBoxSati();
            PopuniComboBoxMinuta();
        }

        private void PopuniComboBoxSati()
        {
            List<int> trajanja = new List<int>();
            for (int i = 0; i <= 24; i += 1)
            {
                trajanja.Add(i);
            }

            cbSatiPocetak.ItemsSource = trajanja.GetRange(0, 24);
            cbSatiKraj.ItemsSource = trajanja.GetRange(1, 24);
        }

        private void PopuniComboBoxMinuta()
        {
            List<int> trajanja = new List<int>();
            for (int i = 0; i <= 60; i += 10)
            {
                trajanja.Add(i);
            }

            cbMinutiPocetak.ItemsSource = trajanja;
            cbMinutiKraj.ItemsSource = trajanja;

            cbMinutiPocetak.SelectedIndex = 0;
            cbMinutiKraj.SelectedIndex = 0;
        }

        private void Button_Click_Godisnji(object sender, RoutedEventArgs e)
        {
            GodisniOdmorLekara gol = new GodisniOdmorLekara(selektovaniLekar);
            gol.ShowDialog();
        }

        private void cbSatiKraj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((int)cbSatiKraj.SelectedValue == 24)
            {
                cbMinutiKraj.SelectedIndex = 0;
                cbMinutiKraj.IsEnabled = false;
            }
            else
            {
                cbMinutiKraj.IsEnabled = true;
            }
        }
    }
}
