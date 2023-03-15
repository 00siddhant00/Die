using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenDoor : MonoBehaviour
{
    SpriteRenderer sr;

    [SerializeField] private Color red, blue;

    [SerializeField] private GameObject door;

    bool open;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = red;
    }

    // Update is called once per frame
    void Update()
    {
        if(open)
        {

        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player")
        {
            sr.color = blue;
            door.SetActive(false);
        }
    }
}
