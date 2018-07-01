using System;

namespace R2D2G2
{
    class Action
    {
        string Name;
        int TimeInMilliseconds;

        public Action(string pName, int pTimeInMilliseconds)
        {
            Name = pName;
            TimeInMilliseconds = pTimeInMilliseconds;
        }
    }
}
