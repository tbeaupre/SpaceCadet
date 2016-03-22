using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spaceman.MapGeneration
{
    public class Room
    {
        int arrayWidth;
        int arrayHeight;
        BlockEnum blockType;
        EdgeStates[,] edgeStatesArray;
        BlockEnum[,] finalBlockArray;
        int depth;
        Random rnd;

        public BlockEnum[,] GetFinalBlockArray()
        {
            return this.finalBlockArray;
        }

        public int GetFinalBlockArrayWidth()
        {
            return this.arrayWidth;
        }

        public int GetFinalBlockArrayHeight()
        {
            return this.arrayHeight;
        }

        public Room (int depth, BlockEnum blockType, Random rnd)
        {
            arrayWidth = (int)Math.Pow(3, depth);
            arrayHeight = (int)Math.Pow(3, depth);
            this.blockType = blockType;
            this.depth = depth;
            this.rnd = rnd;
            this.finalBlockArray = new BlockEnum[arrayWidth, arrayHeight];
            this.edgeStatesArray = new EdgeStates[arrayWidth, arrayHeight];
            initEdgeArray();
        }

        public void initEdgeArray()
        {
            Block tempBlock = new Block(blockType, edgeStatesArray, 0, 0, depth, rnd);
            this.edgeStatesArray = tempBlock.GetOmniBlockArray();
            //fixEdges();
            initFinalBlockArray();
        }

        public void initFinalBlockArray()
        {
            for (int j = 0; j < arrayHeight; j++)
            {
                for (int i = 0; i < arrayWidth; i++)
                {
                    fixOpenBlockEdges(i, j);
                    finalBlockArray[i, j] = edgeStatesArray[i, j].toBlock();
                }
            }
        }

        private bool isFree(EdgeStates edges)
        {
            return (edges.U == EdgeState.Open && edges.R == EdgeState.Open && edges.D == EdgeState.Open && edges.L == EdgeState.Open);
        }

        public void fixEdges()
        {
            for (int j = 0; j < arrayHeight; j++)
            {
                for (int i = 0; i < arrayWidth; i++)
                {
                    openNooks(i, j);
                }
            }
            initFinalBlockArray();
        }

        // All UD or RL tunnels will be widened by one tile. rndFactor will decide how frequently the walls are widened with 1 being all the time.
        public void widenTunnels(int rndFactor)
        {
            for (int j = 0; j < arrayHeight; j++)
            {
                for (int i = 0; i < arrayWidth; i++)
                {
                    widenTunnels(i, j, rndFactor);
                }
            }
            initFinalBlockArray();
        }

        // All UD or RL tunnels will be widened by one tile. rndFactor will decide how frequently the walls are widened with 1 being all the time.
        public void widenTunnels(int i, int j, int rndFactor)
        {
            if (rnd.Next(0, rndFactor) == 0 && i != 0 && j != 0)
            {
                if (edgeStatesArray[i, j].toBlock() == BlockEnum.UD)
                {
                    edgeStatesArray[i, j].L = EdgeState.Open;
                    edgeStatesArray[i - 1, j].R = EdgeState.Open;
                    if (rnd.Next(0, rndFactor) == 0)
                    {
                        edgeStatesArray[i - 1, j].U = EdgeState.Open;
                    }
                    openNooks(i - 1, j + 1);
                    openNooks(i - 1, j);
                }
                if (edgeStatesArray[i, j].toBlock() == BlockEnum.RL)
                {
                    edgeStatesArray[i, j].U = EdgeState.Open;
                    edgeStatesArray[i, j - 1].D = EdgeState.Open;
                    if (rnd.Next(0, rndFactor) == 0)
                    {
                        edgeStatesArray[i, j - 1].L = EdgeState.Open;
                    }
                    openNooks(i + 1, j - 1);
                    openNooks(i, j - 1);
                }
            }
        }

        // Makes sure that there are no closed edges touching open edges.
        public void fixOpenBlockEdges(int i, int j)
        {
            // Need to check to make sure you are not attempting to match the edges of a block which is outside the bounds of the array
            if (j != 0)
                edgeStatesArray[i, j].U = edgeStatesArray[i, j - 1].D;
            if (i != 0)
                edgeStatesArray[i, j].L = edgeStatesArray[i - 1, j].R;
            if (j != arrayHeight - 1)
                edgeStatesArray[i, j].D = edgeStatesArray[i, j + 1].U;
            if (i != arrayWidth - 1)
                edgeStatesArray[i, j].R = edgeStatesArray[i + 1, j].L;
        }

        // If there are adjacent common openings that are separated by walls, the walls will be removed and the map opened up.
        public void openNooks(int i, int j)
        {
            if (i != 0 && hasUDMatch(i, j))
            {
                edgeStatesArray[i, j].L = EdgeState.Open;
                edgeStatesArray[i - 1, j].R = EdgeState.Open;
            }
            if (j != 0 && hasRLMatch(i, j))
            {
                edgeStatesArray[i, j].U = EdgeState.Open;
                edgeStatesArray[i, j - 1].D = EdgeState.Open;
            }
        }

        // Returns whether or not there is a wall between two common Up or Down openings side by side.
        private bool hasUDMatch(int i, int j)
        {
            return ((edgeStatesArray[i, j].U == EdgeState.Open && edgeStatesArray[i - 1, j].U == EdgeState.Open) ||
                (edgeStatesArray[i, j].D == EdgeState.Open && edgeStatesArray[i - 1, j].D == EdgeState.Open));
        }

        // Returns whether or not there is a wall between two common Left or Right openings above one another.
        private bool hasRLMatch(int i, int j)
        {
            return ((edgeStatesArray[i, j].L == EdgeState.Open && edgeStatesArray[i, j - 1].L == EdgeState.Open) ||
                (edgeStatesArray[i, j].R == EdgeState.Open && edgeStatesArray[i, j - 1].R == EdgeState.Open));
        }
    }
}
