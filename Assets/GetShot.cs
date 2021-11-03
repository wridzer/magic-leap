using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetShot : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip hurtSF;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {

            other.GetComponentInChildren<MeshRenderer>().enabled = false;
            other.GetComponent<Collider>().enabled = false;
            audioSource.PlayOneShot(hurtSF);
            other.GetComponent<Bullet>().Destroy();
        }

    }
}
