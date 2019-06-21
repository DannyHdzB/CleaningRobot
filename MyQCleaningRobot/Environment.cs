using System;
using System.Collections.Generic;
using System.Text;

namespace MyQCleaningRobot
{
    public class Environment
    {
        private Map Map;
        private Robot Robot;
        private List<Instruction> CommandsToExecute;
        private List<List<Instruction>> CommandsForBackOff;
        private int BackOffIteration;

        private List<Location> VisitedCells;
        private List<Location> CleanedCells;        
        
        public Environment(string[,] map, Location start, int battery, List<Instruction> commands)
        {
            Map = new Map(map);
            Robot = new Robot(start, battery);
            CommandsToExecute = commands;
            CommandsForBackOff = new List<List<Instruction>>() {
                new List<Instruction>(){
                    new Instruction("Turn Right", "TR", 1, true),
                    new Instruction("Advance", "A", 2, true)
                },
                new List<Instruction>(){
                    new Instruction("Turn Left", "TL", 1, true),
                    new Instruction("Back", "B", 3, true),
                    new Instruction("Turn Right", "TR", 1, true),
                    new Instruction("Advance", "A", 2, true)
                },
                new List<Instruction>(){
                    new Instruction("Turn Left", "TL", 1, true),
                    new Instruction("Turn Left", "TL", 1, true),
                    new Instruction("Advance", "A", 2, true)
                },
                new List<Instruction>(){
                    new Instruction("Turn Right", "TR", 1, true),
                    new Instruction("Back", "B", 3, true),
                    new Instruction("Turn Right", "TR", 1, true),
                    new Instruction("Advance", "A", 2, true)
                },
                new List<Instruction>(){
                    new Instruction("Turn Left", "TL", 1, true),
                    new Instruction("Turn Left", "TL", 1, true),
                    new Instruction("Advance", "A", 2, true)
                }
            };
            BackOffIteration = 0;
            VisitedCells = new List<Location>();
            CleanedCells = new List<Location>();
        }

        public void Run() {
            VisitedCells.Add(Robot.GetPosition());

            while (CommandsToExecute.Count > 0 && Robot.GetBattery() > 0 && Equals(Robot.GetStatus(), Robot.State.On))
            {
                Instruction command = CommandsToExecute[0];
                CommandsToExecute.RemoveAt(0);
                MoveRobot(command);
            }
        }

        private void MoveRobot(Instruction command) {
            if (Robot.HaveBattery(command.GetCost())) {
                Robot.SetBattery(Robot.GetBattery() - command.GetCost());

                Location newPosition = GenerateNextPosition(command);
                if (Map.ValidPosition(newPosition.GetX(), newPosition.GetY()))
                {
                    Robot.SetPosition(newPosition);

                    if (command.GetShortName() == "A") {
                        BackOffIteration = 0;
                        AddCellInArray(VisitedCells, newPosition);
                    }

                    if (command.GetShortName() == "C")
                    {
                        AddCellInArray(CleanedCells, newPosition);
                    }
                }
                else
                {
                    if (BackOffIteration == CommandsForBackOff.Count)
                    {
                        Robot.SetStatus(Robot.State.Stuck);
                    }
                    else
                    {
                        CommandsToExecute.RemoveAll(i => i.GetIsBackOffInstruccion());

                        for (int i = 0; i < CommandsForBackOff[BackOffIteration].Count; i++)
                        {
                            CommandsToExecute.Insert(i, CommandsForBackOff[BackOffIteration][i]);
                        }

                        BackOffIteration++;
                    }
                }
            }
            else
            {
                Robot.SetStatus(Robot.State.Off);
            }
        }

        private Location GenerateNextPosition(Instruction command) {
            Location newPosition = new Location(Robot.GetPosition().GetX(), Robot.GetPosition().GetY(), Robot.GetPosition().GetFacing().ToString());
            switch (command.GetShortName())
            {
                case "TL":
                    newPosition.ChangeFacing(Location.Turn.L);
                    break;
                case "TR":
                    newPosition.ChangeFacing(Location.Turn.R);
                    break;
                case "A":
                    switch (newPosition.GetFacing()) {
                        case Location.Direction.N:
                            newPosition.SetY(newPosition.GetY() - 1);
                            break;
                        case Location.Direction.E:
                            newPosition.SetX(newPosition.GetX() + 1);
                            break;
                        case Location.Direction.S:
                            newPosition.SetY(newPosition.GetY() + 1);
                            break;
                        case Location.Direction.W:
                            newPosition.SetX(newPosition.GetX() - 1);
                            break;
                    }
                    break;
                case "B":
                    switch (newPosition.GetFacing())
                    {
                        case Location.Direction.N:
                            newPosition.SetY(newPosition.GetY() + 1);
                            break;
                        case Location.Direction.E:
                            newPosition.SetX(newPosition.GetX() - 1);
                            break;
                        case Location.Direction.S:
                            newPosition.SetY(newPosition.GetY() - 1);
                            break;
                        case Location.Direction.W:
                            newPosition.SetX(newPosition.GetX() + 1);
                            break;
                    }
                    break;
                case "C": break;
            }
            return newPosition;
        }

        private void AddCellInArray(List<Location> locations, Location newLocation)
        {
            if (!locations.Exists(l=> l.GetX() == newLocation.GetX() && l.GetY() == newLocation.GetY()))
            {
                locations.Add(newLocation);
            }
        }

        private string PrintCells(List<Location> locations) {
            locations.Sort(new SortLocation());
            string result = "";
            locations.ForEach(l => result += l.PrintCoordinates());

            return string.Format("[{0}]", result);
            
        }

        public string GetFinalReport() {
            return string.Format("{{\n\"visited\" : {0},\n\"cleaned\" : {1},\n\"final\" : {2},\n\"battery\" : {3}\n}}", 
                PrintCells(VisitedCells), PrintCells(CleanedCells), Robot.GetPosition(), Robot.GetBattery());
        }

    }
}
