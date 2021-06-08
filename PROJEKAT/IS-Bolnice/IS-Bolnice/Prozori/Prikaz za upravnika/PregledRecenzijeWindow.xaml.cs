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

namespace IS_Bolnice.Prozori.Prikaz_za_upravnika
{
    /// <summary>
    /// Interaction logic for PregledRecenzijeWindow.xaml
    /// </summary>
    public partial class PregledRecenzijeWindow : Window
    {
        public PregledRecenzijeWindow(string tekst)
        {
            InitializeComponent();
            textBlock.Text = tekst;
        }

        private void Potvrdi_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
