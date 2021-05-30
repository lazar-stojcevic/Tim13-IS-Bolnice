using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Baze.Interfejsi
{
    interface IPacijentRepozitorijum
    {
        List<Pacijent> DobaviSve();

        Pacijent DobaviPoJmbg(string jmbg);

        void Sacuvaj(Pacijent noviEntitet);

        void Izmeni(Pacijent noviEntitet);

        void Obrisi(string jmbg);
    }
}
