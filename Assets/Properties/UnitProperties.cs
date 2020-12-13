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
    //Gold value - number of gold needed for spawning unit
    
    public const int UnitMovementRange = 1;
    
    //Archer properties
    public const int ArcherDmg = 20;
    public const int ArcherHp = 50;
    public const int ArcherAttackSpeed = 1;
    public const int ArcherAttackRange = 40; 
    public const int ArcherAttackCooldown = 1;
    public const int ArcherGoldValue = 75;
    
    //AxeMan properties
    public const int AxeManDmg = 20;
    public const int AxeManHp = 115;
    public const int AxeManAttackSpeed = 1;
    public const int AxeManAttackRange = 15; 
    public const int AxeManAttackCooldown = 0;
    public const int AxeManGoldValue = 125;
    
    //Berserker properties
    public const int BerserkerDmg = 15;
    public const int BerserkerHp = 140;
    public const int BerserkerAttackSpeed = 2;
    public const int BerserkerAttackRange = 10; 
    public const int BerserkerAttackCooldown = 0;
    public const int BerserkerGoldValue = 175;
    
    //CrossbowMan properties
    public const int CrossbowManDmg = 40;
    public const int CrossbowManHp = 80;
    public const int CrossbowManAttackSpeed = 1;
    public const int CrossbowManAttackRange = 55; 
    public const int CrossbowManAttackCooldown = 3;
    public const int CrossbowManGoldValue = 200;
    
    //Mage properties
    public const int MageDmg = 55;
    public const int MageHp = 30;
    public const int MageAttackSpeed = 1;
    public const int MageAttackRange = 70; 
    public const int MageAttackCooldown = 6;
    public const int MageGoldValue = 150;
    
    //ShieldMan properties
    public const int ShieldManDmg = 1;
    public const int ShieldManHp = 200;
    public const int ShieldManAttackSpeed = 1;
    public const int ShieldManAttackRange = 10; 
    public const int ShieldManAttackCooldown = 0;
    public const int ShieldManGoldValue = 120;

    //SpearMan properties
    public const int SpearManDmg = 15;
    public const int SpearManHp = 75;
    public const int SpearManAttackSpeed = 1;
    public const int SpearManAttackRange = 25; 
    public const int SpearManAttackCooldown = 0;
    public const int SpearManGoldValue = 80;
    
    //SwordsMan properties
    public const int SwordsManDmg = 10;
    public const int SwordsManHp = 100;
    public const int SwordsManAttackSpeed = 2;
    public const int SwordsManAttackRange = 15; 
    public const int SwordsManAttackCooldown = 0;
    public const int SwordsManGoldValue = 100;
    
}
