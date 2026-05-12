using System.Collections;
using UnityEngine;

public class TrainMoverRepeated : MonoBehaviour
{
    [Header("Точки маршрута")]
    [SerializeField] private Transform pointA;   // Точка отправления (станция A)
    [SerializeField] private Transform pointB;   // Точка назначения (станция B)

    [Header("Настройки")]
    [SerializeField] private float speed = 5f;              // Скорость движения поезда
    [SerializeField] private float intervalBetweenTrips = 5f; // Интервал между поездками (в секундах)

    void Start()
    {
        // Запускаем бесконечный цикл поездок
        StartCoroutine(TripLoop());
    }

    /// <summary>
    /// Основной цикл: ожидание интервала -> поездка -> повтор
    /// </summary>
    IEnumerator TripLoop()
    {
        while (true)
        {
            // Ждём указанный интервал перед следующей поездкой
            yield return new WaitForSeconds(intervalBetweenTrips);
            
            // Запускаем поездку и ждём её завершения
            yield return StartCoroutine(MoveFromAToB());
        }
    }

    /// <summary>
    /// Движение поезда от точки A до точки B
    /// </summary>
    IEnumerator MoveFromAToB()
    {
        // Устанавливаем поезд в точку A
        transform.position = pointA.position;
        
        // Движение к точке B
        while (Vector3.Distance(transform.position, pointB.position) > 0.05f)
        {
            // Плавное движение с заданной скоростью
            transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);
            yield return null; // Ждём следующий кадр
        }
        
        // Фиксируем точную позицию в точке B
        transform.position = pointB.position;
    }
}