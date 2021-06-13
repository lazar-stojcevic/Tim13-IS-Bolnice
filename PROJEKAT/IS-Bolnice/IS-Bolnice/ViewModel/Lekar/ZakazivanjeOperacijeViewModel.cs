using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using IS_Bolnice.DTOs;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Kontroleri.Ustanova;
using IS_Bolnice.Kontroleri.Korisnicki;
using IS_Bolnice.Kontroleri.Termini;
using IS_Bolnice.Model;
using WPFCustomMessageBox;

namespace IS_Bolnice.ViewModel.Lekar
{
    class ZakazivanjeOperacijeViewModel : ViewModel
    {
        private WPFNavigacijaKontroler navigacijaKontroler;
        private string id;
        private Pacijent pacijent;
        private global::Lekar lekar;
        private DateTime vremePocetkaOperacije;
        private DateTime vremeKrajaOperacije;
        private Soba soba;
        private bool hitna;
        private Operacija operacija;
        private string duzinaTrajanjeOperacijeUMinutima;

        private ObservableCollection<global::Lekar> lekari = new ObservableCollection<global::Lekar>();
        private ObservableCollection<Soba> operacioneSale = new ObservableCollection<Soba>();
        private ObservableCollection<Operacija> termini = new ObservableCollection<Operacija>();

        private bool dugmePotrvrdi;

        private LekarKontroler lekarKontroler = new LekarKontroler();
        private PacijentKontroler pacijentKontroler = new PacijentKontroler();
        private OperacijaKontroler operacijaKontroler = new OperacijaKontroler();
        private BolnicaKontroler bolnicaKontroler = new BolnicaKontroler();
        private RadnoVremeKontroler radnoVremeKontroler = new RadnoVremeKontroler();


        public WPFNavigacijaKontroler NavigacijaKontroler
        {
            get { return navigacijaKontroler; }
            set
            {
                navigacijaKontroler = value;
                OnPropertyChanged();
            }
        }

        public Operacija Operacija
        {
            get { return operacija; }
            set
            {
                operacija = value;
                OnPropertyChanged();
                DugmePotrvrdi = true;
            }
        }

        public Pacijent Pacijent
        {
            get { return pacijent; }
            set { pacijent = value; }
        }

        public global::Lekar Lekar
        {
            get { return lekar; }
            set
            {
                lekar = value;
                OnPropertyChanged();
                Execute_PromenaSelektovanihPodataka(null);
            }
        }

