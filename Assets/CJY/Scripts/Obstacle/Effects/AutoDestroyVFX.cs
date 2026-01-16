using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyVFX : MonoBehaviour
{
    private ParticleSystem[] particleSystems;

    private void Awake()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>(true);

        if (particleSystems.Length == 0)
        {
            Debug.LogWarning($"{name} : ParticleSystem이 없어 즉시 제거됩니다.");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        for (int i = 0; i < particleSystems.Length; i++)
        {
            if (particleSystems[i].IsAlive(true))
                return;
        }

        Destroy(gameObject);
    }
}