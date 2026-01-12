using UnityEngine;
using UnityEngine.UI;

public class ShaderTimer : MonoBehaviour
{
    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        image.material.SetFloat("_StartTime", Time.time);
    }

    void OnEnable()
    {
        image.material.SetFloat("_StartTime", Time.time);
    }
}
