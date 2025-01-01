using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Debug = Utility.DebugExtensions;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Items")]
    [SerializeReference] public Roulette roulette;
    [SerializeReference] public BettingBoard bettingBoard;
    
    [Header("Game Data")]
    [SerializeReference] public GameData gameData;
    [SerializeReference] public GameSettings gameSettings;
    
    [Header("Board Size")]
    [Range(3, 6), ValidateInput("ValidatePolygonSides", "Choose another number!"), SerializeField] public int xSize;
    [Range(1, 12), ValidateInput("ValidatePolygonSides", "Choose another number!"), SerializeField] public int ySize;
    [ReadOnly, ShowInInspector] public int polygonSides => xSize * ySize;
    
    private void Awake() => Instance = this;
    public int GetLandIndex() => gameData.landIndex;
    public void SetLandIndex(int index) => gameData.landIndex = index;
    private int[] GetExcludedNumbers() => gameSettings.excludeNumbers.IsSafe() ? gameSettings.excludeNumbers : null;
    
    public bool AddSelected(int number)
    {
        if (!gameData.selectedNumbers.IsNotNull()) return false;
        if (gameData.selectedNumbers.Contains(number)) return false;
        gameData.selectedNumbers.Add(number);
        return true;
    }

    public bool RemoveSelected(int number)
    {
        if (!gameData.selectedNumbers.IsSafe()) return false;
        if (!gameData.selectedNumbers.Contains(number)) return false;
        gameData.selectedNumbers.Remove(number);
        return true;
    }

    private async void Start()
    {
        await UniTask.Delay(100);
        roulette.Init();
        bettingBoard.Init();
        gameData.selectedNumbers.Clear();
    }

    public async void Confirm()
    {
        await roulette.Show();
        await bettingBoard.Hide();
    }
    
    public bool ValidatePolygonSides()
    {
        var excludedNumbers = GetExcludedNumbers();
        if (!excludedNumbers.IsSafe()) return false;
        if (excludedNumbers.Any(number => polygonSides == number)) return false;
        return (360 % polygonSides == 0);
    }
}