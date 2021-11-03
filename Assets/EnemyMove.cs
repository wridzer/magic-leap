using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private GameObject playerInstance;
    [SerializeField] private GameObject BulletInstance;
    [SerializeField] private GameObject shootPoint;
    private Vector3 playerPos;
    private Vector3 destination;
    [SerializeField] private float minY = 1;
    [SerializeField] private float maxY = 3;
    [SerializeField] private float minX = 0;
    [SerializeField] private float maxX = 3;
    [SerializeField] private float minZ = 1;
    [SerializeField] private float maxZ = 2;
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;

    [SerializeField] private float shootTimer;
    [SerializeField] private float shootTimerOnStart = 15f;
    private float timeToShoot;

    private void Start()
    {
        destination = NewPos();
        timeToShoot = shootTimerOnStart;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rotationAngle = Quaternion.LookRotation(playerInstance.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * rotateSpeed);
        if (transform.position == destination)
        {
            destination = NewPos();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        }
        timeToShoot -= Time.deltaTime;
        if (timeToShoot <= 0)
        {
            Shoot();
            timeToShoot = shootTimer;
        }
    }

    private Vector3 NewPos()
    {
        float posX = playerInstance.transform.position.x + Random.Range(minX, maxX);
        float posY = playerInstance.transform.position.y + Random.Range(minY, maxY);
        float posZ = playerInstance.transform.position.z + Random.Range(minZ, maxZ);
        Vector3 dest = new Vector3(posX, posY, posZ);
        return dest;
    }

    private void Shoot()
    {
        Quaternion bulletRot = Quaternion.Euler(shootPoint.transform.eulerAngles + new Vector3(90, 0, 0));
        Instantiate(BulletInstance, shootPoint.transform.position, bulletRot);
    }
}
