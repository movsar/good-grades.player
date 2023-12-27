﻿using Data.Entities.Materials.QuizItems;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Shared.Controls
{
    public partial class CwqChallenge : UserControl
    {
        public enum State { Empty, Success, Failure }
        public CwqChallenge(TextAndImageQuizItem option)
        {
            InitializeComponent();

            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.StreamSource = new MemoryStream(option.Image);
            logo.EndInit();

            imgMain.Source = logo;

        }
    }
}
