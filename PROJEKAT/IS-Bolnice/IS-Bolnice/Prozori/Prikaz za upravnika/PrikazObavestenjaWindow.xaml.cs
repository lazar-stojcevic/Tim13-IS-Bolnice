using IS_Bolnice.Model;
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
    /// Interaction logic for PrikazObavestenjaWindow.xaml
    /// </summary>
    public partial class PrikazObavestenjaWindow : Window
    {
        public PrikazObavestenjaWindow(Obavestenje obavestenje)
        {
            InitializeComponent();
            textBox.Text = obavestenje.Sadrzaj;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
