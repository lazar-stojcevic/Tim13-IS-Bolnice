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
    /// Interaction logic for SekretarWindow.xaml
    /// </summary>
    public partial class SekretarWindow : Window
    {
        private BazaObavestenja bo;

        public SekretarWindow()
        {
            InitializeComponent();

            bo = new BazaObavestenja();

            obavestenjaDataBinding.ItemsSource = bo.SvaObavestenja();
        }

        private void Button_Click_Novo(object sender, RoutedEventArgs e)
        {
            FormiranjeObavestenjaWindow fow = new FormiranjeObavestenjaWindow();
            fow.ShowDialog();

            obavestenjaDataBinding.ItemsSource = bo.SvaObavestenja();
        }

        private void Button_Click_Prikaz(object sender, RoutedEventArgs e)
        {
            PrikazPacijenata pp = new PrikazPacijenata();
            pp.Show();
        }

        private void Button_Click_Izmena(object sender, RoutedEventArgs e)
        {
            Obavestenje o = (Obavestenje)obavestenjaDataBinding.SelectedItem;
            if (o != null)
            {
                IzmenaObavestenjaWindow iow = new IzmenaObavestenjaWindow(o);
                iow.ShowDialog();

                // za osvezavanje prikaza
                obavestenjaDataBinding.ItemsSource = bo.SvaObavestenja();
            }
        }

        private void Button_Click_Brisanje(object sender, RoutedEventArgs e)
        {
            Obavestenje o = (Obavestenje)obavestenjaDataBinding.SelectedItem;
            if (o != null)
            {
                string sMessageBoxText = "Da li ste sigurni da želite da obrišete obaveštenje?";
                string sCaption = "Brisanje obaveštenja";

                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxImage icnMessageBox = MessageBoxImage.Question;

                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        bo.ObrisiObavestenje(o);

                        //za osvezavanje prikaza            
                        obavestenjaDataBinding.ItemsSource = bo.SvaObavestenja();
                        break;

                    case MessageBoxResult.No:
                        /* ... */
                        break;
                }
            }
        }
    }
}
