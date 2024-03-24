using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UserSO : ScriptableObject{
    [SerializeField] private UserInfoSO userInfo;
    
    public UserInfoSO UserInfo => userInfo;
}
