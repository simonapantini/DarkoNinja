using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown; // Tempo tra un attacco e l'altro
    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    [Header("SFX")]
    [SerializeField] private AudioClip attack1Sound;
    [SerializeField] private AudioClip attack3Sound;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }


    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack3())
            Attack3();

        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack1())
            Attack1();

        cooldownTimer += Time.deltaTime;
    }

    private void Attack3()
    {
        anim.SetTrigger("attack3");
        SoundManager.instance.PlaySound(attack3Sound);
        cooldownTimer = 0;
        

    }

    private void Attack1()
    {
        anim.SetTrigger("attack1");
        SoundManager.instance.PlaySound(attack1Sound);
        cooldownTimer = 0;
        
    }
}
