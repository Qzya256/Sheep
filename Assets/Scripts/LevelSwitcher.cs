using UnityEngine;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] levels;
    private void Awake()
    {
        LevelSwitch();
    }
    public void LevelSwitch()
    {
        if (PlayerPrefs.GetInt("CurrentlevelIndex") < levels.Length)
        {
            levels[PlayerPrefs.GetInt("CurrentlevelIndex")].SetActive(true);
        }
        else
        {
            print("AllLevelsComplete");
        }
    }
}
