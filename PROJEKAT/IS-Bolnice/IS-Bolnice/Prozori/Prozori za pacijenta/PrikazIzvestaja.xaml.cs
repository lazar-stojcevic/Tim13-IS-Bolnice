using IS_Bolnice.Kontroleri;
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
    public partial class PrikazIzvestaja : Window
    {
        private string jmbgPacijenta;
        private IzvestajKontroler izvestajKontroler = new IzvestajKontroler();

        public PrikazIzvestaja(string jmbg)
        {
            InitializeComponent();

            jmbgPacijenta = jmbg;
        }

        private void prikaziSve_Click(object sender, RoutedEventArgs e)
        {
            foreach (Izvestaj izvestaj in izvestajKontroler.SviIzvestajiPacijenta(jmbgPacijenta))
            {
                Console.WriteLine("***IZVESTAJ***");
                Console.WriteLine("Lekar: " + izvestaj.Lekar.Jmbg);
                Console.WriteLine("Pacijent: " + izvestaj.Pacijent.Jmbg);
                Console.WriteLine("Opis: " + izvestaj.Opis);
                Console.WriteLine("Datum: " + izvestaj.DatumKreiranja.ToString());
                foreach (Terapija terapija in izvestaj.Terapija)
                {
                    Console.WriteLine("***********TERAPIJA************");
                    Console.WriteLine("Sifra leka: " + terapija.Lek.Sifra);
                    Console.WriteLine("Naziv leka: " + terapija.Lek.Ime);
                    Console.WriteLine("Opis leka: " + terapija.Lek.Opis);
                    Console.WriteLine("Na koliko dana se lek uzima: " + terapija.RazlikaNaKolikoSeDanaUzimaLek.ToString());
                    Console.WriteLine("Ucestalost: " + terapija.UcestanostKonzumiranja.ToString());
                    Console.WriteLine("Vreme pocetka: " + terapija.VremePocetka.ToString());
                    Console.WriteLine("Vreme kraja: " + terapija.VremeKraja.ToString());
                    Console.WriteLine("Opis: " + terapija.Opis.ToString());
                }

            }
        }
    }
}
