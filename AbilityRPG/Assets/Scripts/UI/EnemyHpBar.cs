﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    public Transform enemy;

    public Slider hpSlider;
    public Slider backHpSlider;
    public bool backHpHit = false;

    public float maxHp = 100f;
    public float currentHp = 100f;

    private void Start()
    {
        SetSlider();
    }

    void Update()
    {
        transform.position = enemy.position;
        hpSlider.value = Mathf.Lerp(hpSlider.value, currentHp / maxHp, Time.deltaTime * 5f);

        if(backHpHit)
        {
            backHpSlider.value = Mathf.Lerp(backHpSlider.value, hpSlider.value, Time.deltaTime * 10f);
            // 따라 잡으면
            if(hpSlider.value >= backHpSlider.value - 0.01f)
            {
                backHpHit = false;      // 초기화
                backHpSlider.value = hpSlider.value;
            }
        }
    }

    public void SetSlider()
    {
        maxHp = enemy.gameObject.GetComponent<EnemyLion>().maxHp;
        currentHp = maxHp;
    }

    public void Dmg(float atk)
    {
        currentHp -= atk;
        Invoke("BackHpStart", 0.5f);
    }

    void BackHpStart()
    {
        backHpHit = true;
    }
}