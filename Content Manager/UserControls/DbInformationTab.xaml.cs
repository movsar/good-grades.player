using Content_Manager.Stores;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        ContentStore ContentStore => App.AppHost!.Services.GetRequiredService<ContentStore>();
        private readonly DbMeta _dbMeta;
        public DbInformation()
        {
            InitializeComponent();
            DataContext = this;

            _dbMeta = ContentStore.GetDbMeta();
            txtDbName.Text = _dbMeta.Title;
            txtDbCreatedAt.Text = _dbMeta.CreatedAt.ToString("R");
            txtDescription.Text = _dbMeta.Description;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var newName = txtDbName.Text;
            var newDescription = txtDescription.Text;

            ContentStore.SaveDbMeta(newName, newDescription);
        }

    }
}
