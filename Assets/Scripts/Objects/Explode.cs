using System.Collections;
using UnityEngine;

public class Explode : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float explosiveRadius;
    [SerializeField] private float damage;

    private Animator anim;
    private static readonly int Boom = Animator.StringToHash("Boom");

    private const int Delay = 1;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void MakeExplode(float damage)
    {
        health -= damage;
        if (health<=0)
        {
            StartCoroutine(ExplodeBarrel(Delay));
        }
    }
    private void DoExplosive()
    {
        LayerMask layerMask = LayerMask.GetMask("Player", "Enemy");
        var objectsInRadius = Physics2D.OverlapCircleAll(transform.position, explosiveRadius, layerMask);
        foreach (var objectsI in objectsInRadius)
        {
            if (objectsI.gameObject == gameObject)
            {
                continue;
            }
            var player = objectsI.gameObject.GetComponent<Player.Player>();
            if (player != null)
            {
                player.DoDamage(damage);
            }
            var enemy = objectsI.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DoDamage(damage);
            }
            var zombie = objectsI.gameObject.GetComponent<Zombie>();
            if (zombie != null)
            {
                zombie.DoDamage(damage);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damageDealer = collision.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            MakeExplode(damageDealer.damage);
        }
    }
    IEnumerator ExplodeBarrel(float delay)
    {
        anim.SetTrigger(Boom);
        DoExplosive();
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosiveRadius);
    }
}
