using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*煙のスクリプト*/

public class FogScript : MonoBehaviour
{
    float count = 0;

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;

        if(count > 10.0f)
        {
            //煙のオブジェクトを削除する
            Destroy(this.gameObject);
        }
    }
}
