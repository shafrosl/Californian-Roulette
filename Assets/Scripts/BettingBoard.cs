using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utility;
using Debug = Utility.DebugExtensions;

public class BettingBoard : GameItem
{
    private List<GameObject> grids = new();
    
    [Header("Board Settings")]
    public GridLayoutGroup numbers;
    public GridLayoutGroup thirds;
    public GridLayoutGroup outsideBets;
    
    [Header("Other Things")]
    public GameObject cell;
    public GameObject cell1x;
    public GameObject cell2x;
    public GameObject cell4x;
    public Button spinButton;

    #region Methods

        public override async void Init()
        {
            var gm = GameManager.Instance;
            numbers.constraintCount = gm.xSize;
            var counter = 0;
            for (var y = 0; y < gm.ySize; y++)
            {
                for (var x = 0; x < gm.xSize; x++)
                {
                    var newCell = Instantiate(cell, numbers.transform);
                    newCell.GetComponent<BoardGrid>().Init(gm.xSize % 2 == 0 ? 
                        ((x % gm.xSize) + (y % gm.xSize)) % gm.xSize : 
                        ((x % (gm.xSize - 1)) + (y % (gm.xSize - 1))) % (gm.xSize - 1), ++counter);
                    grids.Add(newCell);

                    if (y != gm.ySize - 1 || x != gm.xSize - 1 ) continue;
                    {
                        for (var j = 0; j < gm.xSize; j++)
                        {
                            var newBtmCell = Instantiate(cell1x, numbers.transform);
                
                            var id = j switch
                            {
                                0 => 37,
                                1 => 38,
                                2 => 39,
                                _ => -1
                            };
                            
                            newBtmCell.GetComponent<BoardGrid>().Init(3, id, "2:1");
                            grids.Add(newCell);
                        }
                    }
                }
            }

            for (var i = 0; i < 3; i++)
            {
                var newCell = Instantiate(cell4x, thirds.transform);
                
                var customText = i switch
                {
                    0 => "1st Third",
                    1 => "2nd Third",
                    2 => "3rd Third",
                    _ => ""
                };
                
                var id = i switch
                {
                    0 => 41,
                    1 => 42,
                    2 => 43,
                    _ => -1
                };
                
                newCell.GetComponent<BoardGrid>().Init(3, id, customText);
                grids.Add(newCell);
            }
            
            for (var i = 0; i < 6; i++)
            {
                var newCell = Instantiate(cell2x, outsideBets.transform);

                var customText = i switch
                {
                    0 => "1 - 18",
                    1 => "Even",
                    2 => "Red",
                    3 => "Black",
                    4 => "Odd",
                    5 => "19 - 36",
                    _ => ""
                };
                
                var cIndex = i switch
                {
                    2 => 1,
                    3 => 0,
                    _ => 3
                };
                
                var id = i switch
                {
                    0 => 55,
                    1 => 52,
                    2 => 50,
                    3 => 51,
                    4 => 53,
                    5 => 54,
                    _ => -1
                };

                newCell.GetComponent<BoardGrid>().Init(cIndex, id, customText);
                grids.Add(newCell);
            }
            
            spinButton.onClick.RemoveAllListeners();
            spinButton.onClick.AddListener(Confirm);
            base.Init();
            await Show();
        }
        
        public bool AddSelectedNumber(int number)
        {
            var gmD = GameManager.Instance.gameData;
            if (!gmD.selectedNumbers.IsNotNull()) return false;
            if (gmD.selectedNumbers.Contains(number)) return false;
            if (gmD.selectedNumbers.Count >= 6) return false;
            gmD.selectedNumbers.Add(number);
            return true;
        }

        public bool RemoveSelectedNumber(int number)
        {
            var gmD = GameManager.Instance.gameData;
            if (!gmD.selectedNumbers.IsSafe()) return false;
            if (!gmD.selectedNumbers.Contains(number)) return false;
            gmD.selectedNumbers.Remove(number);
            return true;
        }
    
        public bool AddSelectedBet(int number)
        {
            var gmD = GameManager.Instance.gameData;
            if (!gmD.selectedBets.IsNotNull()) return false;
            if (gmD.selectedBets.Contains(number)) return false;
            gmD.selectedBets.Add(number);
            return true;
        }

        public bool RemoveSelectedBet(int number)
        {
            var gmD = GameManager.Instance.gameData;
            if (!gmD.selectedBets.IsSafe()) return false;
            if (!gmD.selectedBets.Contains(number)) return false;
            gmD.selectedBets.Remove(number);
            return true;
        }
    
        public async void Confirm()
        {
            var gm = GameManager.Instance;
            await gm.roulette.Show();
            await gm.bettingBoard.Hide();
        }
        
        #endregion
}
