using UnityEngine;
using System.Collections.Generic;
using TetrisGame;

public static class BlockData
{
    public static readonly Dictionary<BlockShapes, Vector3[]> Shapes = new()
    {
        { BlockShapes.I, new Vector3[] {
            new Vector3(-1, 0, 0), new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(2, 0, 0)
        }},
        { BlockShapes.O, new Vector3[] {
            new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0)
        }},
        { BlockShapes.T, new Vector3[] {
            new Vector3(0, 0, 0), new Vector3(-1, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0)
        }},
        { BlockShapes.L, new Vector3[] {
            new Vector3(0, 0, 0), new Vector3(-1, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0)
        }},
        { BlockShapes.J, new Vector3[] {
            new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(-1, 1, 0)
        }},
        { BlockShapes.S, new Vector3[] {
            new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(-1, 1, 0)
        }},
        { BlockShapes.Z, new Vector3[] {
            new Vector3(0, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0)
        }},
    };

}
