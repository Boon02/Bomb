using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : SingletonBehaviour<GameManager>{
    public const float HEIGHT_OBJECT = 0.7f;
    public const float HEIGHT_PLAYER = 0.7f;

    [SerializeField] private UserSO userSO;
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

        player.SetHealth(userSO.UserInfo.Default_Hp);
        player.SetSpeed(userSO.UserInfo.Default_Speed);
        player.SetColliderLayer(userSO.UserInfo.Default_ColliderLayer);
        player.SetRadiusBomb(userSO.UserInfo.Default_radiusBomb);
        player.SetBombID(BombID.Normal);
        player.Init();

        player.OnSetBomb += SetBomb;
        player.OnDead += OnPlayerDead;
        playerBomb = bombSO.GetBombPrefabFromID(BombID.Normal);
    }

    private void OnPlayerDead() {
        Debug.Log("Player Dead, Game Stop!");
    }

    private void SetBomb(int radius, BombID obj) {
        Vector3 position = new Vector3(
            (int)(player.transform.position.x + 0.5f),
            HEIGHT_OBJECT / 2,
            (int)(player.transform.position.z + 0.5f));
        var bomb = Instantiate(playerBomb, position, Quaternion.identity);
        bomb.Init(radius);
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

        for (int i = 0; i < posList[BombBase.Dir.top].Count - 1; i++) {
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

    public void Collect(Player collector, ItemID itemID, int value, ValueTypeBooster type) {
        switch (itemID) {
            case ItemID.Speed:
                switch (type) {
                    case ValueTypeBooster.percent:
                        collector.SetSpeed(collector.MoveSpeed * (1 + value * 1.0f / 100));
                        break;
                    case ValueTypeBooster.amount:
                        collector.SetSpeed(collector.MoveSpeed + value);
                        break;
                }

                break;
            case ItemID.Power:
                switch (type) {
                    case ValueTypeBooster.amount:
                        collector.SetRadiusBomb(collector.RadiusBomb + value);
                        break;
                }

                break;
            default:
                Debug.LogError($"{this.name} missing Collect with item ID : {itemID}");
                break;
        }
    }
}


[Serializable]
public enum BombID{
    Normal
}