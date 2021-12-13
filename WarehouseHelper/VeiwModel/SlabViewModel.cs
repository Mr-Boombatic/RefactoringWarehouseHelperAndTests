using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;


namespace WarehouseHelper.VeiwModel
{
    public class SlabVeiwModel : INotifyPropertyChanged
    {
        StoneСompanyContext db;
        public ArrayList PreviewWarehouseSlabs { get; set; } = new ArrayList();
        public ObservableCollection<Slab> Slabs { get; set; } = new ObservableCollection<Slab>();
        public ObservableCollection<Stone> WarehouseStones { get; set; } = new ObservableCollection<Stone>();
        public Worker Worker { get; set; } = new Worker();
        public Stone SelectedStone { get; set; }

        private RelayCommand addSlabsCommand;
        public RelayCommand AddSlabsCommand
        {
            get
            {
                return addSlabsCommand ?? (addSlabsCommand = new RelayCommand(obj =>
                {
                    // adding to warehouse
                    int totalCount = 0;
                    var stone = db.Stones.Where(s => s.StoneId == SelectedStone.StoneId).First();
                    stone.IsSawn = true;
                    double overallVolume = 0;
                    string exception = null;

                    if (!string.IsNullOrEmpty(exception = Worker.IsValid()))
                    {
                        MessageBox.Show(exception, "Распил", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // forming resulted slabs
                    List<Slab> slabs = new List<Slab>();
                    foreach (var slab in Slabs)
                    {
                        for (int i = 1; i <= slab.Count; i++)
                        {
                            totalCount++;
                            slabs.Add(new Slab()
                            {
                                SlabId = SelectedStone.StoneId + "/" + totalCount,
                                Height = slab.Height,
                                Width = slab.Width,
                                Length = slab.Length,
                                Shift = slab.Shift,
                                Worker = Worker.Name,
                                Date = Worker.Date
                            });
                        }

                        overallVolume += slab.Width * slab.Length * slab.Height * slab.Count;
                    }

                    if (!string.IsNullOrEmpty((exception = overallVolume.IsValidOverallVolume(stone.Length * stone.Width * stone.Height))))
                    {
                        MessageBox.Show(exception, "Распил", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // saving resulted slabs and reloading DataGrid
                    Slabs.Clear();
                    WarehouseStones.Remove(stone);
                    db.Slabs.AddRange(slabs);
                    db.SaveChanges();
                    db.Stones.Load();
                }));
            }
        }

        public SlabVeiwModel(StoneСompanyContext context)
        {
            db = context;
            db.Stones.Load();
            db.Slabs.Load();

            /* Отображение содержимого склада камней */
            WarehouseStones = new ObservableCollection<Stone>(db.Stones.Local.Where(s => !s.IsSawn));

            foreach (var slab in db.Slabs)
            {
                /* type of slab (stone) */
                var stone = (from m in db.Stones.Local.ToArray()
                            where (m.StoneId.ToString() == slab.SlabId.Split(new char[] { '/' })[0])
                            select m).First();

                /* Отображение содержимого склада слэбов */
                if (slab.Processing == null)
                    PreviewWarehouseSlabs.Add(new
                    {
                        Date = slab.Date,
                        Shift = slab.Shift ? "День" : "Ночь",
                        Worker = slab.Worker,
                        StoneId = slab.SlabId,
                        Width = slab.Width,
                        Height = slab.Height,
                        Length = slab.Length,
                        Square = slab.Width * slab.Length,
                        Type = stone.Type
                    });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public static class ExtensionsDouble
    {
        public static string IsValidOverallVolume(this double overralVolume, double stoneVolume)
        {
            string exception = null;
            if (overralVolume < 0)
                exception += "Обьем для слэбов не может быть отрицательным!\n";
            if (stoneVolume < 0)
                exception += "Обьем для камня не может быть отрицательным!\n";
            if (overralVolume > stoneVolume)
                exception += "Обьем для слэбов не может быть больше обьема камня!\n";

            return exception;
        }
    }

    public static class ExtensionsWorker
    {
        public static string IsValid(this Worker worker)
        {
            string exception = null;
            if (worker.Date == null)
                exception += "Не указана дата!\n";
            if (string.IsNullOrEmpty(worker.Name))
                exception += "Не указано имя рабочего!\n";
            //if (worker.Shift == null || string.IsNullOrEmpty(worker.Shift.Text))
               // exception += "Не указана смена рабочего!\n";

            return exception;
        }
    }

    public class Worker
    {
        public DateTime? Date { get; set; } = DateTime.Now.Date;
        public bool? Shift { get; set; }
        public string Name { get; set; }
    }

    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var e in enumerable)
            {
                action(e);
            }
        }
    }
}