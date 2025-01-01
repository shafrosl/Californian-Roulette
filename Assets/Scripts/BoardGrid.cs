using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class BoardGrid : MonoBehaviour, IHighlightHandler
{
    public bool isHighlighted;
    public Color[] colors;
    public Color highlightColor;
    public Image border, cell;
    public Button button;
    public TextMeshProUGUI text;
    private int gridIndex;
    
    public void Init(int cIndex, int index)
    {
        border.color = cell.color = colors[cIndex];
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Highlight);
        gridIndex = index - 1;
        text.text = index.ToString();
    }
    
    public void Highlight()
    {
        if (!isHighlighted)
        {
            if (!GameManager.Instance.AddSelected(gridIndex)) return;
            border.color = highlightColor;
            isHighlighted = true;
        }
        else
        {
            if (!GameManager.Instance.RemoveSelected(gridIndex)) return;
            border.color = cell.color;
            isHighlighted = false;
        }
    }
}