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

namespace IS_Bolnice.Prozori.UpravnikPages
{
    /// <summary>
    /// Interaction logic for SalePage.xaml
    /// </summary>
    public partial class SalePage : Page
    {
        public SalePage()
        {
            InitializeComponent();
            List<Soba> lista = new List<Soba>();
            List<Bolnica> bolnice = new List<Bolnica>();
            BazaBolnica baza = new BazaBolnica();
            bolnice = baza.SveBolnice();
            foreach (Bolnica b in bolnice)
            {
                foreach (Soba s in b.Soba)
                {

                    if (s.Obrisano == false)
                    {
                        lista.Add(s);

                    }
                }

            }

            List<string> tekst = new List<string>();
            foreach (Soba s in lista)
            {
                if (s.Obrisano == false)
                {
                    tekst.Add("ID: " + s.Id + " Sprat: " + s.Sprat.ToString() + " Tip: " + s.Tip);
                }
            }
            listBox.ItemsSource = tekst;
        }

        private void tip_opreme_txt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool sveProstijeSelektovane = false;
            RoomType tip = RoomType.bolnickaSoba;
            switch (tip_sale_txt.SelectedIndex) {
                case 0:
                    sveProstijeSelektovane = true;
                    break;
                case 1:
                    sveProstijeSelektovane = false;
                    tip = RoomType.operacionaSala;
                    break;
                case 2:
                    sveProstijeSelektovane = false;
                    tip = RoomType.bolnickaSoba;
                    break;
                case 3:
                    sveProstijeSelektovane = false;
                    tip = RoomType.ordinacija;
                    break;
                default:
                    sveProstijeSelektovane = false;
                    tip = RoomType.magacin;
                    break;
            }
           
            List<Soba> lista = new List<Soba>();
            List<Bolnica> bolnice = new List<Bolnica>();
            BazaBolnica baza = new BazaBolnica();
            bolnice = baza.SveBolnice();
            foreach (Bolnica b in bolnice)
            {
                foreach (Soba s in b.Soba)
                {

                    if (s.Obrisano == false && (sveProstijeSelektovane == true || s.Tip == tip))
                    {
                        lista.Add(s);

                    }
                }

            }

            List<string> tekst = new List<string>();
            foreach (Soba s in lista)
            {
                if (s.Obrisano == false)
                {
                    tekst.Add("ID: " + s.Id + " Sprat: " + s.Sprat.ToString() + " Tip: " + s.Tip);
                }
            }
            listBox.ItemsSource = tekst;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] niz = listBox.SelectedItem.ToString().Split(' ');
            Page editSale = new EditSalePage(niz[1]);
            this.NavigationService.Navigate(editSale);
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Page addSale = new AddSalePage();
            this.NavigationService.Navigate(addSale);
        }
    }
}
