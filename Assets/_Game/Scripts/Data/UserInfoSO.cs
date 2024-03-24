using UnityEngine;

[CreateAssetMenu]
public class UserInfoSO : ScriptableObject{
    [SerializeField] [Range(0, 20)] private int default_hp = 3;
    [SerializeField] [Range(0, 20)] private float default_speed = 7f;
    [SerializeField] [Range(0, 20)] private int default_radiusBomb = 2;

    [SerializeField] private LayerMask colliderLayer;
    public int Default_Hp => default_hp;
    public float Default_Speed => default_speed;
    public LayerMask Default_ColliderLayer => colliderLayer;
    public int Default_radiusBomb => default_radiusBomb;

}