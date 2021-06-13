using System;
using System.Collections.Generic;
using System.Linq;
using IS_Bolnice.Baze;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.DTOs;
using IS_Bolnice.Model;
using IS_Bolnice.Servisi.Ustanova;

namespace IS_Bolnice.Servisi.Termini
{
    class OperacijaServis
    {
        private IOperacijaRepozitorijum operacijaRepo = new Injector().GetOperacijaRepozitorijum();
        private ILekarRepozitorijum lekarRepo = new Injector().GetLekarRepozitorijum();
        private IBolnicaRepozitorijum bolnicaRepo = new Injector().GetBolnicaRepozitorijum();
        private static int DOVOLJAN_BROJ_ZAKAZANIH_OPERACIJA = 6;
        private static int BROJ_MINUTA_ZA_HITAN_TERMIN = 60;

        public List<Operacija> GetSveOperacije()
        {
            return operacijaRepo.GetSve();
        }

        public List<Operacija> GetSveOperacijeLekara(string jmbgLekara)
        {
            return operacijaRepo.GetSveOperacijeLekara(jmbgLekara);
        }

        public List<Operacija> GetSveBuduceOperacije()
        {
            return operacijaRepo.GetSveBuduceOperacije();
        }

        public List<Operacija> GetSveBuduceOperacijePacijenta(string jmbgPacijenta)
        {
            return operacijaRepo.GetSveBuduceOperacijePacijenta(jmbgPacijenta);
        }

        public List<Operacija> GetSveBuduceOperacijeSale(string idSale)
        {
            return operacijaRepo.GetSveBuduceOperacijeSale(idSale);
        }

        public List<Operacija> GetSveBuduceOperacijeLekara(string jmbgLekara)
        {
            return operacijaRepo.GetSveBuduceOperacijeLekara(jmbgLekara);
        }

        public List<Operacija> GetDostuptniTerminiLekaraZaDatuProstoriju(OperacijaDTO operacija)
        {
            Lekar lekar = lekarRepo.GetPoId(operacija.Lekar.Jmbg);
            return GetSlobodneOperacijeLekaraUNarednomPeriodu(operacija);
        }

        public bool PostojiOperacijaTrenutnoUSali(string idSobe)
        {
            foreach (Operacija operacija in GetSveBuduceOperacijeSale(idSobe))
            {
                if (DateTime.Now > operacija.VremePocetkaOperacije && DateTime.Now < operacija.VremeKrajaOperacije)
                {
                    return true;
                }
            }
            return false;
        }

        public List<Operacija> GetSlobodneOperacijeLekaraUNarednomPeriodu(OperacijaDTO operacija)
        {
            List<Operacija> sviSkorasnjiTermini = GetSviPredloziTerminaOperacije(operacija);
            List<Operacija> terminiURadnomVremenu = GetSviTerminiURadnomVremenuLekara(operacija.Lekar, sviSkorasnjiTermini);
            List<Operacija> slobodniTermini = GetSlobodneOperacijeLekara(terminiURadnomVremenu);
            List<Operacija> terminiZaPrikaz = FiltirajTerminiNaOsnovuRenoviranjeSobe(slobodniTermini, operacija);

            return terminiZaPrikaz;
        }
        private List<Operacija> FiltirajTerminiNaOsnovuRenoviranjeSobe(List<Operacija> slobodniTermini, OperacijaDTO operacija)
        {
            List<Operacija> retVal = new List<Operacija>();
            foreach (Operacija op in slobodniTermini)
            {
                if (!new RenovacijaServis().SalaNaRenoviranjuUOdredjenomPeriodu(operacija.Soba.Id,
                    op.VremePocetkaOperacije))
                {
                    retVal.Add(op);
                }
            }

            return retVal;
        }

