using Data.Entities;
using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services {
    internal static class Utils {
        internal static void SyncLists<TEntity, TModel>(IList<TEntity> entities, List<TModel> models)
            where TEntity : IEntityBase, new()
            where TModel : IModelBase, new() {

            // This method is used to synchronize list of items changed on a model, with that of an entity
            // It checks whether something was removed, updated or added, if added, it also sets items' ids

            if (models == null || entities == null) {
                return;
            }

            // Existing elemenet ids
            var currentItemIds = models.Select(x => x.Id).ToList();

            // Models that have been removed
            var itemsToRemove = entities.Where(rm => !currentItemIds.Contains(rm.Id));

            // Remove corresponding to removed models entities
            foreach (var option in itemsToRemove) {
                entities.Remove(option);
            }

            // Add or update
            foreach (var item in models) {
                var existingItem = entities.FirstOrDefault((rm => rm.Id == item.Id));
                if (existingItem != null) {
                    existingItem.SetFromModel(item);
                } else {
                    var newItem = new TEntity();
                    newItem.SetFromModel(item);
                    entities?.Add(newItem);

                    // Set item id
                    item.Id = newItem.Id;
                }
            }
        }

    }
}
