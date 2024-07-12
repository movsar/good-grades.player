using GGManager.Stores;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GGManager.Services;
using System.IO;
using System.Reflection;
using System.Windows.Media;

namespace GGManager.UserControls
{
    public partial class DbInformation : UserControl
    {
        private readonly ContentStore _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        public event Action Saved;
        private byte[]? _backgroundImage = null;
        public DbInformation()
        {
            InitializeComponent();
            DataContext = this;

            var dbMeta = _contentStore.DbContext.DbMetas.First();
            txtDbName.Text = dbMeta.Title;
            txtDbCreatedAt.Text = dbMeta.CreatedAt.ToString("R");
            txtDescription.Text = dbMeta.Description;
            txtAppVersion.Text = dbMeta.AppVersion;
            txtFilePath.Text = _contentStore.DbContext.Database.GetDbConnection().DataSource;
            _backgroundImage = dbMeta.BackgroundImage;

            if (_backgroundImage?.Length > 0)
            {
                btnChooseBackground.Background = Brushes.LightGreen;
            }
        }
        private void btnChooseBackground_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FileService.SelectImageFilePath();
            if (string.IsNullOrEmpty(filePath)) return;

            // Read, load contents to the object and add to collection
            var content = File.ReadAllBytes(filePath);
            if (content.Length == 0) return;

            _backgroundImage = content;

            btnChooseBackground.Background = Brushes.LightYellow;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Save()
        {
            var newName = txtDbName.Text;
            var newDescription = txtDescription.Text;

            var dbMeta = _contentStore.DbContext.DbMetas.First();
            dbMeta.Title = newName;
            dbMeta.Description = newDescription;
            dbMeta.BackgroundImage = _backgroundImage;
            
            _contentStore.DbContext.SaveChanges();

            if (_backgroundImage?.Length > 0)
            {
                btnChooseBackground.Background = Brushes.LightGreen;
            }
            
            Saved?.Invoke();
        }

        private void txtDbName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Save();
            }
        }

        private void txtDescription_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Save();
            }
        }

        private void txtDbCreatedAt_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Save();
            }
        }

        private void txtAppVersion_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Save();
            }
        }
    }
}
