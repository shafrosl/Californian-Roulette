using Sirenix.OdinInspector;

public class NumberGrid : BoardGrid
{
    [ShowInInspector] protected ColorType colorType;
    
    public override void Init(int cIndex, int index, string customText = null)
    {
        border.color = cell.color = colors[cIndex];
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Highlight);
        colorType = (ColorType)cIndex;
        gridIndex = index - 1;
        text.text = index.ToString();
    }
}
