using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SocialLinkData", menuName = "Data/Social Links")]
public class SocialLinkData : ScriptableObject
{
    public string instagramUrl;
    public string blogUrl;
    public string twitterUrl;

    public string GetUrl(SocialLinkType type)
    {
        switch (type)
        {
            case SocialLinkType.Instagram:
                return instagramUrl;
            case SocialLinkType.Blog:
                return blogUrl;
            case SocialLinkType.Twitter:
                return twitterUrl;
            default:
                return string.Empty;
        }
    }
}
