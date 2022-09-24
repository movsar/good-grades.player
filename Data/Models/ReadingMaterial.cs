using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class ReadingMaterial : ModelBase, IReadingMaterial
    {
        public string Content { get; set; }
        public string Title { get; set; }
    }
}
