using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProperties {
    
    //attack range 1: 1 up, 1 down, 1 left, 1 right
    //attack range 2: 2 up, 2 down, 2 left, 2 right, 1 diagonally
    //the range is rhombus shaped
    
    //Attack speed - number of attack creature can do at once when attacks
    //Attack cooldown - number of turns before creature can attack
    
    public const int UnitMovementRange = 1;
    //Swordman properties
    public const int SwordmanDmg = 5;
    public const int SwordmanHp = 100;
    public const int SwordmanAttackSpeed = 2;
    public const int SwordmanAttackRange = 1; 
    public const int SwordmanAttackCooldown = 1;
}
