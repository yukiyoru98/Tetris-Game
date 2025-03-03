using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField] private GameObject GameEndWindow;
	[SerializeField] private Animation SpeedUpTextAnimation;
	[SerializeField] private Text ElapsedTimeFromStartText;
	[SerializeField] private Text ScoreText;
	[SerializeField] private Text LevelText;
	[SerializeField] private Text GameEnd_TitleText;
	[SerializeField] private Text GameEnd_ScoreText;

	#region UI Functions
	public void PlayAgainBtn()
	{
		GameManager.Instance.Restart();
	}

	public void BackToTitleBtn()
	{
		GameManager.Instance.BackToTitle();
	}
	#endregion

	#region Private Functions
	private void OnEnable()
	{
		GameManager.Instance.OnGameElapsedTimeChanged += GameManager_OnElapsedTimeFromStartChanged;
		GameManager.Instance.OnScoreChanged += GameManager_OnScoreChanged;
		GameManager.Instance.OnSpeedUp += GameManager_OnSpeedUp;
		GameManager.Instance.OnGameEnded += GameManager_OnGameEnded;
	}

	private void OnDisable()
	{
		GameManager.Instance.OnGameElapsedTimeChanged -= GameManager_OnElapsedTimeFromStartChanged;
		GameManager.Instance.OnScoreChanged -= GameManager_OnScoreChanged;
		GameManager.Instance.OnSpeedUp -= GameManager_OnSpeedUp;
		GameManager.Instance.OnGameEnded -= GameManager_OnGameEnded;
	}

	private void GameManager_OnElapsedTimeFromStartChanged(float elapsedTimeFromStart)
	{
		ElapsedTimeFromStartText.text = elapsedTimeFromStart.ToString("F2");
	}

	private void GameManager_OnSpeedUp(int speedLevel)
	{
		ShowSpeedUp();
		SetLevelText(speedLevel);
	}

	public void GameManager_OnScoreChanged(int score)
	{
		ScoreText.text = score.ToString();
	}

	private void GameManager_OnGameEnded(bool isGameOver, int score)
	{
		GameEndWindow.SetActive(true);
		GameEnd_TitleText.text = isGameOver ? "Game Over" : "Time's Up";
		GameEnd_ScoreText.text = $"SCORE : {score}";
	}

	private void SetLevelText(int level)
	{
		LevelText.text = level.ToString();
	}

	private void ShowSpeedUp()
	{
		SpeedUpTextAnimation.Play();
	}
	#endregion
}
