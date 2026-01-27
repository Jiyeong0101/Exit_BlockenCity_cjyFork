using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialLinkButton : MonoBehaviour
{
    [SerializeField] private SocialLinkType linkType;
    [SerializeField] private SocialLinkData linkData;

    public void OpenLink()
    {
        string url = linkData.GetUrl(linkType);

        if (string.IsNullOrEmpty(url))
        {
            Debug.LogWarning($"URL not set : {linkType}");
            return;
        }

        Application.OpenURL(url);
    }
}

