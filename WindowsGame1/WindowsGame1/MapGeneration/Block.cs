using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spaceman.MapGeneration
{
    public class Block
    {
        Random rnd;
        int subWidth;
        int subHeight;

        BlockEnum blockType;
        public EdgeStates[,] edgeStatesArray;
        public BlockEnum[,] finalBlockArray;
        public EdgeStates[,] omniEdgeArray;

        public Block (BlockEnum blockType, EdgeStates[,] omniBlockArray, int x, int y, int depth, Random rnd)
        {
            this.rnd = rnd;
            this.blockType = blockType;
            this.subWidth = 3;
            this.subHeight = 3;
            this.omniEdgeArray = omniBlockArray;
            edgeStatesArray = new EdgeStates[subWidth, subHeight];
            finalBlockArray = new BlockEnum[subWidth, subHeight];
            InitEdgeStatesArray();
            InitFinalBlockArray();
            HandleDepth(x, y, depth - 1);
        }

        public EdgeStates[,] GetOmniBlockArray()
        {
            return omniEdgeArray;
        }

        public void HandleDepth(int x, int y, int depth)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (depth <= 0)
                    {
                        omniEdgeArray[x * 3 + i, y * 3 + j] = edgeStatesArray[i, j];
                    }
                    else {
                        Block tempBlock = new Block(finalBlockArray[i, j], omniEdgeArray, x * 3 + i, y * 3 + j, depth, rnd);
                        this.omniEdgeArray = tempBlock.GetOmniBlockArray();
                    }
                }
            }
        }

        public void InitFinalBlockArray()
        {
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    finalBlockArray[i, j] = edgeStatesArray[i, j].toBlock();
                }
            }
        }
        
        public void InitEdgeStatesArray()
        {
            // Initialize all EdgeStates in the array.
            for (int j = 0; j < 3; j++) {
                for (int i = 0; i < 3; i++)
                {
                    if (blockType == BlockEnum.Open) edgeStatesArray[i, j] = new EdgeStates(true);
                    else edgeStatesArray[i,j] = new EdgeStates(false);
                }
            }

            // Change specific EdgeState blocks depended upon what the overall block is.
            if (blockType != BlockEnum.Open && blockType != BlockEnum.Closed)
            {
                EdgeStates bigEdges = BlockFunctions.GetEdgeStates(blockType);
                edgeStatesArray[1, 1] = new EdgeStates(EdgeState.Either, EdgeState.Either, EdgeState.Either, EdgeState.Either);
                if (bigEdges.U == EdgeState.Open)
                {
                    edgeStatesArray[0, 0].R = EdgeState.Either;
                    edgeStatesArray[1, 0] = new EdgeStates(EdgeState.Open, EdgeState.Either, EdgeState.Open, EdgeState.Either);
                    edgeStatesArray[2, 0].L = EdgeState.Either;
                    edgeStatesArray[1, 1].U = EdgeState.Open;
                }
                if (bigEdges.R == EdgeState.Open)
                {
                    edgeStatesArray[2, 0].D = EdgeState.Either;
                    edgeStatesArray[2, 1] = new EdgeStates(EdgeState.Either, EdgeState.Open, EdgeState.Either, EdgeState.Open);
                    edgeStatesArray[2, 2].U = EdgeState.Either;
                    edgeStatesArray[1, 1].R = EdgeState.Open;
                }
                if (bigEdges.D == EdgeState.Open)
                {
                    edgeStatesArray[0, 2].R = EdgeState.Either;
                    edgeStatesArray[1, 2] = new EdgeStates(EdgeState.Open, EdgeState.Either, EdgeState.Open, EdgeState.Either);
                    edgeStatesArray[2, 2].L = EdgeState.Either;
                    edgeStatesArray[1, 1].D = EdgeState.Open;
                }
                if (bigEdges.L == EdgeState.Open)
                {
                    edgeStatesArray[0, 0].D = EdgeState.Either;
                    edgeStatesArray[0, 1] = new EdgeStates(EdgeState.Either, EdgeState.Open, EdgeState.Either, EdgeState.Open);
                    edgeStatesArray[0, 2].U = EdgeState.Either;
                    edgeStatesArray[1, 1].L = EdgeState.Open;
                }
                MakeConcreteEdgeStates();
            }
        }

        private void MakeConcreteEdgeStates()
        {
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    // If you aren't in the first row or column
                    if (j != 0)
                    {
                        edgeStatesArray[i, j].U = edgeStatesArray[i, j - 1].D;
                    }
                    if (i != 0)
                    {
                        edgeStatesArray[i, j].L = edgeStatesArray[i - 1, j].R;
                    }
                    RandomizeEdgeStates(edgeStatesArray[i, j]);
                }
            }
        }

        private void RandomizeEdgeStates(EdgeStates states)
        {
            if (states.U == EdgeState.Either) states.U = GetRandomEdgeState();
            if (states.R == EdgeState.Either) states.R = GetRandomEdgeState();
            if (states.D == EdgeState.Either) states.D = GetRandomEdgeState();
            if (states.L == EdgeState.Either) states.L = GetRandomEdgeState();
        }

        private EdgeState GetRandomEdgeState()
        {
            if (rnd.Next(0,3) == 0) return EdgeState.Open;
            else return EdgeState.Closed;
        }
    }
}
