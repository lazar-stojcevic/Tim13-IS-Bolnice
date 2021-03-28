using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for LekarIzmenaPregleda.xaml
    /// </summary>
    public partial class LekarIzmenaPregleda : Window
    {
        public DateTime StariDatum { get; set; }
        public string StariSat { get; set; }
        public string StariMinut { get; set; }

        public LekarIzmenaPregleda()
        {
            InitializeComponent();
            BazaLekara baza = new BazaLekara();
            foreach (Lekar p in baza.SviLekari())
            {
                string podaci = p.Ime + " " + p.Prezime + " " + p.Jmbg;
                listaLekara.Items.Add(podaci);
            }
            BazaBolnica bazaBolnica = new BazaBolnica();
            foreach (Bolnica b in bazaBolnica.SveBolnice())
            // TODO: TREBA DA SE DODA PROVERA ZA TRENUTNU BOLNICU
            {
                foreach (Soba s in b.Soba)
                {
                    comboBoxSale.Items.Add(s.Id + " " + s.Kvadratura + "m^2" + " " + s.Tip.ToString());
                }
            }
        }

        private void Button_ClickIzmeni(object sender, RoutedEventArgs e)
        {
            BazaPregleda baza = new BazaPregleda();
            List<Pregled> lista = baza.SviSledeciPregledi();
            File.WriteAllText(@"..\..\Serijalizacija\pregledi.txt", String.Empty);

            foreach (Pregled p in lista)
            {
                if (txtOperJmbg.Text.Equals(p.Pacijent.Jmbg) && p.VremePocetkaPregleda.Hour == Int32.Parse(StariSat) && p.VremePocetkaPregleda.Date.Equals(StariDatum))
                {

                    string idLekara = listaLekara.SelectedItem.ToString().Split(' ')[2];
                    //TODO: OVAJ DEO MORA DA SE VALIDIRA ALI ZA SAD JE OK
                    DateTime pocetak = new DateTime(kalendar.SelectedDate.Value.Year, kalendar.SelectedDate.Value.Month,
                        kalendar.SelectedDate.Value.Day, Int32.Parse(txtHour.Text), Int32.Parse(txtMinute.Text), 0);
                    DateTime kraj = new DateTime(kalendar.SelectedDate.Value.Year, kalendar.SelectedDate.Value.Month,
                        kalendar.SelectedDate.Value.Day, Int32.Parse(txtHour.Text), Int32.Parse(txtMinute.Text), 0);
                    kraj = kraj.AddMinutes(45); //Predpostavka da ce pregled trajati 45 minuta 
                    p.Lekar.Jmbg = idLekara;
                    p.Pacijent.Jmbg = txtOperJmbg.Text;
                    p.VremePocetkaPregleda = pocetak;
                    p.VremeKrajaPregleda = kraj;
                    baza.ZakaziPregled(p);
                }
                else
                {
                    baza.ZakaziPregled(p);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
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

        private void Button_Click_3(object sender, RoutedEventArgs e)
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

        private void Button_Click_4(object sender, RoutedEventArgs e)
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
