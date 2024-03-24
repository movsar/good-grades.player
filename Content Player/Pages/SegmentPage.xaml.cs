﻿using Content_Player.Models;
using Data.Entities;
using Data.Interfaces;
using Shared.Services;
using Shared.Viewers;
using System.Windows.Controls;
using System.Windows.Input;

namespace Content_Player.Pages
{
    public partial class SegmentPage : Page
    {
        public SegmentPage(Segment segment)
        {
            InitializeComponent();
            DataContext = this;

            RtfService.LoadRtfFromText(rtbDescription, segment.Description);
            Title = segment.Title;

            Materials.AddRange(segment.Materials.Cast<IMaterial>());
            Materials.Add(new FakeSegmentMaterial()
            {
                Id = "tasks",
                Title = "Хаарш зер"
            });

            _segment = segment;
        }
        private readonly Segment _segment;

        public string SegmentTitle { get; }
        public List<IMaterial> Materials { get; } = new List<IMaterial>();
     

        private void OnListViewItemSelected()
        {
            if (lvMaterials.SelectedItem == null)
            {
                return;
            }

            var segmentItem = (IMaterial)lvMaterials.SelectedItem;

            switch (segmentItem)
            {
                case Material:
                    var listeningMaterial = segmentItem as Material;
                    var listeningPresenter = new ListeningViewer(listeningMaterial.Title, listeningMaterial.Text, listeningMaterial.Image, listeningMaterial.Audio);
                    listeningPresenter.ShowDialog();

                    break;
                case FakeSegmentMaterial:
                    var assignments = GetAllAssignments();
                    NavigationService.Navigate(new AssignmentsPage(assignments));
                    break;
            }
        }

        private List<IAssignment> GetAllAssignments()
        {
            List<IAssignment> allAssignments = _segment!.MatchingTasks.Cast<IAssignment>().ToList();
            allAssignments.AddRange(_segment!.FillingTasks);
            allAssignments.AddRange(_segment!.BuildingTasks);
            allAssignments.AddRange(_segment!.TestingTasks);
            allAssignments.AddRange(_segment!.SelectingTasks);

            return allAssignments;
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
