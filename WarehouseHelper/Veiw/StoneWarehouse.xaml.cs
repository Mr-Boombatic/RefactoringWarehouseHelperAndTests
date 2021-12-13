

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WarehouseHelper.VeiwModel;

namespace WarehouseHelper.Veiw
{
    /// <summary>
    /// Логика взаимодействия для StoneWarehouse.xaml
    /// </summary>
    public partial class StoneWarehouse : UserControl
    {
        public StoneWarehouse(StoneСompanyContext context)
        {
            InitializeComponent();
            DataContext = new StoneViewModel(context);
        }

        private static bool IsTextAllowed(string text)
        {
            return !regex.IsMatch(text);
        }

        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void CostPerCar_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private static readonly Regex regex = new Regex("^[a-zA-Z]+$");

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
    }
}
