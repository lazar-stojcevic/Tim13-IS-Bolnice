using IS_Bolnice.Repozitorijumi.Interfejsi;

namespace IS_Bolnice.Repozitorijumi
{
    class Injector
    {
        public IAnketaRepozitorijum GetAnketaRepozitorijum()
        {
            return new AnketaFajlRepozitorijum();
        }

        public IBeleskaRepozitorijum GetBeleskaRepozitorijum()
        {
            return new BeleskaFajlRepozitorijum();
        }

        public IBolnicaRepozitorijum GetBolnicaRepozitorijum()
        {
            return new BolnicaFajlRepozitorijum();
        }

        public IHospitalizacijaRepozitorijum GetHospitalizacijaRepozitorijum()
        {
            return new HospitalizacijaFajlRepozitorijum();
        }

        public IIzmenaTerminaRepozitorijum GetIzmenaTerminaRepozitorijum()
        {
            return new IzmenaTerminaFajlRepozitorijum();
        }

        public IIzvestajRepozitorijum GetIzvestajRepozitorijum()
        {
            return new IzvestajFajlRepozitorijum();
        }

        public ILekarRepozitorijum GetLekarRepozitorijum()
        {
            return new LekarFajlRepozitorijum();
        }

        public ILekRepozitorijum GetLekRepozitorijum()
        {
            return new LekFajlRepozitorijum();
        }

        public ILoggerRepozitorijum GetLoggerRepozitorijum()
        {
            return new LoggerFajlRepozitorijum();
        }

        public IObavestenjaRepozitorijum GetObavestenjaRepozitorijum()
        {
            return new ObavestenjeFajlRepozitorijum();
        }

        public IOblastLekaraRepozitorijum GetOblastLekaraRepozitorijum()
        {
            return new OblastLekaraFajlRepozitorijum();
        }

        public IOdgovorNaZahtevRepozitorijum GetOdgovorNaZahtevRepozitorijum()
        {
            return new OdgovorNaZahtevFajlRepozitorijum();
        }

        public IOperacijaRepozitorijum GetOperacijaRepozitorijum()
        {
            return new OperacijaFajlRepozitorijum();
        }

        public IOpremaRepozitorijum GetOpremaRepozitorijum()
        {
            return new OpremaFajlRepozitorijum();
        }

        public IPacijentRepozitorijum GetPacijentRepozitorijum()
        {
            return new PacijentFajlRepozitorijum();
        }

        public IPregledRepozitorijum GetPregledRepozitorijum()
        {
            return new PreglediFajlRepozitorijum();
        }

        public IRadnoVremeRepozitorijum GetRadnoVremeRepozitorijum()
        {
            return new RadnoVremeFajlRepozitorijum();
        }

        public IRecenzijaRepozitorijum GetRecenzijaRepozitorijum()
        {
            return new RecenzijaFajlRepozitorijum();
        }

        public IRenovacijaRepozitorijum GetRenovacijaRepozitorijum()
        {
            return new RenovacijaFajlRepozitorijum();
        }

        public ISadrzajSobeRepozitorijum GetSadrzajSobeRepozitorijum()
        {
            return new SadrzajSobeFajlRepozitorijum();
        }

        public ISastojakRepozitorijum GetSastojakRepozitorijum()
        {
            return new SastojakFajlRepozitorijum();
        }

        public ISekretarRepozitorijum GetSekretarRepozitorijum()
        {
            return new SekretarFajlRepozitorijum();
        }

        public IUpravnikRepozitorijum GetUpravnikRepozitorijum()
        {
            return new UpravnikFajlRepozitorijum();
        }

        public IZahteviZaValidacijuRepozitorijum GetZahteviZaValidacijuRepozitorijum()
        {
            return new ZahteviZaValidacijuFajlRepozitorijum();
        }
    }
}
