using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using IS_Bolnice.Baze;
using IS_Bolnice.Model;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarUvidURecenzije.xaml
    /// </summary>
    public partial class LekarUvidURecenzije : Page
    {
        public LekarUvidURecenzije(string idLekara)
        {
            InitializeComponent();
            List<Survery> recenzije = new List<Survery>();

            foreach (Survery recenzija in new BazaOcena().ReadAllSurvery())
            {
                try
                {
                    if (recenzija.Doctor.Jmbg.Equals(idLekara))
                    {
                        recenzije.Add(recenzija);
                    }
                }
                catch (Exception e)
                {
                        //Nista
                }
                
            }

            listaRecenzija.ItemsSource = recenzije;
        }

        private void Open_OnClick(object sender, RoutedEventArgs e)
        {
            /*
            if (listaRecenzija.SelectedIndex != -1)
            {
                Survery recenzija = (Survery) listaRecenzija.SelectedItem;
                MessageBox.Show(recenzija.Comment, "Recenzija");
            }
            */

            Button button = sender as Button;
            Survery recenzija = button.DataContext as Survery;
            MessageBox.Show(recenzija.Comment, "Recenzija");


        }

    }

    
}
