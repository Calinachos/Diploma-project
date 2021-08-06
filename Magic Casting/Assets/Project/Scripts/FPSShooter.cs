using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FPSShooter : MonoBehaviour
{
    public Camera cam;
    public GameObject projectile;
    public GameObject muzzle;
    public float projectileSpeed = 10;
    public Transform LHFirePoint;
    public Transform RHFirePoint;
    public float fireRate;
    public List<AudioClip> shootSFX;
    [Space]
    [Header("SHAKE OPTIONS & PP")]
    public Volume volume;
    public float chromaticGoal = 0.5f;
    public float chromaticRate = 0.1f;
    public CinemachineImpulseSource impulseSource;
    public float shakeDuration=1;
    public float shakeAmplitude=5;
    public float shakeFrequency=2.5f;
    public int damage = 10;
    
    private ChromaticAberration chromatic;
    private AudioSource audioSource;
    private Animator anim;
    private float timeToFire = 0;   
    private Vector3 destination;
    private bool leftHand;
    private bool chromaticIncrease;

    void Start ()
    {        
        audioSource = GetComponent<AudioSource>();
        volume.profile.TryGet<ChromaticAberration>(out chromatic);
    }

    void Update()
    {
        if (!PauseGame.GameIsPaused)
        {
            anim = GetComponent<Animator>();

            if (Input.GetButton("Fire1") && Time.time >= timeToFire)
            {
                anim.SetBool("Idle", false);
                timeToFire = Time.time + 1 / fireRate;
                ShootProjectile();
            }

            if (Input.GetButtonUp("Fire1"))
            {
                anim.SetBool("Idle", true);
            }
        }
    }

    void ShootProjectile ()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }

        if (Physics.Raycast(ray, out hit))
        {
            destination = hit.point;

        }
        else
            destination = ray.GetPoint(1000);
        
        if(leftHand)
        {
            leftHand = false;
            anim.SetTrigger("Left");
            InstantiateProjectile(LHFirePoint);
        }
        else
        {
            leftHand = true;
            anim.SetTrigger("Right");
            InstantiateProjectile(RHFirePoint);
        }        
    }

    void InstantiateProjectile (Transform handFirePoint)
    {
        ShakeCameraWithImpulse();
        StartCoroutine (ChromaticAberrationPunch());

        var projectileObj = Instantiate (projectile, handFirePoint.position, Quaternion.identity) as GameObject;
        var distance = destination - handFirePoint.position;
        projectileObj.GetComponent<Rigidbody>().velocity = distance.normalized * projectileSpeed;

        var muzzleObj = Instantiate (muzzle, handFirePoint.position, Quaternion.identity) as GameObject;
        Destroy (muzzleObj, 2);

        var time = distance.magnitude/projectileSpeed;
        iTween.PunchPosition(projectileObj, new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f), 0), Random.Range(1, 4));

        var num = Random.Range (0, shootSFX.Count);
        audioSource.PlayOneShot(shootSFX[num]);
    }

    public void ShakeCameraWithImpulse()
    {
        impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = shakeDuration;
        impulseSource.m_ImpulseDefinition.m_AmplitudeGain = shakeAmplitude;
        impulseSource.m_ImpulseDefinition.m_FrequencyGain = shakeFrequency;
        impulseSource.GenerateImpulse();
    }

    IEnumerator ChromaticAberrationPunch()
    {    
        if(!chromaticIncrease)
        {    
            chromaticIncrease = true;
            float amount = 0;
            while (amount < chromaticGoal)
            {
                amount += chromaticRate;
                chromatic.intensity.value = amount;
                yield return new WaitForSeconds (0.05f);
            }
            while (amount > 0)
            {
                amount -= chromaticRate;
                chromatic.intensity.value = amount;
                yield return new WaitForSeconds (0.05f);
            }
            chromaticIncrease = false;
        }
    }
}
