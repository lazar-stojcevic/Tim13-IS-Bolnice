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
            OperacijaFajlRepozitorijum operacijaFajlRepozitorijum = new OperacijaFajlRepozitorijum();
            OperacijaKontroler operacijaKontroler = new OperacijaKontroler();
            RenovacijaFajlRepozitorijum renovacijaFajlRepozitorijum = new RenovacijaFajlRepozitorijum();
            Renovacija renovacija = MakeRenovacija();
            foreach (Operacija operacija in operacijaKontroler.GetSveBuduceOperacijeSale(selektovanaSoba.Id))
            {
                if (operacija.VremePocetkaOperacije > renovacija.DatumPocetka && operacija.VremeKrajaOperacije < renovacija.DatumKraja)
                {
                    MessageBox.Show("Postoje zakazane operacije! Odaberite drugi period!");
                }
                else
                {
                    renovacijaFajlRepozitorijum.Sacuvaj(renovacija);
                    RenoviranjeOprema();
                    this.NavigationService.GoBack();
                }
            }
        }

        private void RenovirajBolnickuSobu() {
            RenovacijaFajlRepozitorijum renovacijaFajlRepozitorijum = new RenovacijaFajlRepozitorijum();
            HospitalizacijaFajlRepozitorijum hospitalizacijaFajlRepozitorijum = new HospitalizacijaFajlRepozitorijum();
            Renovacija renovacija = MakeRenovacija();
            foreach (Hospitalizacija hospitalizacija in hospitalizacijaFajlRepozitorijum.DobaviSveHospitalizacijeZaSobu(selektovanaSoba.Id)) {
                if (hospitalizacija.PocetakHospitalizacije > renovacija.DatumPocetka && hospitalizacija.KrajHospitalizacije < renovacija.DatumKraja)
                {
                    MessageBox.Show("Pacijenti su smešteni u odabranoj sobi! Odaberite drugi period!");
                }
                else
                {
                    renovacijaFajlRepozitorijum.Sacuvaj(renovacija);
                    RenoviranjeOprema();
                    this.NavigationService.GoBack();
                }
            }
        }

        private void RenoviranjeProstorije()
        {
            PregledKontroler pregledKontroler = new PregledKontroler();
            PreglediFajlRepozitorijum preglediFajlRepozitorijum = new PreglediFajlRepozitorijum();
            RenovacijaFajlRepozitorijum renovacijaFajlRepozitorijum = new RenovacijaFajlRepozitorijum();
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
                renovacijaFajlRepozitorijum.Sacuvaj(renovacija);
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
            SadrzajSobeFajlRepozitorijum sadrzajSobeFajlRepozitorijum = new SadrzajSobeFajlRepozitorijum();
            BolnicaFajlRepozitorijum bolnicaFajlRepozitorijum = new BolnicaFajlRepozitorijum();
            List<SadrzajSobe> svaOpremaUSobi = sadrzajSobeFajlRepozitorijum.GetSadrzajSobe(selektovanaSoba.Id);
            List<SadrzajSobe> svaOpremaUMagacinu = sadrzajSobeFajlRepozitorijum.GetSadrzajSobe(bolnicaFajlRepozitorijum.GetMagacin().Id);
            bool postojiOpremaUMagacinu = false;
            foreach (SadrzajSobe opremaUSobi in svaOpremaUSobi) {
                foreach (SadrzajSobe opremaUMagacinu in svaOpremaUMagacinu) {
                    if (opremaUSobi.Predmet.Id.Equals(opremaUMagacinu.Predmet.Id)) {
                        opremaUMagacinu.Kolicina = opremaUMagacinu.Kolicina + opremaUSobi.Kolicina;
                        sadrzajSobeFajlRepozitorijum.Izmeni(opremaUMagacinu);
                        postojiOpremaUMagacinu = true;
                        break;
                    }
                }
                if (!postojiOpremaUMagacinu) {
                    opremaUSobi.Soba.Id = bolnicaFajlRepozitorijum.GetMagacin().Id;
                    sadrzajSobeFajlRepozitorijum.Sacuvaj(opremaUSobi);
                }
                postojiOpremaUMagacinu = false;
            }
        }


        public void ObrisiOpremuIzSobe() {
            SadrzajSobeFajlRepozitorijum sadrzajSobeFajlRepozitorijum = new SadrzajSobeFajlRepozitorijum();
            List<SadrzajSobe> opremaUSobi = sadrzajSobeFajlRepozitorijum.GetSadrzajSobe(selektovanaSoba.Id);
            foreach (SadrzajSobe oprema in opremaUSobi) {
                sadrzajSobeFajlRepozitorijum.Obrisi(oprema.Id);
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