        public ObservableCollection<global::Lekar> Lekari
        {
            get { return lekari; }
            set
            {
                lekari = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Soba> OperacioneSale
        {
            get { return operacioneSale; }
            set
            {
                operacioneSale = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Operacija> Termini
        {
            get { return termini; }
            set
            {
                termini = value;
                OnPropertyChanged();
            }
        }

        public DateTime VremePocetkaOperacije
        {
            get { return vremePocetkaOperacije; }
            set
            {
                vremePocetkaOperacije = value;
                OnPropertyChanged();
            }
        }

        public DateTime VremeKrajaOperacije
        {
            get { return vremeKrajaOperacije; }
            set
            {
                vremeKrajaOperacije = value;
                OnPropertyChanged();
            }
        }

        public Soba Soba
        {
            get { return soba; }
            set
            {
                soba = value;
                OnPropertyChanged();
                Execute_PromenaSelektovanihPodataka(null);
            }
        }

        public string DuzinaTrajanjeOperacijeUMinutima
        {
            get { return duzinaTrajanjeOperacijeUMinutima; }
            set
            {
                duzinaTrajanjeOperacijeUMinutima = value;
                OnPropertyChanged();
                Execute_PromenaDuzineTrajanjeOperacije(null);
            }
        }

        public bool DugmePotrvrdi
        {
            get { return dugmePotrvrdi; }
            set
            {
                dugmePotrvrdi = value;
                OnPropertyChanged();
            }
        }

        public bool Hitna
        {
            get { return hitna; }
            set
            {
                hitna = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand dodajOperaciju;

        private RelayCommand odustaniOdZakazivanjeOperacije;

        private RelayCommand promenaSelektovanihPodataka;

        private RelayCommand promenaDuzineTrajanjeOperacije;


        public RelayCommand DodajOperaciju
        {
            get { return dodajOperaciju; }
            set { dodajOperaciju = value; }
        }

        public RelayCommand PromenaSelektovanihPodataka
        {
            get { return promenaSelektovanihPodataka; }
            set { promenaSelektovanihPodataka = value; }

        }

        public RelayCommand PromenaDuzineTrajanjeOperacije
        {
            get { return promenaDuzineTrajanjeOperacije; }
            set { promenaDuzineTrajanjeOperacije = value; }

        }

        public RelayCommand OdustaniOdZakazivanjeOperacije
        {
            get { return odustaniOdZakazivanjeOperacije; }
            set { odustaniOdZakazivanjeOperacije = value; }
        }

        public void Executed_DodajOperaciju(object obj)
        {
            if (this.Lekar != null && this.Soba != null && operacija != null)
            {
                operacija.Lekar = this.Lekar;
                operacija.VremeKrajaOperacije = operacija.VremePocetkaOperacije.AddMinutes(Int32.Parse(duzinaTrajanjeOperacijeUMinutima));
                operacija.Hitna = this.Hitna;
                operacija.Pacijent = this.Pacijent;
                operacija.Soba = this.Soba;

                operacijaKontroler.ZakaziOperaciju(operacija);
                CustomMessageBox.ShowOK("Uspešno ste kreirali operaciju", "Kreirana operacija", "Dobro", MessageBoxImage.Information);
                navigacijaKontroler.IdiNazad();
            }
            else
            {
                CustomMessageBox.ShowOK("Niste selektovali sve podatke", "Greška", "Dobro", MessageBoxImage.Error);
            }

        }

        public void Execute_CancelEditCommand(object obj)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da želite da odustanete od zakazivanje operacije?", "Zakazivanje operacije", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                navigacijaKontroler.IdiNazad();
            }
        }

        public void Execute_PromenaSelektovanihPodataka(object obj)
        {
            PostavljanjeParametara();
        }

        public void Execute_PromenaDuzineTrajanjeOperacije(object obj)
        {
            PostavljanjeParametara();
        }



        private void PostavljanjeParametara()
        {
            
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (lekar == null || duzinaTrajanjeOperacijeUMinutima == null)
                {
                    DugmePotrvrdi = false;
                    return;
                }

                if (operacija == null || duzinaTrajanjeOperacijeUMinutima == null || lekar == null)
                {
                    DugmePotrvrdi = false;
                }
                else
                {
                    DugmePotrvrdi = true;
                }

                int trajanjeOperacije;
                try
                {
                    trajanjeOperacije = Int32.Parse(duzinaTrajanjeOperacijeUMinutima);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Niste validno uneli dužinu trajanja operacije", "Dužina trajanja operacije",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                OperacijaDTO operacijaDto = new OperacijaDTO()
                {
                    Lekar = lekar,
                    Soba = soba,
                    TrajanjeOperacijeUMinutima = trajanjeOperacije
                };
                RadnoVremeLekara radnoVreme = new RadnoVremeKontroler().GetRadnoVremeLekara(lekar.Jmbg);

                foreach (Operacija termin in operacijaKontroler.GetDostuptniTerminiLekaraZaDatuProstoriju(operacijaDto))
                {
                    termin.Lekar.RadnoVreme = radnoVreme;
                    Termini.Add(termin);
                }

                
                if (Termini.Count != 0)
                {
                    operacija = termini.ElementAt(0);
                }


            }
            finally
            {
                Mouse.OverrideCursor = null;
            }

        }

        public ZakazivanjeOperacijeViewModel(NavigationService navigationService, string jmbgPacijenta)
        {
            NavigacijaKontroler = new WPFNavigacijaKontroler(navigationService);

            DodajOperaciju = new RelayCommand(Executed_DodajOperaciju);

            PromenaSelektovanihPodataka = new RelayCommand(Execute_PromenaSelektovanihPodataka);

            PromenaDuzineTrajanjeOperacije = new RelayCommand(Execute_PromenaDuzineTrajanjeOperacije);

            OdustaniOdZakazivanjeOperacije = new RelayCommand(Execute_CancelEditCommand);


            Pacijent = pacijentKontroler.GetPacijentSaOvimJMBG(jmbgPacijenta);
            foreach (Soba sala in bolnicaKontroler.GetSveOperacioneSale())
            {
                operacioneSale.Add(sala);
            }

            foreach (global::Lekar lk in lekarKontroler.GetSviLekariSpecijalisti())
            {
                Lekari.Add(lk);
            }

            Soba = operacioneSale.ElementAt(0);

            
        }
    }
}
