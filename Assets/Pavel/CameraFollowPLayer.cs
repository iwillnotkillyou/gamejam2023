using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPLayer : MonoBehaviour
{
    public GameObject Player;

    public float Velocity = 50;

    public void Update()
    {
        var target
            = Player.transform.position + Vector3.left*1.8f;
        var direction = (target - transform.position).normalized;
        var rigidbody = GetComponent<Rigidbody2D>();
        if (direction.magnitude > 0)
        {
            //rigidbody.AddForce(rigidbody.mass * (0.1f * direction));
            rigidbody.velocity = Velocity * new Vector2(direction.normalized.x,direction.normalized.y);
            Debug.DrawRay(transform.position, direction, Color.white);
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.Sleep();
        }
    }
}
