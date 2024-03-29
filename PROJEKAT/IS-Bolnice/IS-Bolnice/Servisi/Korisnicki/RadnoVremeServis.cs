﻿using System;
using System.Collections.Generic;
using IS_Bolnice.Repozitorijumi;
using IS_Bolnice.Repozitorijumi.Interfejsi;
using IS_Bolnice.Model;
using IS_Bolnice.Servisi.Informativni;
using IS_Bolnice.Servisi.Termini;

namespace IS_Bolnice.Servisi.Korisnicki
{
    class RadnoVremeServis
    {
        private IRadnoVremeRepozitorijum radnoVremeRepo = new Injector().GetRadnoVremeRepozitorijum();

        public void IzmeniRadnoVreme(RadnoVremeLekara radnoVreme)
        {
            radnoVremeRepo.Izmeni(radnoVreme);
        }

        // ne racunaju se dani u nedelji koji su mu neradni
        public int PreracunajBrojIskoriscenihSlobodnihDanaLekara(string jmbgLekara, List<DateTime> noviSlobodniDani)
        {
            RadnoVremeLekara radnoVremeLekara = radnoVremeRepo.GetRadnoVremeOdredjenogLekara(jmbgLekara);

            int iskorisceniSlobodniDani = 0;
            foreach (var dan in noviSlobodniDani)
            {
                if (!radnoVremeLekara.SlobodniDaniUNedelji.Contains(dan.DayOfWeek))
                {
                    iskorisceniSlobodniDani += 1;
                }
            }

            return iskorisceniSlobodniDani;
        }

        public RadnoVremeLekara GetRadnoVremeLekara(string idLekara)
        {
            return radnoVremeRepo.GetRadnoVremeOdredjenogLekara(idLekara);
        }

