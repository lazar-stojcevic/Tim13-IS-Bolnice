using System;
using IS_Bolnice.Baze;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Model;
using IS_Bolnice.Servisi.CommandInterfejsi;

namespace IS_Bolnice.Servisi.Termini
{
    class IzmenaTerminaServis : IzmenaTerminaCommandInterfejs
    {
        private static int MAX_CHANGES_IN_WEEK = 3;
        private IIzmenaTerminaRepozitorijum izmenaTerminaRepo = new Injector().GetIzmenaTerminaRepozitorijum();

        public void SacuvajIzmenu(IzmenaTermina izmenaTermina)
        {
            izmenaTerminaRepo.Sacuvaj(izmenaTermina);
        }

        public void OdblokirajPacijenta(Pacijent pacijent)
        {
            izmenaTerminaRepo.OdblokirajPacijenta(pacijent);
        }

        public bool DaLiJePacijentMaliciozan(Pacijent pacijent)
        {
            if (BrojIzmenaPacijenta(pacijent) > MAX_CHANGES_IN_WEEK)
            {
                return true;
            }
            return false;
        }

        private int BrojIzmenaPacijenta(Pacijent pacijent)
        {
            int numberOfPatientChanges = 0;
            foreach (IzmenaTermina izmena in izmenaTerminaRepo.GetIzmenePacijenta(pacijent))
            {
                if (DaLiSeIzmenaDogodilaUPrethodihNedeljuDana(izmena))
                {
                    numberOfPatientChanges++;
                }
            }
            return numberOfPatientChanges;
        }

        private bool DaLiSeIzmenaDogodilaUPrethodihNedeljuDana(IzmenaTermina izmenaTermina)
        {
            DateTime now = DateTime.Now;
            System.DateTime ogranicenjeValidnihIzmena = now.AddDays(-7);

            if (izmenaTermina.DatumIzmene > ogranicenjeValidnihIzmena && izmenaTermina.DatumIzmene < now)
            {
                return true;
            }
            return false;
        }
    }
}
