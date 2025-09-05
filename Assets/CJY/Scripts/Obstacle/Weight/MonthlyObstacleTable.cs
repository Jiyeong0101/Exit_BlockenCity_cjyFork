using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObstacleTable", menuName = "Game/Monthly Obstacle Table")]
public class MonthlyObstacleTable : ScriptableObject
{
    [Range(1, 12)]
    public int month;

    [System.Serializable]
    public class Entry
    {
        public ObstacleType type;
        [Range(0, 100)]
        public int weight;
    }

    public List<Entry> obstacles = new List<Entry>();
}
