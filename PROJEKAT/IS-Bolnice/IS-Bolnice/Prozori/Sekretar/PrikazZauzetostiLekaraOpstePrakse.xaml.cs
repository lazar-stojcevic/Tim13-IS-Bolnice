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
    /// Interaction logic for PrikazZauzetostiLekaraOpstePrakse.xaml
    /// </summary>
    public partial class PrikazZauzetostiLekaraOpstePrakse : Window
    {
        private PregledKontroler pregledKontroler = new PregledKontroler();
        public ObservableCollection<Pregled> PreglediSelektovanogLekara
        {
            get;
            set;
        }
        public PrikazZauzetostiLekaraOpstePrakse(Lekar selektovaniLekar)
        {
            InitializeComponent();
            this.DataContext = this;

            txtLekar.Text = selektovaniLekar.Ime + " " + selektovaniLekar.Prezime;
            PreglediSelektovanogLekara =
                new ObservableCollection<Pregled>(pregledKontroler.GetSviBuduciPreglediLekara(selektovaniLekar.Jmbg));
        }
    }
}
