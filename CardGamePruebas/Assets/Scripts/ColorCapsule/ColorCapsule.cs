using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCapsule : MonoBehaviour {
    public List<Material> materialsPlayer;
    public void SetMaterialColor(int aPlayerNumber)
    {
        GetComponent<MeshRenderer>().material = materialsPlayer[aPlayerNumber - 1];
    }
}
