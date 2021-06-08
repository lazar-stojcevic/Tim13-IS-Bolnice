﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Baze.Klase;
using IS_Bolnice.Model;

namespace IS_Bolnice.Baze
{
    class RecenzijaFajlRepozitorijum : GenerickiFajlRepozitorijum<Recenzija>, IRecenzijaRepozitorijum
    {
        public RecenzijaFajlRepozitorijum():base(@"..\..\Datoteke\recenzije.txt")
        {

        }
        public override Recenzija KreirajEntitet(string[] podaciEntiteta)
        {
            Recenzija recenzija = new Recenzija();
            recenzija.Id = podaciEntiteta[0];
            recenzija.Ocena = Int16.Parse(podaciEntiteta[1]);
            recenzija.Opis = podaciEntiteta[2];
            return recenzija;
        }

        public override string KreirajTextZaUpis(Recenzija entitet)
        {
            return entitet.Id + "#" + entitet.Ocena + "#" + entitet.Opis;
        }
    }
}
