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
    public partial class PrikazSvihPacijenata : Window
    {
        private BazaPacijenata bp;
        private ObservableCollection<Pacijent> odabraniPacijentiRef;
        public ObservableCollection<Pacijent> Pacijenti
        {
            get;
            set;
        }

        public PrikazSvihPacijenata(ObservableCollection<Pacijent> odabraniPacijenti)
        {
            InitializeComponent();

            this.DataContext = this;
            bp = new BazaPacijenata();
            Pacijenti = new ObservableCollection<Pacijent>(bp.SviPacijenti());

            odabraniPacijentiRef = odabraniPacijenti;
        }

        private bool SadrziPacijenta(List<Pacijent> pacijenti, Pacijent pacijent)
        {
            foreach (Pacijent p in pacijenti)
            {
                if (p.Jmbg.Equals(pacijent.Jmbg))
                {
                    return true;
                }
            }
            return false;
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dgPacijenti.SelectedItems.Count; i++)
            {
                Pacijent pacijent = (Pacijent)dgPacijenti.SelectedItems[i];
                if (!SadrziPacijenta(odabraniPacijentiRef.ToList(), pacijent))
                {
                    odabraniPacijentiRef.Add(pacijent);
                }
            }
            this.Close();
        }

        private void Button_Click_Odustani(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
