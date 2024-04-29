using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static int EnemiesAlive = 0;

    public int totalEnemy;

    public static int enemyDefeat;

    public Wave[] waves;

    public Transform spawnPoint;

    public float timeBetweenWave = 15f;
    public float countdown = 1f;

    private int waveIndex = 0;
    public GameManager manager;

    void Start()
    {
        EnemiesAlive = 0;
        foreach (Wave wave in waves)
        {
            totalEnemy += wave.count;
        }
        enemyDefeat = 0;
    }

    void Update()
    {
        if (EnemiesAlive > 0)
        {
            return;
        }

        if (waveIndex == waves.Length && PlayerStats.Lives > 0)
        {
            manager.WinGame();
            enabled = false;
        }
        else if (waveIndex == waves.Length && PlayerStats.Lives <= 0)
        {
            enabled = false;
        }

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWave;
            return;
        }
        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        Wave wave = waves[waveIndex];

        EnemiesAlive = wave.count;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(wave.rate);
        }

        waveIndex++;
    }

    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }
}
