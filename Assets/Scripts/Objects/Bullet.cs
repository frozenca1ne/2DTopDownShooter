using UnityEngine;
using Lean.Pool;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        rb.velocity = -transform.up * speed;
    }
    private void OnBecameInvisible()
    {
        if(gameObject.activeSelf)
        {
            LeanPool.Despawn(gameObject);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        LeanPool.Despawn(gameObject);
    }
}
