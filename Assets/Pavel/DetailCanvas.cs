using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DetailCanvas : MonoBehaviour
{
    public List<int> Ids = new List<int>();
    public List<Texture2D> Details = new List<Texture2D>();
    public void Show(int id)
    {
        transform.GetChild(0).GetComponent<Image>().image = Details[Ids.FindIndex(x => x == id)];
    }
}
