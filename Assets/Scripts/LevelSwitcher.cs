using UnityEngine;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] levels;
    private int currentlevelIndex;
    private void Awake()
    {
        LevelSwitch();
    }
    public void LevelSwitch()
    {
        levels[PlayerPrefs.GetInt("CurrentlevelIndex")].SetActive(true);
    }
}