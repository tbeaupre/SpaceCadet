using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spaceman.MapGeneration
{
    public static class BlockFunctions
    {
        // Returns the edgestates associated with a certain block type
        public static EdgeStates GetEdgeStates(BlockEnum block)
        {
            EdgeStates result = new EdgeStates(false); // Creates a new set of edges that are all closed
            if ((int)block - 8 >= 0)
            {
                result.U = EdgeState.Open;
                block -= 8;
            }
            if ((int)block - 4 >= 0)
            {
                result.R = EdgeState.Open;
                block -= 4;
            }
            if ((int)block - 2 >= 0)
            {
                result.D = EdgeState.Open;
                block -= 2;
            }
            if ((int)block - 1 >= 0)
            {
                result.L = EdgeState.Open;
                block -= 1;
            }
            return result;
        }
    }
}
