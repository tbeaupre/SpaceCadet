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
            fixEdges();
            initFinalBlockArray();
        }

        public void initFinalBlockArray()
        {
            for (int j = 0; j < arrayHeight; j++)
            {
                for (int i = 0; i < arrayWidth; i++)
                {
                    finalBlockArray[i, j] = GetBlock(edgeStatesArray[i, j]);
                }
            }
        }

        // Returns the block type associated with a certain set of edgestates
        private BlockEnum GetBlock(EdgeStates edges)
        {
            int result = 0;
            if (edges.U == EdgeState.Open) result += 8;
            if (edges.R == EdgeState.Open) result += 4;
            if (edges.D == EdgeState.Open) result += 2;
            if (edges.L == EdgeState.Open) result += 1;
            return (BlockEnum)result;
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
                    if (isFree(edgeStatesArray[i, j]))
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
                }
            }
        }
    }
}
