using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using static TTConstants;

public class TTAgent : Agent{

    public GameObject ttArea;
    public GameObject ball;
    public bool isAgentA;

    float agent_mult;
    int score;

    Rigidbody agentRB;
    Rigidbody ballRB;
    GameController gameController;

    Team typeA;
    Team typeB;

  
    public override void Initialize() {
       // Debug.Log("Agent initialize: " + getString());
        agentRB = GetComponent<Rigidbody>();
        ballRB = ball.GetComponent<Rigidbody>();
        gameController = ttArea.GetComponent<GameController>();

        agent_mult = isAgentA ? 1f : -1f;

        typeA = new Team(TeamEnum.A);
        typeB = new Team(TeamEnum.B);

        resetRacket();

    }

    public override void OnEpisodeBegin(){
        resetRacket();
    }

    //called everytime agent requests decision
    //choose a type of sensor to use for our project
    public override void CollectObservations(VectorSensor sensor){
        // base.CollectObservations(sensor);
        // Debug.Log("Agent collect observation: " + getString());

        sensor.AddObservation(agent_mult * (transform.position.x - ttArea.transform.position.x));
        sensor.AddObservation(transform.position.y - ttArea.transform.position.y);
        //sensor.AddObservation(transform.position.z - ttArea.transform.position.z);

        sensor.AddObservation(agent_mult * agentRB.velocity.x);
        sensor.AddObservation(agentRB.velocity.y);
        //sensor.AddObservation(agentRB.velocity.z);

        sensor.AddObservation(agent_mult * (ball.transform.position.x - ttArea.transform.position.x));
        sensor.AddObservation(ball.transform.position.y - ttArea.transform.position.y);
        //sensor.AddObservation(ball.transform.position.z - ttArea.transform.position.z);

        sensor.AddObservation(agent_mult * ballRB.velocity.x);
        sensor.AddObservation(ballRB.velocity.y);
        //sensor.AddObservation(ballRB.velocity.z);

        sensor.AddObservation(agent_mult * gameObject.transform.rotation.x);
        //sensor.AddObservation(gameObject.transform.rotation.y);
        //sensor.AddObservation(gameObject.transform.rotation.z);

    }

    public override void Heuristic(in ActionBuffers actionsOut) {
     
        //Debug.Log("Agent Heuristic");
        var contActionsOut = actionsOut.ContinuousActions;

        contActionsOut[0] = Input.GetAxis("Horizontal");
        contActionsOut[1] = Input.GetKey(KeyCode.X) ? 1f : 0f;   // Racket Jumping
        contActionsOut[2] = Input.GetAxis("Vertical"); //Racket Rotation
        //contActionsOut[3] = Input.GetKey(KeyCode.S)? -1f: 0f;// X axis
        //contActionsOut[4] = Input.GetKey(KeyCode.Y) ? -1f : 0f;// Y axis

    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //Debug.Log("on action received: " + getString());
        //execute actions
        ActionSegment<float> actSegment = actionBuffers.ContinuousActions;
        var moveX = Mathf.Clamp(actSegment[0], -1f, 1f) * agent_mult;
        var moveY = Mathf.Clamp(actSegment[1], -1f, 1f);
        var rotateX = Mathf.Clamp(actSegment[2], -1f, 1f) * agent_mult;
        agentRB.velocity = new Vector3(moveX * init_velocity_X,
                                        moveY * init_velocity_Y,
                                        0);

        gameObject.transform.rotation = Quaternion.Euler(-agent_mult * 90f
                                                            + rotateX * 55f,
                                                            90f,
                                                            agent_mult * 180f);
        /******* z-axis *******/
        /*
        var moveX = Mathf.Clamp(actSegment[0], -1f, 1f) * agent_mult;
        var moveY = Mathf.Clamp(actSegment[1], -1f, 1f);
        var moveZ = Mathf.Clamp(actSegment[2], -1f, 1f);
        var rotateX = Mathf.Clamp(actSegment[2], -1f, 1f) * agent_mult;
        var rotateY = Mathf.Clamp(actSegment[4], -1f, 1f) * agent_mult;
        agentRB.velocity = new Vector3(moveX * init_velocity_X,
                                        moveY * init_velocity_Y,
                                        moveZ * init_velocity_Z);

         gameObject.transform.rotation = Quaternion.Euler(-agent_mult * 90f + rotateX * 55f,
                                                            90f + rotateY * 55f,
                                                            agent_mult * 180);
        */
    }

    void OnCollisionEnter(Collision c){
        if (c.gameObject.CompareTag(TTConstants.tag_net)) {
            gameController.agentHitsNet(getTeam());
        }
    }

    public void resetRacket() {
       // Debug.Log("Reset racket: " + getString());

        var x_lb = isAgentA ? TTConstants.init_transform_agent_X_LB:
            agent_mult * TTConstants.init_transform_agent_X_RB;
        var x_rb = isAgentA ? TTConstants.init_transform_agent_X_RB :
            agent_mult * TTConstants.init_transform_agent_X_LB;

        transform.position = new Vector3(Random.Range(x_lb, x_rb),
            Random.Range(TTConstants.init_transform_agent_Y_LB,
            TTConstants.init_transform_agent_Y_UB), 0);
        
        transform.eulerAngles = new Vector3(
                  agent_mult * TTConstants.init_rotate_agent_X,
                   TTConstants.init_rotate_agent_Y,
                   agent_mult * TTConstants.init_rotate_agent_Z);
    }

    public void resetScore()
    {
        score = 0;
    }

    public void addScore(int addBy)
    {
        score = score+addBy;
    }
  
    public int getScore()
    {
        return score;
    }

    public string getString() {
        return "agent" + ((isAgentA == true) ? 
             TTConstants.TeamEnum.A.ToString()
             : TTConstants.TeamEnum.B.ToString());

    }

    public Team getTeam() {
        return isAgentA ? typeA : typeB;
    }
}