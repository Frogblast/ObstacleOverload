using UnityEngine;

public class SpawnMovement : MonoBehaviour
{
    public float Speed { get; set; } = 10f;


    //[SerializeField] bool hasRandomSpeed = true;
    [SerializeField] float despawnDistance = 100f;
    private float spawnLength = 0;
    private Vector3 spawnLocation = Vector3.zero;
    private void Awake()
    {
        spawnLocation = transform.position;
        spawnLength = GetComponentInChildren<Renderer>().bounds.size.z;
        //if (hasRandomSpeed)
        //{
        //    System.Random randomFloat = new System.Random();
        //    Speed *=(float) randomFloat.Next(1,4)/2;
        //}
    }


    private void FixedUpdate()
    {
        Move();
        Despawn();
    }

    private void Despawn()
    {
        if (Vector3.Distance(spawnLocation, transform.position) > spawnLength + despawnDistance)
        {
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        Vector3 currentPos = transform.position;
        transform.position = new Vector3(currentPos.x, currentPos.y, currentPos.z - Speed * Time.deltaTime);
    }


}
