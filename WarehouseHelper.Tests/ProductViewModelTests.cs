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
    public class ProductViewModelTests
    {
        SqliteStoneCompanyContextTest context { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            context = new SqliteStoneCompanyContextTest(ViewModel.ProcessingViewModel);
        }

        [TestCleanup]
        public void Cleanup()
        {
            //context.Dispose();
        }

        // Intagration tests
        [TestMethod]
        public void AddProductCommand_SuitableAllowedSlab_SavedFinishedGoods()
        {
            var productViewModel = new ProductViewModel(context);
            Product product;
            productViewModel.AddedProduct = product = new Product()
            {
                Worker = "test",
                SlabId = context.Slabs.ToList()[0].SlabId,
                Length = 20,
                Height = 20,
                Width = 20,
                Name = "test",
                Date = DateTime.Now,
                ContractNumber = null,
                Cost = 10000
            };
            productViewModel.AddProductCommand.Execute(context.Slabs.ToList()[0]);
            Assert.AreEqual(product.SlabId, context.Products.ToList()[0].SlabId);
        }

        [TestMethod]
        public void AddProductCommand_SuitableAllowedSlab_NotSavedFinishedGoods()
        {
            var productViewModel = new ProductViewModel(context);
            Product product;
            productViewModel.AddedProduct = product = new Product()
            {
                Worker = "test",
                SlabId = context.Slabs.ToList()[0].SlabId,
                Length = 20,
                Height = 20,
                Width = 20,
                Name = "test",
                Date = DateTime.Now,
                ContractNumber = null,
                Cost = 10000
            };
            productViewModel.AddProductCommand.Execute(context.Slabs.ToList()[0]);
            productViewModel.AddProductCommand.Execute(context.Slabs.ToList()[0]);
            Assert.AreEqual(product.SlabId, context.Products.ToList()[0].SlabId);
            Assert.AreEqual(1, context.Products.ToList().Where(x => x.SlabId == product.SlabId).Count());
        }

        [TestMethod]
        public void AddProductCommand_WrongWorkerAndNameProduct_NotSavedFinishedGoods()
        {
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
            Assert.AreNotEqual(product.SlabId, context.Products.ToList()[0].SlabId);
        }
    }
}
