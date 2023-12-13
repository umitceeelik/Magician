using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpell : MonoBehaviour
{
    public virtual void Initialize(Transform wantTip)
    {
        transform.position = wantTip.position;
        transform.rotation = wantTip.rotation;
    }
}
