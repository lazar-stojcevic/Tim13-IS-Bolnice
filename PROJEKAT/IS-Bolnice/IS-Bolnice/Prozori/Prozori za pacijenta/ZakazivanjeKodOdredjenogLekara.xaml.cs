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

namespace IS_Bolnice.Prozori.Prozori_za_pacijenta
{
    public partial class ZakazivanjeKodOdredjenogLekara : Window
    {
        private string jmbgPac;

        private List<Lekar> lekari = new List<Lekar>();
        private List<Pregled> pregledi = new List<Pregled>();

        private PregledKontroler pregledKontroler = new PregledKontroler();
        private LekarKontroler lekarKontroler = new LekarKontroler();
        private BazaPregleda bazaPregleda = new BazaPregleda();
        private BazaIzmena bazaIzmena = new BazaIzmena();

        public ZakazivanjeKodOdredjenogLekara(string jmbgPacijenta)
        {
            InitializeComponent();
            jmbgPac = jmbgPacijenta;

            LekarFajlRepozitorijum bl = new LekarFajlRepozitorijum();
            lekari = bl.LekariOpstePrakse();

            foreach (Lekar l in lekari)
            {
                string imePrezime = l.Ime + " " + l.Prezime;
                lekariList.Items.Add(imePrezime);
            }

        }

        private void potvrdi_Click(object sender, RoutedEventArgs e)
        {
            Pacijent pacijent = new Pacijent();
            pacijent.Jmbg = jmbgPac;

            Pregled pregled = pregledi.ElementAt(terminiList.SelectedIndex);
            pregled.Pacijent = pacijent;

            if (bazaIzmena.IsPatientMalicious(pregled.Pacijent))
            {
                string message = "Izvinjavamo se, ali previše puta ste vršili izmene tokom protekle nedelje";
                MessageBox.Show(message);
            }
            else
            {
                if (pregledKontroler.PacijentImaZakazanPregled(pregled))
                {
                    string message = "Već imate zakazan pregled u tom terminu";
                    MessageBox.Show(message);
                }
                else
                {
                    bazaPregleda.ZakaziPregled(pregled);
                    string message = "Uspešno ste zakazali pregled";
                    MessageBox.Show(message);
                    this.Close();
                }
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

            pregledi = pregledKontroler.GetDostupniTerminiPregledaLekaraUNarednomPeriodu(lekarKontroler.GetLekar(jmbgLekara));

            terminiList.Items.Clear();

            foreach (Pregled p in pregledi)
            {
                terminiList.Items.Add(p.VremePocetkaPregleda);
            }
        }

        private void terminiList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (terminiList.SelectedIndex == -1 || lekariList.SelectedIndex == -1)
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
