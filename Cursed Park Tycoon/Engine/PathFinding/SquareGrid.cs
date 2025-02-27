using System.Collections.Generic;

namespace Sandbox.Engine.PathFinding
{
    public class SquareGrid : IWeightedGraph<Location>
    {
        public static readonly Location[] Directions = new[]
        {
            new Location(0, 1),     //  Up
            new Location(0, -1),    //  Down
            new Location(-1, 0),    //  Left
            new Location(1, 0),     //  Right
        };
        public int Rows { get; set; }
        public int Columns { get; set; }

        public HashSet<Location> walls = new HashSet<Location>();

        public SquareGrid(int rows, int columns)
        {
            Columns = columns;
            Rows = rows;
        }
        public bool InBounds(Location id)
        {
            return 0 <= id.X && id.X < Columns &&
                   0 <= id.Y && id.Y < Rows;
        }
        public bool Passable(Location id)
        {
            return !walls.Contains(id);
        }
        public double Cost(Location a, Location b)
        {
            //  In this grid, all cells have a cost of 1
            return 1;
        }
        public IEnumerable<Location> PassableNeighbors(Location id)
        {
            foreach (Location direction in Directions)
            {
                Location next = new Location(id.X + direction.X, id.Y + direction.Y);

                if (InBounds(next) && Passable(next))
                {
                    yield return next;
                }
            }
        }
    }
}
