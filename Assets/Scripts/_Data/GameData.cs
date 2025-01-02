using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Californian Roulette/Game Data", order = 2)]
public class GameData : ScriptableObject
{
    [SerializeField] public int landIndex;
    [SerializeField] public List<int> selectedNumbers = new();
    [SerializeField] public List<int> selectedBets = new();
    
    [Header("Player Data")]
    [SerializeField] public int playerTotal;
    [SerializeField] public int amountBet;

    public void CheckBetType()
    {
        
    }
    
    public bool CheckWinningNumbers() => selectedNumbers.Any(number => number == landIndex);

    public int CalculateWinnings()
    {
        return amountBet + (amountBet * playerTotal);
    }

    public bool ClearGameData()
    {
        selectedBets.Clear();
        selectedNumbers.Clear();
        landIndex = -1;
        return true;
    }
}