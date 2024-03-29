﻿using System;
using System.Collections.Generic;
using IS_Bolnice.Repozitorijumi;
using IS_Bolnice.Repozitorijumi.Interfejsi;
using IS_Bolnice.Model;

namespace IS_Bolnice.Servisi.Informativni
{
    class ObavestenjeServis
    {
        private IObavestenjaRepozitorijum obavestenjaRepo = new Injector().GetObavestenjaRepozitorijum();

        public void KreirajObavestenje(Obavestenje novoObavestenje)
        {
            obavestenjaRepo.Sacuvaj(novoObavestenje);
        }

        public void IzmeniObavestenje(Obavestenje izmenjeno)
        {
            obavestenjaRepo.Izmeni(izmenjeno);
        }

        public List<Obavestenje> GetSvaSortiranaObavestenja()
        {
            List<Obavestenje> svaObavestenja = obavestenjaRepo.GetSve();
            svaObavestenja.Sort((o1, o2) => DateTime.Compare(o1.VremeKreiranja,o2.VremeKreiranja));
            svaObavestenja.Reverse();
            return svaObavestenja;
        }

        public void ObrisiObavestenje(string idObavestenja)
        {
            obavestenjaRepo.Obrisi(idObavestenja);
        }
    }
}
