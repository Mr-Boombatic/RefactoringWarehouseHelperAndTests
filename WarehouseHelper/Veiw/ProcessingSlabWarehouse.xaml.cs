using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for ProductWarehouse.xaml
    /// </summary>
    public partial class ProcessingSlabWarehouse : UserControl
    {
        public ProcessingSlabWarehouse(StoneСompanyContext context)
        {
            InitializeComponent();
            this.DataContext = new ProcessingVeiwModel(context);
        }
    }
}
