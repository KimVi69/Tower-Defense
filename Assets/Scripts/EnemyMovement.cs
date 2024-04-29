using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Enemy enemy;

    private Transform target;

    private int wavepointIndex = 0;

    void Start()
    {
        enemy = GetComponent<Enemy>();

        target = Waypoint.points[0];
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position;

        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if (wavepointIndex >= Waypoint.points.Length - 1)
        {
            EndPath();
            return;
        }

        wavepointIndex++;
        target = Waypoint.points[wavepointIndex];
    }

    void EndPath()
    {
        PlayerStats.Lives--;
        BuildManager.instance.PlayErrorSound();
        GetComponent<Enemy>().DeathAnimation();
    }
}
