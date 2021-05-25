using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi
{
    class OperacijaServis
    {
        private BazaOperacija bazaOperacija = new BazaOperacija();

        public List<Operacija> GetSveSledeceOperacije()
        {
            return bazaOperacija.SveSledeceOperacije();
        }

        public List<Operacija> GetSveSledeveOperacijePacijenta(string jmbgPacijenta)
        {
            List<Operacija> operacijePacijenta = new List<Operacija>();

            foreach (Operacija o in bazaOperacija.SveSledeceOperacije())
            {
                if (o.Pacijent.Jmbg.Equals(jmbgPacijenta))
                {
                    operacijePacijenta.Add(o);
                }
            }

            return operacijePacijenta;
        }

        public List<Operacija> GetSveSledeceOperacijeSobe(string idSale)
        {
            List<Operacija> operacijeSale = new List<Operacija>();
            foreach (Operacija operacija in bazaOperacija.SveSledeceOperacije())
            {
                if (operacija.Soba.Id.Equals(idSale))
                {
                    operacijeSale.Add(operacija);
                }
            }

            return operacijeSale;
        }
        public List<Operacija> GetSveSledeceOperacijeLekara(string jmbgLekara)
        {
            List<Operacija> ret = new List<Operacija>();
            foreach (Operacija operacija in bazaOperacija.SveSledeceOperacije())
            {
                if (operacija.Lekar.Jmbg.Equals(jmbgLekara))
                {
                    ret.Add(operacija);
                }
            }
            return ret;
        }

        public List<Operacija> DostuptniTerminiLekaraZaDatuProstoriju(string jmbgLekara, string idSale, int duzinaTrajanja)
        {
            Lekar lekar = new BazaLekara().DobaviLekara(jmbgLekara);
            BazaOperacija bazaOperacija = new BazaOperacija();
            return SlobodneOperacijeLekaraUNarednomPeriodu(lekar, duzinaTrajanja, idSale);
        }

        public List<Operacija> SlobodneOperacijeLekaraUNarednomPeriodu(Lekar lekar, int trajanjePregleda, string idSale)
        {
            List<Operacija> sviSkorasnjiTermini = SviPredloziTerminaOperacije(lekar, trajanjePregleda, idSale);
            List<Operacija> terminiURadnomVremenu = SviTerminiURadnomVremenuLekara(lekar, sviSkorasnjiTermini);
            List<Operacija> slobodniTermini = SlobodneOperacijeLekara(lekar, terminiURadnomVremenu, idSale);

            return slobodniTermini;
        }

        private List<Operacija> SviPredloziTerminaOperacije(Lekar lekar, int trajanjeOperacije, string idSale)
        {
            List<Operacija> sveSkorasnjeOperacije = new List<Operacija>();
            DateTime najbliziTermin = NajbliziTermin();


            for (int i = 0; i < 7200; i += 10)
            {
                DateTime pocetakTermina = najbliziTermin.AddMinutes(i);

                Operacija operacija = new Operacija()
                {
                    Lekar = lekar,
                    VremePocetkaOperacije = pocetakTermina,
                    VremeKrajaOperacije = pocetakTermina.AddMinutes(trajanjeOperacije),
                    Soba = new BazaBolnica().GetSobaById(idSale),
                    Hitna = true
                };
                sveSkorasnjeOperacije.Add(operacija);
            }
            return sveSkorasnjeOperacije;
        }

        private DateTime NajbliziTermin()
        {
            DateTime najbliziTermin = DateTime.Now;
            najbliziTermin = najbliziTermin.AddMinutes(1);
            while (najbliziTermin.Minute % 5 != 0)
            {
                najbliziTermin = najbliziTermin.AddMinutes(1);
            }
            return najbliziTermin;
        }


        public bool izmeniOperaciju(DateTime stariDatum, string stariSat, string stariMinut, Operacija novaOperacija)
        {
            BazaOperacija baza = new BazaOperacija();
            List<Operacija> lista = baza.SveSledeceOperacije();
            foreach (Operacija operacija in lista)
            {
                if (novaOperacija.Pacijent.Jmbg.Equals(operacija.Pacijent.Jmbg) &&
                    operacija.VremePocetkaOperacije.Hour == Int32.Parse(stariSat) &&
                    operacija.VremePocetkaOperacije.Date.Equals(stariDatum))
                {
                    Operacija staraOperacija = operacija;
                    baza.IzmeniOperaciju(novaOperacija, staraOperacija);
                    return true;
                }
            }
            return false;
        }

        private List<Operacija> SviTerminiURadnomVremenuLekara(Lekar lekar, List<Operacija> operacije)
        {
            List<Operacija> operacijeURadnomVremenu = new List<Operacija>();

            foreach (Operacija operacija in operacije)
            {
                // TODO: obrisati ovo formiranje intervala i u operaciju dodati polje za interval
                VremenskiInterval termin = new VremenskiInterval(operacija.VremePocetkaOperacije, operacija.VremeKrajaOperacije);

                if (lekar.TerminURadnomVremenuLekara(termin))
                {
                    operacijeURadnomVremenu.Add(operacija);
                }
            }
            return operacijeURadnomVremenu;
        }

        private List<Operacija> SlobodneOperacijeLekara(Lekar lekar, List<Operacija> operacije, string idSale)
        {
            List<Operacija> slobodniTermini = new List<Operacija>();
            foreach (Operacija operacija in operacije)
            {
                if (!TerminSePreklapaKodLekara(lekar.Jmbg, operacija, idSale))
                {
                    slobodniTermini.Add(operacija);
                }
            }
            return slobodniTermini;
        }

        private bool TerminSePreklapaKodLekara(string jmbgLekara, Operacija predlozenaOperacija, string idSale)
        {
            BazaPregleda bazaPregleda = new BazaPregleda();

            VremenskiInterval drugiTermin = new VremenskiInterval(predlozenaOperacija.VremePocetkaOperacije,
                predlozenaOperacija.VremeKrajaOperacije);

            foreach (Pregled zakazaniPregled in bazaPregleda.SviBuduciPreglediKojeLekarIma(jmbgLekara))
            {
                VremenskiInterval prviTermin = new VremenskiInterval(zakazaniPregled.VremePocetkaPregleda,
                    zakazaniPregled.VremeKrajaPregleda);

                if (PreklapanjeTermina(prviTermin, drugiTermin))
                {
                    return true;
                }
            }

            foreach (Operacija zakazanaOperacija in GetSveSledeceOperacijeLekara(jmbgLekara))
            {
                VremenskiInterval prviTermin = new VremenskiInterval(zakazanaOperacija.VremePocetkaOperacije,
                    zakazanaOperacija.VremeKrajaOperacije);

                if (PreklapanjeTermina(prviTermin,drugiTermin))
                {
                    return true;
                }
               
            }

            foreach (Operacija zakazanaOperacija in SveOperacijeUSali(idSale))
            {
                VremenskiInterval prviTermin = new VremenskiInterval(zakazanaOperacija.VremePocetkaOperacije,
                    zakazanaOperacija.VremeKrajaOperacije);

                if (PreklapanjeTermina(prviTermin, drugiTermin))
                {
                    return true;
                }
            }

            return false;
        }
        
        public List<Operacija> SveOperacijeUSali(string idSale)
        {
            List<Operacija> operacijeUSali = new List<Operacija>();
            foreach (Operacija operacija in bazaOperacija.SveSledeceOperacije())
            {
                if (operacija.Soba.Equals(idSale))
                {
                    operacijeUSali.Add(operacija);
                }
            }

            return operacijeUSali;
        }
        
        public bool PreklapanjeTermina(VremenskiInterval predlozen, VremenskiInterval upitan)
        {
            return predlozen.DaLiSePreklapaSa(upitan);
        }

        public void IzmeniOperaciju(Operacija nova, Operacija stara)
        {
            bazaOperacija.IzmeniOperaciju(nova, stara);
        }

        public void ZakaziOperaciju(Operacija operacija)
        {
            bazaOperacija.ZakaziOperaciju(operacija);
        }

        public void OtkaziOperaciju(Operacija operacija)
        {
            bazaOperacija.OtkaziOperaciju(operacija);
        }
    }
}
