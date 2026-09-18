using System.Collections;
using UnityEngine;

public class AutoDestroyVFX : MonoBehaviour
{
    [SerializeField] private float defaultDuration = 2.0f; // 파티클이 없을 때 유지될 시간(초)

    private ParticleSystem[] particleSystems;

    private void Awake()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>(true);

        // 파티클이 전혀 없으면 지정한 시간 후 파괴
        if (particleSystems.Length == 0)
        {
            Destroy(gameObject, defaultDuration);
            enabled = false; // Update 실행 중단
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