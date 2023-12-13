using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protego : BasicSpell
{
    public override void Initialize(Transform wantTip)
    {
        transform.position = wantTip.position;
        transform.eulerAngles = new Vector3(0, wantTip.eulerAngles.y, 0);
    }

    
}
