using Content_Manager.Stores;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            var dbMeta = _contentStore.GetDbMeta();
            txtDbName.Text = dbMeta.Title;
            txtDbCreatedAt.Text = dbMeta.CreatedAt.ToString("R");
            txtDescription.Text = dbMeta.Description;
            txtAppVersion.Text = dbMeta.AppVersion;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var newName = txtDbName.Text;
            var newDescription = txtDescription.Text;

            _contentStore.SaveDbMeta(newName, newDescription);
            Saved?.Invoke();
        }

    }
}
