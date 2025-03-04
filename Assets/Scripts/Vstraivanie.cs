using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vstraivanie : MonoBehaviour
{
    private Vector3 originalScale;
    public float sizeReductionFactor = 0.5f;
    public bool OnPolka = false;// �� ������� ��������� ������

    void Start()
    {
        originalScale = transform.localScale; // ���������� �������� ������
    }

    void OnCollisionEnter2D(Collision2D �ollision)
    {
        if (�ollision.gameObject.CompareTag("Polka"))
        {
            // ��������� ������ � sizeReductionFactor ���
            transform.localScale = originalScale * sizeReductionFactor;
            Invoke("CheckOnPolka", 0.1f);
        }
    }

    void OnCollisionExit2D(Collision2D �ollision)
    {
        if (�ollision.gameObject.CompareTag("Polka"))
        {
            if (OnPolka)
            {
                // ���������� �������� ������
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
