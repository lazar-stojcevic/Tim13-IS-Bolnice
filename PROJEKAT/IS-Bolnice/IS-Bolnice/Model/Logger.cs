using IS_Bolnice.LogIn;

namespace IS_Bolnice.Model
{
    public class Logger: Entitet
    {
        public string KorisnickoIme { get; set; }
        public string Sifra { get; set; }
        public ILogInServis LogInServis { get; set; }
        public string Tip { get; set; }

        public Logger(string korisnickoIme, string sifra, string tip) : base(korisnickoIme)
        {
            KorisnickoIme = korisnickoIme;
            Sifra = sifra;
            Tip = tip;
        }

        public Logger(string korisnickoIme, string sifra, ILogInServis servis, string tip) : base(korisnickoIme)
        {
            KorisnickoIme = korisnickoIme;
            Sifra = sifra;
            LogInServis = servis;
            Tip = tip;
        }
    }
}
