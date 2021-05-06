using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace IS_Bolnice.Prozori.Sekretar
{
    /// <summary>
    /// Interaction logic for SekretarPrikazZakazanihTerminaPacijenta.xaml
    /// </summary>
    public partial class SekretarPrikazZakazanihTerminaPacijenta : Window
    {
        private Pacijent pacijent;
        private BazaPregleda bp;
        private BazaOperacija bo;

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
            bp = new BazaPregleda();
            bo = new BazaOperacija();

            pacijent = p;
            this.DataContext = this;

            pacijentTxt.Text = p.Ime + " " + p.Prezime;

            PreglediPacijenta = new ObservableCollection<Pregled>(bp.SviBuduciPreglediKojePacijentIma(p.Jmbg));
            OperacijePacijenta = new ObservableCollection<Operacija>(bo.OperacijeOdredjenogPacijenta(p));
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
                        bp.OtkaziPregled(pregled);
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
                        bo.OtkaziOperaciju(operacija);
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

                Pregled noviPregled = new Pregled
                {
                    Lekar = stariPregled.Lekar,
                    Pacijent = stariPregled.Pacijent,
                    VremePocetkaPregleda = stariPregled.VremePocetkaPregleda,
                    VremeKrajaPregleda = stariPregled.VremeKrajaPregleda
                };

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

                    bp.IzmeniPregled(noviPregled, stariPregled);
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
                Operacija novaOperacija = new Operacija
                {
                    Lekar = staraOperacija.Lekar,
                    Pacijent = staraOperacija.Pacijent,
                    Soba = staraOperacija.Soba,
                    VremePocetkaOperacije = staraOperacija.VremePocetkaOperacije,
                    VremeKrajaOperacije = staraOperacija.VremeKrajaOperacije
                };
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

                    bo.IzmeniOperaciju(novaOperacija, staraOperacija);
                    OperacijePacijenta[index] = novaOperacija;
                }
            }
        }
    }
}
