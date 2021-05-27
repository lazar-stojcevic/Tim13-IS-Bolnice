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

namespace IS_Bolnice.Prozori.Sekretar
{
    /// <summary>
    /// Interaction logic for NaplacivanjeUsluga.xaml
    /// </summary>
    public partial class NaplacivanjeUsluga : Window
    {
        public ObservableCollection<string> OdabraneUsluge
        {
            get;
            set;
        }


        public NaplacivanjeUsluga(Pacijent selektovaniPacijent)
        {
            InitializeComponent();
            this.DataContext = this;
            OdabraneUsluge = new ObservableCollection<string>();
            tbPacijent.Text = selektovaniPacijent.Ime + " " + selektovaniPacijent.Prezime;
        }

        private void Button_Click_Dodavanje(object sender, RoutedEventArgs e)
        {
            List<string> odabiranjeUsluga = new List<string>();
            PrikazUsluga pu = new PrikazUsluga(odabiranjeUsluga);
            pu.ShowDialog();
            foreach (var usluga in odabiranjeUsluga)
            {
                OdabraneUsluge.Add(usluga);
            }

            ObracunajUkupnuCenu();
        }

        private void ObracunajUkupnuCenu()
        {
            int cena = 0;
            foreach (var usluga in OdabraneUsluge)
            {
                string[] delovi = usluga.Split('-');
                delovi[1] = delovi[1].TrimStart();
                string[] deoCene = delovi[1].Split(' ');
                cena += Int32.Parse(deoCene[0]);
            }

            labelCena.Content = cena;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
