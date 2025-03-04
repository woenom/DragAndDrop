using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragger : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Rigidbody2D rb;
    private float originalGravityScale;
    private AudioSource audioSource;
    public AudioClip DuckSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale; // Store the original gravity scale
        audioSource = GetComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.isKinematic = true; // Disable physics during dragging
        rb.gravityScale = 0; // Disable gravity during dragging
        rb.velocity = Vector2.zero;
        if (DuckSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(DuckSound);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        rb.isKinematic = false; // Enable physics when released
        rb.gravityScale = originalGravityScale; // Restore the original gravity scale
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition + offset;
        }
    }
}