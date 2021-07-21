using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*身体のパーツを選択するスクリプト*/

public class StumpScript : MonoBehaviour
{
    public static string TempStump;         //選択したスタンプ
    [SerializeField] GameObject Frame;      //選択したボタンの枠

    // Start is called before the first frame update
    void Start()
    {
        TempStump = "頭";
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("tempstump = " + TempStump);
    }

    //スタンプ名記録
    public void SetStump()
    {
        TempStump = this.gameObject.name;
        Frame.gameObject.transform.position = this.transform.position;
    }
}
