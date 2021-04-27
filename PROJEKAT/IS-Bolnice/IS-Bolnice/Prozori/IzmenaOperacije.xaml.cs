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
    /// Interaction logic for IzmenaOperacije.xaml
    /// </summary>
    public partial class IzmenaOperacije : Window
    {

        public DateTime StariDatum  { get; set; }
        public string StariSat { get; set; }

        public string StariMinut { get; set; }
        public IzmenaOperacije()
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
            BazaOperacija baza = new BazaOperacija();
            List<Operacija> lista = baza.SveSledeceOperacije();
            File.WriteAllText(@"..\..\Datoteke\operacije.txt", String.Empty);
            foreach (Operacija o in lista)
            {
                if (txtOperJmbg.Text.Equals(o.Pacijent.Jmbg) && o.VremePocetkaOperacije.Hour == Int32.Parse(StariSat) && o.VremePocetkaOperacije.Date.Equals(StariDatum))
                {
                    string idLekara = listaLekara.SelectedItem.ToString().Split(' ')[2];
                    string idSale = comboBoxSale.SelectedItem.ToString().Split(' ')[0];
                    //TODO: OVAJ DEO MORA DA SE VALIDIRA ALI ZA SAD JE OK
                    DateTime pocetak = new DateTime(kalendar.SelectedDate.Value.Year, kalendar.SelectedDate.Value.Month,
                        kalendar.SelectedDate.Value.Day, Int32.Parse(txtHour.Text), Int32.Parse(txtMinute.Text), 0);
                    DateTime kraj = new DateTime(kalendar.SelectedDate.Value.Year, kalendar.SelectedDate.Value.Month,
                        kalendar.SelectedDate.Value.Day, Int32.Parse(txtHour.Text), Int32.Parse(txtMinute.Text), 0);
                    kraj = kraj.AddMinutes(45); //Predpostavka da ce operacija trajati 45 minuta 
                    o.Lekar.Jmbg = idLekara;
                    o.Pacijent.Jmbg = txtOperJmbg.Text;
                    o.Soba.Id = idSale;
                    o.VremePocetkaOperacije = pocetak;
                    o.VremeKrajaOperacije = kraj;
                    baza.ZakaziOperaciju(o);
                }
                else
                {
                    baza.ZakaziOperaciju(o);
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
