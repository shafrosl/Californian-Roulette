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
        var gmBB = GameManager.Instance.bettingBoard;
        if (!isHighlighted)
        {
            if (!gmBB.AddSelectedBet(gridIndex)) return;
            border.color = highlightColor;
            isHighlighted = true;
        }
        else
        {
            if (!gmBB.RemoveSelectedBet(gridIndex)) return;
            border.color = cell.color;
            isHighlighted = false;
        }
    }
}
