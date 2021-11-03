using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float fireSpeed = 2f;
    private Rigidbody rb;
    [SerializeField] private float despawnTimer = 3f;
    private Vector3 _playerPos;
    private AudioSource audioSource;

    [SerializeField] private AudioClip reflectSF;
    [SerializeField] private AudioClip missSF;
    [SerializeField] private AudioClip hurtSF;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        _playerPos = GameObject.Find("VRCamera").transform.position;

        Quaternion rotationAngle = Quaternion.LookRotation(_playerPos - transform.position);
        transform.rotation = rotationAngle;

        rb.freezeRotation = true;
    }

    // we want to store the laser's velocity every frame
    // so we can use this data during collisions to reflect
    private Vector3 oldVelocity;
    void FixedUpdate()
    {
        // because we want the velocity after physics, we put this in fixed update

        transform.Translate(Vector3.forward * fireSpeed * Time.deltaTime);

        despawnTimer -= Time.deltaTime;
        if (despawnTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    // when a collision happens
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "lightsaber")
        {

            // get the point of contact
            ContactPoint contact = collision.contacts[0];

            // reflect our old velocity off the contact point's normal vector
            Vector3 reflectedVelocity = Vector3.Reflect(oldVelocity, contact.normal);

            // assign the reflected velocity back to the rigidbody
            rb.velocity = reflectedVelocity;
            Quaternion rotation = Quaternion.FromToRotation(oldVelocity, reflectedVelocity);
            transform.rotation = rotation * transform.rotation;

            audioSource.PlayOneShot(reflectSF);
        }
        if (collision.transform.tag == "Player")
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            audioSource.PlayOneShot(hurtSF);
            Destroy();
        }
        if (collision.transform.tag != "Player" && collision.transform.tag != "lightsaber")
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            audioSource.PlayOneShot(missSF);
            Destroy();
        }
    }

    public void Destroy()
    {
        float _timer = 5;
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            Destroy(gameObject);
        }
    }

}
