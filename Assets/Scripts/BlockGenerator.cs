using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathologicalGames;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public struct BlockInfo
{
    public BlockSizes size;
    public Vector2 scale;
}

public enum BlockSizes
{
    x1,x2,x3,x4,x5,x6,x7
}

public enum BlockType
{
    Box, Circle, Triangle
}

[System.Serializable]
public struct PrefabTypes
{
    public BlockType type;
    public string blockName;
    public Sprite icon;
}

public class BlockGenerator : MonoBehaviour
{
    [TabGroup("Initialize")][SerializeField] private List<BlockInfo> levelBlocks;
    [TabGroup("Initialize")] [SerializeField]
    private List<PrefabTypes> prefabs;
    [TabGroup("Initialize")][SerializeField] private string iconName;
    [TabGroup("Initialize")][SerializeField] private RectTransform iconsHolder;
    [TabGroup("Initialize")][SerializeField] private Transform blockHolder;

    private List<Block> _blocks;
    private Block _currentBlock;
    private int _currentIndex;
    private bool _isTankEmpty;
    public void Initialize(float firstScale)
    {
        levelBlocks = new List<BlockInfo>();
        for (var i = 0; i < 7; i++)
        {
            var scale = firstScale * (i + 1);
            levelBlocks.Add(new BlockInfo()
            {
                size = (BlockSizes)i,
                scale = new Vector2(scale,scale)
            });
        }
    }
    
    public void Generate(Level level)
    {
        GameController.Instance.tank.SetActive(true);
        Clear();
        _currentIndex = 0;
        _currentBlock = null;
        var poolName = GameController.Instance.poolName;
        this._blocks = new List<Block>();
        foreach (var levelBlock in level.blocks)
        {
            var type = GetBlockType(levelBlock.type);
            var block = PoolManager.Pools[poolName].Spawn(type.blockName, this.blockHolder).gameObject
                .GetComponent<Block>();
            
            block.Initialize(this._blocks.Count,GetScale(levelBlock.size));

            var bIcon = PoolManager.Pools[poolName].Spawn(this.iconName, this.iconsHolder).gameObject.GetComponent<BlockIcon>();
            bIcon.Initialize(type.icon, ((int)levelBlock.size + 1), this._blocks.Count);
            
            _blocks.Add(block);
            
        }

        _isTankEmpty = false;
    }

    public void Clear()
    {
        foreach (Transform icon in iconsHolder)
        {
            PoolManager.Pools[GameController.Instance.poolName].Despawn(icon);
        }

        if(_blocks == null) return;
        foreach (var block in this._blocks)
        {
            block.Remove();
        }
        this._blocks.Clear();
    }
    
    public void ProvideBlock()
    {
        if(_isTankEmpty) return;
        _currentBlock = _blocks[_currentIndex];
        _currentBlock.Grab();
        GameObject.Destroy(iconsHolder.GetChild(0).gameObject);
    }

    public void ReleaseBlock()
    {
        if(_isTankEmpty) return;
        if(_currentBlock == null) return;
        _currentIndex++;
        if (_currentIndex >= _blocks.Count)
        {
            _isTankEmpty = true;
            SetAllBlocksSurprised();
            GameController.Instance.StartCounting();
        }
        
        _currentBlock?.Release();
        _currentBlock = null;
    }

    private void SetAllBlocksSurprised()
    {
        foreach (var block in _blocks)
        {
            block.SetFace(BlockFaces.Surprised);
        }
    }

    private Vector2 GetScale(BlockSizes size)
    {
        return this.levelBlocks.FirstOrDefault(b => b.size == size).scale;
    }
    private PrefabTypes GetBlockType(BlockType type)
    {
        return this.prefabs.FirstOrDefault(b => b.type == type);
    }
    
}
