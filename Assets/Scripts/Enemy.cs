using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;
    public float maxHealth = 100;
    public int damage = 50;
    public float rangeX = 10f;
    public float rangeZ = 10f;
    public float offsetX = 0f;
    public float offsetZ = 0f;
    private string enemyTag = "Turret";
    [Range(0f, 1f)]
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    private bool flip = false;
    public GameObject sprite;
    private float flipSpeed = 20f;
    private Animator animator;
    private bool startCooldown = false;

    public Image healthBar;
    public GameObject healthUI;
    public float health;

    private EnemyMovement move;

    private bool isDead;

    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        move = GetComponent<EnemyMovement>();
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
    }

    void Update()
    {
        if (GameManager.gameEnded)
        {
            move.enabled = false;
            animator.enabled = false;
            healthUI.SetActive(false);
        }

        float targetScaleX = flip ? -1f : 1f;
        float newScaleX = Mathf.MoveTowards(sprite.transform.localScale.x, targetScaleX, Time.deltaTime * flipSpeed);
        sprite.transform.localScale = new Vector3(newScaleX, 1f, 1f);

        if (target == null)
        {
            move.enabled = true;
            flip = false;
            return;
        }

        move.enabled = false;
        if (fireCountdown <= 0f)
        {
            startCooldown = false;
            bool shouldFlip = target.position.x > gameObject.transform.position.x;

            if (shouldFlip)
            {
                flip = true;
            }
 
            animator.SetTrigger("attack");

            fireCountdown = 1f / fireRate;
        }

        if (startCooldown)
            fireCountdown -= Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        healthUI.SetActive(true);

        healthBar.fillAmount = health / maxHealth;

        if (health <= 0 && !isDead)
        {
            DeathAnimation();
        }
    }

    public void DeathAnimation()
    {
        animator.SetTrigger("die");
        healthUI.SetActive(false);
        GetComponent<Enemy>().enabled = false;
        tag = "Untagged";
        move.enabled = false;
    }

    public void Die()
    {
        isDead = true;

        Destroy(gameObject);

        WaveSpawner.EnemiesAlive--;

        WaveSpawner.enemyDefeat++;
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            Vector3 enemyPosition = enemy.transform.position;
            Vector3 towerPosition = transform.position + new Vector3(offsetX, 0f, offsetZ);

            float distanceX = Mathf.Abs(enemyPosition.x - towerPosition.x);
            float distanceZ = Mathf.Abs(enemyPosition.z - towerPosition.z);

            if (distanceX <= rangeX && distanceZ <= rangeZ)
            {
                float distanceToEnemy = Vector3.Distance(towerPosition, enemyPosition);

                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
        }

        target = (nearestEnemy != null && shortestDistance <= Mathf.Max(rangeX, rangeZ)) ? nearestEnemy.transform : null;

    }

    public void Attack()
    {
        if (target != null)
        {
            target.gameObject.GetComponent<Turret>().TakeDamage(damage);
        }
        startCooldown = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 size = new Vector3(rangeX * 2, 2f, rangeZ * 2);
        Vector3 center = transform.position + new Vector3(offsetX, 0f, offsetZ);
        Gizmos.DrawWireCube(center, size);
    }
}
