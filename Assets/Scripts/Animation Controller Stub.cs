using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStub : MonoBehaviour
{
    public TitleSpacecraft spacecraft;

    void Update()
    {
        spacecraft.UpdatePosition();
        spacecraft.UpdateSize();
        spacecraft.UpdateVisibility();
    }
}
