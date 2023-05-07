using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DetailCanvas : MonoBehaviour
{
    public List<int> Ids = new List<int>();
    public List<Sprite> Details = new List<Sprite>();
    public void Show(int id)
    {
        GetComponent<SpriteRenderer>().sprite = Details[Ids.FindIndex(x => x == id)];
    }
}
