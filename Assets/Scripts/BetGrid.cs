public class BetGrid : BoardGrid
{
    public override void Init(int cIndex, int index, string customText = null)
    {
        border.color = cell.color = colors[cIndex];
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Highlight);
        gridIndex = index;
        text.text = customText ?? index.ToString();
    }
    
    public override void Highlight()
    {
        if (!isHighlighted)
        {
            if (!GameManager.Instance.AddSelectedBet(gridIndex)) return;
            border.color = highlightColor;
            isHighlighted = true;
        }
        else
        {
            if (!GameManager.Instance.RemoveSelectedBet(gridIndex)) return;
            border.color = cell.color;
            isHighlighted = false;
        }
    }
}
