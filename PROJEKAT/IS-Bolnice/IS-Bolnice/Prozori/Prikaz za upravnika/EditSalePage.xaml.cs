using IS_Bolnice.Prozori.Prikaz_za_upravnika;
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
    /// Interaction logic for EditSalePage.xaml
    /// </summary>
    public partial class EditSalePage : Page
    {

        Soba selectedSoba;
        public EditSalePage(string selectedId)
        {
            InitializeComponent();
            List<Bolnica> bolnice = new List<Bolnica>();
            Bolnica b1 = new Bolnica();
            BolnicaFajlRepozitorijum baza = new BolnicaFajlRepozitorijum();
            bolnice = baza.DobaviSve();
            foreach (Bolnica b in bolnice)
            {
                foreach (Soba s in b.Soba)
                {
                    if (s.Id.Equals(selectedId))
                    {
                        selectedSoba = s;
                        break;
                    }
                }

            }
            this.id_txt.Text = selectedSoba.Id;
            this.kvadratura_txt.Text = selectedSoba.Kvadratura.ToString();
            this.sprat_txt.Text = selectedSoba.Sprat.ToString();
            switch (selectedSoba.Tip)
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
            BolnicaFajlRepozitorijum baza = new BolnicaFajlRepozitorijum();
            bolnice = baza.DobaviSve();
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
            baza.Sacuvaj(b1);
            Page sale = new SalePage();
            this.NavigationService.Navigate(sale);
        }

        private void Odustani_btn_Click(object sender, RoutedEventArgs e)
        {
            Page sale = new SalePage();
            this.NavigationService.Navigate(sale);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
                MessageBoxResult resultat = MessageBox.Show("Da li ste sigurni da zelite da obrisete sobu?", "", MessageBoxButton.YesNo);
                if (resultat == MessageBoxResult.Yes)
                {
                    List<Bolnica> bolnice = new List<Bolnica>();
                    BolnicaFajlRepozitorijum baza = new BolnicaFajlRepozitorijum();
                    bolnice = baza.DobaviSve();
                    foreach (Bolnica b in bolnice)
                    {
                        foreach (Soba s in b.Soba)
                        {

                            if (s.Id.Equals(selectedSoba.Id))
                            {
                                s.Obrisano = true;
                                baza.Sacuvaj(b);
                                break;
                            }
                        }

                    }
                Page sale = new SalePage();
                this.NavigationService.Navigate(sale);
                }

            
        }

        private void RenoviranjeButton_Click(object sender, RoutedEventArgs e)
        {
            Page renoviranje = new RenoviranjePage(selectedSoba);
            this.NavigationService.Navigate(renoviranje);
        }
    }
}
