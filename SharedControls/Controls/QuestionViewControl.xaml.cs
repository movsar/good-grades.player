using Data.Entities.TaskItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Shared.Controls
{
    public partial class QuestionViewControl : UserControl
    {
        public Question Question { get; }
        public List<string> SelectedOptionIds => GetUserSelections();

        private List<string> GetUserSelections()
        {
            var selections = new List<string>();

            foreach (var item in spOptions.Children)
            {
                if (item is CheckBox)
                {
                    var checkboxOption = item as CheckBox;
                    if (checkboxOption!.IsChecked == true)
                    {
                        selections.Add(checkboxOption.Tag.ToString()!);
                    }
                }

                if (item is RadioButton)
                {
                    var radioButtonOption = item as RadioButton;
                    if (radioButtonOption!.IsChecked == true)
                    {
                        selections.Add(radioButtonOption.Tag.ToString()!);
                    }
                }
            }

            return selections;
        }

        public QuestionViewControl(Question question)
        {
            Question = question;

            // Initialize UI
            InitializeComponent();
            DataContext = this;

            // Add options UI
            var isMultichoice = Question.Options.Where(o => o.IsChecked).Count() > 1;
            spOptions.Children.Clear();
            foreach (var option in Question.Options)
            {
                if (isMultichoice)
                {
                    spOptions.Children.Add(GenerateCheckboxOptionView(option));
                }
                else
                {
                    spOptions.Children.Add(GenerateRadioButtonOptionView(option, Question.Id));
                }
            }
        }

        private RadioButton GenerateRadioButtonOptionView(AssignmentItem option, string groupName)
        {
            var radioButton = new RadioButton()
            {
                Tag = option.Id,
                GroupName = groupName,
                Content = option.Text,
                Style = (Style)FindResource("RadioOptionStyle"),
            };

            return radioButton;
        }
        private CheckBox GenerateCheckboxOptionView(AssignmentItem option)
        {
            var checkbox = new CheckBox()
            {
                Tag = option.Id,
                Content = option.Text,
                Style = (Style)FindResource("CheckboxOptionStyle"),
            };

            return checkbox;
        }
    }
}
