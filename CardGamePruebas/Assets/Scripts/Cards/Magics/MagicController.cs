using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour {

    public virtual bool CanActiveEffect(int aIdFloor)
    {
        return true;
    }
    public virtual void ActiveEffect(int aIdFloor, int aIdCard)
    {

    }
}
