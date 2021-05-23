using IS_Bolnice.Baze;
using IS_Bolnice.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace IS_Bolnice.Prozori.Sekretar
{
    public partial class ZakazivanjeHitnogTermina : Window
    {
        private Pacijent odabraniPacijent = new Pacijent();
        private BazaPacijenata bazaPacijenata = new BazaPacijenata();
        private BazaOblastiLekara bazaOblastiLekara = new BazaOblastiLekara();

        public ObservableCollection<Pregled> PreglediZaOdlaganje
        {
            get;
            set;
        }

        public ObservableCollection<Operacija> OperacijeZaOdlaganje
        {
            get;
            set;
        }

        public ZakazivanjeHitnogTermina()
        {
            InitializeComponent();
            this.DataContext = this;
            PreglediZaOdlaganje = new ObservableCollection<Pregled>();
            OperacijeZaOdlaganje = new ObservableCollection<Operacija>();
            PopunjavanjeOblastiLekara();
            PopunjavanjePonudjenihTrajanja();
        }

        private void PopunjavanjeOblastiLekara()
        {
            List<OblastLekara> sveOblastiIzBaze = bazaOblastiLekara.SveOblasti();
            List<string> sveOblastiZaPrikaz = new List<string>();

            foreach (OblastLekara oblast in sveOblastiIzBaze)
            {
                sveOblastiZaPrikaz.Add(oblast.Naziv);
            }
            comboOblastLekara.ItemsSource = sveOblastiZaPrikaz;
        }

        private void PopunjavanjePonudjenihTrajanja()
        {
            List<double> trajanja = new List<double>();
            for (double i = 0.5; i <= 24; i += 0.5)
            {
                trajanja.Add(i);
            }
            comboTrajanja.ItemsSource = trajanja;
        }

        private void OsvezavanjePrikazaZauzetihTermina(OblastLekara oblastLekara)
        {
            BazaPregleda bazaPregleda = new BazaPregleda();
            BazaOperacija bazaOperacija = new BazaOperacija();

            OsvezavanjePrikazaZauzetihPregleda(bazaPregleda.ZauzetiHitniPreglediLekaraOdredjeneOblasti(oblastLekara));
            OsvezavanjePrikazaZauzetihOperacija(bazaOperacija.ZauzeteOperacijeLekaraOdredjeneOblastiZaOdlaganje(oblastLekara));
        }

        private void OsvezavanjePrikazaZauzetihPregleda(List<Pregled> zauzetiPregledi)
        {
            PreglediZaOdlaganje.Clear();
            foreach (Pregled pregled in zauzetiPregledi)
            {
                PreglediZaOdlaganje.Add(pregled);
            }
        }

        private void OsvezavanjePrikazaZauzetihOperacija (List<Operacija> zauzeteOperacije)
        {
            OperacijeZaOdlaganje.Clear();
            foreach(Operacija operacija in zauzeteOperacije)
            {
                OperacijeZaOdlaganje.Add(operacija);
            }
        }

        private void UpdateTextBox()
        {
            if (odabraniPacijent != null)
            {
                odabraniPacijentTxt.Text = odabraniPacijent.Ime + " " + odabraniPacijent.Prezime + " (JMBG: " + odabraniPacijent.Jmbg + ")";
            }
        }

        private void Button_Click_Postojeci(object sender, RoutedEventArgs e)
        {
            PrikazSvihPacijenata prikazSvihPacijenata = new PrikazSvihPacijenata(odabraniPacijent);
            prikazSvihPacijenata.ShowDialog();
            UpdateTextBox();
        }

        private void Button_Click_Gostujuci(object sender, RoutedEventArgs e)
        {
            DodavanjeGuestNalogaWindow dodavanjeGostujuceg = new DodavanjeGuestNalogaWindow();
            dodavanjeGostujuceg.ShowDialog();
            // u slucaju dodavanja gostujuceg uzima se poslednji iz baze
            odabraniPacijent = bazaPacijenata.poslednjiDodat();
            UpdateTextBox();
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            if (!ValidnoPopunjenaPolja())
            {
                return;
            }

            if ((bool)rbPregled.IsChecked)
            {
                ZakazivanjePregleda();
            }
            else if ((bool)rbOperacija.IsChecked)
            {
                ZakazivanjeOperacije();
            }
            else
            {
                string message = "Odaberite tip termina!";
                MessageBox.Show(message);
            }
        }

        private bool ValidnoPopunjenaPolja()
        {
            return odabraniPacijent != null && comboOblastLekara.SelectedIndex != -1 && comboTrajanja.SelectedIndex != -1;
        }

        private void ZakazivanjePregleda()
        {
            OblastLekara oblastLekara = new OblastLekara((string)comboOblastLekara.SelectedItem);
            double trajanjeTermina = (double)comboTrajanja.SelectedItem;
            BazaPregleda bazaPregleda = new BazaPregleda();
            List<Pregled> slobodniPregledi = bazaPregleda.SlobodniHitniPreglediLekaraOdredjeneOblasti(oblastLekara, trajanjeTermina);
            
            if (slobodniPregledi != null)
            {
                Pregled pregled = slobodniPregledi[0];
                pregled.Pacijent = odabraniPacijent;
                bazaPregleda.ZakaziPregled(pregled);
                string message = "Uspesno zakazan termin";
                MessageBox.Show(message);
                Close();
            }
            else
            {
                string message = "Nema slobodnih termina u skorije vreme! OBRISATI OVO";
                MessageBox.Show(message);
                OsvezavanjePrikazaZauzetihTermina(oblastLekara);
            }
        }

        private void ZakazivanjeOperacije()
        {
            OblastLekara oblastLekara = new OblastLekara((string)comboOblastLekara.SelectedItem);
            double trajanjeTermina = (double)comboTrajanja.SelectedItem;
            BazaOperacija bazaOperacija = new BazaOperacija();
            List<Operacija> slobodneOperacije = bazaOperacija.SlobodneHitneOperacijeLekaraOdredjeneOblasti(oblastLekara, trajanjeTermina);

            if (slobodneOperacije != null)
            {
                Operacija operacija = slobodneOperacije[0];
                operacija.Pacijent = odabraniPacijent;
                bazaOperacija.ZakaziOperaciju(operacija);

                string message = "Uspesno zakazan termin";
                MessageBox.Show(message);
                Close();
            }
            else
            {
                string message = "Nema slobodnih termina u skorije vreme! OBRISATI OVO";
                MessageBox.Show(message);
                OsvezavanjePrikazaZauzetihTermina(oblastLekara);
            }
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_Odlozi_Pregled(object sender, RoutedEventArgs e)
        {
            if (dgPregledi.SelectedItems.Count > 0)
            {
                BazaPregleda bazaPregleda = new BazaPregleda();
                for (int i = 0; i < dgPregledi.SelectedItems.Count; i++)
                {
                    Pregled pregledZaOdlaganje = (Pregled)dgPregledi.SelectedItems[i];
                    bazaPregleda.OdloziPregledStoPre(pregledZaOdlaganje);
                }
                OblastLekara oblastLekara = new OblastLekara((string)comboOblastLekara.SelectedItem);
                OsvezavanjePrikazaZauzetihPregleda(bazaPregleda.ZauzetiHitniPreglediLekaraOdredjeneOblasti(oblastLekara));
            }
        }

        private void Button_Click_Odlozi_Operaciju(object sender, RoutedEventArgs e)
        {
            if (dgOperacije.SelectedItems.Count > 0)
            {
                BazaOperacija bazaOperacija = new BazaOperacija();
                for (int i = 0; i < dgOperacije.SelectedItems.Count; i++)
                {
                    Operacija operacijaZaOdlaganje = (Operacija)dgOperacije.SelectedItems[i];
                    bazaOperacija.OdloziOperaciju(operacijaZaOdlaganje);
                }
                OblastLekara oblastLekara = new OblastLekara((string)comboOblastLekara.SelectedItem);
                OsvezavanjePrikazaZauzetihOperacija(bazaOperacija.ZauzeteOperacijeLekaraOdredjeneOblastiZaOdlaganje(oblastLekara));
            }
        }
    }
}
