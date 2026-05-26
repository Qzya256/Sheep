using UnityEngine;

public class PaddockZoneTrigger : MonoBehaviour
{
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
