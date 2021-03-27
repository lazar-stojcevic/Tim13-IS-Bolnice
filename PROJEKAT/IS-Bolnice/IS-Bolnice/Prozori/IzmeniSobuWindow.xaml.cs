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
    /// Interaction logic for IzmeniSobuWindow.xaml
    /// </summary>
    public partial class IzmeniSobuWindow : Window
    {
        public IzmeniSobuWindow(string id, RoomType tip, int sprat, double kvadr)
        {
            InitializeComponent();
            this.id_txt.Text = id;
            this.kvadratura_txt.Text = kvadr.ToString();
            this.sprat_txt.Text = sprat.ToString();
            switch (tip)
            {
                case RoomType.bolnickaSoba:
                    this.tip_sobe_txt.SelectedIndex = 1;
                    break;
                case RoomType.operacionaSala:
                    this.tip_sobe_txt.SelectedIndex = 0;
                    break;
                case RoomType.magacin:
                    this.tip_sobe_txt.SelectedIndex = 3;
                    break;
                default:
                    this.tip_sobe_txt.SelectedIndex = 2;
                    break;
            }
        }

        private void Izmeni_btn_Click(object sender, RoutedEventArgs e)
        {
            List<Bolnica> bolnice = new List<Bolnica>();
            Bolnica b1 = new Bolnica();
            BazaBolnica baza = new BazaBolnica();
            bolnice = baza.SveBolcine();
            foreach (Bolnica b in bolnice)
            {
                foreach (Soba s in b.Soba)
                {
                    if (s.Id == this.id_txt.Text)
                    {
                        s.Kvadratura = double.Parse(kvadratura_txt.Text);
                        s.Sprat = int.Parse(sprat_txt.Text);
                        s.Tip = (RoomType)tip_sobe_txt.SelectedIndex;
                        b1 = b;
                        break;
                    }
                }

            }
            baza.KreirajBolnicu(b1);
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
