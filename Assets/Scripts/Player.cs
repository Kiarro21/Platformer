using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed; //Скорость персонажа
    public float jumpHeight; //Высота прыжка персонажа
    public Transform groundCheck; //Проверка, находится ли персонаж на земле
    public bool isGrounded, isDead = false; //Находится ли персонаж на замеле?
    Animator anim;
    public int currentHP;
    public int maxHP = 3;
    bool isHit = false;
    public Main main;
    public bool key = false;
    public bool canTelepot = true;
    int coins = 0;
    public bool canHit = true;
    public GameObject diamond, sdiamond, door;
    int diamondCount = 0;
    public Inventory inventory;
    public SoudnEffector soundEffector;
    public Text help, help_hearth, help_bdiamond, help_sdiamond, help_key;

    public Joystick joystick;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHP = maxHP;
    }


    // Update is called once per frame
    
    void Update()
    {
        checkGround();
        if (joystick.Horizontal < 0.3f && joystick.Horizontal > -0.3f && isGrounded && !isDead)
        {
            anim.SetInteger("State", 1);
        }
        else
        {
            Flip();
            if (isGrounded && !isDead)
                anim.SetInteger("State", 2);
        }
        /*
        if (Input.GetKeyDown(KeyCode.Space) &&joystick.Vertical > 0.8f && isGrounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            soundEffector.PlayJumpSound();
        }
        */
    }

    void FixedUpdate()
    {
        if (joystick.Horizontal >= 0.3f)
            rb.velocity = new Vector2(speed, rb.velocity.y);
        else if (joystick.Horizontal <= -0.3f)
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        else
            rb.velocity = new Vector2(0f, rb.velocity.y);
    }

    public void Jump()
    {
        //При нажатии на пробел, то персонаж прыгает
        if (/*Input.GetKeyDown(KeyCode.Space) &&*/isGrounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            soundEffector.PlayJumpSound();
        }
    }

    //Разворот персонажа по оси X
    void Flip()
    {
        if (joystick.Horizontal > 0.3f)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (joystick.Horizontal < -0.3f)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    void checkGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.1f);
        isGrounded = colliders.Length > 1;
        if (!isGrounded && !isDead)
            anim.SetInteger("State", 3);
    }

    //Пересчёт жизней Player'а
    public void RecountHP(int deltaHP)
    {
        if (deltaHP < 0 && canHit)
        {
            currentHP += deltaHP;
            StopCoroutine(OnHit());
            canHit = false;
            isHit = true;
            StartCoroutine(OnHit());
        }
        if (deltaHP > 0 && currentHP < 3)
            currentHP += deltaHP;
        if (currentHP <= 0)
        {
            currentHP += deltaHP;
            anim.SetInteger("State", 4);
            isDead = true;
            //GetComponent<Collider2D>().enabled = false;
            Invoke("Lose", 1.5f);
        }
    }

    IEnumerator OnHit()
    {
        if (isHit)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g - 0.08f, GetComponent<SpriteRenderer>().color.b - 0.08f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g + 0.08f, GetComponent<SpriteRenderer>().color.b + 0.08f);
        }
        if (GetComponent<SpriteRenderer>().color.g >= 1)
        {
            canHit = true;
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
            yield break;
        }
        if (GetComponent<SpriteRenderer>().color.g <=0)
        {
            isHit = false;
            GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f);
        }
        yield return new WaitForSeconds(0.02f);
        StartCoroutine(OnHit()); 
    }

    void Lose()
    {
		isDead = false;
        main.GetComponent<Main>().Lose();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "key")
        {
            Destroy(collision.gameObject);
            key = true;
            inventory.Add_Key();
        }

        if (collision.gameObject.tag == "door")
        {
            if (collision.gameObject.GetComponent<Door>().isOpen && canTelepot)
            {
                collision.gameObject.GetComponent<Door>().Teleport(gameObject);
                canTelepot = false;
                StartCoroutine(TeleportWait());
            }
            else if (key) 
                collision.gameObject.GetComponent<Door>().Unlock();
        }

        if (collision.gameObject.tag == "coin")
        {
            Destroy(collision.gameObject);
            coins++;
            soundEffector.PlayCoinSound();
        }

        if (collision.gameObject.tag == "hearth")
        {
            Destroy(collision.gameObject);
            //RecountHP(1);
            inventory.Add_HP();
        }

        if (collision.gameObject.tag == "bhearth")
        {
            Destroy(collision.gameObject);
            RecountHP(-1);
        }

        if (collision.gameObject.tag == "diamond")
        {
            Destroy(collision.gameObject);
            //StartCoroutine(Invulnerability());
            inventory.Add_Diamond();
        }

        if (collision.gameObject.tag == "sdiamond")
        {
            Destroy(collision.gameObject);
            //StartCoroutine(SpeedBoost());
            inventory.Add_sDiamond();
        }

        if (collision.gameObject.tag == "help")
        {
            help.gameObject.SetActive(true);
        }

        if (collision.gameObject.tag == "help_hearth")
        {
            help_hearth.gameObject.SetActive(true);
        }

        if (collision.gameObject.tag == "help_bdiamond")
        {
            help_bdiamond.gameObject.SetActive(true);
        }

        if (collision.gameObject.tag == "help_sdiamond")
        {
            help_sdiamond.gameObject.SetActive(true);
        }

        if (collision.gameObject.tag == "help_key")
        {
            help_key.gameObject.SetActive(true);
        }
    }

    IEnumerator SpeedBoost()
    {
        diamondCount++;
        sdiamond.SetActive(true);
        CheckDiamond(sdiamond);

        speed = speed * 2;
        sdiamond.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(9f);
        StartCoroutine(DiamondInvis(sdiamond.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        speed = speed / 2;

        diamondCount--;
        sdiamond.SetActive(false);
        CheckDiamond(diamond);
    }

    IEnumerator Invulnerability()
    {
        diamondCount++;
        diamond.SetActive(true);
        CheckDiamond(diamond);

        canHit = false;
        diamond.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        //yield return new WaitForSeconds(4f);
        float t = 4f;
        while(t > 0f)
        {
            canHit = false;
            t -= 0.02f;
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(DiamondInvis(diamond.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        canHit = true;

        diamondCount--;
        diamond.SetActive(false);
        CheckDiamond(sdiamond);
    }

    IEnumerator TeleportWait()
    {
        yield return new WaitForSeconds(1f);
        canTelepot = true;
    }

    void CheckDiamond(GameObject obj)
    {
        if (diamondCount == 1)
        {
            obj.transform.localPosition = new Vector3(0.05f, 0.35f, obj.transform.localPosition.z);
            obj.transform.localPosition = new Vector3(0.05f, 0.35f, obj.transform.localPosition.z);
        }
        else if (diamondCount == 2)
        {
            diamond.transform.localPosition = new Vector3(-0.1f, 0.35f, diamond.transform.localPosition.z);
            sdiamond.transform.localPosition = new Vector3(0.15f, 0.35f, sdiamond.transform.localPosition.z);
        }
    }

    IEnumerator DiamondInvis(SpriteRenderer spr, float time)
    {
        spr.color = new Color(1f, 1f, 1f, spr.color.a - time * 2);
        yield return new WaitForSeconds(time);
        if (spr.color.a > 0)
            StartCoroutine(DiamondInvis(spr, time));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "trampоline")
        {
            StartCoroutine(TrampolineAnim(collision.gameObject.GetComponentInParent<Animator>()));
        }
    }

    IEnumerator TrampolineAnim(Animator An)
    {
        An.SetInteger("isJump", 1);
        yield return new WaitForSeconds(0.5f);
        An.SetInteger("isJump", 2);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ladder")
        {
            rb.velocity = new Vector2(0f, 0f);
            rb.bodyType = RigidbodyType2D.Kinematic;
            transform.Translate(Vector3.up * speed * 0.5f * Time.deltaTime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ladder")
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        if (collision.gameObject.tag == "help")
        {
            help.gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "help_hearth")
        {
            help_hearth.gameObject.SetActive(false);
        }
        if (collision.gameObject.tag == "help_bdiamond")
        {
            help_bdiamond.gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "help_sdiamond")
        {
            help_sdiamond.gameObject.SetActive(false);
        }

        if (collision.gameObject.tag == "help_key")
        {
            help_key.gameObject.SetActive(false);
        }
    }

    public void diamondActivating()
    {
        StartCoroutine(Invulnerability());
    }

    public void sdiamondActivating()
    {
        StartCoroutine(SpeedBoost());
    }

    public int GetCoins()
    {
        return coins;
    }

    public int GetHP()
    {
        return currentHP;
    }
}
