using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave {
    public int number;
    public List<EnemyWavePart> enemies;

    public EnemyWave(int i,List<EnemyWavePart> parts) {
        enemies = parts;
        number = i;
    }
    public EnemyWave(int i) {
        number = i;
        List<EnemyWavePart> a = new List<EnemyWavePart>() {
            new EnemyWavePart(4,Enemies.Goblin)
        };
    }

    
    
}
public class EnemyWavePart{
    public Enemies id = Enemies.None;
    public int number = 0;

    public EnemyWavePart(int n, Enemies id) {
        number = n;
        this.id = id;
        
    }
}
