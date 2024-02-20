using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HookBaseState 
{
    public abstract void EnterState(FishingRodBaseScript hook);

    public abstract void UpdateState(FishingRodBaseScript hook);

    public abstract void FixedUpdateState(FishingRodBaseScript hook);
}
