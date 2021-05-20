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
    /// Interaction logic for LekarRazlogOdbijanjaLeka.xaml
    /// </summary>
    public partial class LekarRazlogOdbijanjaLeka : Window
    {
        BazaZahtevaZaValidacijuLeka bazaZahtevaZaValidaciju = new BazaZahtevaZaValidacijuLeka();
        BazaOdgovoraNaZahteveZaValidacijuLekova bazaOdgovora = new BazaOdgovoraNaZahteveZaValidacijuLekova();

        private ZahtevZaValidacijuKontroler zahtevZaValidacijuKontroler = new ZahtevZaValidacijuKontroler();

        private OdgovoriNaZahtevZaValidacijeKontroler odgovorNaValidacijeKontroler =
            new OdgovoriNaZahtevZaValidacijeKontroler();

        ZahtevZaValidacijuLeka zahtev;
        public LekarRazlogOdbijanjaLeka(ZahtevZaValidacijuLeka zahtevPoslat)
        {
            zahtev = zahtevPoslat;
            InitializeComponent();
        }

        private void Button_ClickPotvrdi(object sender, RoutedEventArgs e)
        {
            OdgovorNaZahtevZaValidaciju odgovor = new OdgovorNaZahtevZaValidaciju();
            odgovor.Obrazlozenje = txtRazlog.Text;
            odgovorNaValidacijeKontroler.KreirajOdgovorNaZahtevZaValidaciju(odgovor, zahtev);
            zahtevZaValidacijuKontroler.ObrisiZahtev(zahtev);
            this.Close();
        }

        private void Button_ClickOdustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
