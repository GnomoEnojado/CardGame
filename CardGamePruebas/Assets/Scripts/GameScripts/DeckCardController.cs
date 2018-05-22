using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckCardController : MonoBehaviour {
    public Card card;
    public Text seCost;
    public Text txtName;
    public Image artImage;

    // Use this for initialization
    void Start () {

        txtName.text = card.name.ToString();
        artImage.sprite = card.artImage;
        seCost.text = card.seCost.ToString();
  
    }
	
}
