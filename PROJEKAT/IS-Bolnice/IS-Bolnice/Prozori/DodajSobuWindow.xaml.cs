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
    /// Interaction logic for DodajSobuWindow.xaml
    /// </summary>
    public partial class DodajSobuWindow : Window
    {
        public DodajSobuWindow()
        {
            InitializeComponent();
        }

        private void Dodaj_btn_Click(object sender, RoutedEventArgs e)
        {
            Soba newS = new Soba();
            newS.Id = id_txt.Text;
            newS.Tip = (RoomType)tip_sobe_txt.SelectedIndex;
            newS.Kvadratura = double.Parse(kvadratura_txt.Text);
            newS.Sprat = int.Parse(sprat_txt.Text);
            List<Bolnica> bolnice = new List<Bolnica>();
            BazaBolnica baza = new BazaBolnica();
            bolnice = baza.SveBolcine();
            int flag = 0;
            foreach (Bolnica b in bolnice)
            {
                foreach (Soba s in b.Soba) {
                    if (s.Id.Equals(newS.Id)) {
                        if (s.Obrisano == true)
                        {
                            s.Obrisano = false;
                            flag = 1;
                            break;
                        }
                        else {
                            MessageBox.Show("Soba sa izabranim ID vec postoji!");
                            flag = 2;
                        }
                    
                    }
                
                }
                if (flag == 0)
                {
                    b.AddSoba(newS);
                    baza.KreirajBolnicu(b);
                }
                else if (flag == 1)
                {
                    baza.KreirajBolnicu(b);
                }
            }
            UpravnikWindow upravnik = new UpravnikWindow();
            upravnik.Show();
            this.Close();
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            UpravnikWindow upravnik = new UpravnikWindow();
            upravnik.Show();
            this.Close();
        }
    }
}
