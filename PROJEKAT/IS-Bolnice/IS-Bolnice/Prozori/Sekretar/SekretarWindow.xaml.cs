using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace IS_Bolnice.Prozori.Sekretar
{
    /// <summary>
    /// Interaction logic for SekretarWindow.xaml
    /// </summary>
    public partial class SekretarWindow : Window
    {
        private BazaObavestenja bo;
        private BazaPacijenata bp;

        public ObservableCollection<Pacijent> Pacijenti
        {
            get;
            set;
        }

        public ObservableCollection<Obavestenje> Obavestenja
        {
            get;
            set;
        }

        public SekretarWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            bp = new BazaPacijenata();
            bo = new BazaObavestenja();

            Pacijenti = new ObservableCollection<Pacijent>(bp.SviPacijenti());
            Obavestenja = new ObservableCollection<Obavestenje>(bo.SvaObavestenja());
        }

        private void OsvezavanjePrikazaPacijenata()
        {
            List<Pacijent> sviPacijenti = bp.SviPacijenti();
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
                        bp.ObrisiPacijenta(p);
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
                        bo.ObrisiObavestenje(o);
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
    }
}
