using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ResourceSystem : Singleton<ResourceSystem>
{
    public List<FishScriptableObject> tier1Fishes{ get; private set; }

    protected override void Awake()
    {
        base.Awake();
        AssembleResources();

    }

    private void AssembleResources()
    {
        tier1Fishes = Resources.LoadAll<FishScriptableObject>("Tier1Fishes").ToList();

    }
}
