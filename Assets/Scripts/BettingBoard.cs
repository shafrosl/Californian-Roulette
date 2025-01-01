using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Debug = Utility.DebugExtensions;

public class BettingBoard : GameItem
{
    private List<GameObject> grids = new();
    
    [Header("Board Settings")]
    public GridLayoutGroup gridLayoutGroup;
    
    [Header("Other Things")]
    public GameObject cell;
    public Button spinButton;

    #region Methods

        public override async void Init()
        {
            var gm = GameManager.Instance;
            gridLayoutGroup.constraintCount = gm.xSize;
            var counter = 0;
            for (var y = 0; y < gm.ySize; y++)
            {
                for (var x = 0; x < gm.xSize; x++)
                {
                    var newCell = Instantiate(cell, gridLayoutGroup.transform);
                    newCell.GetComponent<BoardGrid>().Init(gm.xSize % 2 == 0 ? 
                        ((x % gm.xSize) + (y % gm.xSize)) % gm.xSize : 
                        ((x % (gm.xSize - 1)) + (y % (gm.xSize - 1))) % (gm.xSize - 1), ++counter);
                    grids.Add(newCell);
                }
            }
            
            spinButton.onClick.RemoveAllListeners();
            spinButton.onClick.AddListener(GameManager.Instance.Confirm);
            base.Init();
            await Show();
        }
        
        #endregion
}
