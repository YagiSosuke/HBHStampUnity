using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
クラス内でコルーチンを呼び出すためのスクリプト
*/

public class MyMonobehaviour : MonoBehaviour
{
    public void CallStartCoroutine(IEnumerator iEnumerator)
    {
        StartCoroutine(iEnumerator);
    }
}