        private List<Operacija> GetSviPredloziTerminaOperacije(OperacijaDTO operacijaDto)
        {
            List<Operacija> sveSkorasnjeOperacije = new List<Operacija>();
            DateTime najbliziTermin = NajbliziTermin();


            for (int i = 0; i < 4320; i += 10)
            {
                DateTime pocetakTermina = najbliziTermin.AddMinutes(i);

                Operacija operacija = new Operacija()
                {
                    Lekar = operacijaDto.Lekar,
                    VremePocetkaOperacije = pocetakTermina,
                    VremeKrajaOperacije = pocetakTermina.AddMinutes(operacijaDto.TrajanjeOperacijeUMinutima),
                    Soba = bolnicaRepo.GetSobaById(operacijaDto.Soba.Id),
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


        public bool IzmeniOperaciju(Operacija novaOperacija)
        {
            operacijaRepo.Izmeni(novaOperacija);
            return true;
        }

        private List<Operacija> GetSviTerminiURadnomVremenuLekara(Lekar lekar, List<Operacija> operacije)
        {
            List<Operacija> operacijeURadnomVremenu = new List<Operacija>();

            foreach (Operacija operacija in operacije)
            {
                VremenskiInterval termin = new VremenskiInterval(operacija.VremePocetkaOperacije, operacija.VremeKrajaOperacije);

                if (lekar.RadnoVreme.TerminURadnomVremenuLekara(termin))
                {
                    operacijeURadnomVremenu.Add(operacija);
                }
            }
            return operacijeURadnomVremenu;
        }

        private List<Operacija> GetSlobodneOperacijeLekara(List<Operacija> operacije)
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

            foreach (Pregled zakazaniPregled in preglediFajlRepozitorijum.GetSviBuduciPreglediLekara(predlozenaOperacija.Lekar.Jmbg))
            {
                VremenskiInterval prviTermin = new VremenskiInterval(zakazaniPregled.VremePocetkaPregleda,
                    zakazaniPregled.VremeKrajaPregleda);

                if (PreklapanjeTermina(prviTermin, drugiTermin))
                {
                    return true;
                }
            }

            foreach (Operacija zakazanaOperacija in GetSveBuduceOperacijeLekara(predlozenaOperacija.Lekar.Jmbg))
            {
                VremenskiInterval prviTermin = new VremenskiInterval(zakazanaOperacija.VremePocetkaOperacije,
                    zakazanaOperacija.VremeKrajaOperacije);

                if (PreklapanjeTermina(prviTermin,drugiTermin))
                {
                    return true;
                }
               
            }

            foreach (Operacija zakazanaOperacija in GetSveBuduceOperacijeSale(predlozenaOperacija.Soba.Id))
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

        public void OdloziOperacijuStoPre(Operacija pomeranaOperacija)
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
            if (!operacija.Lekar.RadnoVreme.TerminURadnomVremenuLekara(vremenskiInterval))
            {
                return false;
            }

            if (TerminSePreklapaKodLekaraISale(operacija))
            {
                return false;
            }

            return true;
        }

        public List<Operacija> GetZauzeteOperacijeLekaraOdredjeneOblastiZaOdlaganje(OblastLekara prosledjenaOblast)
        {
            List<Lekar> sviLekariOdredjeneOblasti = lekarRepo.LekariOdredjeneOblasti(prosledjenaOblast.Naziv);
            List<Operacija> skorasnjeOperacijeLekara = new List<Operacija>();

            foreach (Lekar lekar in sviLekariOdredjeneOblasti)
            {
                List<Operacija> naredneOperacijeLekara = GetSveBuduceOperacijeLekara(lekar.Jmbg);
                List<Operacija> operacijeKojeNisuHitne = GetSveOperacijeKojeNisuHitne(naredneOperacijeLekara);
                skorasnjeOperacijeLekara.AddRange(GetOperacijeNarednihSatVremena(operacijeKojeNisuHitne));
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

        private List<Operacija> GetSveOperacijeKojeNisuHitne(List<Operacija> operacije)
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


        private List<Operacija> GetOperacijeNarednihSatVremena(List<Operacija> operacije)
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

        public List<Operacija> GetSlobodneHitneOperacijeLekaraOdredjeneOblasti(OblastLekara prosledjenaOblast, int minutiTrajanjaOperacije)
        {
            foreach (Lekar lekar in lekarRepo.LekariOdredjeneOblasti(prosledjenaOblast.Naziv))
            {
                List<Operacija> slobodniTerminiLekara = GetSlobodneHitneOperacijeLekaraSaTrajanjem(lekar, minutiTrajanjaOperacije);
                if (slobodniTerminiLekara.Count > 0)
                {
                    return slobodniTerminiLekara;
                }
            }
            return null;
        }

        private List<Operacija> GetSlobodneHitneOperacijeLekaraSaTrajanjem(Lekar lekar, int minutiTrajanjaOperacije)
        {
            List<Operacija> sviSkorasnjiTermini = SviPredloziHitnihOperacija(lekar, minutiTrajanjaOperacije);
            List<Operacija> terminiURadnomVremenu = GetSviTerminiURadnomVremenuLekara(lekar, sviSkorasnjiTermini);
            List<Operacija> slobodniTermini = GetSlobodneOperacijeLekara(terminiURadnomVremenu);

            return slobodniTermini;
        }

        private List<Operacija> SviPredloziHitnihOperacija(Lekar lekar, int minutiTrajanjaOperacije)
        {
            List<Operacija> sveSkorasnjeOperacije = new List<Operacija>();
            DateTime najbliziTermin = NajbliziTermin();

            for (int i = 0; i < BROJ_MINUTA_ZA_HITAN_TERMIN; i += 10)
            {
                DateTime pocetakTermina = najbliziTermin.AddMinutes(i);

                foreach (Soba sala in bolnicaRepo.GetSveOperacioneSale())
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

        public int[] BrojOperacijaKodLekaraZaPetMeseci(string idLekara)
        {
            int[] brojOperacija = { 0, 0, 0, 0, 0 };
            List<Operacija> sveOperacijeKodOvogLekara = GetSveOperacijeLekara(idLekara);
            int trenutniMesec = DateTime.Now.Month;
            int mesecPregleda;
            foreach (Operacija operacijaIter in sveOperacijeKodOvogLekara)
            {
                mesecPregleda = operacijaIter.VremePocetkaOperacije.Month;
                if (mesecPregleda == trenutniMesec)
                {
                    brojOperacija[4]++;
                }
                else if (mesecPregleda == (trenutniMesec - 1) || mesecPregleda == (trenutniMesec + 11))
                {
                    brojOperacija[3]++;
                }
                else if (mesecPregleda == (trenutniMesec - 2) || mesecPregleda == (trenutniMesec + 10))
                {
                    brojOperacija[2]++;
                }
                else if (mesecPregleda == (trenutniMesec - 3) || mesecPregleda == (trenutniMesec + 9))
                {
                    brojOperacija[1]++;
                }
                else if (mesecPregleda == (trenutniMesec - 4) || mesecPregleda == (trenutniMesec + 8))
                {
                    brojOperacija[0]++;
                }
            }
            return brojOperacija;
        }

        public int[] BrojOperacijaZaPetMeseci()
        {
            int[] brojOperacija = { 0, 0, 0, 0, 0 };
            List<Operacija> sveOperacijeLekara = GetSveOperacije();
            int trenutniMesec = DateTime.Now.Month;
            int mesecPregleda;
            foreach (Operacija operacijaIter in sveOperacijeLekara)
            {
                mesecPregleda = operacijaIter.VremePocetkaOperacije.Month;
                if (mesecPregleda == trenutniMesec)
                {
                    brojOperacija[4]++;
                }
                else if (mesecPregleda == (trenutniMesec - 1) || mesecPregleda == (trenutniMesec + 11))
                {
                    brojOperacija[3]++;
                }
                else if (mesecPregleda == (trenutniMesec - 2) || mesecPregleda == (trenutniMesec + 10))
                {
                    brojOperacija[2]++;
                }
                else if (mesecPregleda == (trenutniMesec - 3) || mesecPregleda == (trenutniMesec + 9))
                {
                    brojOperacija[1]++;
                }
                else if (mesecPregleda == (trenutniMesec - 4) || mesecPregleda == (trenutniMesec + 8))
                {
                    brojOperacija[0]++;
                }
            }
            return brojOperacija;
        }

        public int[] BrojPrijemaZaPetDana(string idLekara)
        {
            int[] brojOperacija = { 0, 0, 0, 0, 0 };
            List<Operacija> sveOperacijeKodOvogLekara = GetSveBuduceOperacijeLekara(idLekara);
            List<DateTime> narednihPetDana = ListaNaprednihPetDatuma();
            DateTime datumOperacije = new DateTime();
            foreach (Operacija operacijaIter in sveOperacijeKodOvogLekara)
            {
                datumOperacije = operacijaIter.VremePocetkaOperacije;
                for (int i = 0; i < 5; i++)
                {
                    if (datumOperacije.Day == narednihPetDana.ElementAt(i).Day && datumOperacije.Month == narednihPetDana.ElementAt(i).Month && datumOperacije.Year == narednihPetDana.ElementAt(i).Year)
                    {
                        brojOperacija[i]++;
                        break;
                    }
                }
            }
            return brojOperacija;
        }

        private List<DateTime> ListaNaprednihPetDatuma()
        {
            List<DateTime> narednihPetDana = new List<DateTime>();
            narednihPetDana.Add(DateTime.Now);
            narednihPetDana.Add(DateTime.Now.AddDays(1));
            narednihPetDana.Add(DateTime.Now.AddDays(2));
            narednihPetDana.Add(DateTime.Now.AddDays(3));
            narednihPetDana.Add(DateTime.Now.AddDays(4));
            return narednihPetDana;
        }
    }
}
