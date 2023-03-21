using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pursuerEnemy : MonoBehaviour
{
    float playerX;
    public float speed;
    Animator anim;
    bool isAngry = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAngry)
        {
            playerX = GameObject.FindGameObjectWithTag("Player").transform.position.x;
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            if (playerX < transform.position.x)
                transform.eulerAngles = new Vector3(0, 0, 0);
            else if (playerX > transform.position.x)
                transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isAngry = true;
            anim.SetBool("isAngry", true);
        }
    }
}
