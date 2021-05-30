using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Model;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Kontroleri
{
    class PregledKontroler
    {
        private PregledServis pregledServis = new PregledServis();

        public List<Pregled> GetSlobodniTerminiZaIzmenu(Lekar lekar, DateTime datumIzmene)
        {
            LekarKontroler lekarKontroler = new LekarKontroler();
            lekar = lekarKontroler.GetLekar(lekar.Jmbg);
            return pregledServis.SlobodniTerminiZaIzmenuPregledaPacijenta(lekar, datumIzmene);
        }

        public List<Pregled> GetSviPregledi()
        {
            return pregledServis.GetsviPregledi();
        }
        public List<Pregled> GetSviBuduciPregledi()
        {
            return pregledServis.GetSviBuduciPregledi();
        }

        public List<Pregled> GetSviPreglediLekara(string jmbgLekara)
        {
            return pregledServis.GetSviPreglediLekara(jmbgLekara);
        }

        public List<Pregled> GetSviBuduciPreglediPacijenta(string jmbgPacijenta)
        {
            return pregledServis.GetSviBuduciPreglediPacijenta(jmbgPacijenta);
        }

        public List<Pregled> GetSviBuduciPreglediSobe(string idSobe)
        {
            return pregledServis.GetSviBuduciPreglediSobe(idSobe);
        }

        public List<Pregled> GetSviBuduciPreglediLekara(string jmbg)
        {
            return pregledServis.GetSviBuduciPreglediLekara(jmbg);
        }

        public Pregled GetSledeciPregledKodLekara(string jmbg)
        {
            return pregledServis.GetSledeciPregledKodLekara(jmbg);
        }

        public List<Pregled> GetDostupniTerminiPregledaLekaraUNarednomPeriodu(Lekar lekar)
        {
            return pregledServis.GetDostupniTerminiPregledaLekaraUNarednomPeriodu(lekar);
        }

        public bool IzmeniPregled(DateTime stariDatum, string stariSat, string stariMinut, Pregled noviPregled)
        {
            return pregledServis.IzmeniPregled(stariDatum, stariSat, stariMinut, noviPregled);
        }

        public void OtkaziPregled(Pregled pregled)
        {
            pregledServis.OtkaziPregled(pregled);
        }

        public void OdloziPregledStoPre(Pregled pregledZaOdlaganje)
        {
            pregledServis.OdloziPregledStoPre(pregledZaOdlaganje);
        }

        public bool ZakaziPregled(Pregled pregled)
        {
            return pregledServis.ZakaziPregled(pregled);
        }

        public void IzmeniPregled(Pregled novi, Pregled stari)
        {
            pregledServis.IzmeniPregled(novi, stari);
        }

        public List<Pregled> ZauzetiHitniPreglediLekaraOdredjeneOblasti(OblastLekara oblastLekara)
        {
            return pregledServis.ZauzetiHitniPreglediLekaraOdredjeneOblasti(oblastLekara);
        }

        public List<Pregled> SlobodniHitniPreglediLekaraOdredjeneOblasti(OblastLekara oblastLekara, int minutiTrajanjaTermina)
        {
            return pregledServis.SlobodniHitniPreglediLekaraOdredjeneOblasti(oblastLekara, minutiTrajanjaTermina);
        }

        public List<Pregled> SlobodniPreglediLekaraOpstePrakseUNarednomPeriodu()
        {
            return pregledServis.SlobodniPreglediLekaraOpstePrakseUNarednomPeriodu();
        }

        public bool PacijentImaZakazanPregled(Pregled pregledZaProveru)
        {
            return pregledServis.PacijentImaZakazanPregled(pregledZaProveru);
        }
    }
}
