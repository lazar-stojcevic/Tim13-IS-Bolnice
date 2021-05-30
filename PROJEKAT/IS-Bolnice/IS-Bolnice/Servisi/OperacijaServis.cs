using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;

namespace IS_Bolnice.Servisi
{
    class OperacijaServis
    {
        private IOperacijaRepozitorijum operacijaRepo = new OperacijaFajlRepozitorijum();
        private ILekarRepozitorijum lekarRepo = new LekarFajlRepozitorijum();
        private IBolnicaRepozitorijum bolnicaRepo = new BolnicaFajlRepozitorijum();
        private static int DOVOLJAN_BROJ_ZAKAZANIH_OPERACIJA = 6;
        private static int BROJ_MINUTA_ZA_HITAN_TERMIN = 60;

        public List<Operacija> GetSveOperacije()
        {
            return operacijaRepo.DobaviSve();
        }

        public List<Operacija> GetSveSledeceOperacije()
        {
            return operacijaRepo.SveSledeceOperacije();
        }

        public List<Operacija> GetSveSledeveOperacijePacijenta(string jmbgPacijenta)
        {
            List<Operacija> operacijePacijenta = new List<Operacija>();

            foreach (Operacija o in operacijaRepo.SveSledeceOperacije())
            {
                if (o.Pacijent.Jmbg.Equals(jmbgPacijenta))
                {
                    operacijePacijenta.Add(o);
                }
            }

            return operacijePacijenta;
        }

        public List<Operacija> GetSveSledeceOperacijeSale(string idSale)
        {
            List<Operacija> operacijeSale = new List<Operacija>();
            foreach (Operacija operacija in operacijaRepo.SveSledeceOperacije())
            {
                if (operacija.Soba.Id.Equals(idSale))
                {
                    operacijeSale.Add(operacija);
                }
            }

            return operacijeSale;
        }

        public List<Operacija> GetSveOperacijeLekara(string jmbgLekara)
        {
            List<Operacija> ret = new List<Operacija>();
            foreach (Operacija operacija in operacijaRepo.DobaviSve())
            {
                if (operacija.Lekar.Jmbg.Equals(jmbgLekara))
                {
                    ret.Add(operacija);
                }
            }
            return ret;
        }

        public List<Operacija> GetSveSledeceOperacijeLekara(string jmbgLekara)
        {
            List<Operacija> ret = new List<Operacija>();
            foreach (Operacija operacija in operacijaRepo.SveSledeceOperacije())
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
            Lekar lekar = lekarRepo.DobaviPoId(jmbgLekara);
            return SlobodneOperacijeLekaraUNarednomPeriodu(lekar, duzinaTrajanja, idSale);
        }

