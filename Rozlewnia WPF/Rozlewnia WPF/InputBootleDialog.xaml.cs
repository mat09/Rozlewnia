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
    /// Interaction logic for InputBootleDialog.xaml
    /// </summary>
    public partial class InputBootleDialog : Window
    {
        private int id_client;
        public InputBootleDialog(int i)
        {
            id_client = i;
            InitializeComponent();
        }

        private void attachBootle_Click(object sender, RoutedEventArgs e)
        {
            String str = Filter.sql_number(ID);
            if( str.Length==6 ){
                if (DataBase.Instance.call_attachBootle(str, id_client))
                {
                    MessageBoxResult res = MessageBox.Show("Odebrać butle do napełnienia?", "Komunikat", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (DataBase.Instance.call_statusBootle(str, 1))
                        {
                            MessageBox.Show("Odebrano butle.");
                        }
                        else
                            MessageBox.Show("Wystapił bład. Nie możesz ustawić podanego statusu butli.");
                        this.DialogResult = true;
                           
                    }
                    else
                    this.DialogResult = true;
                }
                else
                {
                    msg.Visibility = Visibility.Visible;
                    msg.Content = "Podane ID istnieje w bazie";
                }
            }
            else{
                msg.Visibility = Visibility.Visible;
                msg.Content = "Wpisane ID jest w złym formacie";
            }
        }

        public String ID
        {
            get
            {
                return IdTextBox.Text;
            }
        }
    }
}
