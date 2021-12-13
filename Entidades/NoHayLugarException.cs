using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class NoHayLugarException : Exception
    {
        public NoHayLugarException() 
            : base("No hay lugar para mas planetas")
        {

        }

    }
}
