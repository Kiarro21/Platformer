using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 3f;
    public float timeToDisable = 5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetDisable());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    IEnumerator SetDisable()
    {
        yield return new WaitForSeconds(timeToDisable);
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopCoroutine(SetDisable());
        Destroy(gameObject);
    }
}
