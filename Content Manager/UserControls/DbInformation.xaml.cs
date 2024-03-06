using Content_Manager.Stores;
using Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Content_Manager.UserControls
{
    public partial class DbInformation : UserControl
    {
        private readonly ContentStore _contentStore = App.AppHost!.Services.GetRequiredService<ContentStore>();
        public event Action Saved;
        public DbInformation()
        {
            InitializeComponent();
            DataContext = this;

            var dbMeta = _contentStore.DbContext.DbMetas.First();
            txtDbName.Text = dbMeta.Title;
            txtDbCreatedAt.Text = dbMeta.CreatedAt.ToString("R");
            txtDescription.Text = dbMeta.Description;
            txtAppVersion.Text = dbMeta.AppVersion;
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
            _contentStore.DbContext.SaveChanges();

            Saved?.Invoke();
        }

        private void txtDbName_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
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
