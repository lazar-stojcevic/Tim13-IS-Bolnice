using System;
using System.Collections.Generic;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Servisi.Termini;

namespace IS_Bolnice.Servisi.Ustanova
{
    class RenovacijaServis
    {
        private IRenovacijaRepozitorijum renovacijaRepo = new Injector().GetRenovacijaRepozitorijum();
        IBolnicaRepozitorijum bolnicaFajlRepozitorijum = new Injector().GetBolnicaRepozitorijum();
        IHospitalizacijaRepozitorijum hospitalizacijaFajlRepozitorijum = new Injector().GetHospitalizacijaRepozitorijum();

        Renovacija novaRenovacija;
        List<Soba> sobeKojeSpajamo;

        public bool SalaNaRenoviranjuUOdredjenomPeriodu(string idSobe, DateTime datum)
        {
            foreach (Renovacija renovacijaIter in renovacijaRepo.SveRenovacijeJedneSobe(idSobe))
            {
                if (datum > renovacijaIter.DatumPocetka && datum < renovacijaIter.DatumKraja)
                {
                    return true;
                }
            }

            return false;
        }

        public bool RenoviranjeOperacioneSale(Renovacija novaRenovacija)
        {
            OperacijaServis operacijaServis = new OperacijaServis();
            foreach (Operacija operacija in operacijaServis.GetSveBuduceOperacijeSale(novaRenovacija.ProstorijaZaRenoviranje.Id))
            {
                if (operacija.VremePocetkaOperacije > novaRenovacija.DatumPocetka && operacija.VremeKrajaOperacije < novaRenovacija.DatumKraja)
                {
                    return false;
                }
            }
            renovacijaRepo.Sacuvaj(novaRenovacija);
            return true;
        }

        public bool RenoviranjeProstorije(Renovacija novaRenovacija)
        {
            PregledServis pregledServis = new PregledServis();
            foreach (Pregled pregled in pregledServis.GetSviBuduciPreglediSobe(novaRenovacija.ProstorijaZaRenoviranje.Id))
            {
                if (pregled.VremePocetkaPregleda > novaRenovacija.DatumPocetka && pregled.VremePocetkaPregleda < novaRenovacija.DatumKraja)
                {
                    return false;
                }
            }
            renovacijaRepo.Sacuvaj(novaRenovacija);
            return true;

        }

        public bool RenovirajBolnickuSobu(Renovacija novaRenovacija)
        {
            
            foreach (Hospitalizacija hospitalizacija in hospitalizacijaFajlRepozitorijum.GetSveHospitalizacijeZaSobu(novaRenovacija.ProstorijaZaRenoviranje.Id))
            {
                if (hospitalizacija.PocetakHospitalizacije > novaRenovacija.DatumPocetka && hospitalizacija.KrajHospitalizacije < novaRenovacija.DatumKraja)
                {
                    return false;
                }
            }
            renovacijaRepo.Sacuvaj(novaRenovacija);
            return true;
        }

        public void RenovirajOpremu(int oznakaRadnjeNadOpremom, string idSobe)
        {
            SadrzajSobeServis sadrzajSobeServis = new SadrzajSobeServis();
            switch (oznakaRadnjeNadOpremom)
            {
                case 0:
                    sadrzajSobeServis.ObrisiOpremuIzSobe(idSobe); ;
                    break;
                case 1:
                    foreach (SadrzajSobe sadrzaj in sadrzajSobeServis.GetSadrzajSobe(idSobe))
                    {
                        sadrzajSobeServis.DodajUMagacin(sadrzaj.Predmet, sadrzaj.Kolicina);
                    }
                    sadrzajSobeServis.ObrisiOpremuIzSobe(idSobe);
                    break;
                default:
                    break;
            }
        }

        public bool SpajanjeSobe(List<Soba> sobeZaSpajanje, Renovacija renovacija)
        {
            sobeKojeSpajamo = sobeZaSpajanje;
            novaRenovacija = renovacija;
            if (ProveraRaspolozivostiProstorija())
            {
                SpajanjeSoba();
                return true;
            }
            return false;
        }

