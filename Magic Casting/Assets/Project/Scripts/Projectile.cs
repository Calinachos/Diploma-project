using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class Projectile : MonoBehaviour
{
    public GameObject impactVFX;
    public List<AudioClip> impactSFX;


    public bool collided;
    
    void OnCollisionEnter (Collision co) 
    {
        if (co.gameObject.tag != "Bullet" && co.gameObject.tag != "Player" && !collided)
        {
            collided = true;

            if (impactVFX != null)
            {

                ContactPoint contact = co.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;

                var hitVFX = Instantiate(impactVFX, pos, rot) as GameObject;
                var num = Random.Range (0, impactSFX.Count);
                hitVFX.GetComponent<AudioSource>().PlayOneShot(impactSFX[num]);

                Destroy (hitVFX, 2);
            }

            Destroy(gameObject);
        }
    }
}
