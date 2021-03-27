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
        public LekarRaspored()
        {
            InitializeComponent();
        }

        private void Button_IzmeniOperaciju(object sender, RoutedEventArgs e)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;

            string selektovani = listaOperacija.SelectedItem.ToString();
            string[] delovi = selektovani.Split(' ');
            string ime = delovi[1];
            string prz = delovi[2];
            string jmbg = delovi[3];
            string datum = delovi[9];
            Console.WriteLine(datum);
            string[] vreme = delovi[10].Split(':');

            BazaOperacija baza = new BazaOperacija();
            List<Operacija> operacije = baza.SveSledeceOperacije();
            string izListe = listaOperacija.SelectedItem.ToString();
            IzmenaOperacije izmena = new IzmenaOperacije();
            izmena.txtOperIme.Text = ime;
            izmena.txtOperPrz.Text = prz;
            izmena.txtOperJmbg.Text = jmbg;
            izmena.kalendar.SelectedDate = DateTime.ParseExact(datum,"dd/MM/yyyy", provider);
            izmena.txtHour.Text = vreme[0];
            izmena.txtMinute.Text = vreme[1];
            //OVEJ DEO TREBA MODIFIKOVATI
            izmena.comboBoxSale.SelectedIndex = 0;
            izmena.listaLekara.SelectedIndex = 0;
            izmena.StariSat = vreme[0];
            izmena.StariMinut = vreme[1];
            izmena.StariDatum = DateTime.ParseExact(datum, "dd/MM/yyyy", provider);

            izmena.ShowDialog();

        }

        private void Button_ClickObrisi(object sender, RoutedEventArgs e)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;

            string selektovani = listaOperacija.SelectedItem.ToString();
            string[] delovi = selektovani.Split(' ');
            string ime = delovi[1];
            string prz = delovi[2];
            string jmbg = delovi[3];
            string datum = delovi[9];
            Console.WriteLine(datum);
            string[] vreme = delovi[10].Split(':');


            BazaOperacija baza = new BazaOperacija();
            List<Operacija> lista = baza.SveSledeceOperacije();
            File.WriteAllText(@"..\..\Serijalizacija\operacije.txt", String.Empty);
            foreach (Operacija o in lista)
            {
                if (o.Pacijent.Jmbg.Equals(jmbg) && o.VremePocetaOperacije.Hour == Int32.Parse(vreme[0]) && o.VremePocetaOperacije.Minute == Int32.Parse(vreme[1]) && o.VremePocetaOperacije.Date.Equals(DateTime.ParseExact(datum, "dd/MM/yyyy", provider)))
                {
                    //nista
                }
                else
                {
                    baza.ZakaziOperaciju(o);
                }
            }

        }

        private void Button_IzmeniPregled(object sender, RoutedEventArgs e)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;


            string selektovani = listaPregleda.SelectedItem.ToString();
            string[] delovi = selektovani.Split(' ');
            string ime = delovi[1];
            string prz = delovi[2];
            string jmbg = delovi[3];
            string datum = delovi[9];
            Console.WriteLine(datum);
            string[] vreme = delovi[10].Split(':');

            BazaPregleda baza = new BazaPregleda();
            List<Pregled> pregledi = baza.SviSledeciPregledi();

            string izListe = listaPregleda.SelectedItem.ToString();
            LekarIzmenaPregleda izmena = new LekarIzmenaPregleda();

            izmena.txtOperIme.Text = ime;
            izmena.txtOperPrz.Text = prz;
            izmena.txtOperJmbg.Text = jmbg;
            izmena.kalendar.SelectedDate = DateTime.ParseExact(datum, "dd/MM/yyyy", provider);
            izmena.txtHour.Text = vreme[0];
            izmena.txtMinute.Text = vreme[1];
            //OVEJ DEO TREBA MODIFIKOVATI
            izmena.comboBoxSale.SelectedIndex = 0;
            izmena.listaLekara.SelectedIndex = 0;
            izmena.StariSat = vreme[0];
            izmena.StariMinut = vreme[1];
            izmena.StariDatum = DateTime.ParseExact(datum, "dd/MM/yyyy", provider);

            izmena.ShowDialog();


        }

        private void Button_ClickObrisiPregled(object sender, RoutedEventArgs e)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;

            string selektovani = listaPregleda.SelectedItem.ToString();
            string[] delovi = selektovani.Split(' ');
            string ime = delovi[1];
            string prz = delovi[2];
            string jmbg = delovi[3];
            string datum = delovi[9];
            Console.WriteLine(datum);
            string[] vreme = delovi[10].Split(':');


            BazaPregleda baza = new BazaPregleda();
            List<Pregled> lista = baza.SviSledeciPregledi();
            File.WriteAllText(@"..\..\Serijalizacija\pregledi.txt", String.Empty);
            foreach (Pregled p in lista)
            {
                if (p.Pacijent.Jmbg.Equals(jmbg) && p.VremePocetkaPregleda.Hour == Int32.Parse(vreme[0]) && p.VremePocetkaPregleda.Minute == Int32.Parse(vreme[1]) && p.VremePocetkaPregleda.Date.Equals(DateTime.ParseExact(datum, "dd/MM/yyyy", provider)))
                {
                    //nista
                }
                else
                {
                    baza.ZakaziPregled(p);
                }
            }

        }

    }
}
