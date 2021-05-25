using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IS_Bolnice.Kontroleri;

namespace IS_Bolnice.Prozori.Sekretar
{
    /// <summary>
    /// Interaction logic for PrikazZauzetostiLekaraSpecijalista.xaml
    /// </summary>
    public partial class PrikazZauzetostiLekaraSpecijalista : Window
    {
        private PregledKontroler pregledKontroler = new PregledKontroler();
        private OperacijaKontroler operacijaKontroler = new OperacijaKontroler();
        public ObservableCollection<Pregled> PreglediSelektovanogLekara
        {
            get;
            set;
        }

        public ObservableCollection<Operacija> OperacijeSelektovanogLekara
        {
            get;
            set;
        }
        public PrikazZauzetostiLekaraSpecijalista(Lekar selektovaniLekar)
        {
            InitializeComponent();
            this.DataContext = this;

            txtLekar.Text = selektovaniLekar.Ime + " " + selektovaniLekar.Prezime;

            PreglediSelektovanogLekara =
                new ObservableCollection<Pregled>(pregledKontroler.GetSviBuduciPreglediLekara(selektovaniLekar.Jmbg));
            OperacijeSelektovanogLekara =
                new ObservableCollection<Operacija>(
                    operacijaKontroler.GetSveSledeceOperacijeLekara(selektovaniLekar.Jmbg));
        }
    }
}
