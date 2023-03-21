using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    bool isHit = false;
    public GameObject dropItem;
    public int health = 1;
    //Метод при столкновении с объектом
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isHit)
        {
            collision.gameObject.GetComponent<Player>().RecountHP(-1);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 4f, ForceMode2D.Impulse);
        }
    }

    public IEnumerator Death()
    {
        health--;
        if (health <= 0)
        {
            if (dropItem != null)
            {
                Instantiate(dropItem, transform.position, Quaternion.identity);
            }
            isHit = true;
            GetComponent<Animator>().SetBool("Dead", true);
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GetComponent<Rigidbody2D>().AddForce(transform.up * 3f, ForceMode2D.Impulse);
            GetComponent<Collider2D>().enabled = false;
            transform.GetChild(1).GetComponent<Collider2D>().enabled = false;
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }

    public void startDeath()
    {
        StartCoroutine(Death());
    }
}
