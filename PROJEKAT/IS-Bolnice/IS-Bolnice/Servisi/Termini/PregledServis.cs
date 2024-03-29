﻿using System;
using System.Collections.Generic;
using System.Linq;
using IS_Bolnice.Repozitorijumi;
using IS_Bolnice.Repozitorijumi.Interfejsi;
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Kontroleri.Termini;
using IS_Bolnice.Model;
using IS_Bolnice.Servisi.Ustanova;

namespace IS_Bolnice.Servisi.Termini
{
    class PregledServis
    {
        private readonly int MINUTI_TRAJANJA_PREGLEDA = 45;
        private readonly int DOVOLJAN_BROJ_ZAKAZANIH_PREGLEDA = 6;
        private readonly int MINUTI_INTERVALA_ZA_PREDLAGANJE_PREGLEDA_LEKARU = 5760;
        private readonly int MINUTI_PREDLAGANJA_ZA_3_DANA = 4320;
        private readonly int MINUTI_INTERVALA_ZA_PREDLAGANJE_HITNIH_PREGLEDA = 60;
        private readonly int MINUTI_INTERVALA_ZA_IZMENU_TERMINA_PREGLEDA = 30;


        private IPregledRepozitorijum pregledRepo = new Injector().GetPregledRepozitorijum();
        private ILekarRepozitorijum lekarRepo = new Injector().GetLekarRepozitorijum();
        private IIzmenaTerminaRepozitorijum izmenaTerminaRepo = new Injector().GetIzmenaTerminaRepozitorijum();
        private IRenovacijaRepozitorijum renoviranjeRepozitorijum = new Injector().GetRenovacijaRepozitorijum();

        public Pregled PostaviPrvogSlobodnogLekaraOpstePrakseNaPregled(Pregled pregled)
        {
            foreach(Lekar lekar in lekarRepo.GetSviLekariOpstePrakse())
            {
                pregled.Lekar = lekar;
                if (MozeDaSeZakaze(pregled))
                {
                    break;
                }
            }

            return pregled;
        }

        public List<Pregled> GetsviPregledi()
        {
            return pregledRepo.GetSve();
        }

        public List<Pregled> GetSviPreglediLekara(string jmbgLekara)
        {
            return pregledRepo.GetSviPreglediLekara(jmbgLekara);
        }

        public List<Pregled> GetSviBuduciPregledi()
        {
            return pregledRepo.GetSviBuduciPregledi();
        }

        public List<Pregled> GetSviBuduciPreglediSobe(string idSobe)
        {
            return pregledRepo.GetSviBuduciPreglediSobe(idSobe);
        }

        public List<Pregled> GetSviBuduciPreglediPacijenta(string jmbgPacijenta)
        {
            return pregledRepo.GetSviBuduciPreglediPacijenta(jmbgPacijenta);
        }

        public List<Pregled> GetSviBuduciSortiraniPreglediPacijenta(string jmbgPacijenta)
        {
            List<Pregled> preglediPacijenta = pregledRepo.GetSviBuduciPreglediPacijenta(jmbgPacijenta);
            preglediPacijenta.Sort((x, y) => x.VremePocetkaPregleda.CompareTo(y.VremePocetkaPregleda));

            return preglediPacijenta;
        }

        public List<Pregled> GetSviBuduciPreglediLekara(string jmbgLekara)
        {
            return pregledRepo.GetSviBuduciPreglediLekara(jmbgLekara);
        }

        public Pregled GetSledeciPregledKodLekara(string jmbg)
        {
            Pregled sledeciPregled = new Pregled();
            sledeciPregled.VremePocetkaPregleda = DateTime.MaxValue;
            foreach (Pregled pregled in GetSviBuduciPreglediLekara(jmbg))
            {
                if (pregled.VremePocetkaPregleda < sledeciPregled.VremePocetkaPregleda)
                {
                    sledeciPregled = pregled;
                }
            }

            return sledeciPregled;
        }

        public List<Pregled> GetSlobodniTerminiZaIzmenuPregledaPacijenta(Lekar lekar, DateTime datum)
        {
            List<Pregled> validni = new List<Pregled>();
            
            foreach (Pregled predlozeni in GetSlobodniPreglediTokomRadnogVremenaLekara(lekar, datum))
            {
                if (MozeDaSeZakaze(predlozeni))
                {
                    validni.Add(predlozeni);
                }
            }

            return validni;
        }

