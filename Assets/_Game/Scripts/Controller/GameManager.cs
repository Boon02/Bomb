using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : SingletonBehaviour<GameManager>{
    public const float HEIGHT_OBJECT = 0.7f;       
    public const float HEIGHT_PLAYER = 0.7f;        


    [SerializeField] private BombSO bombSO;
    [SerializeField] private FireObjectBase _fireObjectBasePrefab;
    [SerializeField] private LayerMask colliderFireLayer;
    
    [SerializeField] private Player playerPrefab;
    [SerializeField] private List<Transform> spawnPosList;

    private Player player;
    private BombBase playerBomb;

    private void Start() {
        player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        var pos = spawnPosList[Random.Range(0, spawnPosList.Count)].position;
        player.transform.position = new Vector3(pos.x, HEIGHT_PLAYER, pos.z);
        player.Init(BombID.Normal, 3);
        player.OnSetBomb += SetBomb;
        player.OnDead += OnPlayerDead;
        playerBomb = bombSO.GetBombPrefabFromID(BombID.Normal);
    }

    private void OnPlayerDead() {
        Debug.Log("Player Dead, Game Stop!");
    }

    private int radiusBomb = 3;
    private void SetBomb(BombID obj) {
        Vector3 position = new Vector3(
            (int)(player.transform.position.x + 0.5f), 
            HEIGHT_OBJECT / 2, 
            (int)(player.transform.position.z + 0.5f));
        var bomb = Instantiate(playerBomb, position, Quaternion.identity);
        bomb.Init(radiusBomb);
        bomb.OnBreak += Bomb_OnBreak;
    }

    private void Bomb_OnBreak(Dictionary<BombBase.Dir, List<Vector3Int>> posList) {
        StartCoroutine(Bomb_OnBreakAsync(posList));
    }

    private IEnumerator Bomb_OnBreakAsync(Dictionary<BombBase.Dir, List<Vector3Int>> posList) {
        SetFire(posList[BombBase.Dir.mid][0]);
        yield return null;

        bool isDirTop = false;
        bool isDirDown = false;
        bool isDirLeft = false;
        bool isDirRight = false;
        
        for (int i = 0; i < radiusBomb - 1; i++) {
            if (!isDirTop) isDirTop = SetFire(posList[BombBase.Dir.top][i]);
            if (!isDirDown) isDirDown = SetFire(posList[BombBase.Dir.down][i]);
            if (!isDirLeft) isDirLeft = SetFire(posList[BombBase.Dir.left][i]);
            if (!isDirRight) isDirRight = SetFire(posList[BombBase.Dir.right][i]);
        }
    }

    private bool SetFire(Vector3 pos) {
        var fireObject = Instantiate(_fireObjectBasePrefab, pos, Quaternion.identity);
        fireObject.name = $"{pos}";
        fireObject.transform.position = new Vector3(pos.x, HEIGHT_OBJECT / 2, pos.z);
        return fireObject.IsCastObject(colliderFireLayer);
    }
}


[Serializable]
public enum BombID{
    Normal
}