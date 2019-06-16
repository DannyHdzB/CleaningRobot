using System;
using System.Collections.Generic;
using System.Text;

namespace MyQCleaningRobot
{
    public class Map
    {
        string[,] Setup;

        public Map(string[,] map) {
            Setup = map;
        }

        public int GetXLength() {
            return Setup.GetLength(0);
        }

        public int GetYLength() {
            return Setup.GetLength(1);
        }

        public bool ValidPosition(int x, int y) {
            if (x >= 0 && y >= 0)
            {
                if (GetXLength() > x && GetYLength() > y && Setup[y,x] == "S") {
                    return true;
                }
            }
            return false;
        }
    }
}
