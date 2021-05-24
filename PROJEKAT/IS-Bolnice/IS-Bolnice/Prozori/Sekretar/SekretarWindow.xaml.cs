using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Model;

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
            List<Pacijent> sviPacijenti = pacijentKontroler.GetSviPacijenti();
            Pacijenti.Clear();
            foreach (Pacijent pacijent in sviPacijenti)
            {
                Pacijenti.Add(pacijent);
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

                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxImage icnMessageBox = MessageBoxImage.Question;

                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        pacijentKontroler.ObrisiPacijenta(p);
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

                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxImage icnMessageBox = MessageBoxImage.Question;

                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        obavestenjeKontroler.ObrisiObavestenje(o);
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

        private void MenuItem_Click_Pomeranje_Termina(object sender, RoutedEventArgs e)
        {
            
        }

        private void MenuItem_Click_Otkazivanje_Termina(object sender, RoutedEventArgs e)
        {
            
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
            int index = dgLekariOpstePrakse.SelectedIndex;
            if (index != -1)
            {
                Lekar lekar = LekariOpstePrakse[index];
                RadnoVremeLekara rvl = new RadnoVremeLekara(lekar);
                rvl.ShowDialog();
            }
        }

        private void Button_Click_Radno_Vreme_Lekara_Specijaliste(object sender, RoutedEventArgs e)
        {
            int index = dgLekariSpecijalisti.SelectedIndex;
            if (index != -1)
            {
                Lekar lekar = LekariSpecijalisti[index];
                RadnoVremeLekara rvl = new RadnoVremeLekara(lekar);
                rvl.ShowDialog();
            }
        }

        private void dgLekovi_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = dgLekovi.SelectedIndex;
            if (index != -1)
            {
                Lek selektovaniLek = SviLekovi[index];
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
            int index = dgSale.SelectedIndex;
            if (index != -1)
            {
                Soba selektovanaSoba = SveSobeBolnice[index];
                AzurirajPrikazOpisaSobe(selektovanaSoba);
            }
        }

        private void AzurirajPrikazOpisaSobe(Soba selektovanaSoba)
        {
            labelImeSelektovaneSobe.Content = selektovanaSoba.Id;
            dgOpremaSelektovaneSale.ItemsSource = sadrzajSobeKontroler.GetSadrzajSobe(selektovanaSoba.Id);
        }
    }
}
