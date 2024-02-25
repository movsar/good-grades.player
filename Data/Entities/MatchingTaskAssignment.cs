using Data.Entities.TaskItems;
using Data.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    /*
     * This one allows to set an image and a text for each item, then it could be used for 
     * 2 types of tasks, matching image to text and text to image     
     */
    public class MatchingTaskAssignment : IEntityBase, IAssignment
    {
        [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.Now;
        /******************************************************************/
        public string Title { get; set; }
        public IList<AssignmentItem> Items { get; }
        public bool IsContentSet => Items.Count() > 0;
    }
}
