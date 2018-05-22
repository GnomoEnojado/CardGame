using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Card", menuName = "Card")]
public class Card : ScriptableObject {
    public int TypeCard; //0 monsters // 1 traps // 2 magics
    public string Name;
    public int Id;
    public string Effect;
    public GameObject prefabCard;

    public Sprite artImage;
    public int typeAttack;
    public int rarity;

    public int seCost;
    public int attack;
    public int defense;
    public int hp;
    public int velocity;

  
}
