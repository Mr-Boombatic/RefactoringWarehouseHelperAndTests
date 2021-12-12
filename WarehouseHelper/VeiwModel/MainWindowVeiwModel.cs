using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WarehouseHelper;
using WarehouseHelper.Veiw;

namespace WarehouseHelper.VeiwModel
{   
    class MainWindowVeiwModel : INotifyPropertyChanged
    {
        public UserControl selectedwarehouse;
        public UserControl SelectedWarehouse
        {
            get { return selectedwarehouse; }
            set
            {
                selectedwarehouse = value;
                OnPropertyChanged("SelectedWarehouse");
            }
        }

        DbContextOptions<StoneСompanyContext> Options { get; set; } = new DbContextOptionsBuilder<StoneСompanyContext>()
            .UseSqlServer(@"Server=DESKTOP-GJHHNV9\SQLEXPRESS;Database=StoneСompany;Trusted_Connection=True;")
            .Options;

        private RelayCommand selectedVeiw;
        public RelayCommand SelectStoneWarehouseCommand
        {
            get
            {
                return (selectedVeiw = new RelayCommand(obj =>
                {
                    SelectedWarehouse = new StoneWarehouse(new StoneСompanyContext(Options));
                }));
            }
        }
        public RelayCommand SelectSlabWarehouseCommand
        {
            get
            {
                return (selectedVeiw = new RelayCommand(obj =>
                {
                    SelectedWarehouse = new SlabWearehouse(new StoneСompanyContext(Options));
                }));
            }
        }

        public RelayCommand SelectProсessingSlabWarehouseCommand
        {
            get
            {
                return (selectedVeiw = new RelayCommand(obj =>
                {
                    using (var context = new StoneСompanyContext(Options))
                    {
                        SelectedWarehouse = new ProcessingSlabWarehouse(context);
                    }
                }));
            }
        }

        //public RelayCommand SelectProductWarehouseCommand
        //{
        //    get
        //    {
        //        return (selectedVeiw = new RelayCommand(obj =>
        //        {
        //            SelectedWarehouse = new Veiw.Product();
        //        }));
        //    }
        //}

        //public RelayCommand SellCommand
        //{
        //    get
        //    {
        //        return (selectedVeiw = new RelayCommand(obj =>
        //        {
        //            SelectedWarehouse = new Trade();
        //        }));
        //    }
        //}

        public MainWindowVeiwModel()
        {
            SelectedWarehouse = new StoneWarehouse(new StoneСompanyContext(Options));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
