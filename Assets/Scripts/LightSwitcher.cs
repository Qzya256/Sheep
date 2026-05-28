using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSwitcher : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Color32 lightColor;
    [SerializeField] private GameObject chaserLight;
    [SerializeField] private bool[] isNight;

    void Start()
    {
        if (isNight[PlayerPrefs.GetInt("CurrentlevelIndex")])
        {
            globalLight.color = lightColor;
            chaserLight.SetActive(true);
        }
    }
}
