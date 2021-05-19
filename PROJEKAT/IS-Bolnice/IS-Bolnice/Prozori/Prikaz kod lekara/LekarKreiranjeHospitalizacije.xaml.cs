using IS_Bolnice.Baze;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarKreiranjeHospitalizacije.xaml
    /// </summary>
    public partial class LekarKreiranjeHospitalizacije : Page
    {
        private List<Soba> listaSvihSoba = new List<Soba>();
        public LekarKreiranjeHospitalizacije(string sifra)
        {
            InitializeComponent();
            txtJMBG.Text = sifra;
            BazaBolnica bazaBolnica = new BazaBolnica();
            foreach (Bolnica b in bazaBolnica.SveBolnice())
            {
                foreach (Soba s in b.Soba)
                {
                    if (!s.Zauzeta && s.Tip.Equals(RoomType.bolnickaSoba))
                        listaSvihSoba.Add(s);
                }
            }
            datumKraja.SelectedDate = DateTime.Today.AddDays(1);
            listaSoba.ItemsSource = listaSvihSoba;
        }

        private void ButtonClick_Zakazi(object sender, RoutedEventArgs e)
        {
            if (listaSoba.SelectedIndex != -1)
            {
                Soba soba = (Soba) listaSoba.SelectedItem;
                Hospitalizacija hos = new Hospitalizacija(txtJMBG.Text, soba.Id, datumKraja.SelectedDate.Value);

                BazaHospitalizacija bazaHospitalizacija = new BazaHospitalizacija();
                if (bazaHospitalizacija.KreirajHospitalizaciju(hos))
                {
                    MessageBox.Show("Pacijent poslan na hospitalizaciju", "Kreirana hospitalizacija",
                        MessageBoxButton.OK);
                    NavigationService.GoBack();
                }
                else
                {
                    MessageBox.Show("Pacijent je već na hospitalizaciji", "Nije kreirana hospitalizacija",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Nije selektovana ni jedna soba", "Nije kreirana hospitalizacija",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ButtonClick_Odustani(object sender, RoutedEventArgs e)
        {

        }
    }
}
