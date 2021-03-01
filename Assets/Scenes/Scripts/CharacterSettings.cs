using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSettings", menuName = "Create CharacterSettings", order = 1)]
public class CharacterSettings : ScriptableObject
{
    public float PlayerSpeed = 2f;
    public float BulletSpeed = 3f;
    public float PlayerFireRate;
    public float ShotImpactStrength;
}