using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sandbox.Engine.PathFinding
{
    public class AstarSearch
    {
        //  The most recent calculated path.
        public List<Location> _calculatedPath;
        public Dictionary<Location, Location> CameFrom = new Dictionary<Location, Location>();
        public Dictionary<Location, double> CostSoFar = new Dictionary<Location, double>();
        public ReadOnlyCollection<Location> Path { get; }
        public Location Start { get; private set; }
        public Location Goal { get; private set; }
        public IWeightedGraph<Location> Graph { get; private set; }
        public AstarSearch(IWeightedGraph<Location> graph)
        {
            _calculatedPath = new List<Location>();
            Path = _calculatedPath.AsReadOnly();
            Graph = graph;
        }
        public void CalculatePath(Location start, Location goal)
        {
            //  Clear any previous calculated path.
            _calculatedPath.Clear();

            //  Set the start and goal locations.
            Start = start;
            Goal = goal;

            //  Calculate using A*
            PriorityQueue<Location> frontier = new PriorityQueue<Location>();
            frontier.Enqueue(Start, 0);

            CameFrom[Start] = Start;
            CostSoFar[Start] = 0;

            while (frontier.Count > 0)
            {
                var currentLocation = frontier.Dequeue();

                if (currentLocation.Equals(Goal))
                {
                    break;
                }

                foreach (Location nextLocation in Graph.PassableNeighbors(currentLocation))
                {
                    double newCost = CostSoFar[currentLocation] + Graph.Cost(currentLocation, nextLocation);
                    if (!CostSoFar.ContainsKey(nextLocation) || newCost < CostSoFar[nextLocation])
                    {
                        CostSoFar[nextLocation] = newCost;
                        double priority = newCost + Heuristic(nextLocation, Goal);
                        frontier.Enqueue(nextLocation, priority);
                        CameFrom[nextLocation] = currentLocation;
                    }
                }
            }

            //  Now that we've finished the A* stuff, create the list that contains
            //  each location from start to goal of the shortest path. We have to start
            //  from the goal position and traverse backwards from there.
            Location location = Goal;
            while (location != Start)
            {
                _calculatedPath.Add(location);
                location = CameFrom[location];
            }
            _calculatedPath.Add(Start);

            //  We have to reverse here since we had to traverse from goal to start
            _calculatedPath.Reverse();
        }

        //  Note: a generic version of A* would be abstract over Location and
        //  also Heuristic
        private static double Heuristic(Location a, Location b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

    }
}
