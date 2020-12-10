using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 8f;

        private Rigidbody2D rb;
        private Animator anim;
        
        private Player player;
        
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponentInChildren<Animator>();
            player = FindObjectOfType<Player>();
        }

        private void Update()
        {
        
            Rotate();

        }
        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void Rotate()
        {
            if (player.SetAlive() != true) return;
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var direction = mouseWorldPosition - (Vector2)transform.position;
            transform.up = -direction;

        }

        private void MovePlayer()
        {
            if (player.SetAlive() == true)
            {
                var moveX = Input.GetAxis("Horizontal");
                var moveY = Input.GetAxis("Vertical");
                rb.velocity = new Vector2(moveX, moveY) * speed;

                anim.SetFloat(Speed, rb.velocity.magnitude);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
            
    }
}
