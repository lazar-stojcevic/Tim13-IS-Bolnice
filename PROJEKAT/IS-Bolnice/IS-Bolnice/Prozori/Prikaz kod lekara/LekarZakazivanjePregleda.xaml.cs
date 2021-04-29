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
    /// <summary>
    /// Interaction logic for LekarZakazivanjePregleda.xaml
    /// </summary>
    public partial class LekarZakazivanjePregleda : Window
    {
        private string jmbgPac;
        private List<Lekar> lekari = new List<Lekar>();
        BazaPregleda bp = new BazaPregleda();
        List<Pregled> pregledi = new List<Pregled>();
        public LekarZakazivanjePregleda()
        {
            InitializeComponent();
            BazaLekara baza = new BazaLekara();
            foreach (Lekar p in baza.SviLekari())
            {
                string podaci = p.Ime + " " + p.Prezime + " " + p.Jmbg + " " + p.Tip.ToString();
                listaLekara.Items.Add(podaci);
            }

            BazaLekara bl = new BazaLekara();
            lekari = bl.SviLekari();
        }

        private void Button_ClickZakazi(object sender, RoutedEventArgs e)
        {
            BazaPregleda bazaPregleda = new BazaPregleda();
            Pregled o = new Pregled();
            o.Lekar = new Lekar();
            o.Pacijent = new Pacijent();

            string idLekara = listaLekara.SelectedItem.ToString().Split(' ')[2];

            foreach (Lekar iterLekar in lekari)
            {
                if (iterLekar.Jmbg.Equals(idLekara)){
                    o.Lekar.Ordinacija = iterLekar.Ordinacija;
                    break;
                }
            }

            //TODO: OVAJ NIJE OPTIMALNO ALI STA SAD
            Pregled pregled = pregledi.ElementAt(terminiList.SelectedIndex);

            DateTime pocetak = new DateTime(pregled.VremePocetkaPregleda.Year, pregled.VremePocetkaPregleda.Month,
                //OBAVEZNOOOOOOOOOOOOOOOOOOOOOOOOOOOOO POGLEDATI
                pregled.VremePocetkaPregleda.Day, pregled.VremePocetkaPregleda.Hour, pregled.VremePocetkaPregleda.Minute, 0);
            DateTime kraj = new DateTime(pregled.VremeKrajaPregleda.Year, pregled.VremeKrajaPregleda.Month,
               pregled.VremeKrajaPregleda.Day, pregled.VremeKrajaPregleda.Hour, pregled.VremeKrajaPregleda.Minute, 0);
            o.Lekar.Jmbg = idLekara;
            o.Pacijent.Jmbg = txtOperJmbg.Text;
            o.VremePocetkaPregleda = pocetak;
            o.VremeKrajaPregleda = kraj;
            bazaPregleda.ZakaziPregled(o);
            MessageBox.Show("Pregled uspešno kreiran", "Kreiran pregled", MessageBoxButton.OK, MessageBoxImage.Information);
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
