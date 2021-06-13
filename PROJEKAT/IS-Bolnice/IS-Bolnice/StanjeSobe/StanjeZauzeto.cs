using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.StanjeSobe
{
    public class StanjeZauzeto : IStanjeSobe
    {
        public bool oslobodi()
        {
            return true;
        }

        public bool renoviraj()
        {
            return false;
        }

        public bool zauzmi()
        {
            return false;
        }
    }
}
