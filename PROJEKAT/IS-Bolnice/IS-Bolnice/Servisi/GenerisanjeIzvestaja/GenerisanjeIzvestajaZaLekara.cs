using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Kontroleri;

namespace IS_Bolnice.Servisi.GenerisanjeIzvestaja
{
    public abstract class GenerisanjeIzvestajaZaLekara
    {
        protected Lekar lekar;
        protected VremenskiInterval intervalIzvestaja;
        protected List<Pregled> preglediLekaraZaIzvestaj;
        protected List<Operacija> operacijeLekaraZaIzvestaj;

        public bool GenerisanjeIzvestajaZauzetostiLekara(Lekar lekar, VremenskiInterval intervalIzvestaja)
        {
            this.lekar = lekar;
            this.intervalIzvestaja = PripremiVremenskiInterval(intervalIzvestaja);
            preglediLekaraZaIzvestaj = GetPreglediLekaraUZadatomPeriodu();
            operacijeLekaraZaIzvestaj = GetOperacijeLekaraUZadatomPeriodu();
            return GenerisiIzvestajZauzetostiLekara();
        }

        protected abstract bool GenerisiIzvestajZauzetostiLekara();

        private VremenskiInterval PripremiVremenskiInterval(VremenskiInterval intervalZaPripremu)
        {
            intervalZaPripremu.Kraj = intervalZaPripremu.Kraj.Date.AddHours(23);    // da bi upali i termini tog dana
            intervalZaPripremu.Kraj = intervalZaPripremu.Kraj.AddMinutes(59);
            intervalZaPripremu.Kraj = intervalZaPripremu.Kraj.AddSeconds(59);
            return intervalZaPripremu;
        }

        private List<Pregled> GetPreglediLekaraUZadatomPeriodu()
        {
            List<Pregled> sviPreglediLekaraUPeriodu = new List<Pregled>();
            PregledKontroler pregledKontroler = new PregledKontroler();
            foreach (Pregled pregled in pregledKontroler.GetSviPreglediLekara(lekar.Jmbg))
            {
                VremenskiInterval intervalTermina =
                    new VremenskiInterval(pregled.VremePocetkaPregleda, pregled.VremeKrajaPregleda);
                if (intervalIzvestaja.SadrziInterval(intervalTermina))
                {
                    sviPreglediLekaraUPeriodu.Add(pregled);
                }
            }

            return sviPreglediLekaraUPeriodu;
        }

        private List<Operacija> GetOperacijeLekaraUZadatomPeriodu()
        {
            List<Operacija> sveOperacijeLekaraUPeriodu = new List<Operacija>();
            OperacijaKontroler operacijaKontroler = new OperacijaKontroler();
            foreach (Operacija operacija in operacijaKontroler.GetSveOperacijeLekara(lekar.Jmbg))
            {
                VremenskiInterval intervalTermina =
                    new VremenskiInterval(operacija.VremePocetkaOperacije, operacija.VremeKrajaOperacije);
                if (intervalIzvestaja.SadrziInterval(intervalTermina))
                {
                    sveOperacijeLekaraUPeriodu.Add(operacija);
                }
            }

            return sveOperacijeLekaraUPeriodu;
        }
    }
}
