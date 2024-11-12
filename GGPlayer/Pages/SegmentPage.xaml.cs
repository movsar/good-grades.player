using GGPlayer.Models;
using Data.Entities;
using Data.Interfaces;
using Shared.Services;
using System.Windows.Controls;
using System.Windows.Input;
using Shared.Controls;
using GGPlayer.Services;

namespace GGPlayer.Pages
{
    public partial class SegmentPage : Page
    {
        private readonly ShellNavigationService _navigationService;
        private readonly AssignmentsPage _assignmentsPage;
        private readonly MaterialViewerPage _materialViewerPage;
        private Segment? _segment;
        public List<IMaterial> Materials { get; } = new List<IMaterial>();

        public string? SegmentTitle => _segment?.Title;

        public SegmentPage(ShellNavigationService navigationService,
            AssignmentsPage assignmentsPage,
            MaterialViewerPage materialViewerPage)
        {
            InitializeComponent();
            DataContext = this;

            _navigationService = navigationService;
            _assignmentsPage = assignmentsPage;
            _materialViewerPage = materialViewerPage;
        }

        public void LoadSegment(Segment segment)
        {
            _segment = segment;

            RtfService.LoadRtfFromText(rtbDescription, segment.Description);
            tbSegmentTitle.Text = segment.Title;
            if (segment == null)
            {
                throw new ArgumentNullException(nameof(segment), "Segment cannot be null");
            }

            _segment = segment;


            // Заполнение списка материалов
            Materials.Clear();
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

        private List<IAssignment> GetAllAssignments()
        {
            var allAssignments = new List<IAssignment>();

            if (_segment.MatchingAssignments != null)
            {
                allAssignments.AddRange(_segment.MatchingAssignments);
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
                _assignmentsPage.LoadAssignments(assignments);
                _navigationService.NavigateTo(_assignmentsPage);
            }
            else if (segmentItem is Material material)
            {
                _materialViewerPage.LoadMaterial(material);
                _navigationService.NavigateTo(_materialViewerPage);
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
