using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Repozitorijumi.Interfejsi
{
    interface IPacijentRepozitorijum
    {
        List<Pacijent> GetSve();

        Pacijent GetPoJmbg(string jmbg);

        void Sacuvaj(Pacijent noviEntitet);

        void Izmeni(Pacijent noviEntitet);

        void Obrisi(string jmbg);
    }
}
