using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Model;

namespace IS_Bolnice.Servisi
{
    class RadnoVremeServis
    {
        private IRadnoVremeRepozitorijum radnoVremeRepo = new RadnoVremeFajlRepozitorijum();

        public void IzmeniRadnoVreme(RadnoVremeLekara radnoVreme)
        {
            radnoVremeRepo.Izmeni(radnoVreme);
        }

        // ne racunaju se dani u nedelji koji su mu neradni
        public int PreracunajBrojIskoriscenihSlobodnihDanaLekara(string jmbgLekara, List<DateTime> noviSlobodniDani)
        {
            RadnoVremeLekara radnoVremeLekara = radnoVremeRepo.RadnoVremeOdredjenogLekara(jmbgLekara);

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

        public RadnoVremeLekara DobaRadnoVremeLekara(string idLekara)
        {
            return radnoVremeRepo.RadnoVremeOdredjenogLekara(idLekara);
        }

        public bool PreklapanjeIntervalaGodisnjegOdmoraLekara(List<DateTime> potencijalniSlobodniDani, string jmbgLekara)
        {
            RadnoVremeLekara radnoVremeLekara = radnoVremeRepo.RadnoVremeOdredjenogLekara(jmbgLekara);

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
    }
}
