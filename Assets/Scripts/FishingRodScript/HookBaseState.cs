using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HookBaseState 
{
    public abstract void EnterState(FishingRodBaseScript rod);

    public abstract void UpdateState(FishingRodBaseScript rod);

    public abstract void FixedUpdateState(FishingRodBaseScript rod);
}
