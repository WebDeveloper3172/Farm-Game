using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISource 
{
   State currentState { get; set; }
    void Produce(Dictionary<CollectibleItem , int> itemsNeeded , CollectibleItem itemToProduce);
    void Collect();
}
