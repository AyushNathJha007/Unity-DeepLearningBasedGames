using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public GameObject paddle;

    //public GameObject paddle2;
    public string tagg;
    public GameObject ball;
    Rigidbody2D brb;
    float yvel;
    //float yvel2;
    float paddleMinY = 8.8f;
    float paddleMaxY = 17.4f;
    float paddleMaxSpeed = 15;
    public float numsaved = 0;
    public float numMissed = 0;

    ANN ann;

    void Start()
    {
        ann = new ANN(6, 1, 1, 4, 0.05);
        brb = ball.GetComponent<Rigidbody2D>();
    }

    
    List<double> Run(double bx,double by,double bvx,double bvy,double px,double py,double pv,bool train)
    {
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>();
        inputs.Add(bx);
        inputs.Add(by);
        inputs.Add(bvx);
        inputs.Add(bvy);
        inputs.Add(px);
        inputs.Add(py);
        outputs.Add(pv);
        if (train)
            return (ann.Train(inputs, outputs));
        else
            return (ann.CalcOutput(inputs, outputs));
    }

    // Update is called once per frame
    void Update()
    {
        float posy = Mathf.Clamp(paddle.transform.position.y + (yvel * Time.deltaTime * paddleMaxSpeed), paddleMinY, paddleMaxY);
        paddle.transform.position = new Vector3(paddle.transform.position.x, posy, paddle.transform.position.z);
        List<double> output = new List<double>();
        int layerMask = 1 << 9;
        RaycastHit2D hit = Physics2D.Raycast(ball.transform.position, brb.velocity, 1000, layerMask);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "tops")
            {
                Vector3 reflection = Vector3.Reflect(brb.velocity, hit.normal);
                hit = Physics2D.Raycast(hit.point, reflection, 1000, layerMask);
            }
            if (hit.collider != null && hit.collider.gameObject.tag == tagg)
            {
                float dy = (hit.point.y - paddle.transform.position.y);
                output = Run(ball.transform.position.x, ball.transform.position.y, brb.velocity.x, brb.velocity.y, paddle.transform.position.x, paddle.transform.position.y, dy, true);
                yvel = (float)output[0];
            }
        }

        
    
        else
            yvel = 0;




        /*float posy2 = Mathf.Clamp(paddle2.transform.position.y + (yvel2 * Time.deltaTime * paddleMaxSpeed), paddleMinY, paddleMaxY);
        paddle2.transform.position = new Vector3(paddle2.transform.position.x, posy2, paddle2.transform.position.z);
        List<double> output2 = new List<double>();
        int layerMask2 = 1 << 9;
        RaycastHit2D hit2 = Physics2D.Raycast(ball.transform.position, brb.velocity, 1000, layerMask2);

        if (hit2.collider != null)
        {
            if (hit2.collider.gameObject.tag == "tops")
            {
                Vector3 reflection = Vector3.Reflect(brb.velocity, hit2.normal);
                hit2 = Physics2D.Raycast(hit2.point, reflection, 1000, layerMask2);
            }
            if (hit2.collider != null && hit2.collider.gameObject.tag == "backwall2")
            {
                float dy = (hit2.point.y - paddle2.transform.position.y);
                output = Run(ball.transform.position.x, ball.transform.position.y, brb.velocity.x, brb.velocity.y, paddle2.transform.position.x, paddle2.transform.position.y, dy, true);
                yvel2 = (float)output[0];
            }
        }



        else
            yvel2 = 0;*/

    }

}
