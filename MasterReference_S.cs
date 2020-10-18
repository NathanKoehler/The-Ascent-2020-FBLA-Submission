using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterReference_S : MonoBehaviour
{
    public void ReloadPreviousScene()
    {
        MasterController_S.self.ReloadPreviousScene();
    }
}
