using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CommentController : MonoBehaviour
{
    public static CommentController main;
    public List<int> Ids = new List<int>();
    public List<string> Comments = new List<string>();

    public void Start()
    {
        main = this;
    }

    public void Show(int id)
    {
        if (Ids.Contains(id))
        {
            GetComponent<TextMeshProUGUI>().text = Comments[Ids.FindIndex(x => x == id)];
        }
        GetComponent<TextMeshProUGUI>().text = "";
    }
}
