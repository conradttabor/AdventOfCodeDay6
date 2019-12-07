using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCodeDay6
{
    class Program
    {        
        static void Main(string[] args)
        {
            var inputs = GetInputs("../../../Input.txt");
            var mapEngine = new UniversalOrbitalMapEngine(inputs);
            mapEngine.MapOrbits();
        }

        public static List<Tuple<string, string>> GetInputs(string filePath)
        {
            var inputs = new List<Tuple<string, string>>();
            Stream stream = new FileStream(filePath, FileMode.Open);
            StreamReader reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var split = line.Split(')');
                inputs.Add(new Tuple<string, string>(split[0], split[1].TrimEnd(',')));
            }
            return inputs;
        }
    }
}