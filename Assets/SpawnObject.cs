using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public TextMeshProUGUI text;
     
    public void Spawn(string objectName)
    {
        text.text = objectName;
    }
}
