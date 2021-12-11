﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Data;

namespace WarehouseHelper.VeiwModel
{
    public class StoneVeiwModel
    {
        StoneСompanyContext db { get; set; }
        public ArrayList StoneWarehouse { get; set; } = new ArrayList();
        public ObservableCollection<Order> Orders { get; set; } = new ObservableCollection<Order>();
        public Order SelectedOrder { get; set; }
        public Car Car { get; set; }
        public decimal CarCost
        {
            get { return Car.Cost; }
            set
            {
                Car.Cost = value;
                Mediator.Recount();
            }
        }

        Mediator Mediator;


        private RelayCommand addStonesToWarehouseCommand;
        public RelayCommand AddStonesToWarehouseCommand
        {
            get
            {
                return addStonesToWarehouseCommand ?? (addStonesToWarehouseCommand = new RelayCommand(obj =>
                {
                    db.Cars.Add(Car);

                    foreach (var order in Orders)
                    {
                        db.Stones.Add(new Stone()
                        {
                            Length = order.Length,
                            Height = order.Height,
                            Width = order.Width,
                            Type = order.Type,
                            PricePerCube = order.PricePerCube,
                            Car = Car
                        });
                    }

                    db.SaveChanges();
                }));
            }
        }

        private RelayCommand changeCountRowCommand;
        public RelayCommand AddRowCommand
        {
            get
            {
                return (changeCountRowCommand = new RelayCommand(obj =>
                {
                    Orders.Add(new Order(Mediator));
                    Mediator.Recount();
                }));
            }
        }

        public RelayCommand DeleteRowCommand
        {
            get
            {
                return (changeCountRowCommand = new RelayCommand(obj =>
                {
                    Orders.Remove(SelectedOrder);
                    Mediator.Recount();
                }));
            }
        }

        public StoneVeiwModel(StoneСompanyContext context)
        {
            db = context;
            db.Stones.Load();
            db.Cars.Load();


            foreach (var stone in db.Stones.Local)
            {
                StoneWarehouse.Add(new
                {
                    StoneId = stone.StoneId,
                    Volume = stone.Length * stone.Height * stone.Width,
                    Square = stone.Length * stone.Width * 2 + stone.Length * stone.Height * 2 + stone.Width * stone.Height * 2,
                    StoneCost = stone.PricePerCube * (decimal)(stone.Length * stone.Height * stone.Width),
                    Perimeter = (stone.Length + stone.Width) * 2 + (stone.Length + stone.Height) * 2 + (stone.Width + stone.Height) * 2
                });
            }

            Car = db.Cars.Count() != 0 ? new Car() { CarId = db.Cars.OrderBy(c => c).Last().CarId + 1, Date = DateTime.Now.Date, Cost = 0 }
                : new Car() { CarId = 1, Date = DateTime.Now.Date, Cost = 0 };
            Mediator = new ManagerMediator(Car);
        }
    }

    public partial class Order : Colleague, INotifyPropertyChanged, ICloneable, IDataErrorInfo
    {

        private float width;
        public float Width
        {
            get { return width; }
            set
            {
                width = value;
                ChangeStoneVolume();
            }
        }

        private float length;
        public float Length
        {
            get { return length; }
            set
            {
                length = value;
                ChangeStoneVolume();
            }
        }

        private float height;
        public float Height
        {
            get { return height; }
            set
            {
                height = value;
                ChangeStoneVolume();
            }
        }

        private float volume;
        public float Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                Recount();
                OnPropertyChanged("Volume");
            }
        }

        private decimal cost;
        public decimal Cost
        {
            get { return cost; }
            set
            {
                cost = value;
                OnPropertyChanged("Cost");
            }
        }

        public decimal pricePerCube;
        public decimal PricePerCube
        {
            get { return pricePerCube; }
            set
            {
                pricePerCube = value;
                Recount();
            }
        }
        public string Type { get; set; }
        public int StoneId { get; set; }

        private void ChangeStoneVolume()
        {
            if (Length != 0 && Height != 0 && Width != 0)
                Volume = Length * Height * Width;
        }

        public Order(Mediator mediator) : base(mediator)
        {
            mediator.Add(this);
        }

        public object Clone()
        {
            Order order = new Order(this.mediator)
            {
                Height = this.Height,
                width = this.Width,
                Length = this.Length,
                PricePerCube = this.PricePerCube
            };
            return order;
        }

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Cost":
                        if ((Cost < 0) || (Cost > 100))
                        {
                            error = "Возраст должен быть больше 0 и меньше 100";
                        }
                        break;
                }
                return error;
            }
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    abstract public class Colleague
    {
        protected Mediator mediator;

        public Colleague(Mediator mediator)
        {
            this.mediator = mediator;
        }

        public virtual void Recount(decimal carCost = 0)
        {
            mediator.Recount();
        }
    }

    abstract public class Mediator
    {
        public abstract void Recount();
        public abstract void Add(Order ordere);
    }

    public class ManagerMediator : Mediator
    {
        private readonly List<Order> OrderedStones = new List<Order>();
        private Car Car;
        public override void Recount()
        {
            float overallVolume = OrderedStones.Select(stone => stone.Length * stone.Width * stone.Height).Sum();

            foreach (var stone in OrderedStones)
            {
                try
                {
                    stone.Cost = (decimal) (stone.Volume / overallVolume) * Car.Cost
                                 + (decimal) (stone.Length * stone.Width * stone.Height) * stone.PricePerCube;
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Value was either too large or too small for a Decimal.")
                        stone.Cost = 0;
                }
            }
        }

        public override void Add(Order orderedStone)
        {
            OrderedStones.Add(orderedStone);
        }

        public ManagerMediator(Car car)
        {
            this.Car = car;
        }
    }
}

