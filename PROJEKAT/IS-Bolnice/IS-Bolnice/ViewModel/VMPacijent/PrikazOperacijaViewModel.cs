using IS_Bolnice.Kontroleri;
using IS_Bolnice.Prozori.Prozori_za_pacijenta;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using System.Collections.Generic;
using System.Windows;
using IS_Bolnice.Kontroleri.GenerisanjeIzvestaja;
using IS_Bolnice.Kontroleri.Termini;

namespace IS_Bolnice.ViewModel.VMPacijent
{
    class PrikazOperacijaViewModel
    {
        private PrikazTerminaOperacija prikazTerminaOperacijaProzor;
        private OperacijaKontroler operacijaKontroler = new OperacijaKontroler();
        private string jmbgPacijenta;

        public PrikazOperacijaViewModel(PrikazTerminaOperacija prikazTerminaOperacija, string jmbgPacijenta)
        {
            this.prikazTerminaOperacijaProzor = prikazTerminaOperacija;

            OperacijePacijenta = operacijaKontroler.GetSveBuduceOperacijePacijenta(jmbgPacijenta);
            this.jmbgPacijenta = jmbgPacijenta;
            Izadji = new RelayCommand(IzvrsiIzadjiKomandu);
            Izvestaj = new RelayCommand(IzvrisIzvestajKomandu);
        }

        public List<Operacija> OperacijePacijenta { get; set; }

        public RelayCommand Izadji { get; set; }

        public RelayCommand Izvestaj { get; set; }

        public void IzvrsiIzadjiKomandu(object obj)
        {
            prikazTerminaOperacijaProzor.Close();
        }

        public void IzvrisIzvestajKomandu(object obj)
        {
            KreirajPDFIzvestajOSvimOperacijama();
        }

        public void KreirajPDFIzvestajOSvimOperacijama()
        {
            GenerisanjeIzvestajaZaPacijentaKontroler generisanje = new GenerisanjeIzvestajaZaPacijentaKontroler();
            bool uspesno = generisanje.GenerisiIzvestajBuducihOperacijaPacijenta(jmbgPacijenta);
            if (uspesno)
            {
                string message = "PDF je kreiran";
                MessageBox.Show(message);
            }
            else
            {
                string message = "PDF nije kreiran";
                MessageBox.Show(message);
            }
        }
    }
}
