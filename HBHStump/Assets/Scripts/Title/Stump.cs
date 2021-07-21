using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*タイトル、メニューでのスタンプイラスト*/

public class Stump : MonoBehaviour
{
    GameObject Stamp;
    Vector3 MousePos;

    // Start is called before the first frame update
    void Start()
    {
        Stamp = GameObject.Find("Stamp");
    }

    // Update is called once per frame
    void Update()
    {
        MousePos = Input.mousePosition;
        MousePos.z = 0;

        Stamp.transform.position = MousePos;
    }
}
