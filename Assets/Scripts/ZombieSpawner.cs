using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public Zombie prefab;

    public Zombie[] prefabs;
    public Transform[] spawnPoints;

    public GameManager gameManager;
    private List<Zombie> zombies = new List<Zombie>();

    public float zombieSpawnInterval = 5f;
    public float lastZombieSpawnTime;

    public float HellephantSpwanInterval = 15f;
    public float lastHellephantSpawnTime;


    private void Update()
    {
        if(Time.time > zombieSpawnInterval + lastZombieSpawnTime)
        {
            lastZombieSpawnTime = Time.time;
            CreateZombie();
        }
        if (Time.time > HellephantSpwanInterval + lastHellephantSpawnTime)
        {
            lastHellephantSpawnTime = Time.time;
            CreateHellephant();
        }
    }

    private void CreateZombie()
    {
        var zomBunny = Instantiate(prefabs[0], spawnPoints[0].position, spawnPoints[0].rotation);
        zombies.Add(zomBunny);

        var zomBear = Instantiate(prefabs[1], spawnPoints[1].position, spawnPoints[1].rotation);
        zombies.Add(zomBear);

        zomBunny.OnDead.AddListener(() => gameManager.AddScore(100));
        zomBear.OnDead.AddListener(() => gameManager.AddScore(100));

        zomBunny.OnDead.AddListener(() => zombies.Remove(zomBunny));
        zomBear.OnDead.AddListener(() => zombies.Remove(zomBear));

        zomBunny.OnDead.AddListener(() => Destroy(zomBunny.gameObject, 5f));
        zomBear.OnDead.AddListener(() => Destroy(zomBear.gameObject, 5f));
    }

    private void CreateHellephant()
    {
        var hellephant = Instantiate(prefabs[2], spawnPoints[2].position, spawnPoints[2].rotation);
        zombies.Add(hellephant);

        hellephant.OnDead.AddListener(() => gameManager.AddScore(500));
        hellephant.OnDead.AddListener(() => zombies.Remove(hellephant));
        hellephant.OnDead.AddListener(() => Destroy(hellephant.gameObject, 5f));
    }

}
