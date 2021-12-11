using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WarehouseHelper.VeiwModel
{
    class TradeVeiwModel
    {
        StoneСompanyContext db;
        public ObservableCollection<PreviewProduct> ProductSold { get; set; } = new ObservableCollection<PreviewProduct>();
        public ObservableCollection<Sale> Sales { get; set; }
        public ObservableCollection<PreviewProduct> RemainingInStock { get; set; } = new ObservableCollection<PreviewProduct>();
        public Sale AddedSale { get; set; } = new Sale();
        public PreviewProduct SelectedProduct { get; set; }

        private RelayCommand addForSaleCommand;
        public RelayCommand AddForSaleCommand
        {
            get
            {
                return addForSaleCommand ?? (addForSaleCommand = new RelayCommand(obj =>
                {
                    ProductSold.Add(SelectedProduct);
                    RemainingInStock.Remove(SelectedProduct);
                }));
            }
        }
        private RelayCommand removeFromSaleCommand;
        public RelayCommand RemoveFromSaleCommand
        {
            get
            {
                return removeFromSaleCommand ?? (removeFromSaleCommand = new RelayCommand(obj =>
                {
                    RemainingInStock.Add(SelectedProduct);
                    ProductSold.Remove(SelectedProduct);
                }));
            }
        }

        private RelayCommand sellProductsCommand;
        public RelayCommand SellProductsCommand
        {
            get
            {
                return sellProductsCommand ?? (sellProductsCommand = new RelayCommand(obj =>
                {
                    AddedSale.Date = DateTime.Now.Date;
                    foreach (var product in ProductSold)
                    {
                        var changeProduct = db.Products.Where(p => p.SlabId == product.SlabId).Select(p => p).First();
                        changeProduct.ContractNumber = AddedSale.ContractNumber;
                        changeProduct.Sale = AddedSale;
                        AddedSale.Products.Add(changeProduct);
                        AddedSale.Cost += changeProduct.Cost;
                    }
                    db.Sales.Add(AddedSale);
                    db.SaveChanges();
                }));
            }
        }

        public TradeVeiwModel()
        {
            //db = new StoneСompanyContext();
            db.Products.Load();
            db.Sales.Load();
            Sales = db.Sales.Local.ToObservableCollection();

            foreach (var product in db.Products)
                if (product.ContractNumber == null)
                RemainingInStock.Add(new PreviewProduct()
                {
                    Name = product.Name,
                    SlabId = product.SlabId,
                    Volume = product.Width * product.Length * product.Height,
                    Square = product.Width * product.Length * 2 + 2 * product.Length * product.Height+ 2 * product.Height,
                    Cost = product.Cost
                });
        }
    }

    class PreviewProduct
    {
        public string Name { get; set; }
        public string SlabId { get; set; }
        public double Volume { get; set; }
        public double Square { get; set; }
        public decimal Cost { get; set; }
    }
}
