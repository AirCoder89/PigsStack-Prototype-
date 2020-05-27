using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BlockIcon : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text sizeTxt;

    private int _index;
    
    public void Initialize(Sprite sprite, int size, int index)
    {
        this._index = index;
        gameObject.name = "Icon " + index.ToString();
        this.image.sprite = sprite;
        this.sizeTxt.text = size.ToString();
    }

}
