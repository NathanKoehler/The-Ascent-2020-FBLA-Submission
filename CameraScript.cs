using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_S : MonoBehaviour
{
    public GameObject player;

    [SerializeField]
    private float xRange = 10;
    [SerializeField]
    private float yRange = 10;
    [SerializeField]
    private float maxVelocity = 5;

    private Player_S player_S;
    private Rigidbody2D playerRigid;
    private Transform pTransform;
    private bool facingDir;
    private float lerpStart = 0;
    private bool lerpTrue = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRigid = player.GetComponent<Rigidbody2D>();
        player_S = player.GetComponent<Player_S>();
        pTransform = player.transform;
        facingDir = player_S.facingDir;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector2(Mathf.Clamp(transform.position.x, -xRange, xRange), Mathf.Clamp(transform.position.y, -yRange, yRange));
        CameraControl();
    }

    private void FixedUpdate()
    {

    }


    private void CameraControl()
    {
        /*
        float xPos = pTransform.position.x;
        float yPos = pTransform.position.y;
        float xLPos = transform.localPosition.x;
        float yLPos = transform.localPosition.y;
        float lerpLength = 0;
        
        

        transform.position = new Vector2(xPos + Mathf.Clamp(xLPos, -xRange, xRange), yPos + Mathf.Clamp(yLPos, -yRange, yRange));

        
        if (facingDir != pScript.facingDir)
        {
            facingDir = pScript.facingDir;
            StartCoroutine(ChangeCamera(xPos, yPos, Time.time));
        }
        Debug.Log(Time.time);
        
        if (lerpTrue)
        {
            float lerpTime = (Time.time - lerpStart) * maxVelocity;
            if (pScript.facingDir)
            {
                lerpLength = xPos + xRange - transform.position.x;
                Debug.Log(lerpLength);
                float fractionMoved = lerpTime / (Mathf.Abs(lerpLength));
                if ((Mathf.Abs(lerpLength) > 0.00001))
                    transform.position = new Vector3(Mathf.Lerp(transform.position.x, xPos + xRange, fractionMoved), yPos, -10);
                else
                    lerpTrue = false;
            }
            else
            {
                lerpLength = xPos - xRange - transform.position.x;
                float fractionMoved = lerpTime / (Mathf.Abs(lerpLength));
                if ((Mathf.Abs(lerpLength) > 0.00001))
                    transform.position = new Vector3(Mathf.Lerp(transform.position.x, xPos - xRange, fractionMoved), yPos, -10);
                else
                    lerpTrue = false;
            }
        }
    }


    private IEnumerator ChangeCamera(float xPos, float yPos, float localStart)
    {
        for (int i = 0; i < 32; i++)
        {
            yield return new WaitForSeconds(1/8);

            if (facingDir != pScript.facingDir)
            {
                yield break;
            }
        }
        lerpTrue = true;
        lerpStart = localStart - 4;
    }
    */
    }
}

