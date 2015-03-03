using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Scripts/Platforms/Follow Path")]
public class FollowPath : MonoBehaviour
{
    public enum FollowType
    {
        MoveTowards,
        Lerp
    }

    public FollowType Type = FollowType.MoveTowards;
    public PathDefinition Path;
    public float Speed = 1;
    public float MaxDistanceToGoal = 0.1f;
    [HideInInspector]
    public Vector2 velocity = Vector2.zero;

    private IEnumerator<Transform> currentPoint;
    private Vector3 prevLocation;

    public void Start()
    {
        if (Path == null)
        {
            Debug.LogError("Path cannot be null", gameObject);
            return;
        }

        currentPoint = Path.GetPathEnumerator();
        currentPoint.MoveNext();

        if (currentPoint.Current == null)
            return;

        transform.position = currentPoint.Current.position;
        prevLocation = transform.position;
    }

    public void Update()
    {
        if (currentPoint == null || currentPoint.Current == null)
            return;

        /*if (Type == FollowType.MoveTowards)
            rigidbody2D.position = Vector3.MoveTowards(transform.position, currentPoint.Current.position, Time.deltaTime * Speed);
        else if (Type == FollowType.Lerp)
            transform.position = Vector3.Lerp(transform.position, currentPoint.Current.position, Time.deltaTime * Speed);

        velocity = (transform.position - prevLocation) / Time.deltaTime;
        prevLocation = transform.position;*/

        float distanceSquared = (transform.position - currentPoint.Current.position).sqrMagnitude;
        if (distanceSquared < MaxDistanceToGoal * MaxDistanceToGoal)
            currentPoint.MoveNext();
    }

    public void FixedUpdate()
    {
        Vector2 direction = (currentPoint.Current.position - transform.position).normalized;
        Vector2 distance = direction * Speed;
        GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + distance * Time.deltaTime);

        /*if (Type == FollowType.MoveTowards)
            rigidbody2D.position = Vector2.MoveTowards(transform.position, currentPoint.Current.position, Time.deltaTime * Speed);
        else if (Type == FollowType.Lerp)
            transform.position = Vector3.Lerp(transform.position, currentPoint.Current.position, Time.deltaTime * Speed);*/

        velocity = (transform.position - prevLocation) / Time.deltaTime;
        prevLocation = transform.position;
    }
}