        public bool PreklapanjeIntervalaGodisnjegOdmoraLekara(List<DateTime> potencijalniSlobodniDani, string jmbgLekara)
        {
            RadnoVremeLekara radnoVremeLekara = radnoVremeRepo.GetRadnoVremeOdredjenogLekara(jmbgLekara);

            foreach (var slobodanDan in radnoVremeLekara.SlobodniDani)
            {
                foreach (var pot in potencijalniSlobodniDani)
                {
                    if (pot.Day == slobodanDan.Day && pot.Month == slobodanDan.Month)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool PreklapanjeIntervalaGodisnjegOdmoraSaObavezamaLekara(VremenskiInterval intervalGodisnjegOdmora,
            string jmbgLekara)
        {
            if (PreklapanjeIntervalaSaPregledimaLekara(intervalGodisnjegOdmora, jmbgLekara)) return true;

            if (PreklapanjeIntervalaSaOperacijamaLekara(intervalGodisnjegOdmora, jmbgLekara)) return true;

            return false;
        }

        private bool PreklapanjeIntervalaSaOperacijamaLekara(VremenskiInterval intervalGodisnjegOdmora,
            string jmbgLekara)
        {
            OperacijaServis operacijaServis = new OperacijaServis();
            foreach (var operacija in operacijaServis.GetSveBuduceOperacijeLekara(jmbgLekara))
            {
                VremenskiInterval intervalOperacije =
                    new VremenskiInterval(operacija.VremePocetkaOperacije, operacija.VremeKrajaOperacije);
                if (intervalGodisnjegOdmora.DaLiSePreklapaSa(intervalOperacije))
                {
                    return true;
                }
            }

            return false;
        }

        private bool PreklapanjeIntervalaSaPregledimaLekara(VremenskiInterval intervalGodisnjegOdmora, string jmbgLekara)
        {
            PregledServis pregledServis = new PregledServis();
            foreach (var pregled in pregledServis.GetSviBuduciPreglediLekara(jmbgLekara))
            {
                VremenskiInterval intervalPregleda =
                    new VremenskiInterval(pregled.VremePocetkaPregleda, pregled.VremeKrajaPregleda);
                if (intervalGodisnjegOdmora.DaLiSePreklapaSa(intervalPregleda))
                {
                    return true;
                }
            }

            return false;
        }

        public bool MozeLiSeDodelitiSlobodanDanUNedeljiLekaru(Lekar lekar, DayOfWeek danNedelje)
        {
            if (DanUNedeljiSePreklapaSaBuducimPregledimaLekara(lekar, danNedelje)) return false;

            if (DanUNedeljiSePreklapaSaBuducimOperacijamaLekara(lekar, danNedelje)) return false;

            return true;
        }

        private bool DanUNedeljiSePreklapaSaBuducimOperacijamaLekara(Lekar lekar, DayOfWeek danNedelje)
        {
            OperacijaServis operacijaServis = new OperacijaServis();
            foreach (Operacija operacija in operacijaServis.GetSveBuduceOperacijeLekara(lekar.Jmbg))
            {
                if (operacija.VremePocetkaOperacije.DayOfWeek == danNedelje ||
                    operacija.VremeKrajaOperacije.DayOfWeek == danNedelje)
                {
                    return true;
                }
            }

            return false;
        }

        private bool DanUNedeljiSePreklapaSaBuducimPregledimaLekara(Lekar lekar, DayOfWeek danNedelje)
        {
            PregledServis pregledServis = new PregledServis();
            foreach (Pregled pregled in pregledServis.GetSviBuduciPreglediLekara(lekar.Jmbg))
            {
                if (pregled.VremePocetkaPregleda.DayOfWeek == danNedelje ||
                    pregled.VremeKrajaPregleda.DayOfWeek == danNedelje)
                {
                    return true;
                }
            }

            return false;
        }

        public void OdloziSaObavestenjemTermineLekaraKojiSeNeUklapaju(VremenskiInterval vremenskiInterval, String jmbgLekara)
        {
            List<Pregled> odlozeniPregledi = OdloziTerminePregledaLekaraKojiSeNeUklapaju(vremenskiInterval, jmbgLekara);
            List<Operacija> odlozeneOperacije = OdloziTermineOperacijaLekaraKojiSeNeUklapaju(vremenskiInterval, jmbgLekara);
            PosaljiObavestenjeZaOdlaganjePregledaPacijentima(odlozeniPregledi);
            PosaljiObavestenjeZaOdlaganjeOperacijaPacijentima(odlozeneOperacije);
        }

        private List<Operacija> OdloziTermineOperacijaLekaraKojiSeNeUklapaju(VremenskiInterval vremenskiInterval, string jmbgLekara)
        {
            List<Operacija> odlozene = new List<Operacija>();
            OperacijaServis operacijaServis = new OperacijaServis();
            foreach (Operacija operacija in operacijaServis.GetSveBuduceOperacijeLekara(jmbgLekara))
            {
                VremenskiInterval vremenskiIntervalOperacije =
                    new VremenskiInterval(operacija.VremePocetkaOperacije, operacija.VremeKrajaOperacije);
                if (vremenskiInterval.DaLiJeIstogDatuma(vremenskiIntervalOperacije) &&
                    !vremenskiInterval.SadrziInterval(vremenskiIntervalOperacije))
                {
                    operacijaServis.OdloziOperacijuStoPre(operacija);
                    odlozene.Add(operacija);
                }
            }

            return odlozene;
        }

        private List<Pregled> OdloziTerminePregledaLekaraKojiSeNeUklapaju(VremenskiInterval vremenskiInterval, string jmbgLekara)
        {
            List<Pregled> odlozeni = new List<Pregled>();
            PregledServis pregledServis = new PregledServis();
            foreach (Pregled pregled in pregledServis.GetSviBuduciPreglediLekara(jmbgLekara))
            {
                VremenskiInterval vremenskiIntervalPregleda =
                    new VremenskiInterval(pregled.VremePocetkaPregleda, pregled.VremeKrajaPregleda);
                if (vremenskiInterval.DaLiJeIstogDatuma(vremenskiIntervalPregleda) &&
                    !vremenskiInterval.SadrziInterval(vremenskiIntervalPregleda))
                {
                    pregledServis.OdloziPregledStoPre(pregled);
                    odlozeni.Add(pregled);
                }
            }

            return odlozeni;
        }

        private void PosaljiObavestenjeZaOdlaganjePregledaPacijentima(List<Pregled> odlozeni)
        {
            foreach (Pregled pregled in odlozeni)
            {
                Obavestenje novo = new Obavestenje()
                {
                    Id = Guid.NewGuid().ToString(),
                    Naslov = "Odlaganje pregleda",
                    Sadrzaj = "Poštovani, vaš pregled je odložen za " + pregled.VremePocetkaPregleda.ToString() +
                              " kod lekara " + pregled.Lekar.Ime + " " + pregled.Lekar.Prezime +
                              ", usled nepredviđenih okolnosti. Molimo Vas, pogledajte ažuriran raspored termina u aplikaciji. Izvinjavamo se na novonastaloj situaciji.",
                    VremeKreiranja = DateTime.Now,
                    OdredjeniPacijenti = new List<Pacijent>()
                };
                novo.OdredjeniPacijenti.Add(pregled.Pacijent);
                ObavestenjeServis obavestenjeServis = new ObavestenjeServis();
                obavestenjeServis.KreirajObavestenje(novo);
            }
        }

        private void PosaljiObavestenjeZaOdlaganjeOperacijaPacijentima(List<Operacija> odlozene)
        {
            foreach (Operacija operacija in odlozene)
            {
                Obavestenje novo = new Obavestenje()
                {
                    Id = Guid.NewGuid().ToString(),
                    Naslov = "Odlaganje operacije",
                    Sadrzaj = "Poštovani, vaša operacija je odložena za " + operacija.VremePocetkaOperacije.ToString() +
                              " kod lekara " + operacija.Lekar.Ime + " " + operacija.Lekar.Prezime +
                              ", usled nepredviđenih okolnosti. Molimo Vas, pogledajte ažuriran raspored termina u aplikaciji. Izvinjavamo se na novonastaloj situaciji.",
                    VremeKreiranja = DateTime.Now,
                    OdredjeniPacijenti = new List<Pacijent>()
                };
                novo.OdredjeniPacijenti.Add(operacija.Pacijent);
                ObavestenjeServis obavestenjeServis = new ObavestenjeServis();
                obavestenjeServis.KreirajObavestenje(novo);
            }
        }

        public void OdloziSaObavestenjemBuduceTermineLekaraZbogPromene(Lekar lekar)
        {
            List<Pregled> odlozeniPregledi = OdloziSveBuducePregledeZbogPromene(lekar);
            List<Operacija> odlozeneOperacije = OdloziSveBuduceOperacijeZbogPromene(lekar);

            PosaljiObavestenjeZaOdlaganjePregledaPacijentima(odlozeniPregledi);
            PosaljiObavestenjeZaOdlaganjeOperacijaPacijentima(odlozeneOperacije);
        }

        private List<Pregled> OdloziSveBuducePregledeZbogPromene(Lekar lekar)
        {
            List<Pregled> odlozeniPregledi = new List<Pregled>();
            PregledServis pregledServis = new PregledServis();
            foreach (Pregled pregled in pregledServis.GetSviBuduciPreglediLekara(lekar.Jmbg))
            {
                DateTime sutradan = DateTime.Now.AddDays(1);
                sutradan = sutradan.Date;
                if (pregled.VremePocetkaPregleda > sutradan)
                {
                    VremenskiInterval intervalPregleda =
                        new VremenskiInterval(pregled.VremePocetkaPregleda, pregled.VremeKrajaPregleda);
                    if (!lekar.RadnoVreme.TerminURadnomVremenuLekara(intervalPregleda))
                    {
                        pregledServis.OdloziPregledStoPre(pregled);
                        odlozeniPregledi.Add(pregled);
                    }
                }
            }

            return odlozeniPregledi;
        }

        private List<Operacija> OdloziSveBuduceOperacijeZbogPromene(Lekar lekar)
        {
            List<Operacija> odlozeneOperacije = new List<Operacija>();
            OperacijaServis operacijaServis = new OperacijaServis();
            foreach (Operacija operacija in operacijaServis.GetSveBuduceOperacijeLekara(lekar.Jmbg))
            {
                DateTime sutradan = DateTime.Now.AddDays(1);
                if (operacija.VremePocetkaOperacije > sutradan)
                {
                    VremenskiInterval intervalOperacije =
                        new VremenskiInterval(operacija.VremePocetkaOperacije, operacija.VremeKrajaOperacije);
                    if (!lekar.RadnoVreme.TerminURadnomVremenuLekara(intervalOperacije))
                    {
                        operacijaServis.OdloziOperacijuStoPre(operacija);
                        odlozeneOperacije.Add(operacija);
                    }
                }
            }

            return odlozeneOperacije;
        }
    }
}
