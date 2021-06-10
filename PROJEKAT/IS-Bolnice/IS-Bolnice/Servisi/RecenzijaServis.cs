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
    class RecenzijaServis
    {
        private IRecenzijaRepozitorijum recRepo = new Injector().GetRecenzijaRepozitorijum();
        public void KreirajRecenziju(Recenzija recenzija)
        {
            recRepo.Sacuvaj(recenzija);
        }
    }
}
