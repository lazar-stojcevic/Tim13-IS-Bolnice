using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Model;

namespace IS_Bolnice.Baze.Interfejsi
{
    interface GenerickiRepozitorijum<T> where T : Entitet
    {
        List<T> GetSve();

        T GetPoId(string id);

        void Sacuvaj(T noviEntitet);

        void Izmeni(T noviEntitet);

        void Obrisi(string id);

    }
}
