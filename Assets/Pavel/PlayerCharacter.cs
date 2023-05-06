using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCharacter : MonoBehaviour
{
    private List<GameObject> GetCollisionsEnemy()
    {
        var currentCollisions1 = SceneManager.GetActiveScene()
            .GetRootGameObjects().Where(x => x != gameObject)
            .Select(x => x.GetComponent<Collider2D>())
            .Where(x => x != null).ToList();
        var currentCollisions = currentCollisions1
            .Where(x =>
                x.Distance(GetComponent<Collider2D>()).distance < 0.1)
            .Select(x => x.gameObject).ToList();
        return currentCollisions;
    }

    private List<GameObject> GetCollisions()
    {
        var currentCollisions1 = GameObject
            .FindGameObjectsWithTag("ClueObject")
            .Where(x => x != gameObject)
            .Select(x => x.GetComponent<Collider2D>())
            .Where(x => x != null).ToList();
        print(string.Join(",",currentCollisions1.Select(x=>x.name)));
        var currentCollisions = currentCollisions1
            .Where(x =>
                x.Distance(GetComponent<Collider2D>()).distance < 0.1)
            .Select(x => x.gameObject).Where(x =>
                x.GetComponent<ClueObject>() is not null).ToList();
        return currentCollisions;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) &&
            (transform.childCount == 0))
        {
            var currentCollisions = GetCollisions();
            foreach (var gObject in currentCollisions)
            {
                print(gObject.name);
            }

            if (currentCollisions.Any())
            {
                var c = currentCollisions.First().transform;
                c.SetParent(transform, true);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (transform.childCount > 0)
            {
                var cs = GetCollisions();
                if (cs.Any())
                {
                    var fo = cs.First().gameObject;
                    var oId = fo.GetComponent<ClueObject>().ID;
                    ClueObject.MakeCreated(oId,
                        transform.GetChild(0)
                            .GetComponent<ClueObject>().ID);
                }
                else
                {
                    var c = transform.GetChild(0);
                    c.SetParent(null, true);
                }
            }
        }

        var target
            = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = (target - transform.position).normalized;
        var rigidbody = GetComponent<Rigidbody2D>();
        if (direction.magnitude > 0)
        {
            //rigidbody.AddForce(rigidbody.mass * (0.1f * direction));
            rigidbody.velocity = 10 * direction.normalized;
            Debug.DrawRay(transform.position, direction, Color.white);
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.Sleep();
        }
    }
}