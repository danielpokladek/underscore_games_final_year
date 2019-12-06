using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharBio : ScriptableObject
{
    public string charName;
    public enum CharClass { Ranger, Melee, Mage }
    public CharClass charClass;
}
