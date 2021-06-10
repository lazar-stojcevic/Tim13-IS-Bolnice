using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Bolnice.DTOs;

namespace IS_Bolnice.Servisi
{
    public interface ILogInServis
    {
        LogInDTO GetKorisnika(string korisnickoIme, string sifra);
        
    }
}
