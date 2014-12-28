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
   public partial class InputTakeBootleDialog : Window
    {
        private String surname;
        public String Surname
        {
            get { return surname; }
            set { surname = value; }
        }
        private String name;
        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        private String id;
        public String ID
        {
            get { return id; }
            set { id = value; }
        }

        searchBootleClass selectedBootle;

          public InputTakeBootleDialog()
        {
            InitializeComponent();
            this.DataContext = this;
            resultDataGrid.CanUserAddRows = false;
            resultDataGrid.CanUserDeleteRows = false;
            resultDataGrid.IsReadOnly = true;
            
            resultDataGrid.ItemsSource = DataBase.Instance.searchBootle(Name,Surname,ID,0);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //MessageBox.Show("Name:"+Name+" , Surname="+Surname+" , ID=" + ID);
            resultDataGrid.ItemsSource = DataBase.Instance.searchBootle(Name, Surname, ID,0);
        }

        private void selectBootle_Click(object sender, RoutedEventArgs e)
        {
            selectedBootle =(searchBootleClass) resultDataGrid.SelectedItem;
            if (selectedBootle != null )
            {
                if (resultDataGrid.SelectedCells.Count == 3)
                {
                    if(DataBase.Instance.call_statusBootle(selectedBootle.ID.ToString(),1))
                    {
                        this.DialogResult = true;
                        MessageBox.Show("Odebrano butle do napełnienia.");
                    }
                    else
                        MessageBox.Show("Nie można zmienić statusu butli.");
                    
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
