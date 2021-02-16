using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    private static bool _isOpened = false;

    //надо добавлять систему просчета количества объектов на столе, но времени нет и так, 
    //а затягивать как-то неудобно, но это не сложно реализовать

    private void Update()
    {
        if(_isOpened)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), 0.5f);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0, 1, 1), 0.5f);
        }
    }

    public static void OpenTable()
    {
        _isOpened = true;
    }

    public static void CloseTable()
    {
        _isOpened = false;
    }
}
