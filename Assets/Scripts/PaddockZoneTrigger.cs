using UnityEngine;

public class PaddockZoneTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _crowdLayers;
     private int crowdCount;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PanicFleeFromTransform2D>()!= null)
        {
           crowdCount++;
           GameManager.instance.CrowdChecker(crowdCount);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PanicFleeFromTransform2D>() != null)
        {
            crowdCount--;
        }
    }
}
