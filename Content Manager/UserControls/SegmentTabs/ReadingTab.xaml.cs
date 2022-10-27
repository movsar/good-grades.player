﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Content_Manager.Stores;
using Content_Manager.Interfaces;
using Data.Interfaces;

namespace Content_Manager.UserControls.SegmentTabs
{
    public partial class ReadingTab : UserControl, ISegmentTabControl
    {
        private readonly ContentStore _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        public ReadingTab()
        {
            InitializeComponent();
            DataContext = this;
            RedrawUi();
        }

        public void RedrawUi()
        {
            spReadingMaterialControls.Children.Clear();

            if (_contentStore.SelectedSegment!.ReadingMaterials == null) return;

            foreach (var material in _contentStore.SelectedSegment!.ReadingMaterials)
            {
                var rmcExisting = new ReadingMaterialControl(material);
                rmcExisting.Save += ReadingMaterialControl_Save;
                rmcExisting.Delete += ReadingMaterialControl_Delete;

                spReadingMaterialControls.Children.Add(rmcExisting);
            }

            var rmcNew = new ReadingMaterialControl();
            rmcNew.Save += ReadingMaterialControl_Save;

            spReadingMaterialControls.Children.Add(rmcNew);
        }

        private void ReadingMaterialControl_Delete(string id)
        {
            _contentStore.SelectedSegment!.ReadingMaterials.Remove(_contentStore.GetReadingMaterialById(id));
            _contentStore.SaveCurrentSegment();
            RedrawUi();
        }

        private void ReadingMaterialControl_Save(string? id, IModelBase model)
        {
            _contentStore.SaveCurrentSegment();
            RedrawUi();
        }
    }
}