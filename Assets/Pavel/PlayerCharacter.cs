using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;

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

    public static List<GameObject> GetEnemies(Collider2D col)
    {
        var currentCollisions1 = GameObject
            .FindGameObjectsWithTag("Enemy")
            .Where(x => x != col.gameObject)
            .Select(x => x.GetComponent<Collider2D>())
            .Where(x => x != null).ToList();
        print(currentCollisions1.Count);
        var currentCollisions = currentCollisions1
            .Where(x => x.Distance(col).distance < 0.1)
            .Select(x => x.gameObject).ToList();
        return currentCollisions;
    }

    private static List<GameObject> GetCollisions(Collider2D col)
    {
        var currentCollisions1 = GameObject
            .FindGameObjectsWithTag("ClueObject")
            .Where(x => x != col.gameObject)
            .Select(x => x.GetComponent<Collider2D>())
            .Where(x => x != null).ToList();
        print(currentCollisions1.Count);
        var currentCollisions = currentCollisions1
            .Where(x => x.Distance(col).distance < 0.1)
            .Select(x => x.gameObject).Where(x =>
                x.GetComponent<ClueObject>() is not null).ToList();
        return currentCollisions;
    }

    private List<GameObject> GetCollisions()
    {
        var currentCollisions1 = GameObject
            .FindGameObjectsWithTag("ClueObject")
            .Where(x => x != gameObject)
            .Select(x => x.GetComponent<Collider2D>())
            .Where(x => x != null).ToList();
        print(currentCollisions1.Count);
        var currentCollisions = currentCollisions1
            .Where(x =>
                x.Distance(GetComponent<Collider2D>()).distance < 0.1)
            .Select(x => x.gameObject).Where(x =>
                x.GetComponent<ClueObject>() is not null).ToList();
        return currentCollisions;
    }

    private bool showingDetails = false;

    private void HideDetails()
    {
        SceneManager.GetActiveScene().GetRootGameObjects()
            .First(x => x.tag == "DetailCanvas").SetActive(false);
    }

    private void ShowDetals(int id)
    {
        var o = SceneManager.GetActiveScene().GetRootGameObjects()
            .First(x => x.tag == "DetailCanvas");
        if (o.GetComponent<DetailCanvas>().Ids.Contains(id))
        {
            o.SetActive(true);
            o.GetComponent<DetailCanvas>().Show(id);
        }
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
        else if (Input.GetKeyDown(KeyCode.Space) &&
                 transform.childCount > 0)
        {
            if (showingDetails)
            {
                HideDetails();
            }
            else
            {
                ShowDetals(transform.GetChild(0).GetComponent<ClueObject>().ID);
            }
        }
        else if (Input.GetMouseButtonDown(0) &&
                 (transform.childCount > 0))
        {
            if (showingDetails)
            {
                HideDetails();
            }
            var c = transform.GetChild(0);
            var cs = GetCollisions(c.GetComponent<Collider2D>())
                .Where(x => x.transform != c);
            if (cs.Any())
            {
                var fo = cs.First().gameObject;
                var oId = fo.GetComponent<ClueObject>().ID;
                var id = transform.GetChild(0)
                    .GetComponent<ClueObject>().ID;
                c.SetParent(null, true);
                ClueObject.MakeCreated(oId, id);
            }
            else
            {
                c.SetParent(null, true);
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