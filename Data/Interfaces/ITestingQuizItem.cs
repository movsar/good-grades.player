using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ITestingQuizItem : IModelBase
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public IList<string> Options { get; set; }
    }
}