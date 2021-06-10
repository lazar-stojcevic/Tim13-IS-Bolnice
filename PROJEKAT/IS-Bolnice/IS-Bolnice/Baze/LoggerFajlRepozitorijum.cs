using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.Baze.Interfejsi;
using IS_Bolnice.Baze.Klase;
using IS_Bolnice.Model;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Baze
{
    public class LoggerFajlRepozitorijum: GenerickiFajlRepozitorijum<Logger>, ILoggerRepozitorijum
    {
        public LoggerFajlRepozitorijum() : base(@"..\..\Datoteke\logIn.txt")
        {
        }

        public override Logger KreirajEntitet(string[] podaciEntiteta)
        {
            ILogInServis logInServis;
            switch (podaciEntiteta[2])
            {
                case "P":
                    logInServis = new PacijentServis();
                    break;
                case "U":
                    logInServis = new UpravnikServis();
                    break;
                case "L":
                    logInServis = new LekarServis();
                    break;
                default:
                    logInServis = new SekretarServis();
                    break;

            }
            Logger newLogger = new Logger(podaciEntiteta[0], podaciEntiteta[1], logInServis, podaciEntiteta[2]);
            return newLogger;
        }

        public override string KreirajTextZaUpis(Logger entitet)
        {
            string linija = entitet.KorisnickoIme + "#" + entitet.Sifra + "#" + entitet.Tip;
            return linija;
        }
    }
}
