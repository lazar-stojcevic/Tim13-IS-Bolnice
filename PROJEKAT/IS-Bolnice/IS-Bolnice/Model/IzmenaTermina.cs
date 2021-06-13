using System;

namespace IS_Bolnice.Model
{
    class IzmenaTermina : Entitet
    {
        public IzmenaTermina() { }

        public DateTime DatumIzmene { get; set; }
        public string JmbgPacijenta { get; set; }
    }
}
