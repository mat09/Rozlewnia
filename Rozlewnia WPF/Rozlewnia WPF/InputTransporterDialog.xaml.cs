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
    public partial class InputTransporterDialog : Window
    {
        public InputTransporterDialog()
        {
            InitializeComponent();
        }

        private void AddTransporter_Click(object sender, RoutedEventArgs e)
        {

        }

        public String MyName
        {
            get
            {
                return name.Text;
            }
        }
        public String City
        {
            get
            {
                return city.Text;
            }
        }
        public String Street
        {
            get
            {
                return street.Text;
            }
        }
        public String Postcode
        {
            get
            {
                return post_code_1.Text + "-" + post_code_2.Text;
            }
        }
        public String Flat_number
        {
            get
            {
                return flat_number.Text;
            }
        }
        public String House_number
        {
            get
            {
                return house_number.Text;
            }
        }
        public String Phone_number
        {
            get
            {
                return phone.Text;
            }
        }
    }
}
