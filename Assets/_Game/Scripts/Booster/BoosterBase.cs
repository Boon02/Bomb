using UnityEngine;

public class BoosterBase : MonoBehaviour{
    [SerializeField] protected ItemID itemID;
    [SerializeField] [Min(0)] protected int value;
    [SerializeField] protected ValueTypeBooster type;
    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out ICollect collector)) {
            collector.Collect(itemID, value, type);
            CollectedItem();
        }
    }

    protected virtual void CollectedItem() {
        gameObject.SetActive(false);
    }
}

public enum ValueTypeBooster{
    percent,
    amount
}


