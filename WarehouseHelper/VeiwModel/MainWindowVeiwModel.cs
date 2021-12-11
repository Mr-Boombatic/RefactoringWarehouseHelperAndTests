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

        private RelayCommand selectWarehouse;
        public RelayCommand SelectStoneWarehouseCommand
        {
            get
            {
                return (selectWarehouse = new RelayCommand(obj =>
                {
                    using (var context = new StoneСompanyContext(Options))
                    {
                        SelectedWarehouse = new StoneWarehouse(context);
                    };
                }));
            }
        }
        public RelayCommand SelectSlabWarehouseCommand
        {
            get
            {
                return (selectWarehouse = new RelayCommand(obj =>
                {
                    using (var context = new StoneСompanyContext(Options))
                    {
                        SelectedWarehouse = new SlabWearehouse(context);
                    }
                }));
            }
        }

        public RelayCommand SelectProсessingSlabWarehouseCommand
        {
            get
            {
                return (selectWarehouse = new RelayCommand(obj =>
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
        //        return (selectWarehouse = new RelayCommand(obj =>
        //        {
        //            SelectedWarehouse = new Veiw.Product();
        //        }));
        //    }
        //}

        //public RelayCommand SellCommand
        //{
        //    get
        //    {
        //        return (selectWarehouse = new RelayCommand(obj =>
        //        {
        //            SelectedWarehouse = new Trade();
        //        }));
        //    }
        //}

        DbContextOptions<StoneСompanyContext> Options { get; set; } = new DbContextOptionsBuilder<StoneСompanyContext>()
            .UseSqlServer(@"Server=DESKTOP-8UCPGM6;Database=StoneСompany;Trusted_Connection=True;")
            .Options;

        public MainWindowVeiwModel()
        {
            using (var context = new StoneСompanyContext(Options)) 
            {
                SelectedWarehouse = new StoneWarehouse(context);
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
