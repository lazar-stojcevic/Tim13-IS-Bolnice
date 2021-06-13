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
using System.Windows.Shapes;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Kontroleri.Termini;

namespace IS_Bolnice.Prozori.Prozori_za_pacijenta
{
    public partial class ZakazivanjeUOdredjenomTerminu : Window
    {
        private string jmbgPac;

        private PregledKontroler pregledKontroler = new PregledKontroler();
        private IzmenaTerminaKontroler izmenaTerminaKontroler = new IzmenaTerminaKontroler();
        public ZakazivanjeUOdredjenomTerminu(string jmbgPacijenta)
        {
            InitializeComponent();
            jmbgPac = jmbgPacijenta;
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

        private void btnUpMin_Click(object sender, RoutedEventArgs e)
        {
            int min = Int32.Parse(txtMinute.Text);
            min += 10;

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
            min -= 10;

            if (min < 0)
                min = 50;

            if (min == 0)
            {
                txtMinute.Text = "00";
            }
            else
            {
                txtMinute.Text = min.ToString();
            }
        }

        private void btnDownHrs_Click(object sender, RoutedEventArgs e)
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

        private void btnPotvrdi_Click(object sender, RoutedEventArgs e)
        {
            string vreme = datePicker.SelectedDate.ToString();
            DateTime piker = DateTime.Parse(vreme);
            DateTime now = DateTime.Now;

            if (piker.Day == now.Day)
            {
                string message = "Molimo vas da pomerite datum bar za jedan dan";
                MessageBox.Show(message);
            }
            else
            {
                Pregled pregled = new Pregled();
                Pacijent pacijent = new Pacijent();

                System.DateTime pocetakPregleda = new System.DateTime(piker.Year, piker.Month, piker.Day, Int32.Parse(txtHour.Text), Int32.Parse(txtMinute.Text), 0, 0);
                DateTime krajPregleda = pocetakPregleda.AddMinutes(30);

                pacijent.Jmbg = jmbgPac;
                pregled.Pacijent = pacijent;
                pregled.VremePocetkaPregleda = pocetakPregleda;
                pregled.VremeKrajaPregleda = krajPregleda;

                if (izmenaTerminaKontroler.DaLiJePacijentMaliciozan(pregled.Pacijent))
                {
                    MessageBox.Show("Izvinjavamo se, ali previše puta ste vršili izmene tokom protekle nedelje");
                }
                else
                {
                    if (pregledKontroler.PacijentImaZakazanPregled(pregled))
                    {
                        string message = "Već imate zakazani pregled u tom terminu";
                        MessageBox.Show(message);
                    }
                    else
                    {
                        pregled = pregledKontroler.PostaviPrvogSlobodnogLekaraOpstePrakseNaPregled(pregled);
                        if (pregledKontroler.ZakaziPregled(pregled))
                        {
                            string message = "Uspešno ste zakazali pregled";
                            MessageBox.Show(message);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Za uneseni termin su svi lekari zauzeti");
                        }
                    }
                }

            }
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
