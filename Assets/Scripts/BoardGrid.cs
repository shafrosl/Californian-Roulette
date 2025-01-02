using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public abstract class BoardGrid : MonoBehaviour, IHighlightHandler
{
    public bool isHighlighted;
    public Color[] colors;
    public Color highlightColor;
    public Image border, cell;
    public Button button;
    public TextMeshProUGUI text;
    protected int gridIndex;
    
    public virtual void Init(int cIndex, int index, string customText = null)
    {
        border.color = cell.color = colors[cIndex];
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Highlight);
        gridIndex = index - 1;
        text.text = index.ToString();
    }
    
    public virtual void Highlight()
    {
        if (!isHighlighted)
        {
            if (!GameManager.Instance.AddSelectedNumber(gridIndex)) return;
            border.color = highlightColor;
            isHighlighted = true;
        }
        else
        {
            if (!GameManager.Instance.RemoveSelectedNumber(gridIndex)) return;
            border.color = cell.color;
            isHighlighted = false;
        }
    }
}