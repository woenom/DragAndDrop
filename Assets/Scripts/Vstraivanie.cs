using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vstraivanie : MonoBehaviour
{
    private Vector3 originalScale;
    public float sizeReductionFactor = 0.5f;
    public bool OnPolka = false;// На сколько уменьшить размер

    void Start()
    {
        originalScale = transform.localScale; // Запоминаем исходный размер
    }

    void OnCollisionEnter2D(Collision2D сollision)
    {
        if (сollision.gameObject.CompareTag("Polka"))
        {
            // Уменьшаем размер в sizeReductionFactor раз
            transform.localScale = originalScale * sizeReductionFactor;
            Invoke("CheckOnPolka", 0.1f);
        }
    }

    void OnCollisionExit2D(Collision2D сollision)
    {
        if (сollision.gameObject.CompareTag("Polka"))
        {
            if (OnPolka)
            {
                // Возвращаем исходный размер
                OnPolka = false;
                transform.localScale = originalScale;
            }
        }
    }
    void CheckOnPolka()
    {
        OnPolka = true;
    }
}
