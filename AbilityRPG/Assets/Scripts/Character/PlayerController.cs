﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public float Speed = 2.0f;         // 움직이는 스피드.
    
    protected Joystick joystick;
    protected Joybutton joybutton;
    
    public Animator animator;

    protected NavMeshAgent navAgent;
    public LayerMask layerMask;

    private Transform targetNPC;
    private bool isNpcTarget = false;
    public bool isTalk = false;
    public bool isPopup = false;

    // 임시로 플레이어 체력 감소 테스트용
    public PlayerHpBar playerHpBar;

    void Awake()
    {
        PlayerSetting();
    }

    public void PlayerSetting()
    {
        joystick = FindObjectOfType<Joystick>();
        joybutton = FindObjectOfType<Joybutton>();

        animator = GetComponent<Animator>();

        navAgent = GetComponent<NavMeshAgent>();
        navAgent.stoppingDistance = 0.7f;
        navAgent.speed = Speed;

        // 임시로 플레이어 체력 감소 테스트용
        playerHpBar = gameObject.transform.parent.transform.Find("Canvas").GetComponent<PlayerHpBar>();
    }

    public void FixedUpdate()
    {
        // 팝업창이 떠있는 경우
        if (isPopup)
            return;

        if (isPlayerMoving())
        {
            isNpcTarget = false;

            animator.SetBool("MOVE", true);
            animator.SetBool("THROW", false);
            animator.SetBool("IDLE", false);

            navAgent.SetDestination(this.transform.position);

            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, Mathf.Atan2(joystick.Horizontal, joystick.Vertical) * Mathf.Rad2Deg, 0f);
        }
        else if(isNpcTarget)
        {
            animator.SetBool("MOVE", true);

            // 거리 측정
            float dist = Vector3.Distance(transform.position, targetNPC.position);

            // 도착
            if (navAgent.stoppingDistance >= dist)
            {
                animator.SetBool("MOVE", false);

                // 대화 진행
                isNpcTarget = false;
                isTalk = true;
            }
        }
        else
            animator.SetBool("MOVE", false);

        NpcTargeting();
    }

    public bool isPlayerMoving()
    {
        if(joystick == null)
        {
            Debug.Log("조이스틱이 없음.");
            return false;
        }

        if (joystick.Vertical != 0 || joystick.Horizontal != 0)
            return true;
        else
            return false;
    }

    // npc 클릭시 npc 한테 이동
    public void NpcTargeting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 100f, layerMask))
            {
                if (hitInfo.transform.CompareTag("NPC"))
                {
                    isNpcTarget = true;
                    targetNPC = hitInfo.transform;
                    transform.LookAt(hitInfo.transform);
                    navAgent.SetDestination(hitInfo.transform.position);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Monster"))
            Debug.Log("몬스터로 공격 받음");

        if(other.transform.CompareTag("NextScene"))
            SceneLoadManager.Instance.LoadScene("2_BattleField");

        if (other.transform.CompareTag("BattleStart"))
            FieldManager.Instance.BattleStart();
    }
}
