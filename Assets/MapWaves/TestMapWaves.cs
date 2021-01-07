using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//wave 1: 4 goblins
//wave 2: 8 goblins
//wave 3: 12 goblins
//wave 4: 16 goblins
//wave 5: 4 orcs
//wave 6: 6 orcs
//wave 7: 10 goblins 4 orcs
//wave 8:  14 goblins 6 orcs
//wave 9: 16 goblins 8 orcs
//wave 10: 16 goblins 8 orcs 16 goblins 4 orcs
public class TestMapWaves: MapWaves {
   

   public TestMapWaves() {
      waves = new List<EnemyWave>();
      EnemyWave wave;
      //wave 1
      wave = new EnemyWave(1, new List<EnemyWavePart>() {
         new EnemyWavePart(4,Enemies.Goblin)
      });
      waves.Add(wave);
      
      //wave 2
      wave = new EnemyWave(2, new List<EnemyWavePart>() {
         new EnemyWavePart(4,Enemies.Wolf),
         new EnemyWavePart(8,Enemies.Spider),
         new EnemyWavePart(6,Enemies.Goblin),
      });
      waves.Add(wave);
      
      //wave 3
      wave = new EnemyWave(3,new List<EnemyWavePart>() {
         new EnemyWavePart(16,Enemies.Goblin),
         new EnemyWavePart(16,Enemies.Wolf),
      });
      wave.enemies.Add(new EnemyWavePart(12,Enemies.Goblin));
      waves.Add(wave);
      
      //wave 4
      wave = new EnemyWave(4,new List<EnemyWavePart>() {
         new EnemyWavePart(15,Enemies.Goblin), 
         new EnemyWavePart(15,Enemies.Goblin),
         new EnemyWavePart(20,Enemies.Spider)
      });
      waves.Add(wave);
      
      //wave 5
      wave = new EnemyWave(5,new List<EnemyWavePart>() {
         new EnemyWavePart(8,Enemies.Orc)
      });
      waves.Add(wave);
      
      //wave 6
      wave = new EnemyWave(6,new List<EnemyWavePart>() {
         new EnemyWavePart(12,Enemies.Orc)
      });
      waves.Add(wave);
      
      //wave 7
      wave = new EnemyWave(7,new List<EnemyWavePart>() {
         new EnemyWavePart(20,Enemies.Goblin),
         new EnemyWavePart(12,Enemies.Orc)
      });
      waves.Add(wave);
      
      //wave 8
      wave = new EnemyWave(8,new List<EnemyWavePart>() {
         new EnemyWavePart(14,Enemies.Goblin),
         new EnemyWavePart(15,Enemies.Orc)
      });
      waves.Add(wave);
      
      //wave 9
      wave = new EnemyWave(9,new List<EnemyWavePart>() {
         new EnemyWavePart(16,Enemies.Goblin),
         new EnemyWavePart(20,Enemies.Orc)
      });
      waves.Add(wave);
      
      //wave 10
      wave = new EnemyWave(10,new List<EnemyWavePart>() {
         new EnemyWavePart(16,Enemies.Goblin),
         new EnemyWavePart(18,Enemies.Orc),
         new EnemyWavePart(16,Enemies.Goblin),
         new EnemyWavePart(4,Enemies.Orc)
      });
      waves.Add(wave);
      
      //wave 11
      wave = new EnemyWave(11,new List<EnemyWavePart>() {
         new EnemyWavePart(25,Enemies.Wolf),
         new EnemyWavePart(10,Enemies.Orc),
         new EnemyWavePart(45,Enemies.Spider),
         new EnemyWavePart(30,Enemies.Goblin)
      });
      waves.Add(wave);
      
      //wave 12
      wave = new EnemyWave(11,new List<EnemyWavePart>() {
         new EnemyWavePart(50,Enemies.Wolf),
         new EnemyWavePart(50,Enemies.Orc),
         new EnemyWavePart(50,Enemies.Spider),
         new EnemyWavePart(50,Enemies.Goblin)
      });
      waves.Add(wave);
      
      //wave 13
      wave = new EnemyWave(11,new List<EnemyWavePart>() {
         new EnemyWavePart(100,Enemies.Wolf),
         new EnemyWavePart(100,Enemies.Goblin),
         new EnemyWavePart(100,Enemies.Orc),
         new EnemyWavePart(100,Enemies.Spider),
         new EnemyWavePart(1,Enemies.Wolf),
         new EnemyWavePart(1,Enemies.Goblin),
         new EnemyWavePart(1,Enemies.Orc),
         new EnemyWavePart(1,Enemies.Spider),
      });
      waves.Add(wave);
      
   }
}
