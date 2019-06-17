using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace MyQCleaningRobot
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Please specify the input and output files");
                return;
            }

            string inputFileName = args[0];

            if (!inputFileName.EndsWith(".json")) {
                Console.WriteLine("Input file should be a json file");
                return;
            }

            string outputFileName = args[1];
            if (!outputFileName.EndsWith(".json"))
            {
                Console.WriteLine("Output file should be a json file");
                return;
            }

            string[] inputLines = File.ReadAllLines(inputFileName);
            List<string> mapLines = new List<string>();
            int mapX = 0;
            int mapY = 0;
            bool startProcessMap = false;
            string startLine = "";
            string commandsLine = "";
            string batteryLine = "";

            foreach (string line in inputLines) {
                string escapedString = line.Replace("\"", "").Replace(":", "").Replace(" ", "");

                if (line.Contains("map")) {
                    startProcessMap = true;
                }

                if (line.Contains("start")) {
                    startProcessMap = false;
                    startLine = line.Replace("\"", "").Replace(" ", "").Remove(0,7).TrimEnd(new char[] { ',', '}' });
                }
                if (line.Contains("commands"))
                {
                    commandsLine = escapedString.Remove(0,9).TrimEnd(new char[] {',', ']'});
                }
                if (line.Contains("battery"))
                {
                    batteryLine = escapedString.Remove(0,7);
                }

                if (startProcessMap)
                {
                    string lineToProcess = escapedString.TrimEnd(',');
                    if (!lineToProcess.StartsWith("map") && lineToProcess.ToString() != "]")
                    {
                        lineToProcess = lineToProcess.Remove(0, 1).TrimEnd(new char[] { ',', ']' });

                        mapLines.Add(lineToProcess);
                        int possibleY = lineToProcess.Split(",").Length;
                        if (possibleY > mapY) {
                            mapY = possibleY;
                        }
                        mapX++;
                    }
                    
                }
            }

            //Create map
            string[,] map = new string[mapX, mapY];
            for (int i = 0; i < mapLines.Count; i++) {
                string[] xLine = mapLines[i].Split(",");
                for (int v = 0; v < xLine.Length; v++){
                    map[i, v] = xLine[v];
                }
            }

            //Create start
            string[] position = startLine.Split(",");
            int x = 0;
            int y = 0;
            string facing = "N";
            for (int i = 0; i < position.Length; i++)
            {
                if (position[i].StartsWith("X")) {
                    Int32.TryParse(position[i].Remove(0,2), out x);
                }
                if (position[i].StartsWith("Y")) {
                    Int32.TryParse(position[i].Remove(0, 2), out y);
                }
                if (position[i].StartsWith("facing")) {
                    facing = position[i].Remove(0,7);
                }
            }

            Location start = new Location(x, y, facing);

            List<Instruction> commands = new List<Instruction>();

            string[] shortCommands = commandsLine.Split(",");
            for (int i = 0; i < shortCommands.Length; i++)
            {
                commands.Add(new Instruction(shortCommands[i]));
            }
            
            int battery = 0;
            Int32.TryParse(batteryLine, out battery);

            Environment environment = new Environment(map, start, battery, commands);
            environment.Run();
            string report = environment.GetFinalReport();
            File.WriteAllText(outputFileName, report);
        }
    }
}
