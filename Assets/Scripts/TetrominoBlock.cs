using UnityEngine;
public enum TetrominoType
{
	I, L, O, S, T, Z, J
}

public class TetrominoBlock : MonoBehaviour
{
	[SerializeField] private TetrominoType Type;
	[SerializeField] private Vector3 RotatePivot;

	private int rotationIdx = 0;

	public int X
	{
		get => Mathf.RoundToInt(transform.localPosition.x);
		set
		{
			Vector3 pos = transform.localPosition;
			pos.x = value;
			transform.localPosition = pos;

		}
	}
	public int Y
	{
		get => Mathf.RoundToInt(transform.localPosition.y);
		set
		{
			Vector3 pos = transform.localPosition;
			pos.y = value;
			transform.localPosition = pos;
		}
	}

	public void Rotate()
	{
		foreach (Transform child in transform)
		{
			child.localPosition = new Vector2(child.localPosition.y, -child.localPosition.x);
		}
	}

	public bool TestWallKick(Transform[,] grid, int width, int height)
	{
		switch (Type)
		{
			case TetrominoType.O:
				return true;
			case TetrominoType.I:
				return TryKick(grid, width, height, WallKickData.WALL_KICK_I);
			default:
				return TryKick(grid, width, height, WallKickData.WALL_KICK_LSTJZ);
		}
	}

	bool TryKick(Transform[,] grid, int width, int height, Vector2Int[,] wallKickData)
	{
		int tmpX = X;
		int tmpY = Y;
		for (int i = 0; i < wallKickData.GetLength(1); i++)
		{
			X += wallKickData[rotationIdx, i].x;
			Y += wallKickData[rotationIdx, i].y;
			if (ValidMove(grid, width, height))
			{
				rotationIdx++;
				if (rotationIdx >= wallKickData.GetLength(0))
				{
					rotationIdx = 0;
				}
				return true;
			}
			X = tmpX;
			Y = tmpY;
		}
		return false;
	}

	public bool ValidMove(Transform[,] grid, int width, int height)
	{
		foreach (Transform child in transform)
		{
			int x_int = Mathf.RoundToInt(child.position.x);
			int y_int = Mathf.RoundToInt(child.position.y);
			if (x_int < 0 || x_int >= width || y_int < 0 || y_int >= height)
			{
				return false;
			}
			if (grid[x_int, y_int] != null)
			{
				return false;
			}
		}
		return true;
	}

	public void AddToGrid(Transform[,] grid)
	{
		foreach (Transform child in transform)
		{
			int x_int = Mathf.RoundToInt(child.position.x);
			int y_int = Mathf.RoundToInt(child.position.y);
			grid[x_int, y_int] = child;
		}
	}

	public void Hit()
	{
		float bottomPos = Mathf.Infinity;

		// Shake each child Tile and find the lowest tile's y position
		foreach (Transform child in transform)
		{
			Tile tile = child.GetComponent<Tile>();
			tile.Shake();
			int y_int = Mathf.RoundToInt(child.position.y);
			if (y_int < bottomPos)
			{
				bottomPos = y_int;
			}
		}

		// Calculate position to spawn hit effect
		bottomPos -= Tile.TILE_HEIGHT * 0.5f;
		float pivotWorldPosX = transform.TransformPoint(RotatePivot).x;
		Vector3 hitEffectPos = new Vector3(pivotWorldPosX, bottomPos, 0);

		EffectManager.Instance.ShowHitEffect(hitEffectPos);
		SFXPlayer.Instance.PlaySFX("Hit");
	}

}
