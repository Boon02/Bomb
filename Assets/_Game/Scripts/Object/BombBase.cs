using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombBase : MonoBehaviour{
    public Action<Dictionary<Dir, List<Vector3Int>>> OnBreak;
    private Coroutine _coroutine;
    private float countDownTime = 5f;
    private Collider collider;
    
    public void Init(int radius) {
        if (_coroutine != null) {
            StopCoroutine(_coroutine);
        }

        collider = GetComponent<Collider>();
        collider.enabled = true;
        boomPosList = new Dictionary<Dir, List<Vector3Int>>();
        _coroutine = StartCoroutine(CountBreakAsync(radius));
    }

    [Serializable]
    public enum Dir{
        left,
        right,
        top,
        down,
        mid
    }

    private Dictionary<Dir, List<Vector3Int>> boomPosList;

    private IEnumerator CountBreakAsync(int radius) {
        yield return new WaitForSeconds(countDownTime);
        collider.enabled = false;
        Vector3Int currentPos = new Vector3Int((int)transform.position.x, 0, (int)transform.position.z);
        boomPosList.Add(Dir.mid, new List<Vector3Int>() { currentPos });

        boomPosList.Add(Dir.top, new List<Vector3Int>());
        boomPosList.Add(Dir.down, new List<Vector3Int>());
        boomPosList.Add(Dir.left, new List<Vector3Int>());
        boomPosList.Add(Dir.right, new List<Vector3Int>());

        for (int i = 1; i < radius; i++) {
            if (i != 0) {
                Vector3Int newPosTop = new Vector3Int(currentPos.x, 0, i + currentPos.z);
                Vector3Int newPosDown = new Vector3Int(currentPos.x, 0, -i + currentPos.z);
                Vector3Int newPosLeft = new Vector3Int(-i + currentPos.x, 0, currentPos.z);
                Vector3Int newPosRight = new Vector3Int(i + currentPos.x, 0, currentPos.z);

                boomPosList[Dir.top].Add(newPosTop);
                boomPosList[Dir.down].Add(newPosDown);
                boomPosList[Dir.left].Add(newPosLeft);
                boomPosList[Dir.right].Add(newPosRight);
            }
        }

        OnBreak?.Invoke(boomPosList);
        _coroutine = null;
        gameObject.SetActive(false);
    }

    private void OnDisable() {
        OnBreak = null;
    }
}