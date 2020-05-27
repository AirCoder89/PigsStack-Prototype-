using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using UnityEngine;
public enum BlockFaces
{
    Smile, Scary, Surprised
}

public class Block : MonoBehaviour
{
    public List<GameObject> faces;
    
    private bool _isOnDrag;
    private bool _isInTank;
    private Rigidbody2D _rigidBody;
    private int _index;
    private SpriteRenderer _spRenderer;
    private Collider2D _collider;
    
    public void Initialize(int index, Vector2 scale)
    {
        gameObject.name = "Block " + index.ToString();
        transform.localScale = scale;
        this._index = index;
        this._collider = GetComponent<Collider2D>();
        this._spRenderer = GetComponent<SpriteRenderer>();
        this._rigidBody = GetComponent<Rigidbody2D>();
        this._rigidBody.gravityScale = 0f;
        this._rigidBody.bodyType = RigidbodyType2D.Kinematic;
        this._rigidBody.mass = 10 * transform.localScale.x;
        this._isOnDrag = false;
        this._isInTank = true;
        
        gameObject.SetActive(false);
    }

    public void Grab()
    {
        AudioManager.Instance.PlayRandomPigGrab();
        _isOnDrag = true;
        SetFace(BlockFaces.Scary);
        var color = this._spRenderer.color;
        color.a = 0.5f;
        this._spRenderer.color = color;
        if (this._collider != null) this._collider.enabled = false;
        gameObject.SetActive(true);
    }
    
    public void Release()
    {
        _isOnDrag = false;
        var color = this._spRenderer.color;
        color.a = 1f;
        this._spRenderer.color = color;
        if (this._collider != null) this._collider.enabled = true;
        this._rigidBody.bodyType = RigidbodyType2D.Dynamic;
        this._rigidBody.gravityScale = 1.5f;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Block") || other.gameObject.CompareTag("Ground"))
        {
            AudioManager.Instance.PlayRandomPigHit();
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Block") || other.gameObject.CompareTag("Ground"))
        {
            SetFace(BlockFaces.Surprised, BlockFaces.Smile, 2f);
        }
    }

    private void Update()
    {
        if (_isOnDrag)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(pos.x,pos.y,0);
        }
    }

    public void Remove()
    {
        PoolManager.Pools[GameController.Instance.poolName].Despawn(this.transform);
    }
    
    public void SetFace(BlockFaces face)
    {
        StopAllCoroutines();
        foreach (var f in faces)
        {
            f.SetActive(false);
        }
        faces[(int) face].SetActive(true);
    }
    
    private void SetFace(BlockFaces face1,BlockFaces face2,float time)
    {
        SetFace(face1);
        StartCoroutine(WaitAndChangeFace(time, face2));
    }

    private IEnumerator WaitAndChangeFace(float time,BlockFaces face2)
    {
        yield return new WaitForSeconds(time);
        SetFace(face2);
    }
    
}
