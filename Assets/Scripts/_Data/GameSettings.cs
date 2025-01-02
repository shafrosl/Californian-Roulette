using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Californian Roulette/Game Settings", order = 1)]
public class GameSettings : ScriptableObject
{
    [SerializeField] public int[] excludeNumbers = { 4, 5, 8, 10, 20, 40 };
    [SerializeField] public List<Bet> bets = new();
}