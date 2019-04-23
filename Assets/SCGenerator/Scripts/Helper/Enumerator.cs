using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.SCGenerator.Scripts.Helper
{
    public static class Enumerator
    {
        public enum RoadType
        {
            Empty, Cross, MainVertical, MainHorizontal, StartVertical, EndVertical, StartHorizontal, EndHorizontal, TJoinUp, TJoinRight, TJoinDown, TJoinLeft
        }

        public enum BuildingType
        {
            Base, BaseMedium, Medium, MediumHigh, High
        }

        public enum Orientation
        {
            Neutral, Up, Right, Down, Left
        }
    }
}
