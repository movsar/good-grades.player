using System.Windows;
using System.Windows.Controls;

namespace Content_Manager.UserControls.MaterialControls
{
    /// <summary>
    /// Interaction logic for TaskMaterialControl.xaml
    /// </summary>
    public partial class TaskMaterialControl : UserControl
    {
        public TaskMaterialControl()
        {
            InitializeComponent();

            var fillingTaskType = new ComboBoxItem()
            {
                Content = "Заполнение пробелов"
            };

            var selectingTaskType = new ComboBoxItem()
            {
                Content = "Выбор правильного"
            };

            var testingTaskType = new ComboBoxItem()
            {
                Content = "Тест"
            };

            var matchingTaskType = new ComboBoxItem()
            {
                Content = "Сопоставление"
            };

            var buildingTaskMaterial = new ComboBoxItem()
            {
                Content = "Построение выражений"
            };

            cmbTaskType.Items.Add(fillingTaskType);
            cmbTaskType.Items.Add(selectingTaskType);
            cmbTaskType.Items.Add(testingTaskType);
            cmbTaskType.Items.Add(matchingTaskType);
            cmbTaskType.Items.Add(buildingTaskMaterial);
        }

        private void btnSetData_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbTaskType_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void cmbTaskType_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void cmbTaskType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
