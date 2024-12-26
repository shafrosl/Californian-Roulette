using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static readonly GameManager instance = new();
    public static GameManager Instance => instance;

    public Roulette roulette;
    public BettingBoard bettingBoard;
}
