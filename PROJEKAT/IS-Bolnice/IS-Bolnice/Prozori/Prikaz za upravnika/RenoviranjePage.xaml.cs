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
using System.Windows.Navigation;
using System.Windows.Shapes;
using IS_Bolnice.Kontroleri;

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for RenoviranjePage.xaml
    /// </summary>
    public partial class RenoviranjePage : Page
    {
        Soba selektovanaSoba;



        public RenoviranjePage(Soba soba)
        {
            InitializeComponent();
            selektovanaSoba = soba;
        }

        private void Potvrdi_btn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckDatum(MakeRenovacija()))
            {
                if (selektovanaSoba.Tip == RoomType.operacionaSala)
                {
                    RenoviranjeOperacioneSale();
                }
                else if (selektovanaSoba.Tip == RoomType.bolnickaSoba) {
                    RenovirajBolnickuSobu();
                }
                else {
                    RenoviranjeProstorije();
                }
            }
            else {
                MessageBox.Show("Datumi se ne poklapaju logicki");
            }
        }

        private void RenoviranjeOperacioneSale() {
            BazaOperacija bazaOperacija = new BazaOperacija();
            OperacijaKontroler operacijaKontroler = new OperacijaKontroler();
            BazaRenovacija bazaRenovacija = new BazaRenovacija();
            Renovacija renovacija = MakeRenovacija();
            foreach (Operacija operacija in operacijaKontroler.GetSveSledeceOperacijeSale(selektovanaSoba.Id))
            {
                if (operacija.VremePocetkaOperacije > renovacija.DatumPocetka && operacija.VremeKrajaOperacije < renovacija.DatumKraja)
                {
                    MessageBox.Show("Postoje zakazane operacije! Odaberite drugi period!");
                }
                else
                {
                    bazaRenovacija.KreirajRenovaciju(renovacija);
                    RenoviranjeOprema();
                    this.NavigationService.GoBack();
                }
            }
        }

        private void RenovirajBolnickuSobu() {
            BazaRenovacija bazaRenovacija = new BazaRenovacija();
            BazaHospitalizacija bazaHospitalizacija = new BazaHospitalizacija();
            Renovacija renovacija = MakeRenovacija();
            foreach (Hospitalizacija hospitalizacija in bazaHospitalizacija.GetHospitalizacijeZaSobu(selektovanaSoba.Id)) {
                if (hospitalizacija.PocetakHospitalizacije > renovacija.DatumPocetka && hospitalizacija.KrajHospitalizacije < renovacija.DatumKraja)
                {
                    MessageBox.Show("Pacijenti su smešteni u odabranoj sobi! Odaberite drugi period!");
                }
                else
                {
                    bazaRenovacija.KreirajRenovaciju(renovacija);
                    RenoviranjeOprema();
                    this.NavigationService.GoBack();
                }
            }
        }

        private void RenoviranjeProstorije()
        {
            PregledKontroler pregledKontroler = new PregledKontroler();
            BazaPregleda bazaPregleda = new BazaPregleda();
            BazaRenovacija bazaRenovacija = new BazaRenovacija();
            Renovacija renovacija = MakeRenovacija();
            bool postojiZakazanTermin = false;
            foreach (Pregled pregled in pregledKontroler.GetSviBuduciPreglediSobe(selektovanaSoba.Id))
            {
                if (pregled.VremePocetkaPregleda > renovacija.DatumPocetka && pregled.VremePocetkaPregleda < renovacija.DatumKraja)
                {
                    MessageBox.Show("Postoje zakazani pregledi! Odaberite drugi period!");
                    postojiZakazanTermin = true;
                    break;
                }
            }
            if (!postojiZakazanTermin) {
                bazaRenovacija.KreirajRenovaciju(renovacija);
                RenoviranjeOprema();
                this.NavigationService.GoBack();
            }

        }


        public void RenoviranjeOprema() {
            switch (comboBox_oprema.SelectedIndex) {
                case 0:
                    ObrisiOpremuIzSobe();
                    break;
                case 1:
                    DodajOpremuUMagacin();
                    ObrisiOpremuIzSobe();
                    break;
                default:
                    break;
            }
        }

        public void DodajOpremuUMagacin() {
            BazaSadrzaja bazaSadrzaja = new BazaSadrzaja();
            BazaBolnica bazaBolnica = new BazaBolnica();
            List<SadrzajSobe> svaOpremaUSobi = bazaSadrzaja.GetSadrzajSobe(selektovanaSoba.Id);
            List<SadrzajSobe> svaOpremaUMagacinu = bazaSadrzaja.GetSadrzajSobe(bazaBolnica.GetMagacin().Id);
            bool postojiOpremaUMagacinu = false;
            foreach (SadrzajSobe opremaUSobi in svaOpremaUSobi) {
                foreach (SadrzajSobe opremaUMagacinu in svaOpremaUMagacinu) {
                    if (opremaUSobi.Predmet.Id.Equals(opremaUMagacinu.Predmet.Id)) {
                        opremaUMagacinu.Kolicina = opremaUMagacinu.Kolicina + opremaUSobi.Kolicina;
                        bazaSadrzaja.IzmeniSadrzaj(opremaUMagacinu);
                        postojiOpremaUMagacinu = true;
                        break;
                    }
                }
                if (!postojiOpremaUMagacinu) {
                    opremaUSobi.Soba.Id = bazaBolnica.GetMagacin().Id;
                    bazaSadrzaja.KreirajSadrzaj(opremaUSobi);
                }
                postojiOpremaUMagacinu = false;
            }
        }


        public void ObrisiOpremuIzSobe() {
            BazaSadrzaja bazaSadrzaja = new BazaSadrzaja();
            List<SadrzajSobe> opremaUSobi = bazaSadrzaja.GetSadrzajSobe(selektovanaSoba.Id);
            foreach (SadrzajSobe oprema in opremaUSobi) {
                bazaSadrzaja.ObrisiSadrzaj(oprema);
            }
        }

        public Renovacija MakeRenovacija() {
            Renovacija novaRenovacija = new Renovacija();
            DateTime izbraniDatumZaPocetak = DateTime.Parse(datePicker_pocetak.SelectedDate.ToString());
            DateTime izbraniDatumZaKraj = DateTime.Parse(datePicker_kraj.SelectedDate.ToString());
            novaRenovacija.DatumPocetka = new DateTime(izbraniDatumZaPocetak.Year, izbraniDatumZaPocetak.Month, izbraniDatumZaPocetak.Day);
            novaRenovacija.DatumKraja = new DateTime(izbraniDatumZaKraj.Year, izbraniDatumZaKraj.Month, izbraniDatumZaKraj.Day);
            novaRenovacija.ProstorijaZaRenoviranje = selektovanaSoba;
            return novaRenovacija;
        }

        public bool CheckDatum(Renovacija novaRenovacija) {
            bool ispravno = true;
            if (novaRenovacija.DatumPocetka > novaRenovacija.DatumKraja) {
                ispravno = false;
            }
            return ispravno;
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
