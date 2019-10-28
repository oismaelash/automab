using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tests : MonoBehaviour
{
    public int number;
    public string text;

    [ContextMenu("ConvertIntToByteArray")]
    public void ConvertIntToByteArray()
    {
        foreach (var item in number.IntToByteArray())
        {
            print(item);
        }
    }
}
