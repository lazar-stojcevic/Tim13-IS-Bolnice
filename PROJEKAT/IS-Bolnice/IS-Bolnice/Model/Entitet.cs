using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Model
{
   public class Entitet
    {
        public string Id { get; set; }

        public Entitet()
        {

        }
        public Entitet(string id)
        {
            Id = id;
        }

    }
}
