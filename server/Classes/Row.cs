using System;
using System.Collections.Generic;

namespace Server.Classes
{
    [Serializable]
    public class Row
    {
        public List<Cell> Cells { get; set; }

        public Row(int cellCount)
        {
            Cells = new List<Cell>();

            for (int i = 0; i < cellCount; i++)
                Cells.Add(new Cell());
        }
    }
}