        public bool PostojiPregledTrenutnoUOrdinaciji(string idSobe)
        {
            foreach (Pregled pregled in GetSviBuduciPreglediSobe(idSobe)) 
            {
                if (DateTime.Now > pregled.VremePocetkaPregleda && DateTime.Now < pregled.VremeKrajaPregleda)
                {
                    return true;
                }
            }
            return false;
        }

        private DateTime FormirajPocetakIntervalaZaIzmenu(DateTime datum)
        {
            return new System.DateTime(datum.Year, datum.Month, datum.Day, 0, 0, 0, 0);
        }

        private DateTime FormirajKrajIntervalaZaIzmenu(DateTime datum)
        {
            return new System.DateTime(datum.Year, datum.Month, datum.Day + 1, 0, 0, 0, 0);
        }

        private List<Pregled> GetSlobodniPreglediTokomRadnogVremenaLekara(Lekar lekar, DateTime datum)
        {
            List<Pregled> slobodni = new List<Pregled>();

            DateTime pocetakIntervala = FormirajPocetakIntervalaZaIzmenu(datum);
            DateTime krajIntervala = FormirajKrajIntervalaZaIzmenu(datum).AddMinutes(-30);

            while (pocetakIntervala <= krajIntervala)
            {
                Pregled p = new Pregled(null, lekar, pocetakIntervala, pocetakIntervala.AddMinutes(30));
                pocetakIntervala = pocetakIntervala.AddMinutes(MINUTI_INTERVALA_ZA_IZMENU_TERMINA_PREGLEDA);
                slobodni.Add(p);
            }

            return slobodni;
        }

        public bool ZakaziPregled(Pregled pregled)
        {
            if (MozeDaSeZakaze(pregled))
            {
                pregledRepo.Sacuvaj(pregled);
                UpisiIzmenuURepozitorijum(pregled.Pacijent.Jmbg);
                return true;
            }

            return false;
        }
        public bool IzmeniPregled(Pregled noviPregled)
        {
            pregledRepo.Izmeni(noviPregled);
            UpisiIzmenuURepozitorijum(noviPregled.Pacijent.Jmbg);
            return true;
        }

        private void UpisiIzmenuURepozitorijum(string jmbgPacijenta)
        {
            IzmenaTermina izmena = new IzmenaTermina();
            DateTime sada = DateTime.Now;

            izmena.DatumIzmene = sada;
            izmena.JmbgPacijenta = jmbgPacijenta;

            izmenaTerminaRepo.Sacuvaj(izmena);
        }

        public void OtkaziPregled(Pregled pregled)
        {
            pregledRepo.Obrisi(pregled.Id);
        }
        public void OdloziPregledStoPre(Pregled pomeraniPregled)
        {
            Pregled pregledZaOtkazivanje = new Pregled(pomeraniPregled);

            double vremeOdlaganja = 10;
            do
            {
                pomeraniPregled.VremePocetkaPregleda = pomeraniPregled.VremePocetkaPregleda.AddMinutes(vremeOdlaganja);
                pomeraniPregled.VremeKrajaPregleda = pomeraniPregled.VremeKrajaPregleda.AddMinutes(vremeOdlaganja);
                vremeOdlaganja += 10;
            } while (!MozeDaSeZakaze(pomeraniPregled));

            OtkaziPregled(pregledZaOtkazivanje);
            ZakaziPregled(pomeraniPregled);
        }

        private bool MozeDaSeZakaze(Pregled noviPregled)
        {
            VremenskiInterval vremenskiIntervalNovogPregleda =
                new VremenskiInterval(noviPregled.VremePocetkaPregleda, noviPregled.VremeKrajaPregleda);

            if (!noviPregled.Lekar.RadnoVreme.TerminURadnomVremenuLekara(vremenskiIntervalNovogPregleda))
            {
                return false;
            }
            if (TerminSePreklapaKodLekara(noviPregled.Lekar.Jmbg, noviPregled))
            {
                return false;
            }

            return true;
        }

