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
using IS_Bolnice.Kontroleri;
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
            List<Anketa> recenzije = new List<Anketa>();

            foreach (Anketa recenzija in new AnketaKontroler().GetSveAnketeLekara())
            {
                try
                {
                    if (recenzija.Lekar.Jmbg.Equals(idLekara))
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
            Button button = sender as Button;
            Anketa recenzija = button.DataContext as Anketa;
            MessageBox.Show(recenzija.Komentar, "Recenzija");
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }

    
}
