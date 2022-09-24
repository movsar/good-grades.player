using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{

    public class Segment : ModelBase, ISegment
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<ReadingMaterial> ReadingMaterials { get; set; }
    }
}
