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

namespace IS_Bolnice.Prozori.Prikaz_kod_lekara
{
    /// <summary>
    /// Interaction logic for PodaciOPacijentu.xaml
    /// </summary>
    public partial class PodaciOPacijentu : Page
    {
        public PodaciOPacijentu(string jmbg)
        {
            PacijentKontroler pacijentKontroler = new PacijentKontroler();
            Pacijent p = pacijentKontroler.GetPacijentSaOvimJMBG(jmbg);
            InitializeComponent();

            //TODO: saznaj bolji nacin da ovo sredis
            imeTxt.Text = p.Ime;
            prezimeTxt.Text = p.Prezime;
            jmbgTxt.Text = p.Jmbg;
            DatumTxt.Text = p.DatumRodjenja.ToString("dd.MM.yyyy");
            polTxt.Text = p.Pol.ToString();
            brojTxt.Text = p.BrojTelefona;
            adresaTxt.Text = p.Adresa;
            mailTxt.Text = p.EMail;
            if (p.Alergeni.Count != 0)
            {
                foreach (Sastojak alergen in p.Alergeni)
                {
                    listaAlergena.Items.Add(alergen.Ime);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IstorijaBolesti istorijaBolesti = new IstorijaBolesti(jmbgTxt.Text);
            NavigationService.Navigate(istorijaBolesti);

        }
    }
}
