using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProperties {
    
    //attack range 1: 1 up, 1 down, 1 left, 1 right
    //attack range 2: 2 up, 2 down, 2 left, 2 right, 1 diagonally
    //the range is rhombus shaped
    //range value is 10 times its range, cause floats
    
    //Attack speed - number of attack creature can do at once when attacks
    //Attack cooldown - number of turns before creature can attack
    
    public const int UnitMovementRange = 1;
    //Swordman properties
    public const int SwordsmanDmg = 10;
    public const int SwordsmanHp = 100;
    public const int SwordsmanAttackSpeed = 2;
    public const int SwordsmanAttackRange = 15; 
    public const int SwordsmanAttackCooldown = 0;
    public const int SwordsmanGoldValue = 100;
}
