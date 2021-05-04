using IS_Bolnice.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Baze
{
    class BazaOblastiLekara
    {
        private static string fileLocation = @"..\..\Datoteke\oblastiLekara.txt";

        public List<OblastLekara> SveOblasti()
        {
            List<string> linije = File.ReadAllLines(fileLocation).ToList();
            List<OblastLekara> sveOblasti = NapraviOblasti(linije);
            return sveOblasti;
        }

        private List<OblastLekara> NapraviOblasti(List<string> linijeIzFajla)
        {
            List<OblastLekara> sveOblasti = new List<OblastLekara>();

            foreach(string linija in linijeIzFajla)
            {
                OblastLekara oblast = new OblastLekara(linija);
                sveOblasti.Add(oblast);
            }
            return sveOblasti;
        }

        public void KreirajOblast(OblastLekara novaOblast)
        {
            throw new NotImplementedException();
        }

        public void IzmeniOblast(OblastLekara izmenjenaOblast)
        {
            throw new NotImplementedException();
        }

        public void ObrisiOblast(OblastLekara oblastZaBrisanje)
        {
            throw new NotImplementedException();
        }
    }
}
