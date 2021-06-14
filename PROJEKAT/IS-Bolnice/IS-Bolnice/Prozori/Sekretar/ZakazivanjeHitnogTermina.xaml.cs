using IS_Bolnice.Repozitorijumi;
using IS_Bolnice.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Kontroleri.Korisnicki;
using IS_Bolnice.Kontroleri.Termini;

namespace IS_Bolnice.Prozori.Sekretar
{
    public partial class ZakazivanjeHitnogTermina : Window
    {
        private Pacijent odabraniPacijent = new Pacijent();
        private PacijentKontroler pacijentKontroler = new PacijentKontroler();
        private PregledKontroler pregledKontroler = new PregledKontroler();
        private OperacijaKontroler operacijaKontroler = new OperacijaKontroler();
        private OblastLekaraKontroler oblastLekaraKontroler = new OblastLekaraKontroler();

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
            PopunjavanjePonudjenihTrajanja();
        }

        private void PopunjavanjeOblastiLekara()
        {
            List<OblastLekara> sveOblastiIzBaze = oblastLekaraKontroler.GetSveOblastiLekara();
            List<string> sveOblastiZaPrikaz = new List<string>();

            foreach (OblastLekara oblast in sveOblastiIzBaze)
            {
                if ((bool) rbOperacija.IsChecked && oblast.Naziv.Equals(OblastLekara.oznakaOpstePrakse))
                {
                    continue;
                }
                sveOblastiZaPrikaz.Add(oblast.Naziv);
            }
            comboOblastLekara.ItemsSource = sveOblastiZaPrikaz;
            comboOblastLekara.IsEnabled = true;
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
            OsvezavanjePrikazaZauzetihPregleda(pregledKontroler.GetZauzetiHitniPreglediLekaraOdredjeneOblasti(oblastLekara));
            OsvezavanjePrikazaZauzetihOperacija(operacijaKontroler.GetZauzeteOperacijeLekaraOdredjeneOblastiZaOdlaganje(oblastLekara));
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
            odabraniPacijent = pacijentKontroler.GetPoslednjiDodat();
            if (odabraniPacijent.Guest)
            {
                UpdateTextBox();
            }
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
                InformativniProzor ip = new InformativniProzor("Odaberite tip termina.");
                ip.ShowDialog();
            }
        }

        private bool ValidnoPopunjenaPolja()
        {
            return odabraniPacijent != null && comboOblastLekara.SelectedIndex != -1 && comboTrajanja.SelectedIndex != -1;
        }

        private void ZakazivanjePregleda()
        {
            OblastLekara oblastLekara = new OblastLekara((string)comboOblastLekara.SelectedItem);
            double satiTrajanjaTermina = (double)comboTrajanja.SelectedItem;
            int minutiTrajanjaTermina = (int) (satiTrajanjaTermina * 60);
            List<Pregled> slobodniPregledi =
                pregledKontroler.GetSlobodniHitniPreglediLekaraOdredjeneOblasti(oblastLekara, minutiTrajanjaTermina);

            if (slobodniPregledi != null)
            {
                Pregled pregled = slobodniPregledi[0];
                pregled.Pacijent = odabraniPacijent;
                pregledKontroler.ZakaziPregled(pregled);

                InformativniProzor ip = new InformativniProzor("Uspešno zakazan termin sa početkom u " +
                                                               pregled.VremePocetkaPregleda.ToString("HH:mm") + 
                                                               "h  " + pregled.VremePocetkaPregleda.ToString("dd.MM."));
                ip.ShowDialog();

                Close();
            }
            else
            {
                InformativniProzor ip = new InformativniProzor("Nema slobodnih termina u skorije vreme. Možete odabrati" +
                                                               " neki od ponuđenih da se odloži ukoliko postoji.");
                ip.ShowDialog();
                OsvezavanjePrikazaZauzetihTermina(oblastLekara);
            }
        }

        private void ZakazivanjeOperacije()
        {
            OblastLekara oblastLekara = new OblastLekara((string)comboOblastLekara.SelectedItem);
            double trajanjeTermina = (double)comboTrajanja.SelectedItem;
            int minutiTrajanjaTermina = (int) (trajanjeTermina * 60);
            List<Operacija> slobodneOperacije =
                operacijaKontroler.GetSlobodneHitneOperacijeLekaraOdredjeneOblasti(oblastLekara, minutiTrajanjaTermina);

            if (slobodneOperacije != null)
            {
                Operacija operacija = slobodneOperacije[0];
                operacija.Pacijent = odabraniPacijent;
                operacijaKontroler.ZakaziOperaciju(operacija);

                InformativniProzor ip = new InformativniProzor("Uspešno zakazan termin sa početkom u " + 
                                                               operacija.VremePocetkaOperacije.ToString("HH:mm") +
                                                               "h  " + operacija.VremePocetkaOperacije.ToString("dd.MM."));
                ip.ShowDialog();
                Close();
            }
            else
            {
                InformativniProzor ip = new InformativniProzor("Nema slobodnih termina u skorije vreme. Možete odabrati " +
                                                               "neki od ponuđenih da se odloži ukoliko postoji.");
                ip.ShowDialog();
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
                for (int i = 0; i < dgPregledi.SelectedItems.Count; i++)
                {
                    Pregled pregledZaOdlaganje = (Pregled)dgPregledi.SelectedItems[i];
                    pregledKontroler.OdloziPregledStoPre(pregledZaOdlaganje);
                }
                OblastLekara oblastLekara = new OblastLekara((string)comboOblastLekara.SelectedItem);
                OsvezavanjePrikazaZauzetihPregleda(pregledKontroler.GetZauzetiHitniPreglediLekaraOdredjeneOblasti(oblastLekara));
            }
        }

        private void Button_Click_Odlozi_Operaciju(object sender, RoutedEventArgs e)
        {
            if (dgOperacije.SelectedItems.Count > 0)
            {
                for (int i = 0; i < dgOperacije.SelectedItems.Count; i++)
                {
                    Operacija operacijaZaOdlaganje = (Operacija)dgOperacije.SelectedItems[i];
                    operacijaKontroler.OdloziOperacijuStoPre(operacijaZaOdlaganje);
                }
                OblastLekara oblastLekara = new OblastLekara((string)comboOblastLekara.SelectedItem);
                OsvezavanjePrikazaZauzetihOperacija(operacijaKontroler.GetZauzeteOperacijeLekaraOdredjeneOblastiZaOdlaganje(oblastLekara));
            }
        }

        private void rbPregled_Checked(object sender, RoutedEventArgs e)
        {
            PopunjavanjeOblastiLekara();
        }

        private void rbOperacija_Checked(object sender, RoutedEventArgs e)
        {
            PopunjavanjeOblastiLekara();
        }
    }
}
