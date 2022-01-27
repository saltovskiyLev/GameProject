using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Defenc
{
    public interface IAction
    {
        string Name { get; }
        void Act();
    }
}
