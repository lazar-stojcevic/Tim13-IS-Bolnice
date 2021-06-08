using System;

namespace IS_Bolnice.Model
{
    public class VremenskiInterval
    {
        public DateTime Pocetak { get; set; }
        public DateTime Kraj { get; set; }

        public VremenskiInterval() {}

        public VremenskiInterval(DateTime pocetak, DateTime kraj)
        {
            Pocetak = pocetak;
            Kraj = kraj;
        }

        public bool SadrziInterval(VremenskiInterval interval)
        {
            if (this.Pocetak <= interval.Pocetak && this.Kraj >= interval.Kraj)
            {
                return true;
            }

            return false;
        }

        public bool DaLiSePreklapaSa(VremenskiInterval drugi)
        {
            if ((this.Pocetak <= drugi.Pocetak) &&
                (this.Kraj >= drugi.Pocetak))
            {
                return true;
            }

            if ((this.Pocetak <= drugi.Pocetak) &&
                (drugi.Kraj <= this.Kraj))
            {
                return true;
            }

            if ((drugi.Pocetak <= this.Pocetak) &&
                (this.Kraj) <= drugi.Kraj)
            {
                return true;
            }

            if ((drugi.Pocetak <= this.Pocetak) &&
                (drugi.Kraj >= this.Pocetak))
            {
                return true;
            }

            return false;
        }

        public bool DaLiJeIstogDatuma(VremenskiInterval drugi)
        {
            if (this.Pocetak.Date == drugi.Pocetak.Date || this.Kraj.Date == drugi.Kraj.Date)
            {
                return true;
            }

            return false;
        }
    }
}
