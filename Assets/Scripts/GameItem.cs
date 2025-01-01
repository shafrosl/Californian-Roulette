using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utility;

public class GameItem : MonoBehaviour
{
    [Header("Game Object")]
    public GameObject itemRenderer;

    [Header("State")] 
    public bool isInitialized;
    
    //void Start() => Init();

    public virtual void Init()
    {
        isInitialized = true;
    }

    public virtual async Task Show()
    {
        await UniTask.WaitUntil(() => isInitialized).SuppressCancellationThrow();
        var list = transform.GetAllChildrenInTransform(out var count);
        if (count <= 0) return;
        foreach (var item in list) item.gameObject.SetActive(true);
    }

    public virtual async Task Hide()
    {
        await UniTask.WaitUntil(() => isInitialized).SuppressCancellationThrow();
        var list = transform.GetAllChildrenInTransform(out var count);
        if (count <= 0) return;
        foreach (var item in list) item.gameObject.SetActive(false);
    }
}
