using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.StanjeSobe
{
    public class StanjeSlobodna : IStanjeSobe
    {
        public bool oslobodi()
        {
            return true;
        }

        public bool renoviraj()
        {
            return true;
        }

        public bool zauzmi()
        {
            return true;
        }
    }
}
