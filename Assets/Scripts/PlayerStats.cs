using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public enum StatType {
        maxHealth,
        cannonDamage,
        cannonSpeed,
        fireRate,
        moveSpeed
    }

    private int maxHealthLevel, cannonDamageLevel, cannonSpeedLevel, fireRateLevel, moveSpeedLevel;
}
