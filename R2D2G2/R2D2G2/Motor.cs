using System;
using System.Collections.Generic;
using static System.Collections.IEnumerable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R2D2G2
{
    class Motor
    {
        State state;
        public State State { get => state; set => state = value; }
        public List<State> States;

        public Motor(List<State> pStates)
        {
            States = pStates;
            State = null; //TODO handle null being default State
        }
    }
}
