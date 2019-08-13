using System;
using System.Collections.Generic;
using System.Text;

namespace TechTest.Core.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> Get();
    }
}
