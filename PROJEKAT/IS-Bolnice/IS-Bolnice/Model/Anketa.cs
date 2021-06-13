using System;

namespace IS_Bolnice.Model
{
    class Anketa : Recenzija
    {
        public Anketa() { }
        public Pacijent Pacijent { get; set; }
        public Lekar Lekar { get; set; }
        public Bolnica Bolnica { get; set; }
        public DateTime Trajanje { get; set; }
        public int KojaAnketa { get; set; } // 0 za lekara 1 za bolnicu

        public override string ToString()
        {
            if (KojaAnketa == 0)
            {
                return "0" + "#" + FormirajDatumZaPisanje(Trajanje) + "#" + Ocena.ToString() + "#" + Opis
                    + "#" + Pacijent.Jmbg + "#" + Lekar.Jmbg;
            }
            else
            {
                return "1" + "#" + FormirajDatumZaPisanje(Trajanje) + "#" + Ocena + "#" + Opis
                    + "#" + Pacijent.Jmbg + "#" + Bolnica.Ime;
            }
        }

        private static string formatDatumaZaPisanje = "M/d/yyyy h:mm:ss tt";

        private string FormirajDatumZaPisanje(DateTime datum)
        {
            return datum.ToString(formatDatumaZaPisanje);
        }
    }
}
