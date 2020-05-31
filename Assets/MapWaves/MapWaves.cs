using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapWaves
{
    //todo make better system 
    protected List<EnemyWave> waves;

    public EnemyWave NextWave() {
        EnemyWave res = waves[0];
        waves.RemoveAt(0);
        return res;
    }

    public bool Empty() {
        return waves.Count == 0;
    }
}
