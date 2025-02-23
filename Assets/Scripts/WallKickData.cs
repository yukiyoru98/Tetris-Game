using UnityEngine;
public class WallKickData 
{
    public static readonly Vector2Int[,] WallKick_I =
    {
        { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int(1, 0), new Vector2Int(-2, -1), new Vector2Int(1, 2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(2, 0), new Vector2Int(-1, 2), new Vector2Int(2, -1) },
        { new Vector2Int(0, 0), new Vector2Int(2, 0), new Vector2Int(-1, 0), new Vector2Int(2, 1), new Vector2Int(-1, -2) },
        { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(-2, 0), new Vector2Int(1, -2), new Vector2Int(-2, 1) },
    };

    public static readonly Vector2Int[,] WallKick_LSTJZ =
    {
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, -2), new Vector2Int(-1, -2) },
        { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, 2), new Vector2Int(1, 2) },
        { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(0, 2), new Vector2Int(1, -2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, -1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },
    };
}
