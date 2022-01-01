using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenControler : MonoBehaviour
{
    [SerializeField] GameObject screen;
    float zoomSpeed = 0.05f;
    float moveSpeed = 1.0f;

    void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            var scale = screen.transform.localScale;
            screen.transform.localScale = new Vector2(Mathf.Clamp(scale.x + (zoomSpeed * Time.deltaTime), 0.01f, 0.1f),
                                                      Mathf.Clamp(scale.y + (zoomSpeed * Time.deltaTime), 0.01f, 0.1f));
        }
        else if (Input.GetKey(KeyCode.O))
        {
            var scale = screen.transform.localScale;
            screen.transform.localScale = new Vector2(Mathf.Clamp(scale.x - (zoomSpeed * Time.deltaTime), 0.01f, 0.1f),
                                                      Mathf.Clamp(scale.y - (zoomSpeed * Time.deltaTime), 0.01f, 0.1f));
        }


        if (Input.GetKey(KeyCode.RightArrow))
        {
            var pos = screen.transform.position;
            screen.transform.position = new Vector2(pos.x + moveSpeed * Time.deltaTime, pos.y);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            var pos = screen.transform.position;
            screen.transform.position = new Vector2(pos.x - moveSpeed * Time.deltaTime, pos.y);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            var pos = screen.transform.position;
            screen.transform.position = new Vector2(pos.x, pos.y + moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            var pos = screen.transform.position;
            screen.transform.position = new Vector2(pos.x, pos.y - moveSpeed * Time.deltaTime);
        }
    }
}
