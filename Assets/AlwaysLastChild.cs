using UnityEngine;

public class AlwaysLastChild : MonoBehaviour
{
    void LateUpdate()
    {
        if (transform.GetSiblingIndex() != transform.parent.childCount - 1)
        {
            transform.SetAsLastSibling();
        }
    }
}