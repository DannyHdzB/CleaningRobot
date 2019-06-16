using System;
using System.Collections.Generic;
using System.Text;

namespace MyQCleaningRobot
{
    public class Robot
    {
        public enum State { On, Off, Stuck };

        private int Battery;
        private Location Position;
        private State Status;

        public Robot(Location start, int battery) {
            Position = start;
            Battery = battery;
            Status = State.On;
        }

        public int GetBattery() {
            return Battery;
        }

        public Location GetPosition() {
            return Position;
        }

        public State GetStatus() {
            return Status;
        }

        public void SetBattery(int newBattery){
            Battery = newBattery;
        }

        public void SetPosition(Location newPosition) {
            Position = newPosition;
        }

        public void SetStatus(State newStatus) {
            Status = newStatus;
        }

        public bool HaveBattery(int usage) {
            if (GetBattery() - usage >= 0) {
                return true;
            }
            return false;
        }
    }
}
