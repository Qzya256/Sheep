using UnityEngine;
using System.Collections;

public class TrainMoverRepeated : MonoBehaviour
{
    [Header("Move Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("MovementSettings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float delayBetweenTrips = 5f;
    
    private void Start()
    {
        StartCoroutine(TripLoop());
    }
    IEnumerator TripLoop()
    {
        while (true) 
        {
          yield return new WaitForSeconds(delayBetweenTrips);
          yield return StartCoroutine(MoveFromAtoB());  
        }
    }

    IEnumerator MoveFromAtoB()
    {
        transform.position = pointA.position;

        while (Vector3.Distance(transform.position, pointB.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = pointB.position;
    }
}
