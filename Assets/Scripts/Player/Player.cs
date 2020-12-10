using System;
using System.Collections;
using Lean.Pool;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public Action onPlayerHealth = delegate { };
        public Action onPlayerDeath = delegate { };
        public Action onBulletsCountChange = delegate { };


        [SerializeField] private float fireRate;
        [SerializeField] private float health;
        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform shootPosition;
        [SerializeField] private int bulletsCount;


        private float nextFire;
        private bool isAlive;
    

        private Animator anim;
        private SceneLoader sceneLoader;
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int Shoot1 = Animator.StringToHash("Shoot");

        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
            sceneLoader = FindObjectOfType<SceneLoader>();
            isAlive = true;
        }


        private void Update()
        {
            Shoot();

        }
        public bool SetAlive()
        {
            return isAlive;
        }
        public float ReturnHealth()
        {
            return health;
        }
        public int ReturnBulletCount()
        {
            return bulletsCount;
        }

        public void DoDamage(float damage)
        {
            if (isAlive != true) return;
            health -= damage;
            onPlayerHealth();
            if (!(health <= 0)) return;
            isAlive = false;
            anim.SetBool(Dead, true);
            onPlayerDeath();
            //StartCoroutine(RestartGame());
        }
        public void HealthUp(float index)
        {
            health += index;
            onPlayerHealth();
        }
        public void AddBullets(int bullets)
        {
            bulletsCount += bullets;
            onBulletsCountChange();

        }

        private void Shoot()
        {
            if (Input.GetButtonDown("Fire1") && nextFire <= 0 && isAlive == true)
            {
                if(bulletsCount != 0)
                {
                    anim.SetTrigger(Shoot1);
                    LeanPool.Spawn(bullet, shootPosition.position, transform.rotation);
                    nextFire = fireRate;
                    bulletsCount--;
                    onBulletsCountChange();
                }
            
            }
            if (nextFire > 0)
            {
                nextFire -= Time.deltaTime;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var damageDealer = collision.GetComponent<DamageDealer>();
            if (damageDealer != null)
            {
                DoDamage(damageDealer.damage);
            }
        }


        private IEnumerator RestartGame()
        {
            yield return new WaitForSeconds(3);
            anim.SetBool(Dead, false);
            sceneLoader.LoadActiveScene();
        }
   
    }
}
