using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MovingPlatform_S : MonoBehaviour
{
    [HideInInspector]
    public bool simplifyControls;
    [HideInInspector]
    public Vector2 startPos;
    [HideInInspector]
    public Vector2 endPos;
    [HideInInspector]
    public Vector2 endPosDiff;
    [HideInInspector]
    public float lerpTime;
    [HideInInspector]
    public bool repeat;

    private Rigidbody2D rigid;

    private float angle = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (simplifyControls)
        {
            startPos = transform.position;
            endPos = startPos + endPosDiff;
        }

        rigid = GetComponent<Rigidbody2D>();

        angle = Mathf.Atan2((endPos.y - startPos.y) , (endPos.x - startPos.x));

        StartCoroutine(MovingPlatform(angle, repeat));
    }

    // Update is called once per frame
    void Update()
    {
    }


    private IEnumerator MovingPlatform(float angleNeeded, bool goBack)
    {rigid.velocity = new Vector2(lerpTime * Mathf.Cos(angleNeeded), lerpTime * Mathf.Sin(angleNeeded));
        
        yield return new WaitWhile(() => !(((endPos.y >= startPos.y && transform.position.y >= endPos.y) || (endPos.y < startPos.y && transform.position.y <= endPos.y)) &&
            ((endPos.x >= startPos.x && transform.position.x >= endPos.x) || (endPos.x < startPos.x && transform.position.x <= endPos.x))));

        transform.position = endPos;
        if (goBack)
        {
            Vector2 temp = endPos;
            endPos = startPos;
            startPos = temp;
            StartCoroutine(MovingPlatform(Mathf.Atan2((endPos.y - startPos.y), (endPos.x - startPos.x)), true));
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Rigidbody2D>() != null)
                    child.GetComponent<Rigidbody2D>().velocity = new Vector2(child.GetComponent<Rigidbody2D>().velocity.x, rigid.velocity.y);
            }
        }
        else
        {
            rigid.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.collider.transform.GetComponent<Player_S>().PositionInheritance(true, transform);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.collider.transform.GetComponent<Enemy_Wolf_S>().PositionInheritance(true, transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.collider.transform.GetComponent<Player_S>().PositionInheritance(false, transform);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.collider.transform.GetComponent<Enemy_Wolf_S>().PositionInheritance(true, transform);
        }
    }

    private void EditorGUI()
    {
        
    }
}
