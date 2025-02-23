using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject GameEndWindow;
	[SerializeField] private Animation SpeedUpTextAnimation;
    [SerializeField] private ScoreUI ScoreUI;
    [SerializeField] private Text ElapsedTimeFromStartText;
    [SerializeField] private Text Title;
    [SerializeField] private Text Score;
	[SerializeField] private Text Level;

	private void OnEnable()
	{
		GameManager.Instance.OnGameElapsedTimeChanged += GameManager_OnElapsedTimeFromStartChanged;
		GameManager.Instance.OnSpeedUp += GameManager_OnSpeedUp;
		GameManager.Instance.OnGameEnded += GameManager_OnGameEnded;
	}

	private void OnDisable()
	{
		GameManager.Instance.OnGameElapsedTimeChanged -= GameManager_OnElapsedTimeFromStartChanged;
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

	private void GameManager_OnGameEnded(bool isGameOver, int score)
	{
		GameEndWindow.SetActive(true);
		Title.text = isGameOver ? "Game Over" : "Time's Up";
		Score.text = $"SCORE : {score}";
	}

	private void SetLevelText(int level)
    {
        Level.text = level.ToString();
    }

    private void ShowSpeedUp()
    {
        SpeedUpTextAnimation.Play();
    }

    public void PlayAgainBtn()
    {
        GameManager.Instance.Restart();
    }

    public void BackToTitleBtn()
    {
        GameManager.Instance.BackToTitle();
    }
}
