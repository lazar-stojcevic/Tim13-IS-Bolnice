using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.StanjeSobe
{
    public interface IStanjeSobe
    {
        bool renoviraj();
        bool zauzmi();
        bool oslobodi();
    }
}
