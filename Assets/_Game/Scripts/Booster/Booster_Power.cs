using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Booster_Power : BoosterBase{
    [SerializeField] private Transform visualTransform;

    private void Start() {
        visualTransform.DOLocalRotate(new Vector3(0, 359, 0), 0.03f).SetLoops(-1, LoopType.Incremental);
    }
}