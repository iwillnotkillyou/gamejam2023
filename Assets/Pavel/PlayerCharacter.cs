using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCharacter : MonoBehaviour
{
    private static bool showingDetails;

    public Sprite Grabbing;
    public Sprite Normal;

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

    public static List<GameObject> GetCollisions(Collider2D col)
    {
        var currentCollisions1 = ClueObject.ClueObjects.Where(x => x.activeInHierarchy)
            .Where(x => x != col.gameObject)
            .Select(x => x.GetComponent<Collider2D>())
            .Where(x => x != null).ToList();
        print(currentCollisions1.Count);
        var currentCollisions = currentCollisions1
            .Where(x => x.Distance(col).distance < 0.01)
            .Select(x => x.gameObject).Where(x =>
                x.GetComponent<ClueObject>() is not null).ToList();
        return currentCollisions;
    }

    private List<GameObject> GetCollisions()
    {
        var currentCollisions1 = ClueObject.ClueObjects.Where(x => x.activeInHierarchy)
            .Where(x => x != gameObject)
            .Select(x => x.GetComponent<Collider2D>())
            .Where(x => x != null).ToList();
        var currentCollisions = currentCollisions1
            .Where(x =>
                x.Distance(GetComponent<Collider2D>()).distance < 0.01)
            .Select(x => x.gameObject).Where(x =>
                x.GetComponent<ClueObject>() is not null).ToList();
        return currentCollisions;
    }
    
    public static bool EndGame;

    public static void Die()
    {
        ShowDetails(-2);
        EndGame = true;
    }

    private static void HideDetails()
    {
        uiPauseTime = 3f;
        showingDetails = false;
            SceneManager.GetActiveScene().GetRootGameObjects()
                .First(x => x.tag == "DetailCanvas").SetActive(false);
    }

    public static void ShowDetails(int id)
    {
        uiPauseTime = 3f;
        showingDetails = true;
        var o = SceneManager.GetActiveScene().GetRootGameObjects()
            .First(x => x.tag == "DetailCanvas");
            o.SetActive(true);
            o.GetComponent<DetailCanvas>().Show(id);
    }

    private static bool done = false;

    private void Start()
    {
        showingDetails = false;
        done = false;
        EndGame = false;
        ShowDetails(-1);
        Time.timeScale = 0f;
        uiPauseTime = 0f;
    }

    private const int ec = 1;

    private static float uiPauseTime = 0f;
    private void Update()
    {
        if (uiPauseTime > 0)
        {
            uiPauseTime -= Time.unscaledDeltaTime;
            return;
        }

        if (EndGame)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            return;
        }
        
        if (!EndGame && Input.anyKeyDown && showingDetails)
        {
            print("unpaused");
            HideDetails();
            Time.timeScale = 1f;
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.P) && !showingDetails)
        {
            print("paused");
            ShowDetails(-1);
            Time.timeScale = 0f;
        }
        else if (Input.GetKeyDown(KeyCode.Space) &&
                 (transform.childCount > ec) && !showingDetails)
        {
            print("details");
            ShowDetails(transform.GetChild(ec)
                    .GetComponent<ClueObject>().ID);
            Time.timeScale = 0f;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) &&
            (transform.childCount == ec))
        {
            var currentCollisions = GetCollisions();
            if (currentCollisions.Any())
            {
                var c = currentCollisions.First().transform;
                CommentController.main.Show(c.GetComponent<ClueObject>()
                    .ID);
                if (c.GetComponent<ClueObject>().ID == 10 && ClueObject.level >= 3)
                {
                    c.gameObject.SetActive(false);
                    ClueObject.Activate(11);
                }
                else
                {
                    c.SetParent(transform, true);
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Grabbing;
                    c.position += Vector3.back * 1;
                }
            }
        }
        else if (Input.GetMouseButtonDown(0) &&
                 (transform.childCount > ec))
        {
            var c = transform.GetChild(ec);
            var cs = GetCollisions(c.GetComponent<Collider2D>())
                .Where(x => x.transform != c);
            if (cs.Any())
            {
                var fo = cs.First().gameObject;
                var oId = fo.GetComponent<ClueObject>().ID;
                var id = transform.GetChild(ec)
                    .GetComponent<ClueObject>().ID;
                c.SetParent(null, true);
                ClueObject.MakeCreated(oId, id);
            }
            else
            {
                c.SetParent(null, true);
            }
            
            c.position -= Vector3.back*1;
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Normal;
        }

        var target
            = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = (target - transform.position).normalized;
        var rigidbody = GetComponent<Rigidbody2D>();
        if (direction.magnitude > 0)
        {
            //rigidbody.AddForce(rigidbody.mass * (0.1f * direction));
            rigidbody.velocity = (10-ClueObject.level/2) * direction.normalized;
            Debug.DrawRay(transform.position, direction, Color.white);
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.Sleep();
        }

        rigidbody.position = new Vector2(
            Mathf.Clamp(rigidbody.position.x, 0, ClueObject.Region.x),
            Mathf.Clamp(rigidbody.position.y, 0,
                ClueObject.Region.y));
    }
}