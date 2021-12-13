using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseHelper.VeiwModel;

namespace WarehouseHelper.Tests
{
    public enum ViewModel
    {
        StoneViewModel,
        SlabViewModel,
        ProcessingViewModel,
        ProductViewModel,
        TradeViewModel,
        None
    }

    public class SqliteStoneCompanyContextTest : StoneСompanyContext, IDisposable
    {
        public SqliteStoneCompanyContextTest(ViewModel viewModel = ViewModel.None)
            : base(
                new DbContextOptionsBuilder<StoneСompanyContext>()
                    .UseSqlite(CreateInMemoryDatabase())
                    .Options)
        {
            Seed(viewModel);
        }

        private void Seed(ViewModel viewModel)
        {
            using (var context = new StoneСompanyContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                if (viewModel == ViewModel.None)
                    return;

                // generate data for database by StoneViewModel
                var stoneViewModel = new StoneViewModel(context);
                stoneViewModel.Car = new Car { CarId = 0, Date = DateTime.Today, Cost = 100 };
                var mediator = new ManagerMediator(stoneViewModel.Car); // to recalculaction cost for every stone
                var stone = new OrderedStone(mediator)
                {
                    Height = 10,
                    Width = 10,
                    Length = 10,
                    PricePerCube = 1
                };

                for (int i = 0; i < 3; i++)
                    stoneViewModel.Order.Add((OrderedStone)stone.Clone());

                stoneViewModel.AddStonesToWarehouseCommand.Execute(null);
                context.SaveChanges();

                if (viewModel == ViewModel.StoneViewModel)
                    return;

                // generate data for database by SlabViewModel
                var slabViewModel = new SlabVeiwModel(context);
                slabViewModel.Worker = new Worker()
                { Date = DateTime.Now, Name = "test", Shift = true };

                var slab = new Slab()
                {
                    Height = 4,
                    Width = 4,
                    Length = 4,
                    Date = slabViewModel.Worker.Date,
                    Shift = false,
                    Count = 2,
                    Worker = "test"
                };
                for (int i = 0; i < 3; i++)
                {
                    slabViewModel.SelectedStone = context.Stones.ToList()[i];
                    slabViewModel.Slabs.Add((Slab)slab.Clone());
                    slabViewModel.AddSlabsCommand.Execute(null);
                }

                context.SaveChanges();

                if (viewModel == ViewModel.SlabViewModel)
                    return;

                context.Slabs.SingleOrDefault(b => b.SlabId == "1/1").Processing = "Термо";
                context.Slabs.SingleOrDefault(b => b.SlabId == "1/2").Processing = "Термо";
                context.Slabs.SingleOrDefault(b => b.SlabId == "2/1").Processing = "Полирвока";
                context.Slabs.SingleOrDefault(b => b.SlabId == "2/2").Processing = "Полирвока";

                if (viewModel == ViewModel.StoneViewModel)
                    return;

                // generate data for database by productViewModel

                var productViewModel = new ProductViewModel(context);
                //context.SaveChanges();

                //if (viewModel == ViewModel.SlabViewModel)
                //    return;

                //context.Slabs.SingleOrDefault(b => b.SlabId == "1/1").Processing = "Термо";
                //context.Slabs.SingleOrDefault(b => b.SlabId == "1/2").Processing = "Термо";
                //context.Slabs.SingleOrDefault(b => b.SlabId == "2/1").Processing = "Полирвока";
                //context.Slabs.SingleOrDefault(b => b.SlabId == "2/2").Processing = "Полирвока";
                context.SaveChanges();
            }
        }

        private readonly DbConnection _connection;

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();


            return connection;
        }

        public void Dispose() => _connection.Dispose();
    }
}
