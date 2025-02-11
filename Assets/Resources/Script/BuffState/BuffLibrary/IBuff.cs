using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff
{
    void ApplyEffect(BuffState playerState);
    //void RemoveEffect(BuffState playerState);
}