        public List<Operacija> SlobodneOperacijeLekaraUNarednomPeriodu(Lekar lekar, int trajanjePregleda, string idSale)
        {
            List<Operacija> sviSkorasnjiTermini = SviPredloziTerminaOperacije(lekar, trajanjePregleda, idSale);
            List<Operacija> terminiURadnomVremenu = SviTerminiURadnomVremenuLekara(lekar, sviSkorasnjiTermini);
            List<Operacija> slobodniTermini = SlobodneOperacijeLekara(terminiURadnomVremenu);

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
                    Soba = bolnicaRepo.GetSobaById(idSale),
                    Hitna = true
                };
                Console.Write(operacija.Lekar);
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
            OperacijaFajlRepozitorijum baza = new OperacijaFajlRepozitorijum();
            List<Operacija> lista = baza.SveSledeceOperacije();
            foreach (Operacija operacija in lista)
            {
                if (novaOperacija.Pacijent.Jmbg.Equals(operacija.Pacijent.Jmbg) &&
                    operacija.VremePocetkaOperacije.Hour == Int32.Parse(stariSat) &&
                    operacija.VremePocetkaOperacije.Date.Equals(stariDatum))
                {
                    Operacija staraOperacija = operacija;
                    baza.Izmeni(novaOperacija);
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

        private List<Operacija> SlobodneOperacijeLekara(List<Operacija> operacije)
        {
            List<Operacija> slobodniTermini = new List<Operacija>();
            foreach (Operacija operacija in operacije)
            {
                if (!TerminSePreklapaKodLekaraISale(operacija))
                {
                    slobodniTermini.Add(operacija);
                }
            }
            return slobodniTermini;
        }

        private bool TerminSePreklapaKodLekaraISale(Operacija predlozenaOperacija)
        {
            PreglediFajlRepozitorijum preglediFajlRepozitorijum = new PreglediFajlRepozitorijum();

            VremenskiInterval drugiTermin = new VremenskiInterval(predlozenaOperacija.VremePocetkaOperacije,
                predlozenaOperacija.VremeKrajaOperacije);

            foreach (Pregled zakazaniPregled in preglediFajlRepozitorijum.SviBuduciPreglediKojeLekarIma(predlozenaOperacija.Lekar.Jmbg))
            {
                VremenskiInterval prviTermin = new VremenskiInterval(zakazaniPregled.VremePocetkaPregleda,
                    zakazaniPregled.VremeKrajaPregleda);

                if (PreklapanjeTermina(prviTermin, drugiTermin))
                {
                    return true;
                }
            }

            foreach (Operacija zakazanaOperacija in GetSveSledeceOperacijeLekara(predlozenaOperacija.Lekar.Jmbg))
            {
                VremenskiInterval prviTermin = new VremenskiInterval(zakazanaOperacija.VremePocetkaOperacije,
                    zakazanaOperacija.VremeKrajaOperacije);

                if (PreklapanjeTermina(prviTermin,drugiTermin))
                {
                    return true;
                }
               
            }

            foreach (Operacija zakazanaOperacija in GetSveSledeceOperacijeSale(predlozenaOperacija.Soba.Id))
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
        
        
        public bool PreklapanjeTermina(VremenskiInterval predlozen, VremenskiInterval upitan)
        {
            return predlozen.DaLiSePreklapaSa(upitan);
        }

        public void IzmeniOperaciju(Operacija nova, Operacija stara)
        {
            operacijaRepo.Izmeni(nova);
        }

        public bool ZakaziOperaciju(Operacija operacija)
        {
            if (MozeDaSeZakaze(operacija))
            {
                operacijaRepo.Sacuvaj(operacija);
                return true;
            }

            return false;
        }

        public void OtkaziOperaciju(Operacija operacija)
        {
            operacijaRepo.Obrisi(operacija.Id);
        }

        public void OdloziOperaciju(Operacija pomeranaOperacija)
        {
            Operacija operacijaZaOtkazivanje = new Operacija(pomeranaOperacija);

            double vremeOdlaganja = 10;
            do
            {
                pomeranaOperacija.VremePocetkaOperacije = pomeranaOperacija.VremePocetkaOperacije.AddMinutes(vremeOdlaganja);
                pomeranaOperacija.VremeKrajaOperacije = pomeranaOperacija.VremeKrajaOperacije.AddMinutes(vremeOdlaganja);
                vremeOdlaganja += 10;
            } while (!MozeDaSeZakaze(pomeranaOperacija));

            OtkaziOperaciju(operacijaZaOtkazivanje);
            ZakaziOperaciju(pomeranaOperacija);
        }

        private bool MozeDaSeZakaze(Operacija operacija)
        {
            VremenskiInterval vremenskiInterval =
                new VremenskiInterval(operacija.VremePocetkaOperacije, operacija.VremeKrajaOperacije);
            if (!operacija.Lekar.TerminURadnomVremenuLekara(vremenskiInterval))
            {
                return false;
            }

            if (TerminSePreklapaKodLekaraISale(operacija))
            {
                return false;
            }

            return true;
        }

        public List<Operacija> ZauzeteOperacijeLekaraOdredjeneOblastiZaOdlaganje(OblastLekara prosledjenaOblast)
        {
            List<Lekar> sviLekariOdredjeneOblasti = lekarRepo.LekariOdredjeneOblasti(prosledjenaOblast.Naziv);
            List<Operacija> skorasnjeOperacijeLekara = new List<Operacija>();

            foreach (Lekar lekar in sviLekariOdredjeneOblasti)
            {
                List<Operacija> naredneOperacijeLekara = GetSveSledeceOperacijeLekara(lekar.Jmbg);
                List<Operacija> operacijeKojeNisuHitne = SveOperacijeKojeNisuHitne(naredneOperacijeLekara);
                skorasnjeOperacijeLekara.AddRange(OperacijeNarednihSatVremena(operacijeKojeNisuHitne));
                if (skorasnjeOperacijeLekara.Count > DOVOLJAN_BROJ_ZAKAZANIH_OPERACIJA)
                {
                    return skorasnjeOperacijeLekara;
                }
            }
            return SortiranjeTerminaPoMogucstvuOdlaganja(skorasnjeOperacijeLekara);
        }

        private List<Operacija> SortiranjeTerminaPoMogucstvuOdlaganja(List<Operacija> operacije)
        {
            List<int> vremenaOdlaganja = new List<int>();

            foreach (Operacija operacija in operacije)
            {
                Operacija odlozenaOperacija = new Operacija(operacija);

                int vremeOdlaganja = 10;
                while (!MozeDaSeZakaze(odlozenaOperacija))
                {
                    odlozenaOperacija.VremePocetkaOperacije = odlozenaOperacija.VremePocetkaOperacije.AddMinutes(vremeOdlaganja);
                    odlozenaOperacija.VremeKrajaOperacije = odlozenaOperacija.VremeKrajaOperacije.AddMinutes(vremeOdlaganja);
                    vremeOdlaganja += 10;
                }
                vremenaOdlaganja.Add(vremeOdlaganja);
            }
            return SortirajOperacijePoVremenuOdlaganja(operacije, vremenaOdlaganja);
        }

        private List<Operacija> SortirajOperacijePoVremenuOdlaganja(List<Operacija> operacije, List<int> odlaganja)
        {
            for (int i = 0; i < odlaganja.Count - 1; i++)
            {
                for (int j = 0; j < odlaganja.Count - i - 1; j++)
                {
                    if (odlaganja[j] > odlaganja[j + 1])
                    {
                        int temp = odlaganja[j];
                        odlaganja[j] = odlaganja[j + 1];
                        odlaganja[j + 1] = temp;

                        Operacija tempOperacija = operacije[j];
                        operacije[j] = operacije[j + 1];
                        operacije[j + 1] = tempOperacija;
                    }
                }
            }
            return operacije;
        }

        private List<Operacija> SveOperacijeKojeNisuHitne(List<Operacija> operacije)
        {
            List<Operacija> operacijeKojeNisuHitne = new List<Operacija>();

            foreach (Operacija operacija in operacije)
            {
                if (!operacija.Hitna)
                {
                    operacijeKojeNisuHitne.Add(operacija);
                }
            }

            return operacijeKojeNisuHitne;
        }


        private List<Operacija> OperacijeNarednihSatVremena(List<Operacija> operacije)
        {
            List<Operacija> operacijeiNarednihSatVremena = new List<Operacija>();
            DateTime trenutnoVreme = DateTime.Now;
            DateTime vremeZaSatVremena = trenutnoVreme.AddHours(1);
            foreach (Operacija operacija in operacije)
            {
                if (operacija.VremePocetkaOperacije <= vremeZaSatVremena)
                {
                    operacijeiNarednihSatVremena.Add(operacija);
                }
            }
            return operacijeiNarednihSatVremena;
        }

        public List<Operacija> SlobodneHitneOperacijeLekaraOdredjeneOblasti(OblastLekara prosledjenaOblast, int minutiTrajanjaOperacije)
        {
            foreach (Lekar lekar in lekarRepo.LekariOdredjeneOblasti(prosledjenaOblast.Naziv))
            {
                List<Operacija> slobodniTerminiLekara = SlobodneHitneOperacijeLekaraSaTrajanjem(lekar, minutiTrajanjaOperacije);
                if (slobodniTerminiLekara.Count > 0)
                {
                    return slobodniTerminiLekara;
                }
            }
            return null;
        }

        private List<Operacija> SlobodneHitneOperacijeLekaraSaTrajanjem(Lekar lekar, int minutiTrajanjaOperacije)
        {
            List<Operacija> sviSkorasnjiTermini = SviPredloziHitnihOperacija(lekar, minutiTrajanjaOperacije);
            List<Operacija> terminiURadnomVremenu = SviTerminiURadnomVremenuLekara(lekar, sviSkorasnjiTermini);
            List<Operacija> slobodniTermini = SlobodneOperacijeLekara(terminiURadnomVremenu);

            return slobodniTermini;
        }

        private List<Operacija> SviPredloziHitnihOperacija(Lekar lekar, int minutiTrajanjaOperacije)
        {
            List<Operacija> sveSkorasnjeOperacije = new List<Operacija>();
            DateTime najbliziTermin = NajbliziTermin();

            for (int i = 0; i < BROJ_MINUTA_ZA_HITAN_TERMIN; i += 10)
            {
                DateTime pocetakTermina = najbliziTermin.AddMinutes(i);

                foreach (Soba sala in bolnicaRepo.SveOperacioneSaleOveBolnice())
                {
                    Operacija operacija = new Operacija()
                    {
                        Lekar = lekar,
                        VremePocetkaOperacije = pocetakTermina,
                        VremeKrajaOperacije = pocetakTermina.AddMinutes(minutiTrajanjaOperacije),
                        Soba = sala,
                        Hitna = true
                    };
                    sveSkorasnjeOperacije.Add(operacija);
                }
            }
            return sveSkorasnjeOperacije;
        }
    }
}
