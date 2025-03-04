using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    public float scrollSpeed = 5f;
    public float leftLimit;  // Левая граница камеры
    public float rightLimit; // Правая граница камеры
    public float topLimit; // Верхняя граница камеры
    public float bottomLimit; // Нижняя граница камеры
    public float inertiaMultiplier = 0.98f; // Множитель инерции (чем меньше, тем быстрее замедляется)
    public float minInertiaSpeed = 3f; // Минимальная скорость инерции для остановки

    private Vector3 dragOrigin;
    private bool isDragging = false;
    private Vector3 velocity; // Скорость движения камеры

    private float dragStartTime; // Время начала перетаскивания
    private Vector3 dragStartPos; // Позиция начала перетаскивания

    public string draggableTag = "Draggable"; // Тэг объектов, которые можно перетаскивать

    void Update()
    {
        // Обнаружение касания (или нажатия мыши)
        if (Input.GetMouseButtonDown(0))
        {
            // Получаем объект, на который нажали
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            // Если нажали на объект с тэгом "Draggable", то не двигаем камеру
            if (hit.collider != null && hit.collider.gameObject.CompareTag(draggableTag))
            {
                isDragging = false; // Не начинаем перетаскивание камеры
                return; // Выходим из функции, чтобы не двигать камеру
            }

            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;

            // Запоминаем время и позицию начала перетаскивания
            dragStartTime = Time.time;
            dragStartPos = transform.position;

            velocity = Vector3.zero; // Сбрасываем скорость
        }

        // Прекращение касания
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            // Вычисляем скорость на основе пройденного расстояния и времени
            float dragTime = Time.time - dragStartTime;
            Vector3 dragDistance = transform.position - dragStartPos;

            if (dragTime > 0)
            {
                velocity = dragDistance / dragTime;
            }
        }

        // Перетаскивание
        if (isDragging)
        {
            Vector3 difference = dragOrigin - Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Перемещаем камеру
            transform.position += difference;

            // Обновляем точку начала перетаскивания, чтобы движение было непрерывным
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        // Применяем инерцию
        if (!isDragging && velocity.magnitude > 0)
        {
            transform.position += velocity * Time.deltaTime; // Умножаем на Time.deltaTime

            velocity *= inertiaMultiplier; // Уменьшаем скорость с каждым кадром

            // Если скорость стала достаточно малой, останавливаем движение
            if (velocity.magnitude < minInertiaSpeed)
            {
                velocity = Vector3.zero;
            }
        }

        // Ограничение движения камеры
        float clampedX = Mathf.Clamp(transform.position.x, leftLimit, rightLimit);
        float clampedY = Mathf.Clamp(transform.position.y, bottomLimit, topLimit);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}