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
    }
}
