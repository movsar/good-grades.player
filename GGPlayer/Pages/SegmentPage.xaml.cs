using GGPlayer.Models;
using Data.Entities;
using Data.Interfaces;
using Shared.Services;
using Shared.Viewers;
using System.Windows.Controls;
using System.Windows.Input;
namespace GGPlayer.Pages
{
    public partial class SegmentPage : Page
    {
        private readonly Segment _segment;
        private ShellWindow _shell;
        public string SegmentTitle => _segment.Title;

        public SegmentPage(ShellWindow shell, Segment segment)
        {
            InitializeComponent();
            DataContext = this;

            _shell = shell;

            RtfService.LoadRtfFromText(rtbDescription, segment.Description);
            Title = segment.Title;
            if (segment == null)
            {
                throw new ArgumentNullException(nameof(segment), "Segment cannot be null");
            }

            _segment = segment;


            // Заполнение списка материалов
            if (segment.Materials != null)
            {
                Materials.AddRange(segment.Materials.Cast<IMaterial>());
            }

            // Проверка наличия заданий и добавление кнопки
            var assignments = GetAllAssignments();
            if (assignments != null && assignments.Count > 0)
            {
                // Add a dummy separator
                Materials.Add(new FakeSegmentMaterial()
                {
                    Title = ""
                });

                var fakeSegmentMaterial = new FakeSegmentMaterial()
                {
                    Id = "tasks",
                    Title = "Хаарш зер"
                };
                Materials.Add(fakeSegmentMaterial);
            }
        }

        public List<IMaterial> Materials { get; } = new List<IMaterial>();

        private List<IAssignment> GetAllAssignments()
        {
            var allAssignments = new List<IAssignment>();

            if (_segment.MatchingAssignments != null)
            {
                allAssignments.AddRange(_segment.MatchingAssignments.Cast<IAssignment>());
            }
            if (_segment.FillingAssignments != null)
            {
                allAssignments.AddRange(_segment.FillingAssignments);
            }
            if (_segment.BuildingAssignments != null)
            {
                allAssignments.AddRange(_segment.BuildingAssignments);
            }
            if (_segment.TestingAssignments != null)
            {
                allAssignments.AddRange(_segment.TestingAssignments);
            }
            if (_segment.SelectionAssignments != null)
            {
                allAssignments.AddRange(_segment.SelectionAssignments);
            }

            return allAssignments;
        }

        private void OnListViewItemSelected()
        {
            if (lvMaterials.SelectedItem == null)
            {
                return;
            }

            var segmentItem = (IMaterial)lvMaterials.SelectedItem;

            if (segmentItem is FakeSegmentMaterial && segmentItem.Id == "tasks")
            {
                var assignments = GetAllAssignments();
                _shell.CurrentFrame.Navigate(new AssignmentsPage(_shell, assignments));
            }
            else if (segmentItem is Material material)
            {
                var materialPresenter = new MaterialViewer(material.Title, material.PdfData, material.Audio);
                materialPresenter.ShowDialog();
            }
        }

        private void lvMaterialsItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OnListViewItemSelected();
        }

        private void lvMaterials_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                OnListViewItemSelected();
            }
        }
    }
}
