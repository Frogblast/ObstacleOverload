using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] platforms;
    [SerializeField] private float gap = 1.5f;
    [SerializeField] private float spawnFrequency = 1.0f;
    [SerializeField] private float platformSpeed = 10f;
    [SerializeField] private float locationVarietyX = 5f;

    private float _spawnTimer;
    private float _timeUntilNextSpawn;

    private void Awake()
    {
        _spawnTimer = 0f;
        _timeUntilNextSpawn = 0f;
    }

    private void FixedUpdate()
    {
        _spawnTimer += Time.deltaTime;
        SpawnPlatform();
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

    private void SpawnPlatform()
    {
        if (_spawnTimer >= _timeUntilNextSpawn)
        {
            GameObject platform = PickRandomPlatform();

            float platformLength = platform.GetComponentInChildren<Renderer>().bounds.size.z;
            float nextSpawn = TimeBetweenSpawns(platformSpeed, platformLength);

            Vector3 nextLocation = new Vector3(transform.position.x + RandomFloat(-locationVarietyX, locationVarietyX), transform.position.y, transform.position.z);

            GameObject newPlatform = Instantiate(platform, nextLocation, transform.rotation);

            PlatformMovement movementScript = newPlatform.GetComponent<PlatformMovement>();
            movementScript.Speed = platformSpeed;

            _spawnTimer -= nextSpawn;
        }
    }


    private GameObject PickRandomPlatform()
    {
        int collectionSize = platforms.Length;
        int index = Random.Range(0, collectionSize);
        GameObject platform = platforms[index];
        return platform;
    }

    private float TimeBetweenSpawns(float platformSpeed, float platformLength)
    {
        return (platformLength + gap) * spawnFrequency / platformSpeed;
    }

    private void IncreaseSpeed(float platformSpeed, float amount)
    {
        this.platformSpeed = platformSpeed;
    }

    private void IncreaseGap(float platformSpeed, float amount)
    {

        gap += amount;

    }


    static float RandomFloat(float min, float max)
    {
        System.Random random = new System.Random();
        double val = (random.NextDouble() * (max - min) + min);
        return (float)val;
    }
}
