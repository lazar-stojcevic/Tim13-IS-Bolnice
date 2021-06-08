using IS_Bolnice.Kontroleri;
using IS_Bolnice.Model;
using IS_Bolnice.Prozori.Prozori_za_pacijenta;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace IS_Bolnice.ViewModel.VMPacijent
{
    class KreiranjeBelezkeViewModel : ViewModel
    {
        private string nazivBelezke;
        private string sadrzajBelezke;
        private string duzinaTrajanjaBelezke;

        private KreiranjeBelezke kreiranjeBelezkeProzor;
        private ListView listaZaOsvezavanje;
        private string jmbgPacijenta;

        private BelezkeKontroler belezkeKontroler = new BelezkeKontroler();

        private RelayCommand napraviBelezku;
        private RelayCommand odustani;

        public KreiranjeBelezkeViewModel(KreiranjeBelezke kreiranjeBelezke, string jmbg, ListView listaObavestenja = null)
        {
            listaZaOsvezavanje = listaObavestenja;
            jmbgPacijenta = jmbg;
            kreiranjeBelezkeProzor = kreiranjeBelezke;

            Odustani = new RelayCommand(IzvrsiOdustaniKomandu);
            NapraviBelezku = new RelayCommand(IzvrsiPotvrdiKomandu);

            InicijalizujPolja();
        }

        public string NazivBelezke
        {
            get { return nazivBelezke; }
            set
            {
                nazivBelezke = value;
                OnPropertyChanged();
            }
        }

        public string SadrzajBelezke
        {
            get { return sadrzajBelezke; }
            set
            {
                sadrzajBelezke = value;
                OnPropertyChanged();
            }
        }

        public string DuzinaTrajanjaBelezke
        {
            get { return duzinaTrajanjaBelezke; }
            set
            {
                duzinaTrajanjaBelezke = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand NapraviBelezku
        {
            get { return napraviBelezku; }
            set
            {
                napraviBelezku = value;
            }
        }

        public RelayCommand Odustani
        {
            get { return odustani; }
            set
            {
                odustani = value;
            }
        }

        private void IzvrsiOdustaniKomandu(object obj)
        {
            kreiranjeBelezkeProzor.Close();
        }

        private void IzvrsiPotvrdiKomandu(object obj)
        {
            if (ValidacijaUnosa())
            {
                belezkeKontroler.SacuvajBelezku(NapraviBelesku());
                OsveziPrikaz();
                IzvrsiOdustaniKomandu(null);
            }
        }

        private bool ValidacijaUnosaTeksta()
        {
            string strRegex = "[#]+";
            Regex re = new Regex(strRegex);

            if (re.IsMatch(this.SadrzajBelezke) || re.IsMatch(this.NazivBelezke))
            {
                string message = "Molimo vas da obrišete karakter - tarabu";
                MessageBox.Show(message);
                return false;
            }

            return true;
        }

        private bool ValidacijaUnosaDuzineTrajanjaBelezke()
        {
            string strRegex = "^[0-9]+$";
            Regex re = new Regex(strRegex);

            if (re.IsMatch(this.DuzinaTrajanjaBelezke))
            {

                return true;
            }

            string message = "Molimo vas da unesete broj u polje vreme vazenja u danima";
            MessageBox.Show(message);
            return false;
        }

        private bool ValidacijaUnosa()
        {
            return ValidacijaUnosaDuzineTrajanjaBelezke() && ValidacijaUnosaTeksta();
        }

        private void OsveziPrikaz()
        {
            if (listaZaOsvezavanje != null)
                listaZaOsvezavanje.ItemsSource = belezkeKontroler.SveTrenutneBelezkePacijenta(jmbgPacijenta);
        }

        private Beleska NapraviBelesku()
        {
            Beleska belezka = new Beleska();

            Pacijent pacijent = new Pacijent();
            pacijent.Jmbg = jmbgPacijenta;

            belezka.Pacijent = pacijent;
            belezka.Naziv = this.NazivBelezke;
            belezka.Komentar = this.SadrzajBelezke;
            belezka.PeriodVazenja = Int32.Parse(this.DuzinaTrajanjaBelezke);

            return belezka;
        }

        private void InicijalizujPolja()
        {
            this.NazivBelezke = "";
            this.SadrzajBelezke = "";
            this.DuzinaTrajanjaBelezke = "0";
        }
    }
}
