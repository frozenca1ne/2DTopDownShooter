using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;

public class Zombie : MonoBehaviour
{
    public Action onHealthChange;

    [Header("Move")]
    [SerializeField]
    private float followDistance;
    [SerializeField] private float attackDistance;

    [Header("Attack")]
    [SerializeField]
    private float attackRate;
    [SerializeField] private float damage;
    [SerializeField] private float angleVision = 45;

    [Header("Health")]
    [SerializeField]
    private bool isAlive;
    public float health;

    [SerializeField] private GameObject healthPack;

    private enum ZombieStates
    {
        Stand,
        Move,
        Attack
    }

    private ZombieStates activeState;

    private Animator anim;
    private Player.Player player;
    private Rigidbody2D rb;
    private ZombieMovement zombieMovement;
    private AIPath path;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Shoot = Animator.StringToHash("Shoot");
    private static readonly int Dead = Animator.StringToHash("Dead");

    public bool SetAlive()
    {
        return isAlive;
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        player = FindObjectOfType<Player.Player>();
        rb = GetComponent<Rigidbody2D>();
        zombieMovement = GetComponent<ZombieMovement>();
        path = GetComponent<AIPath>();

        ChangeState(ZombieStates.Stand);
    }
    private void FixedUpdate()
    {
        UpdateState();
        
    }
    private void UpdateState()
    {
        var distance = Vector2.Distance(transform.position, player.transform.position);

        switch (activeState)
        {
            case ZombieStates.Stand:
                if (distance <= followDistance)
                {
                    if(CheckPlayer()<= angleVision)
                    {
                        ChangeStateToMove(distance);
                    }
                }
                anim.SetFloat(Speed,path.velocity.magnitude );
                break;
            case ZombieStates.Move:
                if (distance <= attackDistance && player.SetAlive())
                {
                    ChangeState(ZombieStates.Attack);
                }
                Rotate();
                if (distance >= followDistance)
                {
                    ChangeState(ZombieStates.Stand);
                }
                anim.SetFloat(Speed, path.velocity.magnitude);
                break;
            case ZombieStates.Attack:
                if (distance > attackDistance || player.SetAlive() == false)
                {
                    ChangeState(ZombieStates.Move);

                }
                Rotate();
                anim.SetFloat(Speed, path.velocity.magnitude);                
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ChangeStateToMove(float distance)
    {
        Vector2 direction = player.transform.position - transform.position;
        LayerMask layerMask = LayerMask.GetMask("Walls");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, layerMask);
        if (hit.collider == null && player.SetAlive())
        {
            ChangeState(ZombieStates.Move);
        }
    }

    private void ChangeState(ZombieStates newState)
    {
        activeState = newState;
        switch (activeState)
        {
            case ZombieStates.Stand:
                zombieMovement.SetPatrol(true);
                break;
            case ZombieStates.Move:
                path.enabled = true;
                zombieMovement.SetAngry(false);
                zombieMovement.SetPatrol(false);
                StopAllCoroutines();
                break;
            case ZombieStates.Attack:
                zombieMovement.SetAngry(true);
                StartCoroutine(SetDamage(damage, attackRate));
                
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IEnumerator SetDamage(float damage, float rate)
    {
        while (isAlive)
        {
            yield return new WaitForSeconds(rate);
            player.DoDamage(damage);
            anim.SetTrigger(Shoot);
        }
    }
    private void Rotate()
    {
        if (!isAlive) return;
        var direction = player.transform.position - transform.position;
        transform.up -= direction;

    }
    public void DoDamage(float damage)
    {
        if (isAlive != true) return;
        health -= damage;
        onHealthChange();
        if (!(health <= 0)) return;
        isAlive = false;
        anim.SetBool(Dead, true);
        DropHealthPack();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damageDealer = collision.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            DoDamage(damageDealer.damage);
        }
    }
    private void DropHealthPack()
    {
        var chanceToDrop = UnityEngine.Random.Range(0, 10);
        if(chanceToDrop >= 5)
        {
            Instantiate(healthPack, transform.position, Quaternion.identity);
        }
    }
    private float CheckPlayer()
    {
        Vector2 direction = player.transform.position - transform.position;
        Vector2 zombieVision = -transform.up;
        var vision = Vector2.Angle(zombieVision, direction);
        return vision;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, followDistance);

        Gizmos.color = Color.magenta;
        var lookDirection = -transform.up;
        var v1 = Quaternion.AngleAxis(angleVision, Vector3.forward) * lookDirection;
        var v2 = Quaternion.AngleAxis(-angleVision, Vector3.forward) * lookDirection;
        Gizmos.DrawRay(transform.position, v1 * followDistance);
        Gizmos.DrawRay(transform.position, v2 * followDistance);
    }
}
