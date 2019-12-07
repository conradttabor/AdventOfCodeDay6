using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCodeDay6
{
    public class UniversalOrbitalMapEngine
    {
        public List<Tuple<string, string>> MasterInputs = new List<Tuple<string, string>>();
        public int TotalDistance { get; set; }
        public List<Orbit> Orbits = new List<Orbit>();
        public int DistanceOfTheLowestCommonNode { get; set; }
        public int SANDistanceFromCOM { get; set; }
        public int YOUDistanceFromCOM { get; set; }

        public UniversalOrbitalMapEngine(List<Tuple<string, string>> inputs)
        {
            MasterInputs = inputs;
        }

        public void MapOrbits()
        {
            
            var com = GetCOM(MasterInputs);

            Orbits.Add(new Orbit()
            {
                BasePlanet = com.Item1,
                OrbitingPlanet = com.Item2,
                DistanceFromCOM = 0
            });

            var previousOrbit = Orbits.First();
            var currentPlanet = previousOrbit.OrbitingPlanet;

            CalculateOrbits(currentPlanet, 0);

            Console.WriteLine($"Sum of all orbits: {TotalDistance}");
            Console.WriteLine($"Distance form you to Santa: {CalculateDistance()}");
            Console.ReadLine();
        }        

        private int CalculateDistance()
        {
            return YOUDistanceFromCOM + SANDistanceFromCOM - (2 * DistanceOfTheLowestCommonNode);
        }

        private Tuple<string, string> GetCOM(List<Tuple<string, string>> inputs)
        {
            return inputs.First(x => x.Item1 == "COM");
        }

        public string CalculateOrbits(string currentPlanet, int currentCount)
        {
            var childOrbits = MasterInputs.Where(x => x.Item1.Equals(currentPlanet)).ToList();
            TotalDistance += ++currentCount;
            string path = null;

            foreach (var childOrbit in childOrbits)
            {
                AddChildOrbits(currentPlanet, childOrbit, currentCount);

                path = DeterminePath(childOrbit, currentCount, path);
            }

            if (childOrbits.Count == 0)
            {
                return AddChildlessOrbits(currentPlanet, currentCount);
            }

            return path;
        }

        private string DeterminePath(Tuple<string, string> childOrbit, int currentCount, string currentPath)
        {
            var tempResponse = CalculateOrbits(childOrbit.Item2, currentCount);

            if (IsLowestCommonNode(tempResponse, currentPath))
                DistanceOfTheLowestCommonNode = currentCount;

            return tempResponse ?? currentPath;
        }

        private bool IsLowestCommonNode(string tempResponse, string isSanOrYouPath)
        {
            return (!String.IsNullOrEmpty(tempResponse) && !String.IsNullOrEmpty(isSanOrYouPath))
                && ((tempResponse.Equals("YOU") && isSanOrYouPath.Equals("SAN"))
                || (tempResponse.Equals("SAN") && isSanOrYouPath.Equals("YOU")));
        }

        private void AddChildOrbits(string currentPlanet, Tuple<string, string> childOrbit, int currentCount)
        {
            var orbit = new Orbit()
            {
                BasePlanet = currentPlanet,
                OrbitingPlanet = childOrbit.Item2,
                DistanceFromCOM = currentCount
            };
            Orbits.Add(orbit);
        }

        private string AddChildlessOrbits(string currentPlanet, int currentCount)
        {
            Orbits.Add(new Orbit()
            {
                BasePlanet = currentPlanet,
                OrbitingPlanet = "",
                DistanceFromCOM = currentCount
            });
            if (currentPlanet.Equals("YOU"))
            {
                // You need the distance to COM from the planet YOU is orbiting, not YOU's distance.
                YOUDistanceFromCOM = currentCount - 1;
                return "YOU";
            }
            else if (currentPlanet.Equals("SAN"))
            {
                // You need the distance to COM from the planet SAN is orbiting, not SAN's distance.
                SANDistanceFromCOM = currentCount - 1;
                return "SAN";
            }
            else return null;
        }
    }
}
