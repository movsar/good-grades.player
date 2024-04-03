using Content_Manager.Interfaces;
using Content_Manager.Stores;
using Content_Manager.UserControls;
using Data;
using Data.Entities;
using Data.Entities.TaskItems;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Translations;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Content_Manager.Windows.Editors
{
    public partial class FillingTaskEditor : Window, ITaskEditor
    {
        private FillingAssignment _taskAssignment;
        public IAssignment Assignment => _taskAssignment;
        private ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();

        public FillingTaskEditor(FillingAssignment? taskEntity = null)
        {
            InitializeComponent();
            DataContext = this;

            _taskAssignment = taskEntity ?? new FillingAssignment()
            {
                Title = txtTitle.Text
            };
            txtTitle.Text = _taskAssignment.Title;

            RedrawUi();
        }

        public void RedrawUi()
        {
            spItems.Children.Clear();
            foreach (var item in _taskAssignment.Items)
            {
                var existingQuizItemControl = new AssignmentItemEditControl(TaskType.Filling, item);
                //existingQuizItemControl.Removed += Item_Delete;

                spItems.Children.Add(existingQuizItemControl);
            }

            var newItemControl = new AssignmentItemEditControl(TaskType.Filling);
            spItems.Children.Add(newItemControl);
        }
        private void ValidateInput()
        {
            //switch (_taskType)
            //{
            //    case TaskType.Filling:
            //        var gapOpeners = Regex.Matches(Item.Text, @"\{");
            //        var gapClosers = Regex.Matches(Item.Text, @"\}");
            //        var gappedWords = Regex.Matches(Item.Text, @"\{\W*\w+.*?\}");

            //        if (gapOpeners.Count != gapClosers.Count || gapOpeners.Count != gappedWords.Count)
            //        {
            //            throw new Exception(Ru.ExceptionUncorrectFormate);
            //        }

            //        if (gappedWords.Count == 0)
            //        {
            //            throw new Exception(Ru.ExceptionMinWords);
            //        }

            //        break;
            //}
        }
        private void Item_Create(IEntityBase entity)
        {
            var itemEntity = (AssignmentItem)entity;

            // Add the Task entity
            var taskState = ContentStore.DbContext.Entry(_taskAssignment).State;
            if (taskState == EntityState.Detached || taskState == EntityState.Added)
            {
                ContentStore.SelectedSegment!.FillingTasks.Add(_taskAssignment);
            }

            // Add the Task item entity
            _taskAssignment.Items.Add(itemEntity);

            ContentStore.DbContext.SaveChanges();
            RedrawUi();
        }

        private void Item_Delete(string id)
        {
            var itemToRemove = _taskAssignment.Items.First(i => i.Id == id);
            _taskAssignment.Items.Remove(itemToRemove);
            ContentStore.DbContext.SaveChanges();

            RedrawUi();
        }
    //       try
    //        {
    //            ValidateInput();
    //}
    //        catch (Exception ex)
    //        {
    //            ExceptionService.HandleError(ex, ex.Message);
    //            return;
    //        }
private void txtTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_taskAssignment != null)
            {
                _taskAssignment.Title = txtTitle.Text;
                ContentStore.DbContext.SaveChanges();
            }
        }


    }
}
