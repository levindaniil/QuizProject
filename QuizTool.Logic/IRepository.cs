﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizTool.Logic
{
    public interface IRepository<T>
    {
        IEnumerable<T> Data { get; }
        IEnumerable<T> FindAll(Predicate<T> predicate);
    }
}
