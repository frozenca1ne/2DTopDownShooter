using System.Collections;
using Player;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public float fireRate = 1;

    public GameObject bullet;
    public Transform shootPosition;

    private PlayerMovement player;
    private Animator anim;

    private bool isAlive;
    private static readonly int Dead = Animator.StringToHash("Dead");
    private static readonly int Shoot1 = Animator.StringToHash("Shoot");

    private void Start()
    {
        isAlive = true;
        player = FindObjectOfType<PlayerMovement>();
        anim = GetComponentInChildren<Animator>();

        StartCoroutine(Shoot(fireRate));
    }

    private void Update()
    {
        RotateToPlayer();

    }
    public void DoDamage(float damage)
    {
        if (isAlive != true) return;
        health -= damage;
        if (!(health <= 0)) return;
        isAlive = false;
        anim.SetBool(Dead, true);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damageDealer = collision.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            DoDamage(damageDealer.damage);
        }
    }
    private void RotateToPlayer()
    {
        if (!isAlive) return;
        var playerTransform = player.transform.position;
        var direction = transform.position - playerTransform;
        transform.up += direction;
    }

    private IEnumerator Shoot(float delay)
    {
        
        while(isAlive)
        {
            yield return new WaitForSeconds(delay);
            Instantiate(bullet, shootPosition.position,transform.rotation);
            anim.SetTrigger(Shoot1);
        }
        
    }

}
