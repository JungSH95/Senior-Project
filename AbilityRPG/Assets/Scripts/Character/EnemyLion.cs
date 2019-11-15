﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLion : EnemyFSM
{
    // 사거리 표시
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private new void Start()
    {
        base.Start();

        haveExp = 1;

        attackRange = 2f;
        navAgent.stoppingDistance = 2f;

        attackCoolTime = 3f;
        attackCoolTimeCacl = 3f;
    }

    // 몬스터 능력치 조정
    protected override void InitMonster()
    {
        maxHp = 100f;
        currentHp = maxHp;
    }
}