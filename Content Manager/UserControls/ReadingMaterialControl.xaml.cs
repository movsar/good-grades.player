﻿using Content_Manager.Stores;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Content_Manager.UserControls
{
    /// <summary>
    /// Interaction logic for ReadingMaterialControl.xaml
    /// </summary>
    public partial class ReadingMaterialControl : UserControl
    {
        ContentStore _contentStore { get; }
        public ReadingMaterial Material { get; }
        public ReadingMaterialControl(ContentStore contentStore)
        {
            InitializeComponent();
            DataContext = this;
            _contentStore = contentStore;
        }
        public ReadingMaterialControl(ContentStore contentStore, ReadingMaterial material)
        {
            base(contentStore);
            Material = material;
        }


    }
}
