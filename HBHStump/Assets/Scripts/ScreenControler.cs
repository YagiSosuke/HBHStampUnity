using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

public class ScreenControler : MonoBehaviour
{
    [SerializeField] GameObject screen;
    float zoomSpeed = 0.05f;
    float moveSpeed = 1.0f;
    CancellationToken ct;
    
    async UniTask ZoomIn()
    {
        while (!ct.IsCancellationRequested)
        {
            await UniTask.WaitUntil(() => Input.GetKey(KeyCode.I), cancellationToken: ct);
            var scale = screen.transform.localScale;
            screen.transform.localScale = new Vector2(Mathf.Clamp(scale.x + (zoomSpeed * Time.deltaTime), 0.01f, 0.1f),
                                                      Mathf.Clamp(scale.y + (zoomSpeed * Time.deltaTime), 0.01f, 0.1f));
        }
    }
    async UniTask ZoomOut()
    {
        while (!ct.IsCancellationRequested)
        {
            await UniTask.WaitUntil(() => Input.GetKey(KeyCode.D), cancellationToken: ct);
            var scale = screen.transform.localScale;
            screen.transform.localScale = new Vector2(Mathf.Clamp(scale.x - (zoomSpeed * Time.deltaTime), 0.01f, 0.1f),
                                                      Mathf.Clamp(scale.y - (zoomSpeed * Time.deltaTime), 0.01f, 0.1f));
        }
    }

    async UniTask MoveRight()
    {
        while (!ct.IsCancellationRequested)
        {
            await UniTask.WaitUntil(() => Input.GetKey(KeyCode.RightArrow), cancellationToken: ct);
            var pos = screen.transform.position;
            screen.transform.position = new Vector2(pos.x + moveSpeed * Time.deltaTime, pos.y);
        }
    }
    async UniTask MoveLeft()
    {
        while (!ct.IsCancellationRequested)
        {
            await UniTask.WaitUntil(() => Input.GetKey(KeyCode.LeftArrow), cancellationToken: ct);
            var pos = screen.transform.position;
            screen.transform.position = new Vector2(pos.x - moveSpeed * Time.deltaTime, pos.y);
        }
    }
    async UniTask MoveUp()
    {
        while (!ct.IsCancellationRequested)
        {
            await UniTask.WaitUntil(() => Input.GetKey(KeyCode.UpArrow), cancellationToken: ct);
            var pos = screen.transform.position;
            screen.transform.position = new Vector2(pos.x, pos.y + moveSpeed * Time.deltaTime);
        }
    }
    async UniTask MoveDown()
    {
        while (!ct.IsCancellationRequested)
        {
            await UniTask.WaitUntil(() => Input.GetKey(KeyCode.DownArrow), cancellationToken: ct);
            var pos = screen.transform.position;
            screen.transform.position = new Vector2(pos.x, pos.y - moveSpeed * Time.deltaTime);
        }
    }

    void Start()
    {
        ct = this.GetCancellationTokenOnDestroy();

        ZoomIn().Forget();
        ZoomOut().Forget();

        MoveRight().Forget();
        MoveLeft().Forget();
        MoveUp().Forget();
        MoveDown().Forget();
    }
}
