using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Model;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Sekretar
{
    /// <summary>
    /// Interaction logic for SekretarWindow.xaml
    /// </summary>
    public partial class SekretarWindow : Window
    {
        private ObavestenjeKontroler obavestenjeKontroler = new ObavestenjeKontroler();
        private PacijentKontroler pacijentKontroler = new PacijentKontroler();
        private LekarKontroler lekarKontroler = new LekarKontroler();
        private LekKontroler lekKontroler = new LekKontroler();
        private BolnicaKontroler bolnicaKontroler = new BolnicaKontroler();
        private SadrzajSobeKontroler sadrzajSobeKontroler = new SadrzajSobeKontroler();

        public ObservableCollection<Pacijent> Pacijenti
        {
            get;
            set;
        }

        public ObservableCollection<Lekar> LekariOpstePrakse
        {
            get;
            set;
        }

        public ObservableCollection<Lekar> LekariSpecijalisti
        {
            get;
            set;
        }

        public ObservableCollection<Obavestenje> Obavestenja
        {
            get;
            set;
        }

        public ObservableCollection<Lek> SviLekovi
        {
            get;
            set;
        }

        public ObservableCollection<Soba> SveSobeBolnice
        {
            get;
            set;
        }

        public SekretarWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            Pacijenti = new ObservableCollection<Pacijent>(pacijentKontroler.GetSviPacijenti());
            LekariOpstePrakse = new ObservableCollection<Lekar>(lekarKontroler.GetSviLekariOpstePrakse());
            LekariSpecijalisti = new ObservableCollection<Lekar>(lekarKontroler.GetSviLekariSpecijalisti());
            Obavestenja = new ObservableCollection<Obavestenje>(obavestenjeKontroler.GetSvaSortiranaObavestenja());
            SviLekovi = new ObservableCollection<Lek>(lekKontroler.GetSviLekovi());
            SveSobeBolnice = new ObservableCollection<Soba>(bolnicaKontroler.GetSveSobe());
        }

        private void OsvezavanjePrikazaPacijenata()
        {
            Pacijenti.Clear();
            foreach (Pacijent pacijent in pacijentKontroler.GetSviPacijenti())
            {
                Pacijenti.Add(pacijent);
            }
        }

        private void OsvezavanjePrikazaObavestenja()
        {
            Obavestenja.Clear();
            foreach (var obavestenje in obavestenjeKontroler.GetSvaSortiranaObavestenja())
            {
                Obavestenja.Add(obavestenje);
            }
        }

        private void DodavanjePacijenta()
        {
            DodavanjePacijentaWindow dpw = new DodavanjePacijentaWindow();
            dpw.ShowDialog();
            OsvezavanjePrikazaPacijenata();
        }

        private void DodavanjeGuestPacijenta()
        {
            DodavanjeGuestNalogaWindow dgnw = new DodavanjeGuestNalogaWindow();
            dgnw.ShowDialog();
            // osvezavanje prikaza pacijenata (da ne mora da se prosledjuje referenca u konstruktor)
            OsvezavanjePrikazaPacijenata();
        }

        private void IzmeniPacijenta()
        {
            Pacijent p = (Pacijent)dataGridPacijenti.SelectedItem;
            if (p != null)
            {
                IzmenaPacijentaWindow ipw = new IzmenaPacijentaWindow(p, Pacijenti);
                ipw.ShowDialog();
            }
        }

        private void ObrisiPacijenta()
        {
            Pacijent p = (Pacijent)dataGridPacijenti.SelectedItem;
            if (p != null)
            {
                string sMessageBoxText = "Da li ste sigurni da želite da obrišete pacijenta?";
                string sCaption = "Brisanje pacijenta";

                MessageBoxResult rsltMessageBox = CustomMessageBox.ShowYesNo(sMessageBoxText, sCaption, "Potvrdi",
                    "Odustani", MessageBoxImage.Question);

                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        pacijentKontroler.ObrisiPacijenta(p.Jmbg);
                        Pacijenti.Remove(p);
                        break;

                    case MessageBoxResult.No:
                        /* ... */
                        break;
                }
            }
        }

        private void DodavanjeObavestenja()
        {
            FormiranjeObavestenjaWindow fow = new FormiranjeObavestenjaWindow(Obavestenja);
            fow.ShowDialog();
            OsvezavanjePrikazaObavestenja();
        }

        private void IzmenaObavestenja()
        {
            Obavestenje o = (Obavestenje)obavestenjaDataBinding.SelectedItem;
            if (o != null)
            {
                IzmenaObavestenjaWindow iow = new IzmenaObavestenjaWindow(o, Obavestenja);
                iow.ShowDialog();
            }
        }

        private void BrisanjeObavestenja()
        {
            Obavestenje o = (Obavestenje)obavestenjaDataBinding.SelectedItem;
            if (o != null)
            {
                string sMessageBoxText = "Da li ste sigurni da želite da obrišete obaveštenje?";
                string sCaption = "Brisanje obaveštenja";

                MessageBoxResult rsltMessageBox = CustomMessageBox.ShowYesNo(sMessageBoxText, sCaption, "Potvrdi",
                    "Odustani", MessageBoxImage.Question);

                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        obavestenjeKontroler.ObrisiObavestenje(o.Id);
                        Obavestenja.Remove(o);
                        break;

                    case MessageBoxResult.No:
                        /* ... */
                        break;
                }
            }
        }

        private void ZakazivanjePregleda()
        {
            Pacijent p = (Pacijent)dataGridPacijenti.SelectedItem;
            if (p != null)
            {
                SekretarZakazivanjePregleda szp = new SekretarZakazivanjePregleda(p);
                szp.ShowDialog();
            }
        }

        private void PrikazTerminaPacijenta()
        {
            Pacijent p = (Pacijent)dataGridPacijenti.SelectedItem;
            if (p != null)
            {
                SekretarPrikazZakazanihTerminaPacijenta spzt = new SekretarPrikazZakazanihTerminaPacijenta(p);
                spzt.ShowDialog();
            }
        }

        private void AzuriranjeAlergenaPacijenta()
        {
            Pacijent p = (Pacijent)dataGridPacijenti.SelectedItem;
            if (p != null)
            {
                SekretarAzuriranjeAlergena sa = new SekretarAzuriranjeAlergena(p);
                sa.ShowDialog();
            }
        }

        private void ZakazivanjeOperacije()
        {
            Pacijent p = (Pacijent)dataGridPacijenti.SelectedItem;
            if (p != null)
            {
                SekretarZakazivanjeOperacije szo = new SekretarZakazivanjeOperacije(p);
                szo.ShowDialog();
            }
        }

        private void ZakazivanjeHitnogTermina()
        {
            ZakazivanjeHitnogTermina zakazivanjeHitnogTermina = new ZakazivanjeHitnogTermina();
            zakazivanjeHitnogTermina.ShowDialog();
            OsvezavanjePrikazaPacijenata();
        }

        private void Button_Click_Novi(object sender, RoutedEventArgs e)
        {
            DodavanjePacijenta();
        }

        private void Button_Click_Izmeni(object sender, RoutedEventArgs e)
        {
            IzmeniPacijenta();
        }

        private void Button_Click_Obrisi(object sender, RoutedEventArgs e)
        {
            ObrisiPacijenta();
        }

        private void Button_Click_NoviGuest(object sender, RoutedEventArgs e)
        {
            DodavanjeGuestPacijenta();
        }

        private void Button_Click_NovoObavestenje(object sender, RoutedEventArgs e)
        {
            DodavanjeObavestenja();
        }

        private void Button_Click_IzmenaObavestenja(object sender, RoutedEventArgs e)
        {
            IzmenaObavestenja();
        }

        private void Button_Click_BrisanjeObavestenja(object sender, RoutedEventArgs e)
        {
            BrisanjeObavestenja();
        }

        private void Button_Click_Zakazivanje_Pregleda(object sender, RoutedEventArgs e)
        {
            ZakazivanjePregleda();
        }

        private void Button_Click_Prikaz_Termina(object sender, RoutedEventArgs e)
        {
            PrikazTerminaPacijenta();
        }

        private void Button_Click_Azuriranje_Alergena(object sender, RoutedEventArgs e)
        {
            AzuriranjeAlergenaPacijenta();
        }

        private void Button_Click_Zakazivanje_Operacije(object sender, RoutedEventArgs e)
        {
            ZakazivanjeOperacije();
        }

        private void Button_Click_Hitan_Termin(object sender, RoutedEventArgs e)
        {
            ZakazivanjeHitnogTermina();
        }

        private void MenuItem_Click_Novi_Pacijent(object sender, RoutedEventArgs e)
        {
            DodavanjePacijenta();
        }

        private void MenuItem_Click_Novi_Gostujuci(object sender, RoutedEventArgs e)
        {
            DodavanjeGuestPacijenta();
        }

        private void MenuItem_Click_Novo_Obavestenje(object sender, RoutedEventArgs e)
        {
            DodavanjeObavestenja();
        }

        private void MenuItem_Click_Izmeni_Pacijenta(object sender, RoutedEventArgs e)
        {
            IzmeniPacijenta();
        }

        private void MenuItem_Click_Azuriraj_Alergene(object sender, RoutedEventArgs e)
        {
            AzuriranjeAlergenaPacijenta();
        }

        private void MenuItem_Click_Obrisi_Pacijenta(object sender, RoutedEventArgs e)
        {
            ObrisiPacijenta();
        }

        private void MenuItem_Click_Izmeni_Obavestenje(object sender, RoutedEventArgs e)
        {
            IzmenaObavestenja();
        }

        private void MenuItem_Click_Obrisi_Obavestenje(object sender, RoutedEventArgs e)
        {
            BrisanjeObavestenja();
        }

        private void MenuItem_Click_Prikaz_Termina(object sender, RoutedEventArgs e)
        {
            PrikazTerminaPacijenta();
        }

        private void ToolBar_Button_Click_Novi_Pacijent(object sender, RoutedEventArgs e)
        {
            DodavanjePacijenta();
        }

        private void ToolBar_Button_Click_Novi_Guest_Pacijent(object sender, RoutedEventArgs e)
        {
            DodavanjeGuestPacijenta();
        }

        private void ToolBar_Button_Click_Novo_Obavestenje(object sender, RoutedEventArgs e)
        {
            DodavanjeObavestenja();
        }

        private void Button_Click_Radno_Vreme_Lekara_Opste(object sender, RoutedEventArgs e)
        {
            Lekar lekar = (Lekar) dgLekariOpstePrakse.SelectedItem;
            if (lekar != null)
            {
                RadnoVremeLekara rvl = new RadnoVremeLekara(lekar);
                rvl.ShowDialog();
            }
        }

        private void Button_Click_Radno_Vreme_Lekara_Specijaliste(object sender, RoutedEventArgs e)
        {
            Lekar lekar = (Lekar) dgLekariSpecijalisti.SelectedItem;
            if (lekar != null)
            {
                RadnoVremeLekara rvl = new RadnoVremeLekara(lekar);
                rvl.ShowDialog();
            }
        }

        private void dgLekovi_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Lek selektovaniLek = (Lek) dgLekovi.SelectedItem;
            if (selektovaniLek != null)
            {
                AzurirajPrikazOpisaLeka(selektovaniLek);
            }
        }

        private void AzurirajPrikazOpisaLeka(Lek selektovaniLek)
        {
            labelImeSelektovanogLeka.Content = selektovaniLek.Ime;
            tbOpisSelektovanogLeka.Text = selektovaniLek.Opis;
        }

        private void dgSale_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Soba selektovanaSoba = (Soba) dgSale.SelectedItem;
            if (selektovanaSoba != null)
            {
                AzurirajPrikazOpisaSobe(selektovanaSoba);
            }
        }

        private void AzurirajPrikazOpisaSobe(Soba selektovanaSoba)
        {
            labelImeSelektovaneSobe.Content = selektovanaSoba.Id;
            dgOpremaSelektovaneSobe.ItemsSource = sadrzajSobeKontroler.GetSadrzajSobe(selektovanaSoba.Id);

            if (selektovanaSoba.Tip == RoomType.operacionaSala)
            {
                OperacijaKontroler operacijaKontroler = new OperacijaKontroler();
                dgOperacijeSelektovaneSobe.ItemsSource = operacijaKontroler.GetSveBuduceOperacijeSale(selektovanaSoba.Id);
                labelImeSelektovaneSobe2.Content = selektovanaSoba.Id;
                dgOperacijeSelektovaneSobe.Visibility = Visibility.Visible;
                labelZaTermineSobe.Visibility = Visibility.Visible;
                labelImeSelektovaneSobe2.Visibility = Visibility.Visible;
            }
            else if (selektovanaSoba.Tip == RoomType.ordinacija)
            {
                PregledKontroler pregledKontroler = new PregledKontroler();
                //dgPreglediSelektovaneSobe = pregledKontroler.GetSviBuduciPreglediSobe();
            }
            else
            {
                labelZaTermineSobe.Visibility = Visibility.Hidden;
                labelImeSelektovaneSobe2.Visibility = Visibility.Hidden;
                dgOperacijeSelektovaneSobe.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click_Zauzetost_Lekara_Opste_Prakse(object sender, RoutedEventArgs e)
        {
            Lekar lekar = (Lekar) dgLekariOpstePrakse.SelectedItem;
            if (lekar != null)
            {
                PrikazZauzetostiLekaraOpstePrakse pzlop = new PrikazZauzetostiLekaraOpstePrakse(lekar);
                pzlop.ShowDialog();
            }
        }

        private void Button_Click_Zauzetost_Lekara_Specijaliste(object sender, RoutedEventArgs e)
        {
            Lekar lekar = (Lekar) dgLekariSpecijalisti.SelectedItem;
            if (lekar != null)
            {
                PrikazZauzetostiLekaraSpecijalista pzls = new PrikazZauzetostiLekaraSpecijalista(lekar);
                pzls.ShowDialog();
            }
        }

        private void Button_Click_Naplata_Usluga(object sender, RoutedEventArgs e)
        {
            Pacijent pacijent = (Pacijent) dataGridPacijenti.SelectedItem;
            if (pacijent != null)
            {
                NaplacivanjeUsluga nu = new NaplacivanjeUsluga(pacijent);
                nu.ShowDialog();
            }
        }

        private void Click_Video_Tutorijal(object sender, RoutedEventArgs e)
        {
            VideoTutorijalHitnogZakazivanja v = new VideoTutorijalHitnogZakazivanja();
            v.Show();
        }

        private void Button_Click_Recenzija(object sender, RoutedEventArgs e)
        {
            RecenzijaAplikacijeDesktop recenzija = new RecenzijaAplikacijeDesktop();
            recenzija.Show();
        }
    }
}
