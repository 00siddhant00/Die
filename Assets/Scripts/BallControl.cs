using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BallControl : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite[] side;
    [SerializeField] private GameObject[] platform;
    // [SerializeField] private List<GameObject> proxyPlat;

    Vector2 startPos, endPos, direction;
    Rigidbody2D myRigidbody2D;

    Vector3 dragStartPos = Vector3.zero;
    Vector3 draggingPos = Vector3.zero;

    public LineRenderer lr;

    public float shootPower = 10f;
    public float sec = 0.5f;

    public float maxDistance;
    Vector3 distance;
    int died;

    bool mouseDown;
    public bool isGrounded;
    bool animate;
    bool addTime;
    bool once;

    public LayerMask groundLayer;

    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        lr.positionCount = 2;
        // platform = proxyPlat;
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDown = true;
            startPos = Input.mousePosition;
            dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void Update()
    {
        CheckGrounded();
        //print(myRigidbody2D.velocity);
        if (Input.GetMouseButtonUp(1))
        {
            BeginRandom();
        }
        DrawLine();
        if (myRigidbody2D.velocity == Vector2.zero)
        {
            if (once) return;
            animate = false;
            BeginRandom();
        }
    }

    void DrawLine()
    {
        if (!isGrounded) return;

        if (Input.GetMouseButton(0))
        {
            if (mouseDown)
            {
                draggingPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lr.SetPosition(0, new Vector3(dragStartPos.x, dragStartPos.y, 0f));
                lr.SetPosition(1, new Vector3(draggingPos.x, draggingPos.y, 0f));
            }
        }
    }

    void BeginRandom()
    {
        // if(!addTime) return;
        // try{
        //     platform.RemoveAll(item => item == null);
        // }
        // catch(NullReferenceException n)
        // {
        //print("Hi");
        // }
        // platform = proxyPlat;
        for (int i = 0; i < platform.Length; i++)
        {
            // print(platform[i]);
            // platform.Add(proxyPlat[i]);
            platform[i].SetActive(true);
        }
        int times = 0;
        died = Random.Range(1, 7);
        for (int i = 0; i < side.Length; i++)
        {
            if (died == i)
            {
                sr.sprite = side[i];
            }
        }



        for (int i = 0; i < died; i++)
        {
            // times++;
            // if (times <= died) return;
            int get = GetRandom(platform.Length);
            //print(get);
            // if (get == )
            // {
            platform[get].SetActive(false);
            //     platform.Remove(platform[i]);
            // }

        }
        once = true;
        // print(died);
    }

    int oldval = 0;
    int get;
    int attempts;
    bool once1 = true;

    int GetRandom(int max)
    {
        attempts = 0;
        do
        {
            get = Random.Range(0, max);
            if (once1)
            {
                oldval = get;
                once1 = false;
            }
            attempts++;
        } while (get == oldval && attempts > 10);
        //Debug.Log(attempts + " attempts");
        oldval = get;
        return get;
    }

    void T()
    {
        addTime = false;
    }

    IEnumerator AnimateRandom()
    {
        if (!animate)
        {
            Invoke("T", 0.05f);
            for (int i = 0; i < side.Length; i++)
            {
                if (died == i)
                {
                    sr.sprite = side[i];
                }
            }
            // BeginRandom();
            yield break;
        }
        for (int i = 0; i < side.Length; i++)
        {
            sr.sprite = side[i];
            yield return new WaitForSeconds(sec);
        }
        StartCoroutine("AnimateRandom");
    }

    void OnMouseUp()
    {
        if (!isGrounded) return;
        if (Input.GetMouseButtonUp(0))
        {
            mouseDown = false;
            endPos = Input.mousePosition;
            direction = startPos - endPos;
            print(direction);
            var dir = direction;
            dir.x = Mathf.Abs(direction.x);
            dir.y = Mathf.Abs(direction.y);
            // maxDistance = endPos.x;
            if (dir.x > maxDistance)
            {
                if (direction.x > 0)
                {
                    direction.x = maxDistance;
                }
                else
                {
                    direction.x = -maxDistance;
                }
            }
            if (dir.y > maxDistance)
            {
                if (direction.y > 0)
                {
                    direction.y = maxDistance;
                }
                else
                {
                    direction.y = -maxDistance;
                }
            }
            myRigidbody2D.isKinematic = false;
            myRigidbody2D.AddForce(direction * shootPower);
            lr.SetPosition(0, new Vector3(100f, 100f, 0f));
            lr.SetPosition(1, new Vector3(100f, 100f, 0f));
        }
    }

    void CheckGrounded()
    {
        float distance = 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, distance, groundLayer);
        var other = hit.collider;
        //Debug.DrawLine(transform.position, transform.position + new Vector3(0, -1, 0), Color.green);
        if (other != null)
        {
            animate = false;
            addTime = true;
            // BeginRandom();
            if (other.gameObject.tag == "ground") isGrounded = true;
            else if (other.gameObject.tag == "Move")
            {
                transform.parent = other.gameObject.transform;
                isGrounded = true;
            }

            // Visualize the raycast
            Debug.DrawRay(transform.position, Vector3.down * distance, Color.green);
        }
        else
        {
            transform.parent = null;
            // Visualize the raycast
            Debug.DrawRay(transform.position, Vector3.down * distance, Color.red);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // animate = false;
        //addTime = true;
        //// BeginRandom();
        //if (other.gameObject.tag == "ground") isGrounded = true;
        if (other.gameObject.tag == "Win") LevelManager.LoadNextLevel();
        else if (other.gameObject.tag == "Respawn") LevelManager.Won();
    }

    void OnCollisionExit2D(Collision2D other)
    {
        once = false;
        animate = true;
        StartCoroutine("AnimateRandom");
        if (other.gameObject.tag == "ground") isGrounded = false;
    }
}