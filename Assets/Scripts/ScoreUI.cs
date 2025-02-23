using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreUI : MonoBehaviour
{
    [SerializeField] private Text ScoreText;

    private void OnEnable()
    {
        GameManager.Instance.OnScoreChanged += GameManager_OnScoreChanged;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnScoreChanged -= GameManager_OnScoreChanged;
    }

    public void GameManager_OnScoreChanged(int score)
    {
        ScoreText.text = score.ToString();
    }
}
