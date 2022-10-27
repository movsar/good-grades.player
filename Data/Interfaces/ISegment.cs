using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ISegment : IModelBase
    {
        string Title { get; set; }
        string Description { get; set; }
        ICelebrityWordsQuiz CelebrityWordsQuiz { get; set; }
    }
}
