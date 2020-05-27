using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public struct LevelBlock
{
    public BlockType type;
    public BlockSizes size;
}

[CreateAssetMenu(menuName = "Level", fileName = "New Level")]
public class Level : ScriptableObject
{
    [TableList] public List<LevelBlock> blocks;
}