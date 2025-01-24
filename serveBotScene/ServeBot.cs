using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SingleAgentConstants;

public class ServeBot : MonoBehaviour
{
    public GameObject ballObj;
    Rigidbody ballRB;
    SingleAgentBall ball;
    int score;

    void Start()
    {
        Debug.Log("ball starts");

        ballRB = ballObj.GetComponent<Rigidbody>();
        ball = ballObj.GetComponent<SingleAgentBall>();
        serveBall();
    }

    //Randomly initializes ball at diffrent position and velocity
   public void serveBall() {
        
        //Debug.Log("serving ball");
        var ballPos_X = gameObject.transform.position.x + 0.1f;
        var ballPos_Y = Random.Range(init_transform_ball_Y_LB, init_transform_ball_Y_UB);
        var ballPos_Z = Random.Range(init_transform_ball_Z_LB, init_transform_ball_Z_UB);

        gameObject.transform.position = new Vector3(gameObject.transform.position.x,
            gameObject.transform.position.y, ballPos_Z);

        ballRB.transform.position = new Vector3(ballPos_X, ballPos_Y,
            ballPos_Z);


        var x_vel = Random.Range(2.7f, 4f);
        var y_vel = 0f;

        x_vel = Random.Range(2.7f, 3f);

        if (x_vel <= 2.9f)
           y_vel = Random.Range(3.7f, 4f);

        else if(x_vel <= 3f)
            y_vel = Random.Range(3.3f, 3.8f);

        else if (x_vel <= 3.5f)
            y_vel = Random.Range(3.3f, 4f);

        else if (x_vel <= 4f)
            y_vel = Random.Range(3f, 3.5f);

  
        var z_vel = 0f;
        if (ballPos_Z < 0)
            z_vel = Random.Range(0f, 0.5f);
        else
            z_vel = Random.Range(-0.5f, 0f);

        Debug.Log("x: " + x_vel + ", y_vel: " + y_vel + ", z_vel: " + z_vel);

        ball.setLastHitAgent(TeamEnum.BOT);
        ballRB.velocity = new Vector3(x_vel, y_vel, z_vel);

    }


    public void resetScore()
    {
        score = 0;
    }

    public void addScore(int addBy)
    {
        score = score + addBy;
    }

    public int getScore()
    {
        return score;
    }

}
