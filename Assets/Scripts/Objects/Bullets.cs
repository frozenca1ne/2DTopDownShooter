using UnityEngine;

public class Bullets : MonoBehaviour
{

    [SerializeField] private int bullets;

    Player.Player player;

    private void Start()
    {
        player = FindObjectOfType<Player.Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        player.AddBullets(bullets);
        Destroy(gameObject);
    }
}
