using UnityEngine;
using UnityEngine.EventSystems;
using Utility;

public class BoardGrid : MonoBehaviour, IHighlightHandler, IButtonHandler
{
    public Material highlightMaterial;
    public bool isHighlighted;
    
    private PolygonCollider2D _polygonCollider2D;

    public void Init(Material mat)
    {
        if (gameObject.TryGetComponent<PolygonCollider2D>(out var collider))
        {
            _polygonCollider2D = collider;
            highlightMaterial = mat;
        }
        
        PolygonColliderExtensions.CreatePolygon2DColliderPoints(gameObject.GetComponent<MeshFilter>(), gameObject.GetComponent<PolygonCollider2D>());
        CreateButton();
    }
    
    public void Highlight()
    {
        if (!isHighlighted)
        {
            if (!_polygonCollider2D) return;
            _polygonCollider2D.HighlightAroundCollider(Color.yellow, highlightMaterial);
            isHighlighted = true;
        }
        else
        {
            if (!_polygonCollider2D) return;
            _polygonCollider2D.RemoveHighlightAroundCollider();
            isHighlighted = false;
        }
    }

    public void CreateButton()
    {
        if (!gameObject.TryGetComponent<EventTrigger>(out var trigger))
        {
            trigger = gameObject.AddComponent<EventTrigger>();
        }
        
        var trigEvent = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        
        trigEvent.callback.RemoveAllListeners();
        trigEvent.callback.AddListener(OnAction);
        trigger.triggers.Add(trigEvent);
    }

    public void OnAction(BaseEventData eventData) => Highlight();
}
