using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties
{
    //attack range 1: 1 up, 1 down, 1 left, 1 right
    //attack range 2: 2 up, 2 down, 2 left, 2 right, 1 diagonally
    //the range is rhombus shaped
    //range value is 10 times its range, cause floats
    
    //Attack speed - number of attack creature can do at once when attacks
    //Attack cooldown - number of turns before creature can attack
    //Gold value - number of gold awarded for killing enemy
    
    //Goblin properties
    public const int GoblinDmg = 15;
    public const int GoblinHp = 50;
    public const int GoblinAttackSpeed = 2;
    public const int GoblinAttackRange = 15;
    public const int GoblinAttackCooldown = 0;
    public const int GoblinStamina = 30;
    public const int GoblinGoldValue = 40;
    
    
    
    //Orc properties
    public const int OrcDmg = 60;
    public const int OrcHp = 100;
    public const int OrcAttackSpeed = 1;
    public const int OrcAttackRange = 10;
    public const int OrcAttackCooldown = 2;
    public const int OrcStamina = 15;
    public const int OrcGoldValue = 80;
    
    //Spider properties
    public const int SpiderDmg = 8;
    public const int SpiderHp = 30;
    public const int SpiderAttackSpeed = 4;
    public const int SpiderAttackRange = 15;
    public const int SpiderAttackCooldown = 0;
    public const int SpiderStamina = 45;
    public const int SpiderGoldValue = 30;
    
    //Wolf properties
    public const int WolfDmg = 20;
    public const int WolfHp = 60;
    public const int WolfAttackSpeed = 2;
    public const int WolfAttackRange = 15;
    public const int WolfAttackCooldown = 0;
    public const int WolfStamina = 35;
    public const int WolfGoldValue = 50;
    
}
