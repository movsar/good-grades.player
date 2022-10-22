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

namespace Shared.Controls
{
    /// <summary>
    /// Interaction logic for DropBox.xaml
    /// </summary>
    public partial class CwqOptionBox : UserControl
    {
        public string WordsCollection
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("WordsCollection", typeof(string), typeof(CwqOptionBox), new PropertyMetadata(string.Empty));
        
        public CwqOptionBox()
        {
            DataContext = this;
            InitializeComponent();
        }

        public CwqOptionBox(QuizItem option)
        {
            DataContext = this;
            InitializeComponent();
            WordsCollection = option.Text;
        }
    }
}
