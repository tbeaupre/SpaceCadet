using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spaceman.MapGeneration
{
    public class EdgeStates
    {
        public EdgeState U;
        public EdgeState R;
        public EdgeState D;
        public EdgeState L;

        public EdgeStates(bool open)
        {
            if (open)
            {
                U = EdgeState.Open;
                R = EdgeState.Open;
                D = EdgeState.Open;
                L = EdgeState.Open;
            }
            else
            { 
                U = EdgeState.Closed;
                R = EdgeState.Closed;
                D = EdgeState.Closed;
                L = EdgeState.Closed;
            }
        }

        public EdgeStates(EdgeState U, EdgeState R, EdgeState D, EdgeState L)
        {
            this.U = U;
            this.R = R;
            this.D = D;
            this.L = L;
        }

        public BlockEnum toBlock()
        {
            int result = 0;
            if (this.U == EdgeState.Open) result += 8;
            if (this.R == EdgeState.Open) result += 4;
            if (this.D == EdgeState.Open) result += 2;
            if (this.L == EdgeState.Open) result += 1;
            return (BlockEnum)result;
        }
    }
}
