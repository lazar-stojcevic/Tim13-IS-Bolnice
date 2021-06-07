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

namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for OdredjivanjeTermina.xaml
    /// </summary>
    public partial class OdredjivanjeTermina : Window
    {
        private List<DateTime> termin;

        public OdredjivanjeTermina(List<DateTime> terminParam, Lekar lekar)
        {
            InitializeComponent();

            termin = terminParam;

            if (termin.Count != 0)
            {
                kalendar.SelectedDate = termin[0];
            }

            lekarTxt.Text = lekar.Ime + " " + lekar.Prezime;
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime pocetakTermina = new DateTime(kalendar.SelectedDate.Value.Year, kalendar.SelectedDate.Value.Month,
                    kalendar.SelectedDate.Value.Day, Int32.Parse(txtHour.Text), Int32.Parse(txtMinute.Text), 0);
                DateTime krajTermina = new DateTime(kalendar.SelectedDate.Value.Year, kalendar.SelectedDate.Value.Month,
                    kalendar.SelectedDate.Value.Day, Int32.Parse(txtHour.Text), Int32.Parse(txtMinute.Text), 0);
                krajTermina = krajTermina.AddMinutes(45); //Predpostavka da ce pregled trajati 45 minuta 

                if (pocetakTermina < DateTime.Now)
                {
                    MessageBox.Show("Nemoguće je odabrati vreme pre sadašnjeg vremena.");
                    return;
                }

                if (termin.Count == 0)
                {
                    termin.Add(pocetakTermina);
                    termin.Add(krajTermina);
                }
                else
                {
                    termin[0] = pocetakTermina;
                    termin[1] = krajTermina;
                }
                this.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Nevalidno unesen termin.");
            }
        }

        private void Button_Click_Sati_Plus(object sender, RoutedEventArgs e)
        {
            int sati = Int32.Parse(txtHour.Text);
            sati += 1;

            if (sati > 23)
                sati = 0;

            if (sati == 0)
            {
                txtHour.Text = "00";
            }
            else
            {
                txtHour.Text = sati.ToString();
            }
        }

        private void Button_Click_Sati_Minus(object sender, RoutedEventArgs e)
        {
            int sati = Int32.Parse(txtHour.Text);
            sati -= 1;

            if (sati < 0)
                sati = 23;

            if (sati == 0)
            {
                txtHour.Text = "00";
            }
            else
            {
                txtHour.Text = sati.ToString();
            }
        }

        private void Button_Click_Minuti_Plus(object sender, RoutedEventArgs e)
        {
            int min = Int32.Parse(txtMinute.Text);
            min += 1;

            if (min > 59)
                min = 0;

            if (min == 0)
            {
                txtMinute.Text = "00";
            }
            else
            {
                txtMinute.Text = min.ToString();
            }
        }

        private void Button_Click_Minuti_Minus(object sender, RoutedEventArgs e)
        {
            int min = Int32.Parse(txtMinute.Text);
            min -= 1;

            if (min < 0)
                min = 59;

            if (min == 0)
            {
                txtMinute.Text = "00";
            }
            else
            {
                txtMinute.Text = min.ToString();
            }
        }
    }
}
