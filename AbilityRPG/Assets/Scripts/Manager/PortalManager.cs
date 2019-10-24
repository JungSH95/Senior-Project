﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public GameObject L_potal;
    public GameObject T_potal;
    public GameObject R_potal;
    public GameObject B_potal;

    public Transform movePos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.transform.position = movePos.position;
        }
    }
}
