using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
        private set { instance = value; }
    }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Debug.LogError("Duplicated Singleton.");
            Destroy(Instance);
            return;
        }
		Instance = this;
    }

    [SerializeField] private TetrominoBlock[] TetrominoPrefabs;
    [SerializeField] private TetrominoBlock GhostPrefab;

    public event Action<int> OnScoreChanged; // int score
    public event Action<int> OnSpeedUp; // int speedLevel
	public event Action<float> OnGameElapsedTimeChanged; // float GameElapsedTime
	public event Action<bool, int> OnGameEnded; // bool isGameOver, int score

	private const int WIDTH = 10;
    private const int HEIGHT = 20;

    private const int SPAWN_X = WIDTH / 2 - 1;
    private const int SPAWN_Y = HEIGHT - 2;

    private const float TETROMINO_DROP_TIME_INTERVAL = 0.3f;
    private const float MAX_GAME_TIME = 60f;

	private const int SPEED_UP_SCORE_INTERVAL = 400;
	private const int SCORE_PER_LINE = 100;

    private float tetrominoDropSpeedMultiplier = 1f;
    private int speedLevel = 1;
    private float gameElapsedTime;
    private int score;
    private int nextSpeedUpScore;

    public int Score
    {
        get => score;
        private set
        {
            score = value;
            OnScoreChanged?.Invoke(score);
        }
	}

	private float GameElapsedTime
    {
        get => gameElapsedTime;

        set
        {
            gameElapsedTime = value;
            OnGameElapsedTimeChanged?.Invoke(gameElapsedTime);
        }
    }

    private IEnumerator Start()
    {
        // Initial setup
		GameElapsedTime = 0f;
		tetrominoDropSpeedMultiplier = 1f;
		speedLevel = 1;
		Score = 0;
		nextSpeedUpScore = SPEED_UP_SCORE_INTERVAL;

		while (true)
        {
            yield return PlayGame();
            break;
        }

    }

    private IEnumerator PlayGame()
    {
        Transform[,] grid = new Transform[WIDTH, HEIGHT];
        TetrominoBlock tetromino = CreateNewTetromino(grid);
		TetrominoBlock ghostTetromino = Instantiate(GhostPrefab);
        float tetrominoElapsedTime = 0f;
        bool isGameOver = false;

		while (GameElapsedTime <= MAX_GAME_TIME)
        {
            GhostFollow(grid, ghostTetromino, tetromino);
            GameElapsedTime += Time.deltaTime;
            tetrominoElapsedTime += Time.deltaTime;

            bool didTetrominoHit;
			HandleInput(grid, tetromino, out didTetrominoHit);

            // Move tetromino down if reached drop time
			if (tetrominoElapsedTime >= (TETROMINO_DROP_TIME_INTERVAL * tetrominoDropSpeedMultiplier))
            {
                if (!MoveTetrominoDown(grid, tetromino))
                {
                    didTetrominoHit |= true;
                }
                tetrominoElapsedTime = 0; 
            }

            if(didTetrominoHit)
            {
                KillLines(grid);
				tetromino = CreateNewTetromino(grid);

				if (tetromino == null) // cannot create new tetromino, meaning that the board is full
				{
					isGameOver = true;
					break;
				}
			}

			yield return null;
        }

        // GameOver
        yield return new WaitForSeconds(1f);
        OnGameEnded?.Invoke(isGameOver, score);

	}

    private void HandleInput(Transform[,] grid, TetrominoBlock tetromino, out bool didTetrominoHit)
    {
        didTetrominoHit = false;

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			MoveTetrominoLeft(grid, tetromino);
		}

		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			MoveTetrominoRight(grid, tetromino);
		}

		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			RotateTetromino(grid, tetromino);
		}

		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			if (!MoveTetrominoDown(grid, tetromino))
			{
				didTetrominoHit = true;
			}
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			HardDrop(grid, tetromino);
			didTetrominoHit = true;
		}
	}

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    private TetrominoBlock CreateNewTetromino(Transform[,] grid)
    {
        TetrominoBlock tetromino = Instantiate(TetrominoPrefabs[Random.Range(0, TetrominoPrefabs.Length)]);
        tetromino.X = SPAWN_X;
        tetromino.Y = SPAWN_Y;
        if (!tetromino.ValidMove(grid, WIDTH, HEIGHT)) return null;
        return tetromino;
    }

    private static void GhostFollow(Transform[,] grid, TetrominoBlock ghost, TetrominoBlock followed)
    {
        ghost.X = followed.X;
        ghost.Y = followed.Y;
        for(int i=0; i<followed.transform.childCount; i++)
        {
            ghost.transform.GetChild(i).position =  followed.transform.GetChild(i).position;
        }

        while (ghost.ValidMove(grid, WIDTH, HEIGHT))
        {
            ghost.Y--;
        }
        ghost.Y++;
    }

    private static void RotateTetromino(Transform[,] grid, TetrominoBlock tetromino)
    {
        tetromino.Rotate();
        if (!tetromino.TestWallKick(grid, WIDTH, HEIGHT))
        {
            tetromino.Rotate();
        }
    }
    private static void MoveTetrominoLeft(Transform[,] grid, TetrominoBlock tetromino)
    {
        tetromino.X--;
        if (!tetromino.ValidMove(grid, WIDTH, HEIGHT))
        {
            tetromino.X++;
        }
    }
    private static void MoveTetrominoRight(Transform[,] grid, TetrominoBlock tetromino)
    {
        tetromino.X++;
        if (!tetromino.ValidMove(grid, WIDTH, HEIGHT))
        {
            tetromino.X--;
        }
    }

	private static bool MoveTetrominoDown(Transform[,] grid, TetrominoBlock tetromino)
    {
        tetromino.Y--;
        if (tetromino.ValidMove(grid, WIDTH, HEIGHT))
        {
            return true; // move down action is valid
        }
        tetromino.Y++;

        tetromino.Hit();
        tetromino.AddTetrominoToGrid(grid);

        return false; // cannot move down, so tetromino hit
	}
    
    private static void HardDrop(Transform[,] grid, TetrominoBlock tetromino)
    {
        while (MoveTetrominoDown(grid, tetromino))
        {
            continue;
        }
    }

    public void KillLines(Transform[,] grid)
    {
        int killedLines = 0;
        for (int row = HEIGHT - 1; row >= 0; row--)
        {
            if (HasLine(row, grid))
            {
                RemoveLineTiles(row, grid);
                RowDown(row, grid);
			    EffectManager.Instance.ShowKillLineEffect(new Vector3(SPAWN_X + 1, row - 0.5f, 0));
				killedLines++;
            }
        }

        if (killedLines > 0)
        {
            SFXPlayer.Instance.PlaySFX("KillLine");
			AddScore(killedLines);
		}
	}

    public static bool HasLine(int row, Transform[,] grid)
    {
        for (int i = 0; i < WIDTH; i++)
        {
            if (grid[i, row] == null) return false;
        }
        return true;
    }

    public static void RemoveLineTiles(int row, Transform[,] grid)
    {
        for (int j = 0; j < WIDTH; j++)
        {
            grid[j, row].GetComponent<Tile>().Kill();
            grid[j, row] = null;
        }
    }

    public static void RowDown(int row, Transform[,] grid)
    {
        for (int i = row; i < HEIGHT; i++)
        {
            for (int j = 0; j < WIDTH; j++)
            {
                if (grid[j, i] != null && grid[j, i - 1] == null)
                {
                    grid[j, i].transform.position = new Vector2(j, i - 1);
                    grid[j, i - 1] = grid[j, i];
                    grid[j, i] = null;
                }
            }
        }
    }
	
	public void AddScore(int lines)
	{
		Score += SCORE_PER_LINE * lines;
        while (speedLevel < 4 && Score >= nextSpeedUpScore)
		{
			SpeedUp();
		}
	}
	
    public void SpeedUp()
    {
		speedLevel++;
		nextSpeedUpScore += SPEED_UP_SCORE_INTERVAL;
		tetrominoDropSpeedMultiplier -= 0.2f;

        OnSpeedUp?.Invoke(speedLevel);
	}
}
