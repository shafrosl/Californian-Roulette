using System.Collections.Generic;
using UnityEngine;
using Utility;
using Debug = Utility.DebugExtensions;

public class BettingBoard : MonoBehaviour
{
    private List<GameObject> grids = new();
    
    [Header("Board Settings")]
    public GameObject board;
    [Range(3, 6)] public int xSize;
    [Range(1, 12)] public int ySize;
    [Range(0.1f, 2)] public float xLength;
    [Range(0.1f, 2)] public float yLength;
    
    [Header("Other Things")]
    public Material boardMaterial;
    public Material highlightMaterial;

    void Start() => Init();

    #region Methods

        private void Init()
        {
            grids = MeshExtensions.DrawGrid(xSize, ySize, boardMaterial, xLength, yLength);
            grids.ForEach(x =>
            {
                x.transform.SetParent(board.transform);
                x.AddComponent<BoardGrid>().Init(highlightMaterial);
            });
            
            CentraliseBoard();
        }

        private void CentraliseBoard() => board.transform.position = board.transform.position.Modify(x: -(float)xSize/2 * xLength, y: (-(float)ySize/2) * yLength);

        #endregion
}
