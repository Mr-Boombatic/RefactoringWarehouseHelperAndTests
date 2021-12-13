using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseHelper.VeiwModel;

namespace WarehouseHelper.Tests
{
    [TestClass]
    public class TradeViewModelTests
    {
        SqliteStoneCompanyContextTest context { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            context = new SqliteStoneCompanyContextTest(ViewModel.ProcessingViewModel);
            var productViewModel = new ProductViewModel(context);
            Product product;
            productViewModel.AddedProduct = product = new Product()
            {
                Worker = "",
                SlabId = context.Slabs.ToList()[0].SlabId,
                Length = 20,
                Height = 20,
                Width = 20,
                Name = "",
                Date = DateTime.Now,
                ContractNumber = null,
                Cost = 10000
            };
            productViewModel.AddProductCommand.Execute(context.Slabs.ToList()[0]);
        }

        [TestMethod]
        public void AddForSaleCommand_SelectedProductForSale_AddToSalesGridView()
        {
            var tradeViewModel = new WarehouseHelper.VeiwModel.TradeVeiwModel(context);
            var product = context.Products.ToList()[0];
            tradeViewModel.SelectedProduct = new PreviewProduct() {
                Name = product.Name,
                SlabId = product.SlabId,
                Volume = product.Height * product.Width * product.Length,
                Cost = product.Cost,
                Square = product.Width * product.Length * 2 + 2 * product.Length * product.Height + 2 * product.Height
            };
            tradeViewModel.AddForSaleCommand.Execute(null);

            Assert.AreEqual(tradeViewModel.ProductSold[0].SlabId, product.SlabId);
        }
    }
}
