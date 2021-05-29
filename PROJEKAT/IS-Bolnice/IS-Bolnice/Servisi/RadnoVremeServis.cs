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
        private IRadnoVremeRepozitorijum radnoVremeRepo = new BazaRadnogVremena();

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

        public bool PreklapanjeIntervalaGodisnjegOdmoraLekara(List<DateTime> potencijalniSlobodniDani, string jmbgLekara)
        {
            RadnoVremeLekara radnoVremeLekara = radnoVremeRepo.RadnoVremeOdredjenogLekara(jmbgLekara);

            foreach (var slobodanDan in radnoVremeLekara.SlobodniDani)
            {
                foreach (var pot in potencijalniSlobodniDani)
                {
                    if (pot.Day == slobodanDan.Day)
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
            foreach (var operacija in operacijaServis.GetSveSledeceOperacijeLekara(jmbgLekara))
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
            foreach (var pregled in pregledServis.SviBuduciPreglediKojeLekarIma(jmbgLekara))
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
    }
}
