using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Shared.Controls
{
    public partial class BuildingItemViewControl : UserControl
    {
        public BuildingItemViewControl(string phrase)
        {
            InitializeComponent();
            DataContext = this;

            foreach (string word in phrase.Split(" ").OrderBy(w => Guid.NewGuid()))
            {
                var btnWord = new Button()
                {
                    Content = word,
                    //Style = (Style)FindResource("ButtonStyle")
                };
                spItems.Children.Add(btnWord);
            }
        }
    }
}
