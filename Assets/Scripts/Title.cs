using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] private GameObject HowToPlayPanel;

    public void HowToPlayBtn()
    {
        HowToPlayPanel.SetActive(true);
    }
    public  void StartBtn()
    {
        SceneManager.LoadScene("Game");
    }
}
