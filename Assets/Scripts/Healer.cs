using UnityEngine;
using UnityEngine.UI;

public class Healer : MonoBehaviour
{
    public TurretBlueprint turretBlueprint;
    public GameObject rangeUI;
    private Transform target;
    private Animator animator;
    public float rangeX = 10f;
    public float rangeZ = 10f;
    public float offsetX = 0f;
    public float offsetZ = 0f;
    private float flipSpeed = 20f;
    public string targetTag = "Enemy";
    [Range(0f, 1f)]
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    private bool flip = false;
    public GameObject sprite;
    public int damage = 50;

    private bool startCooldown = false;

    public Image healthBar;
    public GameObject healthUI;
    public float maxHealth = 100;
    private float health;

    public AudioSource attackSFX;

    void Start()
    {
        rangeUI.SetActive(false);
        health = maxHealth;
        animator = GetComponent<Animator>();
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
    }

    void Update()
    {
        if (GameManager.gameEnded)
        {
            animator.enabled = false;
            if (healthUI != null)
                healthUI.SetActive(false);
        }

        if (target == null)
            return;

        if (fireCountdown <= 0f)
        {
            startCooldown = false;
            bool shouldFlip = target.position.x < gameObject.transform.position.x;

            if (shouldFlip)
            {
                flip = true;
            }
            else
            {
                flip = false;
            }

            animator.SetTrigger("attack");

            fireCountdown = 1f / fireRate;
        }

        if (startCooldown)
            fireCountdown -= Time.deltaTime;

        float targetScaleX = flip ? -1f : 1f;
        float newScaleX = Mathf.MoveTowards(sprite.transform.localScale.x, targetScaleX, Time.deltaTime * flipSpeed);
        sprite.transform.localScale = new Vector3(newScaleX, 1f, 1f);
    }

    public void Attack()
    {
        if (target != null)
        {
            target.gameObject.GetComponent<Turret>().TakeDamage(damage);
            attackSFX.Play();
        }
        startCooldown = true;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        healthUI.SetActive(true);

        healthBar.fillAmount = health / maxHealth;

        if (health <= 0)
        {
            BuildManager.instance.DeselectNode();
            BuildManager.instance.PlayRetreatSFX();
            animator.SetTrigger("sell");
            healthUI.SetActive(false);
            GetComponent<Healer>().enabled = false;
            tag = "Untagged";
        }
    }

    public void Die()
    {
        turretBlueprint.button.SetActive(true);
        turretBlueprint.button.GetComponent<TurretCooldown>().StartCooldown(turretBlueprint.cooldown, turretBlueprint);
        Destroy(gameObject);
    }

    void UpdateTarget()
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag(targetTag);

        float lowestHealth = Mathf.Infinity;
        GameObject targetTurret = null;

        foreach (GameObject turret in turrets)
        {
            float turretHealth = turret.GetComponent<Turret>().health;

            float turretMaxHealth = turret.GetComponent<Turret>().maxHealth;

            Vector3 turretPosition = turret.transform.position;
            Vector3 healerPosition = transform.position + new Vector3(offsetX, 0f, offsetZ);

            float distanceX = Mathf.Abs(turretPosition.x - healerPosition.x);
            float distanceZ = Mathf.Abs(turretPosition.z - healerPosition.z);

            if (distanceX <= rangeX && distanceZ <= rangeZ && turretHealth < turretMaxHealth)
            {
                if (turretHealth < lowestHealth)
                {
                    lowestHealth = turretHealth;
                    targetTurret = turret;
                }
            }
        }

        target = (targetTurret != null && lowestHealth < Mathf.Infinity) ? targetTurret.transform : null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 size = new Vector3(rangeX * 2, 2f, rangeZ * 2);
        Vector3 center = transform.position + new Vector3(offsetX, 0f, offsetZ);
        Gizmos.DrawWireCube(center, size);
    }
}
