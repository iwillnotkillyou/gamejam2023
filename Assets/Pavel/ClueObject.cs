using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClueObject : MonoBehaviour
{
    
    public static List<GameObject> ClueObjects = null;

    public static List<int> DontDeactivate = new(){2};

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

    public static Dictionary<int, int> spawns1
        = new()
        {
            { 19, 3 },
            { 23, 3 },
            { 21, 3 }
        };

    public static Dictionary<(int, int), int> recipesBase1
        = new()
        {
            { (4, 2), 18 },
            { (5, 6), 2 },
            { (10, 17), 9 },
            { (9, 8), 7 },
            { (7, 13), 3 },
            { (15, 16), 14 },
            { (14, 2), 13 },
            { (11, 12), 4 },
            { (21, 23), 22 }
        };

    public static int level = 0;
    public static Dictionary<int, int> spawns
        = new()
        {
            { 10, 1 },
            { 16, 1 },
            { 17, 1 },
            { 8, 2 },
            { 15, 2 },
            { 12, 3 },
            { 23, 3 },
            { 21, 3 }
        };

    public static Dictionary<(int, int), int> recipesBase
        = new()
        {
            { (4, 2), 18 },
            { (5, 6), 2 },
            { (10, 17), 9 },
            { (9, 8), 7 },
            { (7, 13), 3 },
            { (15, 16), 14 },
            { (14, 2), 13 },
            { (11, 12), 4 }
        };

    public static List<(HashSet<int>, int)> recipes = recipesBase
        .Select(x => (new HashSet<int> { x.Key.Item1, x.Key.Item2 },
            x.Value)).ToList();

    public static Vector2 Region = new(40, 30);

    [HideInInspector]
    public int ID;

    public static bool Contains((int id1, int id2) ids, int id)
    {
        return (ids.id1 == id) && (ids.id2 == id);
    }

    public static void DeActivate(int id)
    {
        if (DontDeactivate is not null && DontDeactivate.Contains(id))
        {
            print(id);
            return;
        }
        
        print($"removed {id}");
        var gos = ClueObjects;
        var o = gos.First(y => y.GetComponent<ClueObject>().ID == id);
        o.SetActive(false);
    }

    public static void Activate(int id, Vector3? poss = null)
    {
        print(id);
        var gos = ClueObjects;
        var o = gos.First(y => y.GetComponent<ClueObject>().ID == id);

        o.transform.position = poss ?? new Vector2(Random.Range(0, Region.x),
            Random.Range(0, Region.y));
        o.SetActive(true);
        if (poss is null)
        {
            int i = 0;
            while (PlayerCharacter.GetCollisions(o.GetComponent<Collider2D>()).Any() && i < 10)
            {
                o.transform.position = new Vector2(Random.Range(0, Region.x), Random.Range(0, Region.y));
                i++;
            }
        }
    }

    public static bool FinalCheck(int id1, int id2)
    {
        if (id1 == 18)
        {
            PlayerCharacter.ShowDetails(30+id2);
            PlayerCharacter.EndGame = true;
            return true;
        }

        return false;
    }

    public static void TriggerFinal()
    {
        foreach (var o in ClueObjects)
        {
            if (o.GetComponent<ClueObject>().ID != 18)
            {
                o.SetActive(false);
            }
        }
        
        for (int keyId = 30; keyId <= 37; keyId++)
        {
            Activate(keyId);
        }
    }

    public static void MakeCreated(int id1, int id2)
    {
        var hs = new HashSet<int> { id1, id2 };
        if (hs.Contains(18) && hs.Contains(3))
        {
            TriggerFinal();
        }
        
        if (hs.Contains(6))
        {
            SpawnEnemies.mainSpawner.SetActive(true);
            Camera.main.transform.GetChild(0).GetComponent<Lighter>().TurnIntoLighter();
            GameObject.FindGameObjectWithTag("GlobalLight").SetActive(false);
        }
        print(string.Join(",",hs.Select(x=>x.ToString())));
        var c1 = FinalCheck(id1, id2);
        var c2 = FinalCheck(id2, id1);
        if (c1 || c2)
        {
            return;
        }

        print(string.Join(",",recipes.Select(x=> string.Join(",",x.Item1.Select(x=>x.ToString()).Append(x.Item2.ToString())))));
       
        var vs = recipes.Select((x, i) => (x, ind: i)).Where(x => x.Item1.Item1.SetEquals(hs))
            ;
        if (!vs.Any())
        {
            print("fail");
            //fail sound
            return;
        }

        //play success sound
        //play appearing sound
        level++;
        if (level % 2 == 0)
        {
            SpawnEnemies.mainSpawner.GetComponent<SpawnEnemies>().Decay();
        }
        var r = vs.First();
        recipes.RemoveAt(r.ind);
        Activate(r.x.Item2, GameObject.FindGameObjectWithTag("Player").transform.position);
        DeActivate(id1);
        DeActivate(id2);
        var inActiveIds = ClueObjects.Where(x => !x.activeInHierarchy)
            .Select(x => x.GetComponent<ClueObject>().ID).ToList();
        foreach (var id in inActiveIds)
        {
            if (spawns.ContainsKey(id) && spawns[id] <= level)
            {
                Activate(id);
                if (id != 10)
                {
                    spawns.Remove(id);
                }
            }
        }
    }

    public static void F(List<int> activeIds)
    {
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