using Pathfinding;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    [Header("Patrol")]
    [SerializeField]
    private bool isPatrol;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float minDistance;
    [SerializeField] private Transform[] patrolPositions;


    private Player.Player player;
    private Zombie zombie;
    private Rigidbody2D rb;
    private AIDestinationSetter destination;
    private AIPath path;

    private int patrolIndex = 0;
    private bool isAngry;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        zombie = GetComponent<Zombie>();
        player = FindObjectOfType<Player.Player>();
        destination = GetComponent<AIDestinationSetter>();
        path = GetComponent<AIPath>();
        
    }
    private void FixedUpdate()
    {
        Moving();
    }
    private void Moving()
    {
        if (zombie.SetAlive())
        {
            if (!isAngry)
            {
                if (!isPatrol)
                {
                    MoveToPlayer();
                }
                else if (isPatrol)
                {
                    DoPatrol();
                }
            }
            if (isAngry)
            {
                StopMovement();
            }
        }
        else
        {
            StopMovement();
        }
    }

    private void MoveToPlayer()
    {
        //Vector3 direcrion = player.transform.position - transform.position;
        //rb.velocity = direcrion.normalized * moveSpeed;
        destination.target = player.transform;
    }

    private void DoPatrol()
    {
        var nextPosition = patrolPositions[patrolIndex];
        Vector2 moveToNextPosition = nextPosition.transform.position - transform.position;
        transform.up -= (Vector3)moveToNextPosition;
        //rb.velocity = moveToNextPosition.normalized * patrolSpeed;
        destination.target = nextPosition;
        var distance = Vector2.Distance(transform.position, nextPosition.transform.position);
        if (!(distance <= minDistance)) return;
        patrolIndex++;
        if (patrolIndex == patrolPositions.Length)
        {
            patrolIndex = 0;
        }
    }
    private void StopMovement()
    {
        rb.velocity = Vector2.zero;
        path.enabled = false;
    }

    public void SetAngry(bool angry)
    {
        isAngry = angry;
    }
   public void SetPatrol(bool patrol)
    {
        isPatrol = patrol;
    }
}
