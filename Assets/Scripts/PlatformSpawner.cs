using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] platforms;
    [SerializeField] private float gap = 10f;
    [SerializeField] private float spawnFrequencyMultiplier = 1.1f;
    [SerializeField] private float locationVarietyX = 5f;

    private float _platformSpeed;
    private float _spawnTimer;
    private float _timeUntilNextSpawn;
    private bool _firstSpawnHappened = false;

    private void Awake()
    {
        _platformSpeed = 1f;
        _spawnTimer = 0f;
        _timeUntilNextSpawn = 0f;
    }
    private void Start()
    {
        SpawnPlatform();
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
        if (ShouldSpawn())
        {
            GameObject platform;
            Vector3 nextLocation;
            if (_firstSpawnHappened)
            {
                platform = PickRandomPlatform();
                nextLocation = new Vector3(transform.position.x + RandomFloat(-locationVarietyX, locationVarietyX),
                                            transform.position.y, transform.position.z);
            }
            else
            {
                platform = platforms[0];
                nextLocation = Vector3.zero;
                _firstSpawnHappened = true;
            }

            float platformLength = platform.GetComponentInChildren<Renderer>().bounds.size.z;
            float nextSpawnTime = TimeBetweenSpawns(_platformSpeed, platformLength);
  
            GameObject newPlatform = Instantiate(platform, nextLocation, transform.rotation);

            PlatformMovement movementScript = newPlatform.GetComponent<PlatformMovement>();
            movementScript.Speed = _platformSpeed;

            _spawnTimer -= nextSpawnTime;
        }
    }

    private bool ShouldSpawn()
    {
        return _spawnTimer >= _timeUntilNextSpawn;
    }

    private GameObject PickRandomPlatform()
    {
        int collectionSize = platforms.Length;
        int index = Random.Range(0, collectionSize);
        GameObject platform = platforms[index];
        return platform;
    }

    //TODO: intervallen stämmer ej som det är nu
    private float TimeBetweenSpawns(float platformSpeed, float platformLength)
    {
        //return (platformLength + gap) * spawnFrequencyMultiplier / _platformSpeed;
        Debug.Log("Time Between Spawns: " + ((platformSpeed + gap) * spawnFrequencyMultiplier) / platformLength);
        return ((platformSpeed + gap) * spawnFrequencyMultiplier) / platformLength;

    }

    private void IncreaseSpeed(float platformSpeed, float amount)
    {
        this._platformSpeed = platformSpeed;
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
