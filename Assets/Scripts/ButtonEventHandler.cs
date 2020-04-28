using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEventHandler : MonoBehaviour
{
   
    public void OnClickSpin()
    {
        GameExtension.OnClickSpinButton();
    }
    
    public void OnClickStopSpin()
    {
        GameExtension.OnStopSpin();
    }
}
