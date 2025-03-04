using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    public float scrollSpeed = 5f;
    public float leftLimit;  // ����� ������� ������
    public float rightLimit; // ������ ������� ������
    public float topLimit; // ������� ������� ������
    public float bottomLimit; // ������ ������� ������
    public float inertiaMultiplier = 0.98f; // ��������� ������� (��� ������, ��� ������� �����������)
    public float minInertiaSpeed = 3f; // ����������� �������� ������� ��� ���������

    private Vector3 dragOrigin;
    private bool isDragging = false;
    private Vector3 velocity; // �������� �������� ������

    private float dragStartTime; // ����� ������ ��������������
    private Vector3 dragStartPos; // ������� ������ ��������������

    public string draggableTag = "Draggable"; // ��� ��������, ������� ����� �������������

    void Update()
    {
        // ����������� ������� (��� ������� ����)
        if (Input.GetMouseButtonDown(0))
        {
            // �������� ������, �� ������� ������
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            // ���� ������ �� ������ � ����� "Draggable", �� �� ������� ������
            if (hit.collider != null && hit.collider.gameObject.CompareTag(draggableTag))
            {
                isDragging = false; // �� �������� �������������� ������
                return; // ������� �� �������, ����� �� ������� ������
            }

            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;

            // ���������� ����� � ������� ������ ��������������
            dragStartTime = Time.time;
            dragStartPos = transform.position;

            velocity = Vector3.zero; // ���������� ��������
        }

        // ����������� �������
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            // ��������� �������� �� ������ ����������� ���������� � �������
            float dragTime = Time.time - dragStartTime;
            Vector3 dragDistance = transform.position - dragStartPos;

            if (dragTime > 0)
            {
                velocity = dragDistance / dragTime;
            }
        }

        // ��������������
        if (isDragging)
        {
            Vector3 difference = dragOrigin - Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // ���������� ������
            transform.position += difference;

            // ��������� ����� ������ ��������������, ����� �������� ���� �����������
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        // ��������� �������
        if (!isDragging && velocity.magnitude > 0)
        {
            transform.position += velocity * Time.deltaTime; // �������� �� Time.deltaTime

            velocity *= inertiaMultiplier; // ��������� �������� � ������ ������

            // ���� �������� ����� ���������� �����, ������������� ��������
            if (velocity.magnitude < minInertiaSpeed)
            {
                velocity = Vector3.zero;
            }
        }

        // ����������� �������� ������
        float clampedX = Mathf.Clamp(transform.position.x, leftLimit, rightLimit);
        float clampedY = Mathf.Clamp(transform.position.y, bottomLimit, topLimit);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}