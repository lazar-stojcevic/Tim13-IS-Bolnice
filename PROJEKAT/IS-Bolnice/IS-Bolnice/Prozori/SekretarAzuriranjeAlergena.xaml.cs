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

namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for SekretarAzuriranjeAlergena.xaml
    /// </summary>
    public partial class SekretarAzuriranjeAlergena : Window
    {
        private Pacijent pacijent;
        private BazaPregleda bp;

        public ObservableCollection<string> AlergeniPacijenta
        {
            get;
            set;
        }

        public SekretarAzuriranjeAlergena(Pacijent p)
        {
            InitializeComponent();

            this.DataContext = this;
            //TODO: implementirati cuvanje liste alergena pacijenta u bazi pacijenata!
            //AlergeniPacijenta = new ObservableCollection<string>(p.Alergeni);
        }
    }
}
