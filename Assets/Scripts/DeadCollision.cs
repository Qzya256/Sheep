using UnityEngine;

public class DeadCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<ChaserTag>() != null)
        {
            GameManager.instance.DeadChaser();
        }
        if (collision.transform.GetComponent<CrowdTag>() != null)
        {
            GameManager.instance.DeedCrowd();
        }
        collision.gameObject.SetActive(false);
    }
}
