using IS_Bolnice.Model;
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
    /// Interaction logic for IzmenaOperacije.xaml
    /// </summary>
    public partial class IzmenaOperacije : Window
    {
        List<Lekar> lekariSpecijalisti = new List<Lekar>();
        BazaPregleda bp = new BazaPregleda();
        BazaOperacija bo = new BazaOperacija();
        BazaLekara bl = new BazaLekara();
        List<Operacija> operacije = new List<Operacija>();

        public DateTime StariDatum  { get; set; }
        public string StariSat { get; set; }

        public string StariMinut { get; set; }
        public IzmenaOperacije()
        {
            InitializeComponent();
            List<Lekar> sviLekari = bl.SviLekari();
            foreach (Lekar p in sviLekari)
            {
                // lekari specijalisi
                if (!p.JelLekarOpstePrakse())
                {
                    string podaci = p.Ime + " " + p.Prezime + " " + p.Jmbg + " " + p.Oblast.Naziv;
                    listaLekara.Items.Add(podaci);
                    lekariSpecijalisti.Add(p);
                }

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
            foreach (Operacija operacija in lista)
            {
                if (txtOperJmbg.Text.Equals(operacija.Pacijent.Jmbg) && operacija.VremePocetkaOperacije.Hour == Int32.Parse(StariSat) && operacija.VremePocetkaOperacije.Date.Equals(StariDatum))
                {
                    string idLekara = listaLekara.SelectedItem.ToString().Split(' ')[2];
                    string idSale = comboBoxSale.SelectedItem.ToString().Split(' ')[0];
                    Operacija operacijaSelektovana = operacije.ElementAt(terminiList.SelectedIndex);
                    //TODO: OVAJ DEO MORA DA SE VALIDIRA ALI ZA SAD JE OK
                    DateTime pocetak = new DateTime(operacijaSelektovana.VremePocetkaOperacije.Year, operacijaSelektovana.VremePocetkaOperacije.Month,
                operacijaSelektovana.VremePocetkaOperacije.Day, operacijaSelektovana.VremePocetkaOperacije.Hour, operacijaSelektovana.VremePocetkaOperacije.Minute, 0);
                    DateTime kraj = new DateTime(operacijaSelektovana.VremeKrajaOperacije.Year, operacijaSelektovana.VremeKrajaOperacije.Month,
                operacijaSelektovana.VremeKrajaOperacije.Day, operacijaSelektovana.VremeKrajaOperacije.Hour, operacijaSelektovana.VremeKrajaOperacije.Minute, 0);
                    kraj = kraj.AddMinutes(45); //Predpostavka da ce operacija trajati 45 minuta 

                    operacija.Lekar.Jmbg = idLekara;
                    operacija.Pacijent.Jmbg = txtOperJmbg.Text;
                    operacija.Soba.Id = idSale;
                    operacija.VremePocetkaOperacije = pocetak;
                    operacija.VremeKrajaOperacije = kraj;
                    operacija.Hitna = (bool)boxHitno.IsChecked;

                    baza.ZakaziOperaciju(operacija);
                }
                else
                {
                    baza.ZakaziOperaciju(operacija);
                }
            }
            MessageBox.Show("Operacijacija uspešno izmenjena", "Izmenjena operacija", MessageBoxButton.OK, MessageBoxImage.Information);
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

            string jmbgLekara = lekariSpecijalisti.ElementAt(listaLekara.SelectedIndex).Jmbg;
            string idSale = comboBoxSale.SelectedItem.ToString().Split(' ')[0];
            operacije = bo.PonudjeniSlobodniTerminiLekara(jmbgLekara, idSale);

            terminiList.Items.Clear();

            foreach (Operacija operacija in operacije)
            {
                terminiList.Items.Add(operacija.VremePocetkaOperacije);
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
