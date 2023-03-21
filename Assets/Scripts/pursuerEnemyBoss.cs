using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pursuerEnemyBoss : MonoBehaviour
{
    float playerX;
    public float speed;
    Animator anim;
    public bool isAngry = false;
    public Sprite _2hearts, _1heart;
    public Transform groundDetect;
    private bool isGrounded = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetect.position, Vector2.down, 1f);

        if (isAngry)
        {
            playerX = GameObject.FindGameObjectWithTag("Player").transform.position.x;
            if (playerX < transform.position.x)
                transform.eulerAngles = new Vector3(0, -180, 0);
            if (playerX > transform.position.x)
                transform.eulerAngles = new Vector3(0, 0, 0);
            if (groundInfo.collider)
            {
                anim.SetBool("isAngry", true);
                transform.Translate(Vector2.right * speed * Time.deltaTime);
            }
            else
                anim.SetBool("isAngry", false);
        }

        if (GetComponent<Enemy>().health == 2)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _2hearts;
            speed = 1.5f;
        }
        if (GetComponent<Enemy>().health == 1)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _1heart;
            speed = 2f;
        }
        else if (GetComponent<Enemy>().health <= 0)
            transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isAngry = true;
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
