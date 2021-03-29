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
    public partial class PacijentZakazivanjePregledaProzor : Window
    {
        private string jmbgPac;
        private List<Lekar> lekari = new List<Lekar>();


        public PacijentZakazivanjePregledaProzor(string jmbgPacijenta)
        {
            InitializeComponent();
            jmbgPac = jmbgPacijenta;
            BazaLekara bl = new BazaLekara();
            lekari = bl.LekariOpstePrakse();
            
            foreach(Lekar l in lekari)
            {
                string imePrezime = l.Ime + " " + l.Prezime;
                comboLekari.Items.Add(imePrezime);
            }
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
            BazaPregleda bp = new BazaPregleda();
            // index selektovanog lekara
            int redniBrojLekara = comboLekari.SelectedIndex;

            //vreme pocetka i kraj pregleda
            DateTime s = datePicker.SelectedDate.Value.Date;
            System.DateTime selectedTime = new System.DateTime(s.Year, s.Month, s.Day, Int32.Parse(txtHour.Text), Int32.Parse(txtMinute.Text), 0, 0);
            DateTime selectedTimeEnd = selectedTime.AddMinutes(30);
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

                bp.ZakaziPregled(pregled);
                MessageBox.Show("Uspesno ste zakazali pregled");
            }
            this.Close();
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
