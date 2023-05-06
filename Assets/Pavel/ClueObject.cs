using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClueObject : MonoBehaviour
{
    public static Dictionary<(int, int), int> recipesBase
        = new() { { (16, 17), 9 } };

    public static List<(HashSet<int>, int)> recipes = recipesBase
        .Select(x => (new HashSet<int> { x.Key.Item1, x.Key.Item1 },
            x.Value)).ToList();

    public static Vector2 Region = new(20, 20);

    public int ID;

    public static bool Contains((int id1, int id2) ids, int id)
    {
        return (ids.id1 == id) && (ids.id2 == id);
    }

    public static void Activate(int id)
    {
        //play appearing sound
        var gos = GameObject.FindGameObjectsWithTag("ClueObject");
        var o = gos.First(y => y.GetComponent<ClueObject>().ID == id);

        o.transform.position = new Vector2(Random.Range(0, Region.x),
            Random.Range(0, Region.y));
        o.SetActive(true);
    }

    public static void MakeCreated(int id1, int id2)
    {
        var hs = new HashSet<int>() { id1, id2 };
        var vs = recipes.Where(x => x.Item1.SetEquals(hs)).Select((x,i) => (x,ind : i));
        if (!vs.Any())
        {
            //fail sound
            return;
        }

        //play success sound

        var r = vs.First();
        recipes.RemoveAt(r.ind);
        Activate(r.x.Item2);
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