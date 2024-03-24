using System;

public interface ICollect{
    public void Collect(ItemID itemID, int value, ValueTypeBooster type);
}

[Serializable]
public enum ItemID{
    None,
    Speed,
    Power
}