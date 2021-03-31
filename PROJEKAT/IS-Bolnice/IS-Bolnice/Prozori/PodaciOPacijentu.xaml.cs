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

namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for PodaciOPacijentu.xaml
    /// </summary>
    public partial class PodaciOPacijentu : Window
    {
        public PodaciOPacijentu(string jmbg)
        {
            BazaPacijenata baza = new BazaPacijenata();
            Pacijent p = baza.PacijentSaOvimJMBG(jmbg);
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
        }
    }
}
