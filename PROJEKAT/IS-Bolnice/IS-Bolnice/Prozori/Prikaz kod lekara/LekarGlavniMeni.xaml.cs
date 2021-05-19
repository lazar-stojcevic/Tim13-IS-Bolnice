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
using System.Windows.Shapes;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarGlavniMeni.xaml
    /// </summary>
    public partial class LekarGlavniMeni : Page
    {
        public LekarGlavniMeni(string jmbgLekara)
        {
            Sifra = jmbgLekara;
            InitializeComponent();
        }

        private void ButtonRaspored_Click(object sender, RoutedEventArgs e)
        {
            var raspored = new LekarRaspored(Sifra);

            this.NavigationService.Navigate(raspored);
        }

        private void ButtonPregled_Click(object sender, RoutedEventArgs e)
        {
            Page pregled = new LekarWindow(Sifra);
            this.NavigationService.Navigate(pregled);  
        }

        private void odjavaClick(object sender, RoutedEventArgs e)
        { 
            

            MainWindow prijava = new MainWindow();
            Application.Current.Windows[0].Close();
            prijava.ShowDialog();

        }

        private void validacijaLekovaClick(object sender, RoutedEventArgs e)
        {
            Page validacija = new LekarValidacijaLekova(Sifra);
            this.NavigationService.Navigate(validacija);

        }

        private void Button_UvidUOdobreneLekoveClick(object sender, RoutedEventArgs e)
        {
           Page uvidULekove = new LekarUvidUOdobreneLekove();
           this.NavigationService.Navigate(uvidULekove);
        }

        public string Sifra { get; set; }

        private void Button_UvidUHospitalizacijeClick(object sender, RoutedEventArgs e)
        {
            LekarPrikazSvihHospitalizacija sveHospitalizacije = new LekarPrikazSvihHospitalizacija();
            NavigationService.Navigate(sveHospitalizacije);
        }
    }

}
