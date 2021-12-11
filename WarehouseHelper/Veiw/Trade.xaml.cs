using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WarehouseHelper.VeiwModel;

namespace WarehouseHelper.Veiw
{
    /// <summary>
    /// Interaction logic for Trade.xaml
    /// </summary>
    public partial class Trade : UserControl
    {
        public Trade()
        {
            InitializeComponent();
            DataContext = new TradeVeiwModel();
        }
    }
}
