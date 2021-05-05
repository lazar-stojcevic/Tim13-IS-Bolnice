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
    /// Interaction logic for LekarDodavanjeSastojaka.xaml
    /// </summary>
    public partial class LekarDodavanjeSastojaka : Window
    {
        private ListBox lista = new ListBox();
        private BazaSastojaka bazaSastojaka = new BazaSastojaka();
        public LekarDodavanjeSastojaka(ListBox listaSastojaka)
        {
            InitializeComponent();
            foreach (Sastojak iter in bazaSastojaka.SviSastojci())
            {
                bool vecPostoji = false;
                foreach (string sastojak in listaSastojaka.Items)
                {
                    if (sastojak.Equals(iter.Ime))
                    {
                        vecPostoji = true;
                        break;
                    }
                }
                if (!vecPostoji)
                    listaSvihSastojaka.Items.Add(iter.Ime);
            }

            lista = listaSastojaka;
        }

        private void Button_ClickKraj(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_ClickDodaj(object sender, RoutedEventArgs e)
        {
            if (listaSvihSastojaka.SelectedIndex != -1)
            {
                bool vecPostoji = false;
                foreach (string sastojak in lista.Items)
                {
                    if (sastojak.Equals(listaSvihSastojaka.SelectedItem))
                    {
                        vecPostoji = true;
                        break;
                    }
                }

                if (!vecPostoji)
                    lista.Items.Add(listaSvihSastojaka.SelectedItem);
            }
        }
    }
}
