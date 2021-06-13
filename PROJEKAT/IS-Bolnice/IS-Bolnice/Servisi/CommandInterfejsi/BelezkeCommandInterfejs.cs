using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Servisi.CommandInterfejsi
{
    interface BelezkeCommandInterfejs
    {
        void ObrisiBelezku(string idBeleske);
        List<Beleska> GetSveTrenutneBelezkePacijenta(string jmbgPacijenta);
        void SacuvajBelezku(Beleska beleska);
        void IzmeniBelezku(Beleska izmenjenaBeleska);
    }
}
