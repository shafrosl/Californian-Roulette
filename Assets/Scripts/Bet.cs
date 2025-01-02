using System;
using UnityEngine;

[Serializable]
public class Bet
{
    [SerializeReference] public BetType betType;
    [SerializeReference] public int odds;
}