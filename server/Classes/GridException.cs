using System;

namespace Server.Classes
{
    class GridException : Exception 
    {
        public GridException(string message) : base(message){}
    }
}
