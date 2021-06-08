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
using IS_Bolnice.ViewModel.Lekar;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for LekarGlavniMeni.xaml
    /// </summary>
    public partial class LekarGlavniMeni : Page
    {
        public string Sifra { get; set; }
        public LekarGlavniMeni(string jmbgLekara)
        {
            Sifra = jmbgLekara;
            InitializeComponent();
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            this.DataContext = new GlavniMeniViewModel(this.NavigationService, Sifra);
        }

       
        /*
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
   MessageBoxResult result = CustomMessageBox.ShowYesNo("Da li ste sigurni da želite da se odjavite?", "Odjava", "Da", "Ne", MessageBoxImage.Question);
   if (result == MessageBoxResult.Yes)
   {
       MainWindow prijava = new MainWindow();
       Application.Current.Windows[0].Close();
       prijava.ShowDialog();
   }
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


private void Button_UvidUHospitalizacijeClick(object sender, RoutedEventArgs e)
{
   LekarPrikazSvihHospitalizacija sveHospitalizacije = new LekarPrikazSvihHospitalizacija();
   NavigationService.Navigate(sveHospitalizacije);
}

private void ButtonClick_InventarPoSalama(object sender, RoutedEventArgs e)
{
   LekarInventarPoSalama inventarPoSalama = new LekarInventarPoSalama();
   NavigationService.Navigate(inventarPoSalama);
}

private void Button_VidiRecenzije(object sender, RoutedEventArgs e)
{
   LekarUvidURecenzije recenzije = new LekarUvidURecenzije(Sifra);
   NavigationService.Navigate(recenzije);
}

private void ButtonClick_Godisnji(object sender, RoutedEventArgs e)
{
   LekarZahtevZaGodisnji zahtevZaGodisnji = new LekarZahtevZaGodisnji();
   NavigationService.Navigate(zahtevZaGodisnji);
}

private void ButtonObavestenja_Click(object sender, RoutedEventArgs e)
{
   LekarObavestenja lekarObavestenja = new LekarObavestenja();
   NavigationService.Navigate(lekarObavestenja);
}

private void Button_Click(object sender, RoutedEventArgs e)
{
   LekarPotrosnja lekarPotrosnja = new LekarPotrosnja();
   NavigationService.Navigate(lekarPotrosnja);
}

private void ButtonRecenzija_Click(object sender, RoutedEventArgs e)
{
   RecenzijaAplikacije recenzijaAplikacije = new RecenzijaAplikacije();
   NavigationService.Navigate(recenzijaAplikacije);
}
*/
    }

}
