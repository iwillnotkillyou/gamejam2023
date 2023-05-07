using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailCanvas : MonoBehaviour
{
    public List<int> Ids = new List<int>();
    public List<Sprite> Details = new List<Sprite>();
    public void Show(int id)
    {
        if (Ids.Contains(id))
        {
            GetComponent<Image>().sprite = Details[Ids.FindIndex(x => x == id)];
        }
        else
        {
            GetComponent<Image>().sprite = ClueObject.ClueObjects.First(y => y.GetComponent<ClueObject>().ID == id).GetComponent<SpriteRenderer>().sprite;
        }
    }
}
