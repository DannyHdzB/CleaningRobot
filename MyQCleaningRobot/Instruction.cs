﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyQCleaningRobot
{
    public class Instruction
    {
        public enum shortInstruction { TL, TR, A, B, C};

        private string Name;
        private shortInstruction ShortName;
        private int Cost;
        private bool IsBackOffInstruction;

        public Instruction(string name, string shortName, int cost, bool isBackOffInstruction)
        {
            Name = name;
            ShortName = (shortInstruction)Enum.Parse(typeof(shortInstruction), shortName);
            Cost = cost;
            IsBackOffInstruction = isBackOffInstruction;
        }

        public Instruction(string shortName)
        {
            ShortName = (shortInstruction)Enum.Parse(typeof(shortInstruction), shortName);
            IsBackOffInstruction = false;

            switch (ShortName) {
                case shortInstruction.TL:
                    Name = "Turn Left";
                    Cost = 1;
                    break;
                case shortInstruction.TR:
                    Name = "Turn Right";
                    Cost = 1;
                    break;
                case shortInstruction.A:
                    Name = "Advance";
                    Cost = 2;
                    break;
                case shortInstruction.B:
                    Name = "Back";
                    Cost = 3;
                    break;
                case shortInstruction.C:
                    Name = "Clean";
                    Cost = 5;
                    break;
            }
        }


        public string GetName() {
            return Name;
        }

        public string GetShortName() {
            return ShortName.ToString();
        }

        public int GetCost() {
            return Cost;
        }

        public bool GetIsBackOffInstruccion()
        {
            return IsBackOffInstruction;
        }
    }
}
