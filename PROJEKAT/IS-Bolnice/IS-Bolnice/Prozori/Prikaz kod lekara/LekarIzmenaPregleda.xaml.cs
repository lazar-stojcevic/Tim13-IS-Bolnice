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

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarIzmenaPregleda.xaml
    /// </summary>
    public partial class LekarIzmenaPregleda : Window
    {
        public DateTime StariDatum { get; set; }
        public string StariSat { get; set; }
        public string StariMinut { get; set; }

        private string jmbgPac;
        private List<Lekar> lekari = new List<Lekar>();
        BazaPregleda bp = new BazaPregleda();
        List<Pregled> pregledi = new List<Pregled>();
        public LekarIzmenaPregleda()
        {
            InitializeComponent();
            BazaLekara baza = new BazaLekara();
            foreach (Lekar lekar in baza.SviLekari())
            {
                string podaci = lekar.Ime + " " + lekar.Prezime + " " + lekar.Jmbg;
                listaLekara.Items.Add(podaci);
                lekari.Add(lekar);
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
            File.WriteAllText(@"..\..\Datoteke\pregledi.txt", String.Empty);

            foreach (Pregled p in lista)
            {
                if (txtOperJmbg.Text.Equals(p.Pacijent.Jmbg) && p.VremePocetkaPregleda.Hour == Int32.Parse(StariSat) && p.VremePocetkaPregleda.Date.Equals(StariDatum))
                {

                    string idLekara = listaLekara.SelectedItem.ToString().Split(' ')[2];
                    Pregled pregled = pregledi.ElementAt(terminiList.SelectedIndex);

                    //TODO: OVAJ DEO MORA DA SE VALIDIRA ALI ZA SAD JE OK
                    DateTime pocetak = new DateTime(pregled.VremePocetkaPregleda.Year, pregled.VremePocetkaPregleda.Month,
                        pregled.VremePocetkaPregleda.Day, pregled.VremePocetkaPregleda.Hour, pregled.VremePocetkaPregleda.Minute, 0);
                    DateTime kraj = new DateTime(pregled.VremeKrajaPregleda.Year, pregled.VremeKrajaPregleda.Month,
                       pregled.VremeKrajaPregleda.Day, pregled.VremeKrajaPregleda.Hour, pregled.VremeKrajaPregleda.Minute, 0);
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
            MessageBox.Show("Pregled uspešno izmenjen", "Izmenjen pregled", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lekariList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaLekara.SelectedIndex == -1) { return; }
            if (terminiList.SelectedIndex == -1 || listaLekara.SelectedIndex == -1)
            {
                potvrdi.IsEnabled = false;
            }
            else
            {
                potvrdi.IsEnabled = true;
            }

            string jmbgLekara = lekari.ElementAt(listaLekara.SelectedIndex).Jmbg;
            pregledi = bp.PonudjeniSlobodniPreglediLekara(jmbgLekara);

            terminiList.Items.Clear();

            foreach (Pregled p in pregledi)
            {
                terminiList.Items.Add(p.VremePocetkaPregleda);
            }

            if (terminiList.Items.Count != 0)
            {
                terminiList.SelectedIndex = 0;
            }

            if (terminiList.SelectedIndex == -1 || listaLekara.SelectedIndex == -1)
            {
                potvrdi.IsEnabled = false;
            }
            else
            {
                potvrdi.IsEnabled = true;
            }

        }

    }
}
