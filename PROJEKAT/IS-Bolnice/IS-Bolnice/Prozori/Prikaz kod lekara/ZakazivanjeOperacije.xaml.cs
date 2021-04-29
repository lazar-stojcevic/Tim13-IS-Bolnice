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

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    public partial class ZakazivanjeOperacije : Window
    {
        List<Lekar> lekariSpecijalisti = new List<Lekar>();
        BazaPregleda bp = new BazaPregleda();
        BazaOperacija bo = new BazaOperacija();
        List<Operacija> operacije = new List<Operacija>();
        public ZakazivanjeOperacije()
        {
            InitializeComponent();
            BazaLekara bl = new BazaLekara();
            List<Lekar> sviLekari = bl.SviLekari();
            foreach (Lekar p in sviLekari)
            {
                if (p.Tip.Equals(TipLekara.lekarSpecijalista))
                {
                    string podaci = p.Ime + " " + p.Prezime + " " + p.Jmbg;
                    listaLekara.Items.Add(podaci);
                    lekariSpecijalisti.Add(p);
                }
                
            }

            BazaBolnica bazaBolnica = new BazaBolnica();
            foreach (Bolnica b in bazaBolnica.SveBolnice())
            {
                foreach(Soba s in b.Soba)
                {
                    comboBoxSale.Items.Add(s.Id + " " + s.Kvadratura+ "m^2"+" "+ s.Tip.ToString());
                }
            }
            }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BazaOperacija baza = new BazaOperacija();
            Operacija operacija = operacije.ElementAt(terminiList.SelectedIndex);
            string idLekara = listaLekara.SelectedItem.ToString().Split(' ')[2];
            string idSale = comboBoxSale.SelectedItem.ToString().Split(' ')[0];
            //TODO: OVAJ DEO MORA DA SE VALIDIRA ALI ZA SAD JE OK

            DateTime pocetak = new DateTime(operacija.VremePocetkaOperacije.Year, operacija.VremePocetkaOperacije.Month,
                operacija.VremePocetkaOperacije.Day, operacija.VremePocetkaOperacije.Hour, operacija.VremePocetkaOperacije.Minute, 0);
            DateTime kraj = new DateTime(operacija.VremeKrajaOperacije.Year, operacija.VremeKrajaOperacije.Month,
                operacija.VremeKrajaOperacije.Day, operacija.VremeKrajaOperacije.Hour, operacija.VremeKrajaOperacije.Minute, 0);
            kraj = kraj.AddMinutes(45);
            operacija.Lekar.Jmbg = idLekara;
            operacija.Pacijent.Jmbg = txtOperJmbg.Text;
            operacija.Soba.Id = idSale;
            operacija.VremePocetkaOperacije = pocetak;
            operacija.VremeKrajaOperacije = kraj;
            baza.ZakaziOperaciju(operacija);
            MessageBox.Show("Operacijacija uspešno kreirana", "Kreirana operacija", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
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
