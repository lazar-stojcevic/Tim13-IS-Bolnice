using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi
{
    class LekarServis
    {
        private BazaLekara bazaLekara = new BazaLekara();
        public List<Lekar> GetSviLekariSpecijalisti()
        {
            List<Lekar> listaSvihLekara = new List<Lekar>();
            foreach (Lekar lekar in bazaLekara.SviLekari())
            {
                if (!lekar.JelLekarOpstePrakse())
                {
                    listaSvihLekara.Add(lekar);
                }
            }

            return listaSvihLekara;
        }

        public List<Lekar> GetSviLekariOpstePrakse()
        {
            List<Lekar> listaSvihLekaraOpstePrakse = new List<Lekar>();
            foreach (Lekar lekar in bazaLekara.SviLekari())
            {
                if (lekar.JelLekarOpstePrakse())
                {
                    listaSvihLekaraOpstePrakse.Add(lekar);
                }
            }

            return listaSvihLekaraOpstePrakse;
        }

        public List<Lekar> GetSviLekari()
        {
            return bazaLekara.SviLekari();
        }

        public List<Operacija> dostuptniTerminiLekaraZaDatuProstoriju(string jmbgLekara, string idSale, int duzinaTrajanja)
        {
            //Lekar
            Lekar lekar = new Lekar();
            foreach (Lekar l in GetSviLekari())
            {
                if (l.Jmbg.Equals(jmbgLekara))
                {
                    lekar = l;
                    break;
                }
            }
            //Liste
            List<Operacija> validni = new List<Operacija>();
            List<Operacija> sveOperacije = new OperacijaServis().GetSveSledeceOperacije();
            List<Operacija> slobodni = new List<Operacija>();
           // slobodni = new BazaOperacija().SlobodneHitneOperacijeLekaraSaTrajanjem2(lekar, duzinaTrajanja);
            
            DateTime sutra = DateTime.Now.AddDays(1);
            for (int i = 0; i < 5; i++)
            {
                System.DateTime pocetakIntervala = new System.DateTime(sutra.Year, sutra.Month, sutra.Day, lekar.RadnoVreme.StandardnoRadnoVreme.Pocetak.Hour, lekar.RadnoVreme.StandardnoRadnoVreme.Pocetak.Minute, 0, 0);
                System.DateTime krajIntervala = new System.DateTime(sutra.Year, sutra.Month, sutra.Day, lekar.RadnoVreme.StandardnoRadnoVreme.Kraj.Hour, lekar.RadnoVreme.StandardnoRadnoVreme.Kraj.Minute, 0, 0);
                pocetakIntervala = pocetakIntervala.AddDays(i);
                krajIntervala = krajIntervala.AddDays(i);
                krajIntervala = krajIntervala.AddMinutes(-30);

                while (pocetakIntervala <= krajIntervala)
                {
                    Operacija o = new Operacija();
                    o.Lekar = lekar;
                    o.VremePocetkaOperacije = pocetakIntervala;
                    pocetakIntervala = pocetakIntervala.AddMinutes(10);
                    o.VremeKrajaOperacije = o.VremePocetkaOperacije.AddMinutes(duzinaTrajanja);
                    o.Soba = new Soba(idSale);
                    slobodni.Add(o);
                }
            } 

            foreach (Operacija predlozeni in slobodni)
            {
                bool isValid = !TerminSePreklapaKodLekara(jmbgLekara, predlozeni);

                foreach (Operacija operacija in sveOperacije)
                {
                    if (PreklapanjeTerminaUSali(idSale, predlozeni, operacija))
                    {
                        isValid = false;
                        break;
                    }
                }
                if (isValid)
                {
                    validni.Add(predlozeni);
                }
            }

            return validni;
            
        }

        public List<Operacija> SveSledeceOperacijeDatogLekara(string jmbgLekara)
        {
            List<Operacija> povratnaVrednost = new List<Operacija>();
            List<Operacija> sveNaredneOperacijeSvihLekara = new BazaOperacija().SveSledeceOperacije();
            foreach (Operacija iterOperacija in sveNaredneOperacijeSvihLekara)
            {
                if (iterOperacija.Lekar.Jmbg.Equals(jmbgLekara))
                {
                    povratnaVrednost.Add(iterOperacija);
                }
            }
            return povratnaVrednost;
        }


        private static bool PreklapanjeTerminaUSali(string idSale, Operacija predlozeni, Operacija operacija)
        {
            if (predlozeni.VremePocetkaOperacije <= operacija.VremePocetkaOperacije && predlozeni.VremeKrajaOperacije >= operacija.VremeKrajaOperacije)
            {
                return true;
            }
            if (predlozeni.VremePocetkaOperacije >= operacija.VremePocetkaOperacije && predlozeni.VremeKrajaOperacije <= operacija.VremeKrajaOperacije)
            {
                return true;
            }
            if (predlozeni.VremePocetkaOperacije >= operacija.VremePocetkaOperacije && predlozeni.VremePocetkaOperacije <= operacija.VremeKrajaOperacije)
            {
                return true;
            }
            if (predlozeni.VremePocetkaOperacije <= operacija.VremePocetkaOperacije && predlozeni.VremeKrajaOperacije >= operacija.VremePocetkaOperacije)
            {
                return true;
            }

            return false;
        }


        private bool TerminSePreklapaKodLekara(string jmbgLekara, Operacija predlozenaOperacija)
        {
            BazaPregleda bazaPregleda = new BazaPregleda();
            foreach (Pregled zakazaniPregled in bazaPregleda.SviBuduciPreglediKojeLekarIma(jmbgLekara))
            {
                if (PreklapanjeTerminaPregleda(predlozenaOperacija, zakazaniPregled))
                {
                    return true;
                }
            }

            foreach (Operacija zakazanaOperacija in SveSledeceOperacijeDatogLekara(jmbgLekara))
            {
                if (predlozenaOperacija.Soba.Jednaka(zakazanaOperacija.Soba) && PreklapanjeTerminaOperacija(predlozenaOperacija, zakazanaOperacija))
                {
                    return true;
                }
            }

            return false;
        }

        private bool PreklapanjeTerminaOperacija(Operacija predlozenaOperacija, Operacija zakazanaOperacija)
        {
            if (predlozenaOperacija.VremePocetkaOperacije <= zakazanaOperacija.VremePocetkaOperacije && predlozenaOperacija.VremeKrajaOperacije >= zakazanaOperacija.VremeKrajaOperacije)
            {
                return true;
            }
            if (predlozenaOperacija.VremePocetkaOperacije >= zakazanaOperacija.VremePocetkaOperacije && predlozenaOperacija.VremeKrajaOperacije <= zakazanaOperacija.VremeKrajaOperacije)
            {
                return true;
            }
            if (predlozenaOperacija.VremePocetkaOperacije >= zakazanaOperacija.VremePocetkaOperacije && predlozenaOperacija.VremePocetkaOperacije <= zakazanaOperacija.VremeKrajaOperacije)
            {
                return true;
            }
            if (predlozenaOperacija.VremePocetkaOperacije <= zakazanaOperacija.VremePocetkaOperacije && predlozenaOperacija.VremeKrajaOperacije >= zakazanaOperacija.VremePocetkaOperacije)
            {
                return true;
            }

            return false;
        }

        private bool PreklapanjeTerminaPregleda(Operacija predlozenaOperacija, Pregled zakazaniPregled)
        {
            if (predlozenaOperacija.VremePocetkaOperacije <= zakazaniPregled.VremePocetkaPregleda && predlozenaOperacija.VremeKrajaOperacije >= zakazaniPregled.VremeKrajaPregleda)
            {
                return true;
            }
            if (predlozenaOperacija.VremePocetkaOperacije >= zakazaniPregled.VremePocetkaPregleda && predlozenaOperacija.VremeKrajaOperacije <= zakazaniPregled.VremeKrajaPregleda)
            {
                return true;
            }
            if (predlozenaOperacija.VremePocetkaOperacije >= zakazaniPregled.VremePocetkaPregleda && predlozenaOperacija.VremePocetkaOperacije <= zakazaniPregled.VremeKrajaPregleda)
            {
                return true;
            }
            if (predlozenaOperacija.VremePocetkaOperacije <= zakazaniPregled.VremePocetkaPregleda && predlozenaOperacija.VremeKrajaOperacije >= zakazaniPregled.VremePocetkaPregleda)
            {
                return true;
            }

            return false;
        }

       

    }
}
