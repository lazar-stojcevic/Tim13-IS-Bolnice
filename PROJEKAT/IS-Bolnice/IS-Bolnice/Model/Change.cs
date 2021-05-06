using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IS_Bolnice.Model
{
    // na klas dijagramu treba spojiti sa pacijentom
    class Change
    {
        public Change() { }

        public DateTime DateOfChange { get; set; }
        public string JmbgOfPatient { get; set; }
    }
}
