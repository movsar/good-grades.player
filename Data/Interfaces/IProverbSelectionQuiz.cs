﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IProverbSelectionQuiz : IModelBase
    {
        public string CorrectQuizId { get; set; }
    }
}
