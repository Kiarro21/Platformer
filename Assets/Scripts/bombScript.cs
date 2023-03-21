using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombScript : MonoBehaviour
{
    public GameObject effectPrefab;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(effectPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(gameObject);
    }
}
