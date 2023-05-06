using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClueObject : MonoBehaviour
{
    
    public static List<GameObject> ClueObjects = null;

    public static int? DontDeactivate = 2;

    public void Start()
    {
        if (ClueObjects is null)
        {
            ClueObjects = GetClueObjects();
            List<int> starting = new List<int>() { 1,5,6 };
            foreach (var n in starting)
            {
                Activate(n);
            }
        }
    }

    public List<GameObject> GetClueObjects()
    {
        List<GameObject> r = new List<GameObject>();
        var os = new List<Transform>();
        foreach (Transform c in GameObject.FindGameObjectWithTag("Objects").transform)
        {
            os.Add(c);
        }

        foreach (var c in os)
        {
            c.SetParent(null,true);
            c.GetComponent<ClueObject>().ID = int.Parse(c.name.Split('-').First());
            r.Add(c.gameObject);
            c.gameObject.SetActive(false);
            c.transform.localScale = new Vector3(0.2f,0.2f,1);
        }
        return r;
    }

    public static Dictionary<(int, int), int> recipesBase
        = new()
        {
            { (5, 6), 2 },
            { (18, 17), 9 },
            { (9, 8), 7 },
            { (7, 13), 3 },
            { (15, 16), 14 },
            { (14, 2), 3 },
            { (11, 12), 4 }
        };

    public static List<(HashSet<int>, int)> recipes = recipesBase
        .Select(x => (new HashSet<int> { x.Key.Item1, x.Key.Item2 },
            x.Value)).ToList();

    public static Vector2 Region = new(5, 5);

    [HideInInspector]
    public int ID;

    public static bool Contains((int id1, int id2) ids, int id)
    {
        return (ids.id1 == id) && (ids.id2 == id);
    }

    public static void DeActivate(int id)
    {
        if (DontDeactivate is not null && DontDeactivate.Value == id)
        {
            return;
        }

        //play appearing sound
        print($"removed {id}");
        var gos = ClueObjects;
        var o = gos.First(y => y.GetComponent<ClueObject>().ID == id);
        o.SetActive(false);
    }

    public static void Activate(int id)
    {
        print(id);
        //play appearing sound
        var gos = ClueObjects;
        var o = gos.First(y => y.GetComponent<ClueObject>().ID == id);

        o.transform.position = new Vector2(Random.Range(0, Region.x),
            Random.Range(0, Region.y));
        o.SetActive(true);
    }

    public static void MakeCreated(int id1, int id2)
    {
        var hs = new HashSet<int> { id1, id2 };
        if (hs.Contains(6))
        {
            Camera.main.transform.GetChild(0).GetComponent<Lighter>().TurnIntoLighter();
        }
        print(string.Join(",",recipes.Select(x=> string.Join(",",x.Item1.Select(x=>x.ToString()).Append(x.Item2.ToString())))));
        print(string.Join(",",hs.Select(x=>x.ToString())));
        var vs = recipes.Where(x => x.Item1.SetEquals(hs))
            .Select((x, i) => (x, ind: i));
        if (!vs.Any())
        {
            print("fail");
            //fail sound
            return;
        }

        //play success sound

        var r = vs.First();
        recipes.RemoveAt(r.ind);
        Activate(r.x.Item2);
        DeActivate(id1);
        DeActivate(id2);
        var gos = GameObject.FindGameObjectsWithTag("ClueObject");
        var activeIds = gos.Where(x => x.activeInHierarchy)
            .Select(x => x.GetComponent<ClueObject>().ID).ToList();
        foreach (var x in recipes)
        {
            var first = x.Item1.First();
            var second = x.Item1.Skip(1).First();
            if (activeIds.Contains(first) &&
                !activeIds.Contains(second))
            {
                Activate(second);
            }
            else if (activeIds.Contains(second) &&
                     !activeIds.Contains(first))
            {
                Activate(first);
            }
        }
    }
}