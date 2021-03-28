using System.Collections.Generic;
using System.Windows;


namespace IS_Bolnice.Prozori
{
    /// <summary>
    /// Interaction logic for SekretarWindow.xaml
    /// </summary>
    public partial class SekretarWindow : Window
    {
        private BazaPacijenata bp;

        public SekretarWindow()
        {
            InitializeComponent();

            bp = new BazaPacijenata();

            PacijentiDataBinding.ItemsSource = bp.SviPacijenti();
        }

        private void Button_Click_Novi(object sender, RoutedEventArgs e)
        {
            DodavanjePacijentaWindow dpw = new DodavanjePacijentaWindow();
            dpw.ShowDialog();

            //za osvezavanje prikaza            
            PacijentiDataBinding.ItemsSource = bp.SviPacijenti();
        }

        private void Button_Click_Izmeni(object sender, RoutedEventArgs e)
        {
            Pacijent p = (Pacijent) PacijentiDataBinding.SelectedItem;
            if (p != null)
            {
                IzmenaPacijentaWindow ipw = new IzmenaPacijentaWindow(p);
                ipw.ShowDialog();

                //za osvezavanje prikaza            
                PacijentiDataBinding.ItemsSource = bp.SviPacijenti();
            }
        }

        private void Button_Click_Obrisi(object sender, RoutedEventArgs e)
        {
            Pacijent p = (Pacijent)PacijentiDataBinding.SelectedItem;
            if (p != null)
            {
                string sMessageBoxText = "Da li ste sigurni da želite da obrišete pacijenta?";
                string sCaption = "Brisanje pacijenta";

                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxImage icnMessageBox = MessageBoxImage.Question;

                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                switch (rsltMessageBox)
                {
                    case MessageBoxResult.Yes:
                        bp.ObrisiPacijenta(p);

                        //za osvezavanje prikaza            
                        PacijentiDataBinding.ItemsSource = bp.SviPacijenti();
                        break;

                    case MessageBoxResult.No:
                        /* ... */
                        break;
                }
            } 
        }

        private void Button_Click_NoviGuest(object sender, RoutedEventArgs e)
        {
            DodavanjeGuestNalogaWindow dgw = new DodavanjeGuestNalogaWindow();
            dgw.ShowDialog();

            PacijentiDataBinding.ItemsSource = bp.SviPacijenti();
        }
    }
}
