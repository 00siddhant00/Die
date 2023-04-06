using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementType : MonoBehaviour
{
    public enum TypeOfMovement
    {
        None,
        Move,
        Rotate
    }
    public TypeOfMovement movementType;

    //[SerializeField] private float length;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool clockWise;

    public int statingPoint;
    [SerializeField] private Transform[] points;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        //startPos = transform.position;
        if (movementType == TypeOfMovement.Move)
            transform.position = points[statingPoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        SelectMovementType();
    }

    void SelectMovementType()
    {
        switch (movementType)
        {
            case TypeOfMovement.None:
                transform.rotation = Quaternion.identity;
                break;
            case TypeOfMovement.Move:
                Move();
                break;
            case TypeOfMovement.Rotate:
                Rotate(rotationSpeed * 100, clockWise);
                break;
        }
    }

    Vector3 LerpBetweenMinusOneAndOne(Vector3 startPos, Vector3 endPos, float speed)
    {
        float t = Oscillate(speed, 1);
        //t = Mathf.InverseLerp(0, 1, t);
        return Vector3.Lerp(startPos, endPos, t);
        //float t = Mathf.Clamp01(Mathf.PingPong(Time.time * speed, 2f) - 1f); // oscillate between -1 and 1 based on time and speed
        //Vector3 lerpPos = new Vector3(t, 0f, 0f); // or use whichever axis you need
        //return Vector3.Lerp(startPos, endPos, (t + 1f) / 2f); // interpolate between startPos and endPos based on t
    }

    public float Oscillate(float speed, float max)
    {
        float t = Time.time * speed;
        return Mathf.PingPong(t, max);
    }

    int i;
    bool switchL;

    void Move()
    {
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[i].position, moveSpeed * Time.deltaTime);
    }

    void Rotate(float rot_speed, bool clockWise)
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * (rot_speed = clockWise ? -rot_speed : rot_speed));
    }
}
