using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi.CommandInterfejsi
{
    interface AnketaCommandInterfejs
    {
        List<Pregled> GetSviPreglediZaAnketu(string patientJmbg);
        void SacuvajAnketu(Anketa anketa);
        List<Anketa> GetSveAnketeLekara();
        bool DaLiJeVremeZaAnketuBolnice(string jmbgPacijenta);
    }
}
