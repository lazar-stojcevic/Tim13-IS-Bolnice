using IS_Bolnice.Prozori;
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

namespace IS_Bolnice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click_1(object sender, RoutedEventArgs e)
        {
            string korisnik = txtUserId.Text;

            if(korisnik.Equals("sekretar"))
            {
                SekretarWindow sw = new SekretarWindow();
                sw.Show();
            }
            else if (korisnik.Equals("upravnik"))
            {
                UpravnikWindow uw = new UpravnikWindow();
                uw.Show();
            }
            else if (korisnik.Equals("lekar"))
            {
                LekarWindow lw = new LekarWindow();
                lw.Show();
            }
            else // pacijent
            {
                PacijentWindow pw = new PacijentWindow();
                pw.Show();
            }

            this.Close();
        }

        private void btnClose_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
