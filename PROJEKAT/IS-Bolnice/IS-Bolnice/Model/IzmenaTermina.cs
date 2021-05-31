using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Model
{
    // na klas dijagramu treba spojiti sa pacijentom
    class IzmenaTermina : Entitet
    {
        public IzmenaTermina() { }

        public DateTime DatumIzmene { get; set; }
        public string JmbgPacijenta { get; set; }
    }
}