        public List<Pregled> GetDostupniTerminiPregledaLekaraUNarednomPeriodu(Lekar lekar)
        {
            List<Pregled> sviSkorasnjiTermini = SviPredloziSkorasnjihPregleda(lekar, MINUTI_TRAJANJA_PREGLEDA, MINUTI_INTERVALA_ZA_PREDLAGANJE_PREGLEDA_LEKARU);
            List<Pregled> terminiURadnomVremenu = GetSviTerminiURadnomVremenuLekara(lekar, sviSkorasnjiTermini);
            List<Pregled> slobodniTermini = GetSlobodniPreglediLekara(lekar, terminiURadnomVremenu);

            return slobodniTermini;
        }

        private List<Pregled> SviPredloziSkorasnjihPregleda(Lekar lekar, int minutiTrajanjaPregleda, int minutiIntervalaPredlaganja)
        {
            List<Pregled> sviSkorasnjiPregledi = new List<Pregled>();
            DateTime najbliziTermin = NajbliziTermin();


            for (int i = 0; i < minutiIntervalaPredlaganja; i += 10)
            {
                DateTime pocetakTermina = najbliziTermin.AddMinutes(i);

                Pregled pregled = new Pregled()
                {
                    Lekar = lekar,
                    VremePocetkaPregleda = pocetakTermina,
                    VremeKrajaPregleda = pocetakTermina.AddMinutes(minutiTrajanjaPregleda)
                };
                sviSkorasnjiPregledi.Add(pregled);
            }
            return sviSkorasnjiPregledi;
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

        private List<Pregled> GetSviTerminiURadnomVremenuLekara(Lekar lekar, List<Pregled> pregledi)
        {
            List<Pregled> preglediURadnomVremenu = new List<Pregled>();

            foreach (Pregled pregled in pregledi)
            {
                VremenskiInterval termin = new VremenskiInterval(pregled.VremePocetkaPregleda, pregled.VremeKrajaPregleda);

                if (lekar.RadnoVreme.TerminURadnomVremenuLekara(termin))
                {
                    preglediURadnomVremenu.Add(pregled);
                }
            }
            return preglediURadnomVremenu;
        }

        private List<Pregled> GetSlobodniPreglediLekara(Lekar lekar, List<Pregled> pregledi)
        {
            List<Pregled> slobodniTermini = new List<Pregled>();
            foreach (Pregled pregled in pregledi)
            {
                if (!TerminSePreklapaKodLekara(lekar.Jmbg, pregled) && !(new RenovacijaServis().SalaNaRenoviranjuUOdredjenomPeriodu(lekar.Ordinacija.Id, pregled.VremePocetkaPregleda)))
                {
                    slobodniTermini.Add(pregled);
                }
            }
            return slobodniTermini;
        }

        public bool TerminSePreklapaKodLekara(string jmbgLekara, Pregled predlozeniPregled)
        {
            VremenskiInterval drugiTermin = new VremenskiInterval(predlozeniPregled.VremePocetkaPregleda,
                predlozeniPregled.VremeKrajaPregleda);

            foreach (Pregled zakazaniPregled in GetSviBuduciPreglediLekara(jmbgLekara))
            {
                VremenskiInterval prviTermin = new VremenskiInterval(zakazaniPregled.VremePocetkaPregleda,
                    zakazaniPregled.VremeKrajaPregleda);

                if (prviTermin.DaLiSePreklapaSa(drugiTermin))
                {
                    return true;
                }

            }

            foreach (Operacija zakazanaOperacija in new OperacijaKontroler().GetSveBuduceOperacijeLekara(jmbgLekara))
            {
                VremenskiInterval prviTermin = new VremenskiInterval(zakazanaOperacija.VremePocetkaOperacije,
                    zakazanaOperacija.VremeKrajaOperacije);

                if (prviTermin.DaLiSePreklapaSa(drugiTermin))
                {
                    return true;
                }
            }

            return false;
        }

        public List<Pregled> GetZauzetiHitniPreglediLekaraOdredjeneOblasti(OblastLekara prosledjenaOblast)
        {
            List<Lekar> sviLekariOdredjeneOblasti = lekarRepo.LekariOdredjeneOblasti(prosledjenaOblast.Naziv);
            List<Pregled> skorasnjiZauzetiPreglediLekara = new List<Pregled>();

            foreach (Lekar lekar in sviLekariOdredjeneOblasti)
            {
                List<Pregled> naredniPreglediLekara = GetSviBuduciPreglediLekara(lekar.Jmbg);
                skorasnjiZauzetiPreglediLekara.AddRange(GetPreglediNarednihSatVremena(naredniPreglediLekara));
                if (skorasnjiZauzetiPreglediLekara.Count() > DOVOLJAN_BROJ_ZAKAZANIH_PREGLEDA)
                {
                    return skorasnjiZauzetiPreglediLekara;
                }
            }
            return SortiranjeTerminaPoMogucstvuOdlaganja(skorasnjiZauzetiPreglediLekara);
        }

        private List<Pregled> GetPreglediNarednihSatVremena(List<Pregled> pregledi)
        {
            List<Pregled> preglediNarednihSatVremena = new List<Pregled>();
            DateTime trenutnoVreme = DateTime.Now;
            DateTime vremeZaSatVremena = trenutnoVreme.AddHours(1);
            foreach (Pregled pregled in pregledi)
            {
                if (pregled.VremePocetkaPregleda <= vremeZaSatVremena)
                {
                    preglediNarednihSatVremena.Add(pregled);
                }
            }
            return preglediNarednihSatVremena;
        }

        private List<Pregled> SortiranjeTerminaPoMogucstvuOdlaganja(List<Pregled> pregledi)
        {
            List<int> vremenaOdlaganja = new List<int>();

            foreach (Pregled pregled in pregledi)
            {
                Pregled odlozeniPregled = new Pregled(pregled);

                int vremeOdlaganja = 10;
                while (!MozeDaSeZakaze(odlozeniPregled))
                {
                    odlozeniPregled.VremePocetkaPregleda = odlozeniPregled.VremePocetkaPregleda.AddMinutes(vremeOdlaganja);
                    odlozeniPregled.VremeKrajaPregleda = odlozeniPregled.VremeKrajaPregleda.AddMinutes(vremeOdlaganja);
                    vremeOdlaganja += 10;
                }
                vremenaOdlaganja.Add(vremeOdlaganja);
            }
            return SortirajPregledePoVremenuOdlaganja(pregledi, vremenaOdlaganja);
        }
        private List<Pregled> SortirajPregledePoVremenuOdlaganja(List<Pregled> pregledi, List<int> odlaganja)
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

                        Pregled tempPregled = pregledi[j];
                        pregledi[j] = pregledi[j + 1];
                        pregledi[j + 1] = tempPregled;
                    }
                }
            }
            return pregledi;
        }

        public List<Pregled> GetSlobodniPreglediLekaraOpstePrakseUNarednomPeriodu()
        {
            foreach (Lekar lekar in lekarRepo.GetSviLekariOpstePrakse())
            {
                List<Pregled> slobodniTerminiLekara =
                    SviPredloziSkorasnjihPregleda(lekar, MINUTI_TRAJANJA_PREGLEDA, MINUTI_PREDLAGANJA_ZA_3_DANA);
                List<Pregled> terminiURadnomVremenu = GetSviTerminiURadnomVremenuLekara(lekar, slobodniTerminiLekara);
                List<Pregled> slobodniTermini = GetSlobodniPreglediLekara(lekar, terminiURadnomVremenu);
                if (slobodniTermini.Count > 0)
                {
                    return slobodniTermini;
                }
            }

            return null;
        }

        public List<Pregled> GetSlobodniHitniPreglediLekaraOdredjeneOblasti(OblastLekara prosledjenaOblast, int minutiTrajanjaPregleda)
        {
            foreach (Lekar lekar in lekarRepo.LekariOdredjeneOblasti(prosledjenaOblast.Naziv))
            {
                List<Pregled> slobodniTerminiLekara = GetSlobodniHitniPreglediLekaraSaTrajanjem(lekar, minutiTrajanjaPregleda);
                if (slobodniTerminiLekara.Count() > 0)
                {
                    return slobodniTerminiLekara;
                }
            }
            return null;
        }

        private List<Pregled> GetSlobodniHitniPreglediLekaraSaTrajanjem(Lekar lekar, int minutiTrajanjePregleda)
        {
            List<Pregled> sviSkorasnjiTermini = SviPredloziSkorasnjihPregleda(lekar, minutiTrajanjePregleda, MINUTI_INTERVALA_ZA_PREDLAGANJE_HITNIH_PREGLEDA);
            List<Pregled> terminiURadnomVremenu = GetSviTerminiURadnomVremenuLekara(lekar, sviSkorasnjiTermini);
            List<Pregled> slobodniTermini = GetSlobodniPreglediLekara(lekar, terminiURadnomVremenu);

            return slobodniTermini;
        }

        //PROVERA DA LI PACIJENT VEC IMA ZAKAZANI PREGLED U TOM TERMINU
        public bool PacijentImaZakazanPregled(Pregled pregledZaProveru)
        {
            VremenskiInterval intervalPregledaZaProveru =
                new VremenskiInterval(pregledZaProveru.VremePocetkaPregleda, pregledZaProveru.VremeKrajaPregleda);

            foreach (Pregled pregled in GetSviBuduciPreglediPacijenta(pregledZaProveru.Pacijent.Jmbg))
            {
                VremenskiInterval intervalPregleda =
                    new VremenskiInterval(pregled.VremePocetkaPregleda, pregled.VremeKrajaPregleda);
                if (intervalPregledaZaProveru.DaLiSePreklapaSa(intervalPregleda))
                {
                    return true;
                }
            }

            return false;
        }
        public int[] BrojPregledaZaPetMeseci()
        {
            int[] brojPregleda = new int[5];
            List<Pregled> sviPreglediKodOvogLekara = GetsviPregledi();
            int trenutniMesec = DateTime.Now.Month;
            int mesecPregleda;
            foreach (Pregled pregledIter in sviPreglediKodOvogLekara)
            {
                mesecPregleda = pregledIter.VremePocetkaPregleda.Month;
                if (mesecPregleda == trenutniMesec)
                {
                    brojPregleda[4]++;
                }
                else if (mesecPregleda == (trenutniMesec - 1) || mesecPregleda == (trenutniMesec + 11))
                {
                    brojPregleda[3]++;
                }
                else if (mesecPregleda == (trenutniMesec - 2) || mesecPregleda == (trenutniMesec + 10))
                {
                    brojPregleda[2]++;
                }
                else if (mesecPregleda == (trenutniMesec - 3) || mesecPregleda == (trenutniMesec + 9))
                {
                    brojPregleda[1]++;
                }
                else if (mesecPregleda == (trenutniMesec - 4) || mesecPregleda == (trenutniMesec + 8))
                {
                    brojPregleda[0]++;
                }
            }
            return brojPregleda;
        }

        public int[] BrojPregledaKodLekaraZaPetMeseci(string idLekara)
        {
            int[] brojPregleda = new int[5];
            List<Pregled> sviPreglediKodOvogLekara = GetSviPreglediLekara(idLekara);
            int trenutniMesec = DateTime.Now.Month;
            int mesecPregleda;
            foreach (Pregled pregledIter in sviPreglediKodOvogLekara)
            {
                mesecPregleda = pregledIter.VremePocetkaPregleda.Month;
                if (mesecPregleda == trenutniMesec)
                {
                    brojPregleda[4]++;
                }
                else if (mesecPregleda == (trenutniMesec - 1) || mesecPregleda == (trenutniMesec + 11))
                {
                    brojPregleda[3]++;
                }
                else if (mesecPregleda == (trenutniMesec - 2) || mesecPregleda == (trenutniMesec + 10))
                {
                    brojPregleda[2]++;
                }
                else if (mesecPregleda == (trenutniMesec - 3) || mesecPregleda == (trenutniMesec + 9))
                {
                    brojPregleda[1]++;
                }
                else if (mesecPregleda == (trenutniMesec - 4) || mesecPregleda == (trenutniMesec + 8))
                {
                    brojPregleda[0]++;
                }
            }
            return brojPregleda;
        }

        public int[] BrojPrijemaZaPetDana(string idLekara)
        {
            int[] brojPregleda = { 0, 0, 0, 0, 0 };
            List<Pregled> sviPreglediKodOvogLekara = GetSviBuduciPreglediLekara(idLekara);
            List<DateTime> narednihPetDana = ListaNaprednihPetDatuma();
            DateTime datumPregleda = new DateTime();
            foreach (Pregled pregledIter in sviPreglediKodOvogLekara)
            {
                datumPregleda = pregledIter.VremePocetkaPregleda;
                for (int i = 0; i < 5; i++)
                {
                    if (datumPregleda.Day == narednihPetDana.ElementAt(i).Day && datumPregleda.Month == narednihPetDana.ElementAt(i).Month && datumPregleda.Year == narednihPetDana.ElementAt(i).Year)
                    {
                        brojPregleda[i]++;
                        break;
                    }
                }
            }
            return brojPregleda;
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
