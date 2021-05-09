using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace IS_Bolnice.Prozori.Sekretar
{
    /// <summary>
    /// Interaction logic for SekretarPrikazZakazanihTerminaPacijenta.xaml
    /// </summary>
    public partial class SekretarPrikazZakazanihTerminaPacijenta : Window
    {
        private BazaPregleda bazaPregleda = new BazaPregleda();
        private BazaOperacija bazaOperacija = new BazaOperacija();

        public ObservableCollection<Pregled> PreglediPacijenta
        {
            get;
            set;
        }

        public ObservableCollection<Operacija> OperacijePacijenta
        {
            get;
            set;
        }

        public SekretarPrikazZakazanihTerminaPacijenta(Pacijent p)
        {
            InitializeComponent();

            this.DataContext = this;

            pacijentTxt.Text = p.Ime + " " + p.Prezime;

            PreglediPacijenta = new ObservableCollection<Pregled>(bazaPregleda.SviBuduciPreglediKojePacijentIma(p.Jmbg));
            OperacijePacijenta = new ObservableCollection<Operacija>(bazaOperacija.SveSledeceOperacijeZaPacijenta(p));
        }

        private void Button_Click_Otkazi_Pregled(object sender, RoutedEventArgs e)
        {
            Pregled pregled = PreglediPacijenta[dataGridPregledi.SelectedIndex];
            if (pregled != null)
            {
                string sMessageBoxText = "Da li ste sigurni da želite da otkažete termin pregleda?";
                string sCaption = "Otkazivanje termina pregleda";

                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxImage icnMessageBox = MessageBoxImage.Question;

                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        bazaPregleda.OtkaziPregled(pregled);
                        PreglediPacijenta.Remove(pregled);
                        break;

                    case MessageBoxResult.No:
                        /* ... */
                        break;
                }
            }
        }

        private void Button_Click_Otkazi_Operaciju(object sender, RoutedEventArgs e)
        {
            Operacija operacija = OperacijePacijenta[dataGridOperacije.SelectedIndex];
            if (operacija != null)
            {
                string sMessageBoxText = "Da li ste sigurni da želite da otkažete termin operacije?";
                string sCaption = "Otkazivanje termina operacije";

                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxImage icnMessageBox = MessageBoxImage.Question;

                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        bazaOperacija.OtkaziOperaciju(operacija);
                        OperacijePacijenta.Remove(operacija);
                        break;

                    case MessageBoxResult.No:
                        /* ... */
                        break;
                }
            }
        }

        private void Button_Click_Pomeri_Pregled(object sender, RoutedEventArgs e)
        {
            int index = dataGridPregledi.SelectedIndex;
            if (index != -1)
            {
                Pregled stariPregled = PreglediPacijenta[index];
                Lekar lekar = stariPregled.Lekar;

                Pregled noviPregled = new Pregled(stariPregled);

                // na prvom mestu je pocetak termina a na drugom kraj termina
                List < DateTime > termin = new List<DateTime>();
                termin.Add(stariPregled.VremePocetkaPregleda);
                termin.Add(stariPregled.VremeKrajaPregleda);

                OdredjivanjeTermina ot = new OdredjivanjeTermina(termin, lekar);
                ot.ShowDialog();

                // unutar liste termin moze da se nalazi samo pocetak i samo kraj termina
                if (termin.Count == 2)
                {
                    noviPregled.VremePocetkaPregleda = termin[0];
                    noviPregled.VremeKrajaPregleda = termin[1];

                    bazaPregleda.IzmeniPregled(noviPregled, stariPregled);
                    PreglediPacijenta[index] = noviPregled;
                }
            }
        }

        private void Button_Click_Pomeri_Operaciju(object sender, RoutedEventArgs e)
        {
            int index = dataGridOperacije.SelectedIndex;
            if (index != -1)
            {
                Operacija staraOperacija = OperacijePacijenta[index];
                Operacija novaOperacija = new Operacija(staraOperacija);
                Lekar lekar = staraOperacija.Lekar;

                // na prvom mestu je pocetak termina a na drugom kraj termina
                List<DateTime> termin = new List<DateTime>();
                termin.Add(staraOperacija.VremePocetkaOperacije);
                termin.Add(staraOperacija.VremeKrajaOperacije);

                OdredjivanjeTermina ot = new OdredjivanjeTermina(termin, lekar);
                ot.ShowDialog();

                // unutar liste termin moze da se nalazi samo pocetak i samo kraj termina
                if (termin.Count == 2)
                {
                    novaOperacija.VremePocetkaOperacije = termin[0];
                    TimeSpan trajanje = staraOperacija.VremeKrajaOperacije.Subtract(staraOperacija.VremePocetkaOperacije);
                    novaOperacija.VremeKrajaOperacije = novaOperacija.VremePocetkaOperacije.Add(trajanje);

                    bazaOperacija.IzmeniOperaciju(novaOperacija, staraOperacija);
                    OperacijePacijenta[index] = novaOperacija;
                }
            }
        }
    }
}
