using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WarehouseHelper.VeiwModel
{
    public class ProcessingVeiwModel
    {
        StoneСompanyContext db;
        public ArrayList ProcessingSlabs { get; set; } = new ArrayList();
        public ArrayList Slabs { get; set; } = new ArrayList();
        public ObservableCollection<string> SelectedSlabs { get; set; } = new ObservableCollection<string>();
        public string SelectedSlab
        {
            set 
            {
                if (!SelectedSlabs.Contains(value))
                    SelectedSlabs.Add(value);
            }
        }

        private RelayCommand processSlabsCommand;
        public RelayCommand ProcessSlabsCommand
        {
            get
            {
                return processSlabsCommand ?? (processSlabsCommand = new RelayCommand(obj =>
                {
                    string selectedWayOfProcessing = null;
                    foreach (var wayOfProcessing in (obj as UIElementCollection))
                    {
                        var radioButton = (RadioButton)wayOfProcessing;
                        if (radioButton.IsChecked == true)
                            selectedWayOfProcessing = radioButton.Content.ToString();
                    }

                    foreach (var slabId in SelectedSlabs)
                    {
                        var selectedSlab = (from slab in db.Slabs
                                    where (slab.SlabId == slabId)
                                    select slab).First();

                        selectedSlab.Processing = selectedWayOfProcessing;
                    }

                    db.SaveChanges();
                }));
            }
        }

        public ProcessingVeiwModel(StoneСompanyContext context)
        {
            db = context;
            db.Slabs.Load();
            db.Stones.Load();

            foreach (var slab in db.Slabs.Local.ToArray())
            {
                var slabStone = (from stone in db.Stones.Local.ToArray()
                            where (stone.StoneId.ToString() == slab.SlabId.Split(new char[] { '/' })[0])
                            select stone).First();

                if (slab.Processing != null)
                {
                    ProcessingSlabs.Add(new
                    {
                        Type = slabStone.Type,
                        Length = slab.Length,
                        Width = slab.Width,
                        Height = slab.Height,
                        Shift = slab.Shift ? "День" : "Ночь",
                        SlabId = slab.SlabId,
                        StoneId = slabStone.StoneId,
                        Worker = slab.Worker,
                        Processing = slab.Processing,
                        Square = slab.Length * slab.Width,
                        Date = slab.Date,
                        Perimeter = slab.Length * slab.Width * 2 + slab.Length * slab.Height * 2 + slab.Width * slab.Height * 2
                    });
                }
                else
                {
                    Slabs.Add(new
                    {
                        Type = slabStone.Type,
                        Length = slab.Length,
                        Width = slab.Width,
                        Height = slab.Height,
                        SlabId = slab.SlabId,
                        Shift = slab.Shift ? "День" : "Ночь",
                        Worker = slab.Worker,
                        Date = slab.Date
                    });
                }
            }
        }
    }
}

