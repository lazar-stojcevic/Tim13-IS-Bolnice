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
using IS_Bolnice.Model;
using IS_Bolnice.Servisi;
using WPFCustomMessageBox;

namespace IS_Bolnice.Prozori.Sekretar
{
    /// <summary>
    /// Interaction logic for RecenzijaAplikacijeDesktop.xaml
    /// </summary>
    public partial class RecenzijaAplikacijeDesktop : Window
    {
        public RecenzijaAplikacijeDesktop()
        {
            InitializeComponent();
        }

        private void Button_Click_Potvrdi(object sender, RoutedEventArgs e)
        {
            try
            {
                Recenzija recenzija = new Recenzija();
                recenzija.Ocena = Int16.Parse(ocena.Text);
                recenzija.Opis = opis.Text;
                new RecenzijaServis().KreirajRecenziju(recenzija);
                InformativniProzor ip = new InformativniProzor("Uspešno ste poslali recenziju.");
                ip.ShowDialog();
                Close();
            }
            catch
            {
                InformativniProzor ip = new InformativniProzor("Niste selektovali ocenu.");
                ip.ShowDialog();
            }
        }

        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
