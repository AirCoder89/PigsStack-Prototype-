using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour
{
    [SerializeField] private GameObject mark;
    void Update()
    {
        mark.SetActive(transform.childCount > 0);
    }
}