        public void RazdvajanjeSobe(Renovacija renovacija, List<Soba> noveSobe)
        {
            novaRenovacija = renovacija;
            BolnicaServis bolnicaServis = new BolnicaServis();
            List<Soba> updateSoba = bolnicaFajlRepozitorijum.GetSobe();
            if (ProvaraRaspolozivostiProstorije(renovacija.ProstorijaZaRenoviranje))
            {
                foreach (Soba iterSoba in noveSobe)
                {
                    updateSoba.Add(iterSoba);
                }
                foreach (Soba sobaIter in updateSoba)
                {
                    if (sobaIter.Id.Equals(renovacija.ProstorijaZaRenoviranje.Id))
                    {
                        sobaIter.Obrisano = true;
                        break;
                    }
                }
                Bolnica novaBolnica = bolnicaFajlRepozitorijum.GetBolnica();
                novaBolnica.Soba = updateSoba;
                bolnicaFajlRepozitorijum.Izmeni(novaBolnica);
            }
        }

        private void SpajanjeSoba()
        {
            KreirajNovuSobu();
            renovacijaRepo.Sacuvaj(novaRenovacija);
            UkloniSobe();
        }

        private void UkloniSobe()
        {
            List<Soba> updateSoba = bolnicaFajlRepozitorijum.GetSobe();
            foreach (Soba selected in sobeKojeSpajamo)
            {
                foreach (Soba sobaIter in updateSoba)
                {
                    if (sobaIter.Id.Equals(selected.Id))
                    {
                        sobaIter.Obrisano = true;
                        break;
                    }
                }
            }
            Bolnica novaBolnica = bolnicaFajlRepozitorijum.GetBolnica();
            novaBolnica.Soba = updateSoba;
            bolnicaFajlRepozitorijum.Izmeni(novaBolnica);
        }

        private void KreirajNovuSobu()
        {
            foreach (Soba sobaIter in sobeKojeSpajamo)
            {
                novaRenovacija.ProstorijaZaRenoviranje.Kvadratura = novaRenovacija.ProstorijaZaRenoviranje.Kvadratura + sobaIter.Kvadratura;
                novaRenovacija.ProstorijaZaRenoviranje.Sprat = sobaIter.Sprat;
            }
            Bolnica novaBolnica = bolnicaFajlRepozitorijum.GetBolnica();
            novaBolnica.AddSoba(novaRenovacija.ProstorijaZaRenoviranje);
            bolnicaFajlRepozitorijum.Izmeni(novaBolnica);

        }

        private bool ProveraRaspolozivostiProstorija()
        {
            foreach (Soba soba in sobeKojeSpajamo)
            {
                if (!ProvaraRaspolozivostiProstorije(soba)) {
                    return false;
                }
            }
            return true;
        }

        private bool ProvaraRaspolozivostiProstorije(Soba odabranaSoba)
        {
            bool prostorijaJeSlobodna = true;
            if (odabranaSoba.Tip == RoomType.operacionaSala)
            {
                prostorijaJeSlobodna = ProveraOperacioneSale(odabranaSoba.Id);
            }
            else if (odabranaSoba.Tip == RoomType.bolnickaSoba)
            {
                prostorijaJeSlobodna = ProveraBolnickeSobe(odabranaSoba.Id);
            }
            else
            {
                prostorijaJeSlobodna = ProveraProstorije(odabranaSoba.Id);
            }
            return prostorijaJeSlobodna;

        }

        private bool ProveraOperacioneSale(string idSobe)
        {
            OperacijaServis operacijaServis = new OperacijaServis();
            foreach (Operacija operacija in operacijaServis.GetSveBuduceOperacijeSale(idSobe))
            {
                    if (operacija.VremePocetkaOperacije > novaRenovacija.DatumPocetka)
                    {
                        return false;
                    }
            }
            return true;
        }

        private bool ProveraBolnickeSobe(string idSobe)
        {
            foreach (Hospitalizacija hospitalizacija in hospitalizacijaFajlRepozitorijum.GetSveHospitalizacijeZaSobu(idSobe))
                {
                    if (hospitalizacija.PocetakHospitalizacije > novaRenovacija.DatumPocetka)
                    {
                        return false;
                    }
                }
            return true;
        }

        private bool ProveraProstorije(string idSobe)
        {
            PregledServis pregledServis = new PregledServis();
            foreach (Pregled pregled in pregledServis.GetSviBuduciPreglediSobe(idSobe))
                {
                    if (pregled.VremePocetkaPregleda > novaRenovacija.DatumPocetka)
                    {
                        return false;
                    }
                }
            return true;

        }

        
        
    }
}
