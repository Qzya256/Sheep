using UnityEngine;

public class PaddockZoneTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _crowdLayers;
    [SerializeField] private int crowdCount;
    [SerializeField] private int crowdMaxCount;
    [SerializeField] private GameObject gameOverPanel;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PanicFleeFromTransform2D>()!= null)
        {
            crowdCount++;
            CrowdChecker();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PanicFleeFromTransform2D>() != null)
        {
            crowdCount--;
        }
    }

    private void CrowdChecker()
    {
        if (crowdCount >= crowdMaxCount)
        {
            gameOverPanel.SetActive(true);
            PlayerPrefs.SetInt("CurrentlevelIndex", PlayerPrefs.GetInt("CurrentlevelIndex") + 1);
        }
    }
}
