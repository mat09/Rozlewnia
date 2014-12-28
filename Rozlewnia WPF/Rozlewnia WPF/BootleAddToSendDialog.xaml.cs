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

namespace Rozlewnia_WPF
{
    /// <summary>
    /// Interaction logic for BootleAddToSendDialog.xaml
    /// </summary>
    public partial class BootleAddToSendDialog : Window
    {
        private String id_trans;
        static int rowAmmount=3;

        public BootleAddToSendDialog(String id)
        {
            id_trans = id;
            InitializeComponent();
            dt.CanUserAddRows = false;
            dt.CanUserDeleteRows = false;
            dt.IsReadOnly = true;
            dt.ItemsSource = DataBase.Instance.searchBootle(null, null, null, 1, -1);
        }

        private void AddBootleToSend_click(object sender, RoutedEventArgs e)
        {
            List<searchBootleClass> bootle = dt.SelectedItems.OfType<searchBootleClass>().ToList();
            if (bootle != null)
            {
                if(dt.SelectedCells.Count >= rowAmmount)
                {
                    if (DataBase.Instance.changeTransport(bootle, null, 3, id_trans, true,null))
                    {
                        this.DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show("Bład");
                    }
                }
                else
                {
                    MessageBox.Show("Zaznacz butle.");
                }
            }
        }
    }
}
