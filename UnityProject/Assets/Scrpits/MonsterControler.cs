using UnityEngine;
using System.Collections;

public class MonsterControler : MonoBehaviour 
{
    public float scrollSpeed;
    // Use this for initialization

    Vector3 pos;

    Animator anim;

    void Start()
    {
        anim = gameObject.transform.Find("MonsterAnim").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float xAxisValue = Input.GetAxis("Horizontal");
        float yAxisValue = Input.GetAxis("Vertical");

        if (Camera.current != null)
        {
            Vector3 movement = Vector3.zero;

            movement.x += xAxisValue;
            movement.y += yAxisValue;

            movement.Normalize();

            movement *= Time.deltaTime * scrollSpeed;

            

            anim.SetFloat("Speed", 0);// movement.magnitude * scrollSpeed);

            pos += movement;

            gameObject.transform.Translate(movement);

            var p2 = gameObject.transform.position;

            p2 = new Vector3(p2.x, p2.y, Camera.current.transform.position.z);

            Camera.current.transform.position = p2;// Translate(movement * scrollSpeed * Time.deltaTime);// * Time.deltaTime);//new Vector3(xAxisValue, yAxisValue, 0.0f));
            gameObject.transform.position = pos;
        }

        Vector3 zPos = gameObject.transform.position;

        zPos.z = pos.y * 0.01f - 6.025f;

        gameObject.transform.position = zPos;
    }
}