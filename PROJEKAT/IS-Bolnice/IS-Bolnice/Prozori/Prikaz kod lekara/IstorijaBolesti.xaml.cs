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
using IS_Bolnice.Kontroleri;
using IS_Bolnice.Model;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for IstorijaBolesti.xaml
    /// </summary>
    public partial class IstorijaBolesti : Page
    {
        public IstorijaBolesti(string jmbgPacijenta)
        {
            InitializeComponent();
            OperacijaKontroler operacijaKontroler = new OperacijaKontroler();
            PregledKontroler pregledKontroler = new PregledKontroler();

            List<Pregled> pregledi = new List<Pregled>();

            pregledi.Add(new Pregled("430", new PacijentKontroler().GetPacijentSaOvimJMBG(jmbgPacijenta), DateTime.Now.AddDays(-5), new OblastLekara("Opste prakse")));
            pregledi.Add(new Pregled("666", new PacijentKontroler().GetPacijentSaOvimJMBG(jmbgPacijenta), DateTime.Now.AddDays(-10), new OblastLekara("Urolog")));
            pregledi.Add(new Pregled("666", new PacijentKontroler().GetPacijentSaOvimJMBG(jmbgPacijenta), DateTime.Now.AddDays(-3), new OblastLekara("Pulmolog")));

            listaPregleda.ItemsSource = pregledi;


            List<Operacija> operacije = new List<Operacija>();

            operacije.Add(new Operacija("430", new PacijentKontroler().GetPacijentSaOvimJMBG(jmbgPacijenta), DateTime.Now.AddDays(-5), new OblastLekara("Opste prakse")));
            operacije.Add(new Operacija("666", new PacijentKontroler().GetPacijentSaOvimJMBG(jmbgPacijenta), DateTime.Now.AddDays(-10), new OblastLekara("Urolog")));
            operacije.Add(new Operacija("666", new PacijentKontroler().GetPacijentSaOvimJMBG(jmbgPacijenta), DateTime.Now.AddDays(-3), new OblastLekara("Pulmolog")));

            listaOperacija.ItemsSource = operacije;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
