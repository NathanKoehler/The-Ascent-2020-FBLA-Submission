using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lumin_S : MonoBehaviour
{
    private Rigidbody2D rigid;
    public Animator animator;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        

        if (MasterController_S.self.hasLumin)
        {
            animator.SetTrigger("Start Idle");
            Attach(MasterController_S.player_S.transform.GetChild(0).gameObject);

        }
    }

    public void Attach(GameObject attached)
    {
        SpringJoint2D springJoint = attached.AddComponent<SpringJoint2D>();
        springJoint.connectedBody = rigid;
        springJoint.frequency = 0.4f;
        springJoint.dampingRatio = 0.1f;
        springJoint.autoConfigureDistance = false;
        springJoint.distance = 3f;
    }

    
    public void ActivateLumin()
    {
        MasterController_S.self.hasLumin = true;
    }
}
