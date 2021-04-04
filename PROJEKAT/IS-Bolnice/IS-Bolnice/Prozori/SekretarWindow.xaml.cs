using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private BazaPacijenata bp;

        public ObservableCollection<Pacijent> Pacijenti
        {
            get;
            set;
        }

        public ObservableCollection<Obavestenje> Obavestenja
        {
            get;
            set;
        }

        public SekretarWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            bp = new BazaPacijenata();
            bo = new BazaObavestenja();

            Pacijenti = new ObservableCollection<Pacijent>(bp.SviPacijenti());
            Obavestenja = new ObservableCollection<Obavestenje>(bo.SvaObavestenja());
        }

        private void Button_Click_Novi(object sender, RoutedEventArgs e)
        {
            DodavanjePacijentaWindow dpw = new DodavanjePacijentaWindow(Pacijenti);
            dpw.ShowDialog();
        }

        private void Button_Click_Izmeni(object sender, RoutedEventArgs e)
        {
            Pacijent p = (Pacijent)dataGridPacijenti.SelectedItem;
            if (p != null)
            {
                IzmenaPacijentaWindow ipw = new IzmenaPacijentaWindow(p, Pacijenti);
                ipw.ShowDialog();
            }
        }

        private void Button_Click_Obrisi(object sender, RoutedEventArgs e)
        {
            Pacijent p = (Pacijent)dataGridPacijenti.SelectedItem;
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
                        Pacijenti.Remove(p);
                        break;

                    case MessageBoxResult.No:
                        /* ... */
                        break;
                }
            }
        }

        private void Button_Click_NoviGuest(object sender, RoutedEventArgs e)
        {
            DodavanjeGuestNalogaWindow dgnw = new DodavanjeGuestNalogaWindow(Pacijenti);
            dgnw.ShowDialog();
        }

        private void Button_Click_NovoObavestenje(object sender, RoutedEventArgs e)
        {
            FormiranjeObavestenjaWindow fow = new FormiranjeObavestenjaWindow(Obavestenja);
            fow.ShowDialog();
        }

        private void Button_Click_IzmenaObavestenja(object sender, RoutedEventArgs e)
        {
            Obavestenje o = (Obavestenje)obavestenjaDataBinding.SelectedItem;
            if (o != null)
            {
                IzmenaObavestenjaWindow iow = new IzmenaObavestenjaWindow(o, Obavestenja);
                iow.ShowDialog();
            }
        }

        private void Button_Click_BrisanjeObavestenja(object sender, RoutedEventArgs e)
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
                        Obavestenja.Remove(o);
                        break;

                    case MessageBoxResult.No:
                        /* ... */
                        break;
                }
            }
}


/*
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
                        /* ... //
                        break;
                }
            }
        }
*/
                }
            }
