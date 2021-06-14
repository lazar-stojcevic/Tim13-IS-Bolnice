using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.LogIn;
using IS_Bolnice.Model;
using IS_Bolnice.Repozitorijumi.Interfejsi;
using IS_Bolnice.Repozitorijumi.Klase;
using IS_Bolnice.Servisi;

namespace IS_Bolnice.Repozitorijumi
{
    public class LoggerFajlRepozitorijum: GenerickiFajlRepozitorijum<Model.Logger>, ILoggerRepozitorijum
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
                    logInServis = new PacijentLogIn();
                    break;
                case "U":
                    logInServis = new UpravnikLogIn();
                    break;
                case "L":
                    logInServis = new LekarLogIn();
                    break;
                default:
                    logInServis = new SekretarLogIn();
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
