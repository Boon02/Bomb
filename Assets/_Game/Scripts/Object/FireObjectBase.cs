using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class FireObjectBase : MonoBehaviour{
    public bool IsCastObject(LayerMask colliderLayer) {
        var hits = Physics.OverlapSphere(transform.position, GameManager.HEIGHT_OBJECT/2, colliderLayer);

        if (hits.Length > 0) {
            foreach (var hit in hits) {
                if (hit.transform.TryGetComponent(out IInteract t)) {
                    Debug.LogWarning($"{name} interact {t}");
                    t.Interact();
                }
            }
            
            return true;
        }

        return false;
    }
}
