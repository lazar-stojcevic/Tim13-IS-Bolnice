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
    public partial class PacijentIzmenaTermina : Window
    {
        private string jmbgPac;
        private List<Lekar> lekari = new List<Lekar>();

        private Pregled stariPregled;

        public PacijentIzmenaTermina(string jmbgPacijenta, string jmbgLekara,string vreme, DateTime datum)
        {
            InitializeComponent();
            
            BazaLekara bl = new BazaLekara();
            lekari = bl.LekariOpstePrakse();

            // prikaz lekara, termina, ...
            int i = 0;
            foreach (Lekar l in lekari)
            {
                string imePrezime = l.Ime + " " + l.Prezime;
                comboLekari.Items.Add(imePrezime);
                
                if(l.Jmbg == jmbgLekara)
                {
                    comboLekari.SelectedIndex = i;
                }
                i += 1;
            }

            datePicker.SelectedDate = datum;

            string[] items = vreme.Split(':');
            txtHour.Text = items[0];
            txtMinute.Text = items[1];

            jmbgPac = jmbgPacijenta;

            stariPregled = new Pregled();
            Lekar ll = new Lekar();
            ll.Jmbg = jmbgLekara;
            Pacijent p = new Pacijent();
            p.Jmbg = jmbgPacijenta;

            stariPregled.Lekar = ll;
            stariPregled.pacijent = p;
            stariPregled.VremePocetkaPregleda = datum;
        }

        private void btnUpHrs_Click(object sender, RoutedEventArgs e)
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

        private void btnDownHrs_Click(object sender, RoutedEventArgs e)
        {
            int sati = Int32.Parse(txtHour.Text);
            sati -= 1;

            if (sati < 0)
                sati = 59;

            if (sati == 0)
            {
                txtHour.Text = "00";
            }
            else
            {
                txtHour.Text = sati.ToString();
            }
        }

        private void btnUpMin_Click(object sender, RoutedEventArgs e)
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

        private void btnDownMin_Click(object sender, RoutedEventArgs e)
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

        private void btnPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            // DODATI PROVERE I POZVATI METODU IZMENI

            BazaPregleda bp = new BazaPregleda();
            // index selektovanog lekara
            int redniBrojLekara = comboLekari.SelectedIndex;

            //vreme pocetka i kraj pregleda
            DateTime s = datePicker.SelectedDate.Value.Date;
            System.DateTime selectedTime = new System.DateTime(s.Year, s.Month, s.Day, Int32.Parse(txtHour.Text), Int32.Parse(txtMinute.Text), 0, 0);

            DateTime selectedTimeEnd = selectedTime.AddMinutes(30);
            stariPregled.VremeKrajaPregleda = stariPregled.VremePocetkaPregleda.AddMinutes(30);

            DateTime currTimeNow = DateTime.Now;

            //-1 u zagradi je buduce vreme
            int comparation = currTimeNow.CompareTo(selectedTime);

            // provera da li je zakazivanje za tekuci dan 
            int isSameDay = Int32.Parse(s.Day.ToString()) - Int32.Parse(currTimeNow.Day.ToString());

            // odabran termin je u buducnost, nije ovoga dana i lekar je selektovan
            if (comparation == -1 && isSameDay != 0 && comboLekari.SelectedIndex != -1)
            {
                Pregled pregled = new Pregled();
                Pacijent pacijent = new Pacijent();
                pacijent.Jmbg = jmbgPac;
                pregled.VremePocetkaPregleda = selectedTime;
                pregled.VremeKrajaPregleda = selectedTimeEnd;
                pregled.Lekar = lekari.ElementAt(comboLekari.SelectedIndex);
                pregled.Pacijent = pacijent;
                bp.IzmeniPregled(pregled, stariPregled);

                this.Close();
            }
            
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

