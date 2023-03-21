using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetle : MonoBehaviour
{
    public float speed = 4f;
    bool isWait = false; //Ожидание скрытия/выпрыгивания
    bool isHidden = false; //На поверхности или под землёй
    public float waitTime = 4f; //Время нахождения на земле/под землёй
    public Transform point; //Точка выпрыгивания и скрытия

    // Start is called before the first frame update
    void Start()
    {
        point.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWait)
            transform.position = Vector3.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
        if (transform.position == point.position)
        {
            if (isHidden)
            {
                point.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                isHidden = false;
            }
            else
            {
                point.transform.position = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
                isHidden = true;
            }
            isWait = true;
            StartCoroutine(Waiting());
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(waitTime);
        isWait = false;
    }
}
