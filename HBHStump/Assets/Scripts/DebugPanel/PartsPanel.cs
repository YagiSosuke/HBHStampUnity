using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*身体のパーツを選択するスクリプト*/

public class PartsPanel : MonoBehaviour
{
    [SerializeField] Parts parts;
    [SerializeField] GameObject Frame;


    public void SetParts()
    {
        Stamp.Instance.SetParts(parts);
        Frame.gameObject.transform.position = transform.position;
    }
}
