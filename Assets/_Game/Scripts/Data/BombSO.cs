using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BombSO : ScriptableObject{
    [SerializeField] private List<BombInfo> _bombInfos;

    public BombBase GetBombPrefabFromID(BombID bombID) {
        return _bombInfos.Find(i => i.BombID == bombID).BombPrefab;
    }
}

[Serializable]
public class BombInfo{
    [SerializeField] private BombBase bombPrefab;
    [SerializeField] private BombID bombID;

    public BombID BombID => bombID;
    public BombBase BombPrefab => bombPrefab;
}