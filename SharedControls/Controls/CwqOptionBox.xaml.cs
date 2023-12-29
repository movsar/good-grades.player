using Data.Entities.Materials.TaskItems;
using System.Windows;
using System.Windows.Controls;

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

        public CwqOptionBox(TextItemEntity option)
        {
            DataContext = this;
            InitializeComponent();
            WordsCollection = option.Text;
        }
    }
}
