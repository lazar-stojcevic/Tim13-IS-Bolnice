﻿using System;
using System.Collections.Generic;
using IS_Bolnice.Kontroleri.Korisnicki;
using IS_Bolnice.Model;
using IS_Bolnice.Servisi.Termini;

namespace IS_Bolnice.Kontroleri.Termini
{
    class PregledKontroler
    {
        private PregledServis pregledServis = new PregledServis();

        public Pregled PostaviPrvogSlobodnogLekaraOpstePrakseNaPregled(Pregled pregled)
        {
            return  pregledServis.PostaviPrvogSlobodnogLekaraOpstePrakseNaPregled(pregled);
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

        public List<Pregled> GetSviBuduciSortiraniPreglediPacijenta(string jmbgPacijenta)
        {
            return pregledServis.GetSviBuduciSortiraniPreglediPacijenta(jmbgPacijenta);
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

        public List<Pregled> GetSlobodniTerminiZaIzmenu(Lekar lekar, DateTime datumIzmene)
        {
            LekarKontroler lekarKontroler = new LekarKontroler();
            lekar = lekarKontroler.GetLekar(lekar.Jmbg);
            return pregledServis.GetSlobodniTerminiZaIzmenuPregledaPacijenta(lekar, datumIzmene);
        }

        public List<Pregled> GetDostupniTerminiPregledaLekaraUNarednomPeriodu(Lekar lekar)
        {
            return pregledServis.GetDostupniTerminiPregledaLekaraUNarednomPeriodu(lekar);
        }

        public bool IzmeniPregled(Pregled noviPregled)
        {
            return pregledServis.IzmeniPregled(noviPregled);
        }

        public void IzmeniPregled(Pregled novi, Pregled stari)
        {
            novi.Id = stari.Id;
            pregledServis.IzmeniPregled(novi);
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

        public List<Pregled> GetZauzetiHitniPreglediLekaraOdredjeneOblasti(OblastLekara oblastLekara)
        {
            return pregledServis.GetZauzetiHitniPreglediLekaraOdredjeneOblasti(oblastLekara);
        }

        public List<Pregled> GetSlobodniHitniPreglediLekaraOdredjeneOblasti(OblastLekara oblastLekara, int minutiTrajanjaTermina)
        {
            return pregledServis.GetSlobodniHitniPreglediLekaraOdredjeneOblasti(oblastLekara, minutiTrajanjaTermina);
        }

        public List<Pregled> GetSlobodniPreglediLekaraOpstePrakseUNarednomPeriodu()
        {
            return pregledServis.GetSlobodniPreglediLekaraOpstePrakseUNarednomPeriodu();
        }

        public bool PacijentImaZakazanPregled(Pregled pregledZaProveru)
        {
            return pregledServis.PacijentImaZakazanPregled(pregledZaProveru);
        }

        public int[] BrojPregledaKodLekaraZaPetMeseci(string idLekara) {

            return pregledServis.BrojPregledaKodLekaraZaPetMeseci(idLekara);
        }

        public int[] BrojPregledaZaPetMeseci()
        {
            return pregledServis.BrojPregledaZaPetMeseci();
        }

        public int[] BrojPrijemaZaPetDana(string idLekara)
        {
            return pregledServis.BrojPrijemaZaPetDana(idLekara);
        }
    }
}
