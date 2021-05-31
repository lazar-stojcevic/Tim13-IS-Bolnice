using IS_Bolnice.Baze;
using IS_Bolnice.Model;
using System;

namespace IS_Bolnice.Servisi
{
    class IzmenaTerminaServis
    {
        private static int MAX_CHANGES_IN_WEEK = 3;
        private IzmenaTerminaFajlRepozitorijum izmenaTerminaRepo = new IzmenaTerminaFajlRepozitorijum();

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
