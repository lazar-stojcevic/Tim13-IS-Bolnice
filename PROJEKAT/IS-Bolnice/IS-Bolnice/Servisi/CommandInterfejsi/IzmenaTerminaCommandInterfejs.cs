using IS_Bolnice.Model;

namespace IS_Bolnice.Servisi.CommandInterfejsi
{
    interface IzmenaTerminaCommandInterfejs
    {
        void SacuvajIzmenu(IzmenaTermina izmenaTermina);

        void OdblokirajPacijenta(Pacijent pacijent);

        bool DaLiJePacijentMaliciozan(Pacijent pacijent);
    }
}
