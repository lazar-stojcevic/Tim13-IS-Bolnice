using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for LekarRaspored.xaml
    /// </summary>
    public partial class LekarRaspored : Window
    {
        private BazaOperacija operacije = new BazaOperacija();
        private BazaPregleda pregledi = new BazaPregleda();
        private string sifra;
        public LekarRaspored(string id)
        {
            InitializeComponent();
            sifra = id;
            //operacije = new BazaOperacija();
            //pregledi = new BazaPregleda();
            List<Operacija> op = operacije.SveSledeceOperacijeZaLekara(id);
            listaOperacija.ItemsSource = op;
            List<Pregled> pr = pregledi.PreglediOdredjenogLekara(id);
            listaPregleda.ItemsSource = pr;
        }

        private void Button_IzmeniOperaciju(object sender, RoutedEventArgs e)
        {
            if (listaOperacija.SelectedIndex == -1)
            {
                MessageBox.Show("Nista nije selektovano", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Operacija selektovani = (Operacija)listaOperacija.SelectedItem;
            string ime = selektovani.Pacijent.Ime;
            string prz = selektovani.Pacijent.Prezime;
            string jmbg = selektovani.Pacijent.Jmbg;
            DateTime datum = selektovani.VremePocetaOperacije.Date;
            Console.WriteLine(datum);
            DateTime vreme = selektovani.VremePocetaOperacije;


            BazaOperacija baza = new BazaOperacija();
            List<Operacija> operacije = baza.SveSledeceOperacije();
            string izListe = listaOperacija.SelectedItem.ToString();
            IzmenaOperacije izmena = new IzmenaOperacije();
            izmena.txtOperIme.Text = ime;
            izmena.txtOperPrz.Text = prz;
            izmena.txtOperJmbg.Text = jmbg;
            izmena.kalendar.SelectedDate = datum;
            izmena.txtHour.Text = vreme.Hour.ToString();
            izmena.txtMinute.Text = vreme.Minute.ToString();
            //OVEJ DEO TREBA MODIFIKOVATI
            izmena.comboBoxSale.SelectedIndex = 0;
            izmena.listaLekara.SelectedIndex = 0;
            izmena.StariSat = vreme.Hour.ToString();
            izmena.StariMinut = vreme.Minute.ToString();
            izmena.StariDatum = datum;

            izmena.ShowDialog();

        }

        private void Button_ClickObrisi(object sender, RoutedEventArgs e)
        {
            if (listaOperacija.SelectedIndex == -1)
            {
                MessageBox.Show("Nista nije selektovano", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Operacija selektovani = (Operacija)listaOperacija.SelectedItem;
            string ime = selektovani.Pacijent.Ime;
            string prz = selektovani.Pacijent.Prezime;
            string jmbg = selektovani.Pacijent.Jmbg;
            DateTime datum = selektovani.VremePocetaOperacije.Date;
            Console.WriteLine(datum);
            DateTime vreme = selektovani.VremePocetaOperacije;


            BazaOperacija baza = new BazaOperacija();
            List<Operacija> lista = baza.SveSledeceOperacije();
            File.WriteAllText(@"..\..\Datoteke\operacije.txt", String.Empty);
            foreach (Operacija o in lista)
            {
                if (o.Pacijent.Jmbg.Equals(jmbg) && o.VremePocetaOperacije.Hour == vreme.Hour && o.VremePocetaOperacije.Minute == vreme.Minute && o.VremePocetaOperacije.Date.Equals(datum))
                {
                    //nista
                }
                else
                {
                    baza.ZakaziOperaciju(o);
                }
            }

            List<Operacija> op = operacije.SveSledeceOperacijeZaLekara(sifra);
            listaOperacija.ItemsSource = op;

        }

        private void Button_IzmeniPregled(object sender, RoutedEventArgs e)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;

            if (listaPregleda.SelectedIndex == -1) { 
                MessageBox.Show("Nista nije selektovano","Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Pregled selektovani = (Pregled)listaPregleda.SelectedItem;
            //string[] delovi = selektovani.Split(' ');
            string ime = selektovani.Pacijent.Ime;
            string prz = selektovani.Pacijent.Prezime;
            string jmbg = selektovani.Pacijent.Jmbg;
            DateTime datum = selektovani.VremePocetkaPregleda.Date;
            Console.WriteLine(datum);
            DateTime vreme = selektovani.VremePocetkaPregleda;

            BazaPregleda baza = new BazaPregleda();
            List<Pregled> pregledi = baza.SviSledeciPregledi();

            LekarIzmenaPregleda izmena = new LekarIzmenaPregleda();

            izmena.txtOperIme.Text = ime;
            izmena.txtOperPrz.Text = prz;
            izmena.txtOperJmbg.Text = jmbg;
            izmena.kalendar.SelectedDate = datum;
            izmena.txtHour.Text = vreme.Hour.ToString();
            izmena.txtMinute.Text = vreme.Minute.ToString();
            //OVEJ DEO TREBA MODIFIKOVATI
            izmena.comboBoxSale.SelectedIndex = 0;
            izmena.listaLekara.SelectedIndex = 0;
            izmena.StariSat = vreme.Hour.ToString();
            izmena.StariMinut = vreme.Minute.ToString();
            izmena.StariDatum = datum;

            izmena.ShowDialog();


        }

        private void Button_ClickObrisiPregled(object sender, RoutedEventArgs e)
        {
            if (listaPregleda.SelectedIndex == -1)
            {
                MessageBox.Show("Nista nije selektovano", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            CultureInfo provider = CultureInfo.InvariantCulture;

            Pregled selektovani = (Pregled)listaPregleda.SelectedItem;
            string jmbg = selektovani.Pacijent.Jmbg;
            string jmbgLekara = selektovani.Lekar.Jmbg;
            DateTime datum = selektovani.VremePocetkaPregleda.Date;
            Console.WriteLine(datum);
            DateTime vreme = selektovani.VremePocetkaPregleda;


            BazaPregleda baza = new BazaPregleda();
            List<Pregled> lista = baza.SviSledeciPregledi();
            File.WriteAllText(@"..\..\Datoteke\pregledi.txt", String.Empty);
            foreach (Pregled p in lista)
            {
                if (p.Pacijent.Jmbg.Equals(jmbg) && p.VremePocetkaPregleda.Hour == vreme.Hour && p.VremePocetkaPregleda.Minute == vreme.Minute && p.VremePocetkaPregleda.Date.Equals(datum.Date) && jmbgLekara.Equals(p.Lekar.Jmbg))
                {
                    //nista
                }
                else
                {
                    baza.ZakaziPregled(p);
                }
            }

            List<Pregled> pr = pregledi.PreglediOdredjenogLekara(sifra);
            listaPregleda.ItemsSource = pr;

        }

    }
}
