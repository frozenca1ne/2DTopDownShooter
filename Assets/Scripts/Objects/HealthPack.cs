using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public float health;

    Player.Player player;

    private void Start()
    {
        player = FindObjectOfType<Player.Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        player.HealthUp(health);
        Destroy(gameObject);
    }
}
