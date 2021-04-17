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

namespace IS_Bolnice.Prozori.Prozori_za_pacijenta
{
    public partial class ZakazivanjeKodOdredjenogLekara : Window
    {
        private string jmbgPac;
        private List<Lekar> lekari = new List<Lekar>();
        BazaPregleda bp = new BazaPregleda();
        List<Pregled> pregledi = new List<Pregled>();
        public ZakazivanjeKodOdredjenogLekara(string jmbgPacijenta)
        {
            InitializeComponent();
            jmbgPac = jmbgPacijenta;

            BazaLekara bl = new BazaLekara();
            lekari = bl.LekariOpstePrakse();

            foreach (Lekar l in lekari)
            {
                string imePrezime = l.Ime + " " + l.Prezime;
                lekariList.Items.Add(imePrezime);
            }

        }

        private void potvrdi_Click(object sender, RoutedEventArgs e)
        {
            Pregled pregled = pregledi.ElementAt(terminiList.SelectedIndex);
            Pacijent pacijent = new Pacijent();
            pacijent.Jmbg = jmbgPac;
            pregled.Pacijent = pacijent;
            if (bp.PacijentImaZakazanPregled(pregled))
            {
                string message = "Već imate zakazan pregled u tom terminu";
                MessageBox.Show(message);
            }
            else
            {
                bp.ZakaziPregled(pregled);
                string message = "Uspešno ste zakazali pregled";
                MessageBox.Show(message);
                this.Close();
            }

        }

        private void odustani_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lekariList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (terminiList.SelectedIndex == -1 || lekariList.SelectedIndex == -1)
            {
                potvrdi.IsEnabled = false;
            }
            else
            {
                potvrdi.IsEnabled = true;
            }

            string jmbgLekara = lekari.ElementAt(lekariList.SelectedIndex).Jmbg;

            pregledi = bp.PonudjeniSlobodniPreglediLekara(jmbgLekara);

            terminiList.Items.Clear();

            foreach (Pregled p in pregledi)
            {
                terminiList.Items.Add(p.VremePocetkaPregleda);
            }
        }

        private void terminiList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (terminiList.SelectedIndex == -1 || lekariList.SelectedIndex == -1) {
                potvrdi.IsEnabled = false;
            }
            else {
                potvrdi.IsEnabled = true;
            }
        }
    }
}
