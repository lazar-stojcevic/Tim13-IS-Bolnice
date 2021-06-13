using IS_Bolnice.Servisi.Termini;

namespace IS_Bolnice.Kontroleri.Termini
{
    class IzmenaTerminaKontroler
    {
        private IzmenaTerminaServis izmenaTerminaServis = new IzmenaTerminaServis();

        public bool DaLiJePacijentMaliciozan(Pacijent pacijent) {
            return izmenaTerminaServis.DaLiJePacijentMaliciozan(pacijent);
        }

        public void OdblokirajPacijenta(Pacijent pacijent)
        {
            izmenaTerminaServis.OdblokirajPacijenta(pacijent);
        }
    }
}
