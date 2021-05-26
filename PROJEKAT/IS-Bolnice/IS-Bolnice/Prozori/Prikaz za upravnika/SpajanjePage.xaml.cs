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
    /// Interaction logic for SpajanjePage.xaml
    /// </summary>
    public partial class SpajanjePage : Page
    {
        List<Soba> selectovaneSobe;
        Soba novaSoba;

        public SpajanjePage(List<Soba> sobeZaSpajanje)
        {
            InitializeComponent();
            selectovaneSobe = sobeZaSpajanje;
            novaSoba = new Soba();
            novaSoba.Kvadratura = 0;
        }

        private void Potvrdi_btn_Click(object sender, RoutedEventArgs e)
        {
            novaSoba.Id = id_txt.Text;
            novaSoba.Tip = (RoomType)tip_sobe_txt.SelectedIndex;
            if (CheckDatum(MakeRenovacija()))
            {
                if (ProveraRaspolozivostiProstorija()) {
                    SpajanjeSoba();
                }
            }
            else
            {
                MessageBox.Show("Datumi se ne poklapaju logicki");
            }
        }

        private void SpajanjeSoba() {
            KreirajNovuSobu();
            Renovacija renovacija = MakeRenovacija();
            BazaRenovacija bazaRenovacija = new BazaRenovacija();
            bazaRenovacija.KreirajRenovaciju(renovacija);
            UkloniSobe();
        }

        private void PrebaciOpremuUMagacin() {
            BazaBolnica bazaBolnica = new BazaBolnica();
            List<Soba> updateSoba = bazaBolnica.GetSobe();
            foreach (Soba selected in selectovaneSobe)
            {
                DodajOpremuUMagacin(selected.Id);
                ObrisiOpremuIzSobe(selected.Id);
            }
        }

        private void UkloniSobe() {
            BazaBolnica bazaBolnica = new BazaBolnica();
            List<Soba> updateSoba = bazaBolnica.GetSobe();
            foreach (Soba selected in selectovaneSobe) {
                foreach (Soba sobaIter in updateSoba) {
                    if (sobaIter.Id.Equals(selected.Id)) {
                        sobaIter.Obrisano = true;
                        break;
                    }
                }
            }
            Bolnica novaBolnica = bazaBolnica.GetBolnica();
            novaBolnica.Soba = updateSoba;
            bazaBolnica.KreirajBolnicu(novaBolnica);
            this.NavigationService.GoBack();
        }

        public void DodajOpremuUMagacin(string idSobe)
        {
            BazaSadrzaja bazaSadrzaja = new BazaSadrzaja();
            BazaBolnica bazaBolnica = new BazaBolnica();
            List<SadrzajSobe> svaOpremaUSobi = bazaSadrzaja.GetSadrzajSobe(idSobe);
            List<SadrzajSobe> svaOpremaUMagacinu = bazaSadrzaja.GetSadrzajSobe(bazaBolnica.GetMagacin().Id);
            bool postojiOpremaUMagacinu = false;
            foreach (SadrzajSobe opremaUSobi in svaOpremaUSobi)
            {
                foreach (SadrzajSobe opremaUMagacinu in svaOpremaUMagacinu)
                {
                    if (opremaUSobi.Predmet.Id.Equals(opremaUMagacinu.Predmet.Id))
                    {
                        opremaUMagacinu.Kolicina = opremaUMagacinu.Kolicina + opremaUSobi.Kolicina;
                        bazaSadrzaja.IzmeniSadrzaj(opremaUMagacinu);
                        postojiOpremaUMagacinu = true;
                        break;
                    }
                }
                if (!postojiOpremaUMagacinu)
                {
                    opremaUSobi.Soba.Id = bazaBolnica.GetMagacin().Id;
                    bazaSadrzaja.KreirajSadrzaj(opremaUSobi);
                }
                postojiOpremaUMagacinu = false;
            }
        }


        public void ObrisiOpremuIzSobe(string idSobe)
        {
            BazaSadrzaja bazaSadrzaja = new BazaSadrzaja();
            List<SadrzajSobe> opremaUSobi = bazaSadrzaja.GetSadrzajSobe(idSobe);
            foreach (SadrzajSobe oprema in opremaUSobi)
            {
                bazaSadrzaja.ObrisiSadrzaj(oprema);
            }
        }

        private void KreirajNovuSobu() {
            foreach (Soba sobaIter in selectovaneSobe) {
                novaSoba.Kvadratura = novaSoba.Kvadratura + sobaIter.Kvadratura;
                novaSoba.Sprat = sobaIter.Sprat;
            }
            BazaBolnica bazaBolnica = new BazaBolnica();
            Bolnica novaBolnica = bazaBolnica.GetBolnica();
            novaBolnica.AddSoba(novaSoba);
            bazaBolnica.KreirajBolnicu(novaBolnica);
            
        }

        private bool ProveraRaspolozivostiProstorija() {
            bool prostorijaJeSlobodna = true;
            foreach (Soba soba in selectovaneSobe) {
                if (soba.Tip == RoomType.operacionaSala)
                {
                    if (!ProveraOperacioneSale(soba.Id)) {
                        prostorijaJeSlobodna = false;
                    }
                }
                else if (soba.Tip == RoomType.bolnickaSoba)
                {
                    if (!ProveraBolnickeSobe(soba.Id))
                    {
                        prostorijaJeSlobodna = false;
                    }
                }
                else
                {
                    if (!ProveraProstorije(soba.Id))
                    {
                        prostorijaJeSlobodna = false;
                    }
                }
            }
            return prostorijaJeSlobodna;
        }

        private bool ProveraOperacioneSale(string idSobe)
        {
            OperacijaKontroler operacijaKontroler = new OperacijaKontroler();
            BazaOperacija bazaOperacija = new BazaOperacija();
            BazaRenovacija bazaRenovacija = new BazaRenovacija();
            Renovacija renovacija = MakeRenovacija();
            foreach (Operacija operacija in operacijaKontroler.GetSveSledeceOperacijeSale(idSobe))
            {
                if (operacija.VremePocetkaOperacije > renovacija.DatumPocetka && operacija.VremeKrajaOperacije < renovacija.DatumKraja)
                {
                    MessageBox.Show("Postoje zakazane operacije! Odaberite drugi period!");
                    return false;
                }
            }
            return true;
        }

        private bool ProveraBolnickeSobe(string idSobe) {
            BazaRenovacija bazaRenovacija = new BazaRenovacija();
            BazaHospitalizacija bazaHospitalizacija = new BazaHospitalizacija();
            Renovacija renovacija = MakeRenovacija();
            foreach (Hospitalizacija hospitalizacija in bazaHospitalizacija.GetHospitalizacijeZaSobu(idSobe)) {
                if (hospitalizacija.PocetakHospitalizacije > renovacija.DatumPocetka && hospitalizacija.KrajHospitalizacije < renovacija.DatumKraja)
                {
                    MessageBox.Show("Pacijenti su smešteni u odabranoj sobi! Odaberite drugi period!");
                    return false;
                }
            }
            return true;
        }

        private bool ProveraProstorije(string idSobe)
        {
            PregledKontroler pregledKontroler = new PregledKontroler();
            BazaPregleda bazaPregleda = new BazaPregleda();
            BazaRenovacija bazaRenovacija = new BazaRenovacija();
            Renovacija renovacija = MakeRenovacija();
            foreach (Pregled pregled in pregledKontroler.GetSviBuduciPreglediSobe(idSobe))
            {
                if (pregled.VremePocetkaPregleda > renovacija.DatumPocetka && pregled.VremePocetkaPregleda < renovacija.DatumKraja)
                {
                    MessageBox.Show("Postoje zakazani pregledi! Odaberite drugi period!");
                    return false;
                }
            }
            return true;

        }

        private Renovacija MakeRenovacija()
        {
            Renovacija novaRenovacija = new Renovacija();
            DateTime izbraniDatumZaPocetak = DateTime.Parse(datePicker_pocetak.SelectedDate.ToString());
            DateTime izbraniDatumZaKraj = DateTime.Parse(datePicker_kraj.SelectedDate.ToString());
            novaRenovacija.DatumPocetka = new DateTime(izbraniDatumZaPocetak.Year, izbraniDatumZaPocetak.Month, izbraniDatumZaPocetak.Day);
            novaRenovacija.DatumKraja = new DateTime(izbraniDatumZaKraj.Year, izbraniDatumZaKraj.Month, izbraniDatumZaKraj.Day);
            novaRenovacija.ProstorijaZaRenoviranje = novaSoba;
            return novaRenovacija;
        }

        public bool CheckDatum(Renovacija novaRenovacija)
        {
            bool ispravno = true;
            if (novaRenovacija.DatumPocetka > novaRenovacija.DatumKraja)
            {
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
