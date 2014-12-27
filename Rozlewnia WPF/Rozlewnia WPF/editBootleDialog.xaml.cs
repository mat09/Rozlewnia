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
    /// Interaction logic for editBootleDialog.xaml
    /// </summary>
    public partial class editBootleDialog : Window
    {
        private editBootleClass selectedBootle;
        private string p;

        public editBootleDialog(String ID)
        {
            p = ID;
            InitializeComponent();
            dt.CanUserAddRows = false;
            dt.CanUserDeleteRows = false;
            dt.IsReadOnly = true;
            dt.ItemsSource = DataBase.Instance.editBootle(p);
        }

        
        private void Add(object sender, RoutedEventArgs e)
        {
            InputBootleDialog dialog = new InputBootleDialog(int.Parse(p));
            if (dialog.ShowDialog() == true)
            {
                MessageBox.Show("Przypisano butle do klienta");
                dt.ItemsSource = DataBase.Instance.editBootle(p);
            }
            else
            {
                MessageBox.Show("Nieznany bład, spróbuj za chwilę");
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            selectedBootle = (editBootleClass)dt.SelectedItem;
            if (selectedBootle != null)
            {
                if (dt.SelectedCells.Count == 2)
                {
                    if (DataBase.Instance.call_deleteBootle(selectedBootle.ID))
                    {
                        dt.ItemsSource = DataBase.Instance.editBootle(p);
                        MessageBox.Show("Usunięto butlę.");
                    }
                    else
                        MessageBox.Show("Nie można usunąć statusu butli. Prawdopodobnie butle jest w trakcie napełniania.");

                }
                else
                {
                    MessageBox.Show("Zaznacz tylko jeden wiersz");
                }

            }
            else
            {
                MessageBox.Show("Zaznacz wiersz");
            }
        }
    }
}
