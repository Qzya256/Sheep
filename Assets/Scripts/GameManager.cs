using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject deeadPanel;
    [SerializeField] private int[] crowdMaxCounts;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
    }
    public void CrowdChecker(int crowdCount)
    {
        if (crowdCount >= crowdMaxCounts[PlayerPrefs.GetInt("CurrentlevelIndex")])
        {
            GameOver();
            PlayerPrefs.SetInt("CurrentlevelIndex", PlayerPrefs.GetInt("CurrentlevelIndex") + 1);
        }
    }
    public void DeadChaser()
    {
        deeadPanel.SetActive(true);
    }
    public void DeedCrowd()
    {
        crowdMaxCounts[PlayerPrefs.GetInt("CurrentlevelIndex")] -= 1;
        if (crowdMaxCounts[PlayerPrefs.GetInt("CurrentlevelIndex")] == 0)
        {
            GameOver();
        }
    }
}
