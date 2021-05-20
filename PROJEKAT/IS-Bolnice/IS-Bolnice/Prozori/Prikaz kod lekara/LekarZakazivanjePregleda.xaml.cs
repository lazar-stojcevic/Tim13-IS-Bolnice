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

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarZakazivanjePregleda.xaml
    /// </summary>
    public partial class LekarZakazivanjePregleda : Window
    {
        private List<Lekar> lekari = new List<Lekar>();

        private PregledKontroler pregledKontroler = new PregledKontroler();

        List<Pregled> pregledi = new List<Pregled>();
        public LekarZakazivanjePregleda()
        {
            InitializeComponent();
            LekarKontroler lekarKontroler = new LekarKontroler();
            foreach (Lekar lekar in lekarKontroler.GetSviLekari())
            {
                string podaci = lekar.Ime + " " + lekar.Prezime + " " + lekar.Jmbg + " " + lekar.Oblast.Naziv;
                listaLekara.Items.Add(podaci);
            }

            lekari = lekarKontroler.GetSviLekari();
        }

        private void Button_ClickZakazi(object sender, RoutedEventArgs e)
        {
            Pregled noviPregled = KreirajNoviPregled();
            pregledKontroler.ZakaziPregled(noviPregled);
            MessageBox.Show("Pregled uspešno kreiran", "Kreiran pregled", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private Pregled KreirajNoviPregled()
        {
            Pregled noviPregled = new Pregled();
            noviPregled.Lekar = new Lekar();
            noviPregled.Pacijent = new Pacijent();

            string idLekara = listaLekara.SelectedItem.ToString().Split(' ')[2];

            foreach (Lekar iterLekar in lekari)
            {
                if (iterLekar.Jmbg.Equals(idLekara))
                {
                    noviPregled.Lekar.Ordinacija = iterLekar.Ordinacija;
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
            noviPregled.Lekar.Jmbg = idLekara;
            noviPregled.Pacijent.Jmbg = txtOperJmbg.Text;
            noviPregled.VremePocetkaPregleda = pocetak;
            noviPregled.VremeKrajaPregleda = kraj;
            return noviPregled;
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
            pregledi = pregledKontroler.PonudjeniSlobodniTerminiLekara(jmbgLekara);

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
