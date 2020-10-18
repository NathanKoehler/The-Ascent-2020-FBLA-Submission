using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomedToDieParticles_S : MonoBehaviour
{
    public ParticleSystem particles;

    public bool hasDelay;
    public float delay;

    private void Start()
    {
        if (particles == null)
            particles = GetComponent<ParticleSystem>();
        if (!hasDelay)
            particles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        else
            StartCoroutine("Wait");
    }

    private void Update()
    {
        if (!particles.IsAlive())
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(delay);
        particles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
    }
}
