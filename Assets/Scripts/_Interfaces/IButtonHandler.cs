using UnityEngine.EventSystems;

public interface IButtonHandler
{
    public void CreateButton();
    public void OnAction(BaseEventData eventData);
}
