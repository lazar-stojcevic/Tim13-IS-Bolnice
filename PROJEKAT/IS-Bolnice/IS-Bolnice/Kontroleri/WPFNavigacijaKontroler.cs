using IS_Bolnice.Prozori.UpravnikPages;
using IS_Bolnice.ViewModel.Upravnik;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using IS_Bolnice.Prozori.Prikaz_kod_lekara;
using WPFCustomMessageBox;
using IS_Bolnice.Prozori.Prikaz_za_upravnika;

namespace IS_Bolnice.Kontroleri
{
    class WPFNavigacijaKontroler
    {
        private NavigationService navService;

        public WPFNavigacijaKontroler(NavigationService service)
        {
            navService = service;
        }

        public void PrikaziObavestenja()
        {
            navService.Navigate(new ObavestenjaPage());
        }

        public void PrikaziAddSalePage()
        {
            navService.Navigate(new AddSalePage());
        }

        public void PrikaziSalePage()
        {
            navService.Navigate(new SalePage());
        }

        public void PrikaziEditSalePage(string idSale)
        {
            navService.Navigate(new EditSalePage(idSale));
        }

        public void IdiNazad()
        {
            navService.GoBack();
        }

        public void PrikaziIzmenuPregleda(Pregled pregled)
        {
            navService.Navigate(new LekarIzmenaPregleda(pregled));
        }

        public void PrikaziIzmenuOperacije(Operacija operacija)
        {
            navService.Navigate(new IzmenaOperacije(operacija.Id));
        }

        public void PrikaziPacijenta(Pacijent pacijent)
        {
            navService.Navigate(new PodaciOPacijentu(pacijent.Jmbg));
        }

        public void OtvoriPregled(Pregled pregled)
        {
            LekarWindow lekarWindow = new LekarWindow(pregled.Lekar.Id);
            lekarWindow.txtIme.Text = pregled.Pacijent.Ime;
            lekarWindow.txtPrz.Text = pregled.Pacijent.Prezime;
            lekarWindow.txtJMBG.Text = pregled.Pacijent.Jmbg;
            lekarWindow.btnHospitalizacija.IsEnabled = true;
            lekarWindow.btnPodaci.IsEnabled = true;
            lekarWindow.btnIzvestaj.IsEnabled = true;
            lekarWindow.btnOperacija.IsEnabled = true;
            lekarWindow.btnPregled.IsEnabled = true;


            navService.Navigate(lekarWindow);
        }

        public void ZapocniPregled(string jmbgLekara)
        {
            LekarWindow lekarWindow = new LekarWindow(jmbgLekara);
            navService.Navigate(lekarWindow);
        }

        public void OtvoriRaspored(string jmbgLekara)
        {
            LekarRaspored lekarRaspored = new LekarRaspored(jmbgLekara);
            navService.Navigate(lekarRaspored);
        }

        public void OtvoriInventare()
        {
            LekarInventarPoSalama inventar = new LekarInventarPoSalama();
            navService.Navigate(inventar);
        }

        public void OtvoriRecenzije(string jmbgLekara)
        {
            LekarUvidURecenzije recenzije = new LekarUvidURecenzije(jmbgLekara);
            navService.Navigate(recenzije);
        }

        public void OtvoriZahtevZaGodinji()
        {
            LekarZahtevZaGodisnji zahtevZaGodisnji = new LekarZahtevZaGodisnji();
            navService.Navigate(zahtevZaGodisnji);
        }

        public void OtvoriValidacijuLekova(string jmbgLekara)
        {
            LekarValidacijaLekova validacija = new LekarValidacijaLekova(jmbgLekara);
            navService.Navigate(validacija);
        }

        public void OtvoriIzvestajOPotresnji()
        {
            LekarPotrosnja lekarPotrosnja = new LekarPotrosnja();
            navService.Navigate(lekarPotrosnja);
        }

        public void OtvoriUvidUOdobreneLekove()
        {
            LekarUvidUOdobreneLekove odobreniLekovi = new LekarUvidUOdobreneLekove();
            navService.Navigate(odobreniLekovi);
        }

        public void OtvoriSveHospitalizacije()
        {
            LekarPrikazSvihHospitalizacija hospitalizacije = new LekarPrikazSvihHospitalizacija();
            navService.Navigate(hospitalizacije);
        }

        public void OdjaviSe()
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da želite da se odjavite?", "Odjava", "Da", "Ne", MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                MainWindow prijava = new MainWindow();
                Application.Current.Windows[0].Close();
                prijava.ShowDialog();
            }
        }

        public void OstaviRecenziju()
        {
            RecenzijaAplikacije recenzija = new RecenzijaAplikacije();
            navService.Navigate(recenzija);
        }

        public void OtvoriObavestenja()
        {
            LekarObavestenja obavestenja = new LekarObavestenja();
            navService.Navigate(obavestenja);
        }
    }
}
