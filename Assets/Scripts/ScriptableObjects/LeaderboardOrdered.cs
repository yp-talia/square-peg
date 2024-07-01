using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

// For now, this is a class with one variable, but it'll likely expand

public class Positions
{
public String name;

}

[CreateAssetMenu(fileName = "Leaderboard Ordered", menuName = "Leaderboard Ordered")]
public class LeaderboardOrdered : ScriptableObject
{
    public Positions[] position;
}
