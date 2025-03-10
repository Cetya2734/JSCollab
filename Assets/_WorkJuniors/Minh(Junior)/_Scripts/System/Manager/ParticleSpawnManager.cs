using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawnManager : MonoBehaviour
{
    public static ParticleSpawnManager Instance { get; private set; }

    public enum ParticleType { Hit, Death, Explosion, Heal } // Add new types here

    [System.Serializable]
    public class ParticlePrefab
    {
        public ParticleType type;
        public GameObject prefab;
    }

    [Header("Particle Settings")]
    public List<ParticlePrefab> particlePrefabs; // List to hold all particle prefabs
    public int poolSize = 10;
    public float lifetime = 2f;

    private Dictionary<ParticleType, Queue<GameObject>> particlePools;
    private Dictionary<ParticleType, GameObject> particlePrefabsDict;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitializePools();
    }

    private void InitializePools()
    {
        particlePools = new Dictionary<ParticleType, Queue<GameObject>>();
        particlePrefabsDict = new Dictionary<ParticleType, GameObject>();

        foreach (var entry in particlePrefabs)
        {
            particlePrefabsDict[entry.type] = entry.prefab;
            Queue<GameObject> pool = new Queue<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(entry.prefab);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }

            particlePools[entry.type] = pool;
        }
    }

    public void SpawnParticle(ParticleType type, Vector3 position)
    {
        if (particlePools.ContainsKey(type) && particlePools[type].Count > 0)
        {
            GameObject particle = particlePools[type].Dequeue();
            particle.transform.position = position;
            particle.SetActive(true);
            // StartCoroutine(DeactivateAfterTime(particle, type, lifetime));
        }
    }
    //
    // private IEnumerator DeactivateAfterTime(GameObject particle, ParticleType type, float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     particle.SetActive(false);
    //     particlePools[type].Enqueue(particle);
    // }
}
