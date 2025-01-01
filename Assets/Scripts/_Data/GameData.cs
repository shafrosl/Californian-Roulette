using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Californian Roulette/Game Data", order = 2)]
public class GameData : ScriptableObject
{
    [SerializeField] public int landIndex;
    [SerializeField] public List<int> selectedNumbers = new();
}