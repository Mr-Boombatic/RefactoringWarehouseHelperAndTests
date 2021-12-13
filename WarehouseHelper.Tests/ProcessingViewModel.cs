using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarehouseHelper.VeiwModel;

namespace WarehouseHelper.Tests
{
    [TestClass]
    public class ProcessingViewModelTests
    {
        SqliteStoneCompanyContextTest context { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            context = new SqliteStoneCompanyContextTest(ViewModel.ProcessingViewModel);
            context.Slabs.ToList();
            // generate data for database by StoneViewModel
            //string relativePath = @"Database\DataForTestingProcessing.db";
            //var parentdir = Directory.GetCurrentDirectory();
            //string myString = parentdir.Replace("WarehouseHelper.Tests\\bin\\Debug\\net6.0-windows", "");
            //string absolutePath = Path.Combine(myString, relativePath);
            //string connectionString = string.Format("Data Source={0}", absolutePath);
            //context = new SqliteStoneCompanyContextTest(connectionString);
            //context.Slabs.Load();
        }

        [TestMethod]
        public void IsValid_EmptyWorkerShift_ShownModalDialog()
        {
            // Arrange
            var viewModel = new SlabVeiwModel(context);

            // Act
            var textOfModalDialog = (new Worker()
                { Date = DateTime.Now, Name = "test", Shift = null }).IsValid();
            Assert.AreEqual("Не указана смена рабочего!\n", textOfModalDialog);
        }
    }
}
