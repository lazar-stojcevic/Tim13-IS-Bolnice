using IS_Bolnice.Prozori.UpravnikPages;
using IS_Bolnice.ViewModel.Upravnik;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace IS_Bolnice.Kontroleri
{
    class WPFNavigacijaKontroler
    {
        private NavigationService navService;

        public WPFNavigacijaKontroler(NavigationService service)
        {
            navService = service;
        }

        public void PrikaziAddSalePage()
        {
            navService.Navigate(new AddSalePage());
        }

        public void PrikaziSalePage()
        {
            navService.Navigate(new SalePage());
        }

        public void PrikaziEditSalePage(string idSale)
        {
            navService.Navigate(new EditSalePage(idSale));
        }

        public void IdiNazad()
        {
            navService.GoBack();
        }
    }
}
