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

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarRaspored.xaml
    /// </summary>
    public partial class LekarRaspored : Page
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
            List<Pregled> pr = pregledi.SviBuduciPreglediKojeLekarIma(id);
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
            DateTime datum = selektovani.VremePocetkaOperacije.Date;
            Console.WriteLine(datum);
            DateTime vreme = selektovani.VremePocetkaOperacije;


            BazaOperacija baza = new BazaOperacija();
            List<Operacija> operacije = baza.SveSledeceOperacije();
            string izListe = listaOperacija.SelectedItem.ToString();
            IzmenaOperacije izmena = new IzmenaOperacije();
            izmena.txtOperIme.Text = ime;
            izmena.txtOperPrz.Text = prz;
            izmena.txtOperJmbg.Text = jmbg;
            izmena.terminiList.Items.Add(selektovani.VremePocetkaOperacije);
            izmena.boxHitno.IsChecked = selektovani.Hitna;
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
            DateTime datum = selektovani.VremePocetkaOperacije.Date;
            Console.WriteLine(datum);
            DateTime vreme = selektovani.VremePocetkaOperacije;


            BazaOperacija baza = new BazaOperacija();
            List<Operacija> lista = baza.SveSledeceOperacije();
            File.WriteAllText(@"..\..\Datoteke\operacije.txt", String.Empty);
            foreach (Operacija o in lista)
            {
                if (o.Pacijent.Jmbg.Equals(jmbg) && o.VremePocetkaOperacije.Hour == vreme.Hour && o.VremePocetkaOperacije.Minute == vreme.Minute && o.VremePocetkaOperacije.Date.Equals(datum))
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
            List<Pregled> pregledi = baza.SviPregledi();

            LekarIzmenaPregleda izmena = new LekarIzmenaPregleda();

            izmena.txtOperIme.Text = ime;
            izmena.txtOperPrz.Text = prz;
            izmena.txtOperJmbg.Text = jmbg;
            izmena.terminiList.Items.Add(selektovani.VremePocetkaPregleda);
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
            List<Pregled> lista = baza.SviPregledi();
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

            List<Pregled> pr = pregledi.SviBuduciPreglediKojeLekarIma(sifra);
            listaPregleda.ItemsSource = pr;

        }

        private void Button_VidiPacijenta(object sender, RoutedEventArgs e)
        {
            if (listaOperacija.SelectedIndex == -1)
            {
                MessageBox.Show("Nista nije selektovano", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Operacija selektovana = (Operacija)listaOperacija.SelectedItem;
            PodaciOPacijentu pod = new PodaciOPacijentu(selektovana.Pacijent.Jmbg);
            pod.ShowDialog();
        }

        private void Button_VidiPacijentaPr(object sender, RoutedEventArgs e)
        {
            if (listaPregleda.SelectedIndex == -1)
            {
                MessageBox.Show("Nista nije selektovano", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Pregled selektovana = (Pregled)listaPregleda.SelectedItem;
            PodaciOPacijentu pod = new PodaciOPacijentu(selektovana.Pacijent.Jmbg);
            pod.ShowDialog();

        }

        private void Button_OtvoriPregled(object sender, RoutedEventArgs e)
        {
            if (listaPregleda.SelectedIndex == -1)
            {
                MessageBox.Show("Nista nije selektovano", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Pregled selektovanPregled = (Pregled)listaPregleda.SelectedItem;
            LekarWindow prozorZaPregled = new LekarWindow(sifra);
            //prozorZaPregled.Sifra = sifra;

            prozorZaPregled.txtJMBG.Text = selektovanPregled.Pacijent.Jmbg;
            prozorZaPregled.txtIme.Text = selektovanPregled.Pacijent.Ime;
            prozorZaPregled.txtPrz.Text = selektovanPregled.Pacijent.Prezime;

            prozorZaPregled.btnPregled.IsEnabled = true;
            prozorZaPregled.btnOperacija.IsEnabled = true;
            prozorZaPregled.btnIzvestaj.IsEnabled = true;
            this.NavigationService.Navigate(prozorZaPregled);

        }
    }
}
