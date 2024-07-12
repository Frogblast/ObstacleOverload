using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnObjects;
    [SerializeField] private float gap = 10f;
    [SerializeField] private float spawnFrequencyMultiplier = 1.1f;
    [SerializeField] private float locationVarietyX = 5f;
    [SerializeField] private float locationVarietyY = 1f;
    [SerializeField] private float locationVarietyZ = 10f;
    [SerializeField] private float randomFrequencyAmount = 0f;



    private float _spawnSpeed;
    private float _spawnTimer;
    private float _timeUntilNextSpawn;

    private void Awake()
    {
        _spawnSpeed = 1f;
        _spawnTimer = 0f;
        _timeUntilNextSpawn = 0f;
    }

    private void FixedUpdate()
    {
        _spawnTimer += Time.deltaTime;
        Spawn();
    }

    private void OnEnable()
    {
        DifficultyManager.OnIncreaseDifficulty += IncreaseGap;
        DifficultyManager.OnIncreaseDifficulty += IncreaseSpeed;
        DifficultyManager.OnStart += IncreaseSpeed;
    }

    private void OnDisable()
    {
        DifficultyManager.OnIncreaseDifficulty -= IncreaseGap;
        DifficultyManager.OnIncreaseDifficulty -= IncreaseSpeed;
        DifficultyManager.OnStart -= IncreaseSpeed;
    }

    private void Spawn()
    {
        if (ShouldSpawn())
        {
            GameObject newSpawn;
            Vector3 nextLocation;

            newSpawn = PickRandomSpawnObject();
            nextLocation = new Vector3(transform.position.x + RandomFloat(-locationVarietyX, locationVarietyX),
                                           RandomFloat(-locationVarietyY, locationVarietyY), transform.position.z + RandomFloat(-locationVarietyZ, locationVarietyZ));
            
            float spawnObjectLength = newSpawn.GetComponentInChildren<Renderer>().bounds.size.z;
            float nextSpawnTime = TimeBetweenSpawns(spawnObjectLength);
  
            GameObject newSpawnLocated = Instantiate(newSpawn, nextLocation, transform.rotation);

            SpawnMovement movementScript = newSpawnLocated.GetComponent<SpawnMovement>();
            movementScript.Speed = _spawnSpeed;

            _spawnTimer -= nextSpawnTime;
        }
    }

    private bool ShouldSpawn()
    {
        // If randomFrequencyAmount is zero the spawn rate will be constant
        return _spawnTimer >= _timeUntilNextSpawn + RandomFloat(-randomFrequencyAmount, randomFrequencyAmount);
    }

    private GameObject PickRandomSpawnObject()
    {
        int collectionSize = spawnObjects.Length;
        int index = Random.Range(0, collectionSize);
        GameObject spawn = spawnObjects[index];
        return spawn;
    }

    private float TimeBetweenSpawns(float spawnLength)
    {
        return (spawnLength + gap) * spawnFrequencyMultiplier / _spawnSpeed;
    }

    private void IncreaseSpeed(float platformSpeed, float amount)
    {
        this._spawnSpeed = platformSpeed;
    }

    private void IncreaseGap(float platformSpeed, float amount)
    {
        gap += amount;
    }

    private float RandomFloat(float min, float max)
    {
        System.Random random = new System.Random();
        double val = (random.NextDouble() * (max - min) + min);
        return (float)val;
    }
}
