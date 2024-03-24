using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObjectBase : MonoBehaviour{
    public Action<List<Transform>> OnTriggerTarget;
    
    public bool IsCastObject(LayerMask colliderLayer) {
        List<Transform> targets = new List<Transform>();
        var hits = Physics.SphereCastAll(
            transform.position, 
            GameManager.HEIGHT_OBJECT/2, 
            transform.forward, 
            GameManager.HEIGHT_OBJECT/2,
            colliderLayer);

        if (hits.Length > 0) {
            foreach (var hit in hits) {
                targets.Add(hit.transform);
            }
            
            OnTriggerTarget?.Invoke(targets);
            return true;
        }

        return false;
    }
}
