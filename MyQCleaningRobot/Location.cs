using System;
using System.Collections.Generic;
using System.Text;

namespace MyQCleaningRobot
{
    public class Location
    {
        public enum Direction { N, E, S, W }
        public enum Turn { R, L }

        private Direction Facing;
        private int X;
        private int Y;

        public Location() { }
        
        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Location(int x, int y, string facing) {
            X = x;
            Y = y;
            Facing = (Direction)Enum.Parse(typeof(Direction), facing);
        }

        public Direction GetFacing()
        {
            return Facing;
        }

        public int GetX()
        {
            return X;
        }

        public int GetY()
        {
            return Y;
        }

        public void SetFacing(Direction direction) {
            Facing = direction;
        }

        public void SetX(int x) {
            X = x;
        }

        public void SetY(int y) {
            Y = y;
        }

        public void ChangeFacing(Turn turn) {
            Direction newDirection = Direction.N;

            if (Equals(Turn.R, turn))
            {
                switch (Facing)
                {
                    case Direction.N:
                        newDirection = Direction.E;
                        break;
                    case Direction.E:
                        newDirection = Direction.S;
                        break;
                    case Direction.S:
                        newDirection = Direction.W;
                        break;
                }
            }
            else
            {
                switch (Facing)
                {
                    case Direction.N:
                        newDirection = Direction.W;
                        break;
                    case Direction.S:
                        newDirection = Direction.E;
                        break;
                    case Direction.W:
                        newDirection = Direction.S;
                        break;
                }
            }

            SetFacing(newDirection);
        }

        public string PrintCoordinates() {
            return string.Format("{{ \"X\" : {0}, \"Y\" : {1} }}", X, Y);
        }

        public override string ToString()
        {
            return string.Format("{{ \"X\" : {0}, \"Y\" : {1}, \"facing\" : \"{2}\"}}", X, Y, Facing);
        }
    }

    public class SortLocation : IComparer<Location>
    {
        public int Compare(Location a, Location b) {

            if (a.GetX() > b.GetX())
            {
                return 1;
            }
            else
            {
                if (a.GetX() < b.GetX())
                {
                    return -1;
                }
                else {
                    if (a.GetY() > b.GetY())
                    {
                        return 1;
                    } else if (a.GetY() < b.GetY())
                    {
                        return -1;
                    }
                }
            }

            return 0;
        }
    }
}
