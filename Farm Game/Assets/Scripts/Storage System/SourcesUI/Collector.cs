using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : UIDrag
{
    protected override void OnCollide(PlaceableObject collidedSource)
    {
        collidedSource.GetComponent<ISource>().Collect();
    }
}
