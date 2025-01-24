using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using static TTConstants;

public class Ball : MonoBehaviour
{
    public GameController gameController;
    Rigidbody ballRB;

    ObjectTypeEnum lastCollidedWith;
    TeamEnum lastHitAgent;
    TeamEnum nextAgentTurn;

    Team typeA;
    Team typeB;

    void Start()
    {
        Debug.Log("ball starts");

        ballRB = GetComponent<Rigidbody>();
        typeA = new Team(TeamEnum.A);
        typeB = new Team(TeamEnum.B);

        resetParameters();
    }
   
    void OnCollisionEnter(Collision c) {
        /*************** if ball collides with agent **********/

        if (c.gameObject.CompareTag(tag_agentA)
            || c.gameObject.CompareTag(tag_agentB))
        {
           // Debug.Log("ball hits agent: " + c.gameObject.tag);

            TeamEnum agentTypeEnum =
                c.gameObject.CompareTag(tag_agentA)?
                TeamEnum.A : TeamEnum.B;

            Team agentType = agentTypeEnum.Equals(TeamEnum.A)?typeA: typeB;

            //calls ballHitsAgent()
            gameController.ballHitsAgent(agentType, lastCollidedWith,
                lastHitAgent, nextAgentTurn);

            //setting parameters
            lastHitAgent = agentTypeEnum;

            lastCollidedWith = agentTypeEnum.Equals(TeamEnum.A) ?
                ObjectTypeEnum.AGENT_A : ObjectTypeEnum.AGENT_B;

            nextAgentTurn = TeamEnum.NA;
        }

        /*************** if ball collides with floor **********/

        else if (c.gameObject.CompareTag(tag_floorA)
            || c.gameObject.CompareTag(tag_floorB))
        {
          //  Debug.Log("ball hits floor: " + c.gameObject.tag);

            TeamEnum floorTypeEnum =
                c.gameObject.CompareTag(tag_floorA) ?
                TeamEnum.A : TeamEnum.B;

            Team floorType = floorTypeEnum.Equals(TeamEnum.A) ? typeA : typeB;

            //calls ballHitsFloor()
            gameController.ballHitsFloor(floorType, lastCollidedWith,
                lastHitAgent, nextAgentTurn);

            lastCollidedWith = floorTypeEnum.Equals(TeamEnum.A) ?
                ObjectTypeEnum.FLOOR_A : ObjectTypeEnum.FLOOR_B;

            nextAgentTurn = floorTypeEnum;
        }

        /*************** if ball collides with net or boundary **********/

        else if (c.gameObject.CompareTag(tag_net)
            || c.gameObject.CompareTag(tag_boundaryA)
            || c.gameObject.CompareTag(tag_boundaryB)) {

           // Debug.Log("ball hits boundary: " + c.gameObject.tag);

            TeamEnum boundaryTypeEnum;
            if (c.gameObject.CompareTag(tag_net))
                boundaryTypeEnum = TeamEnum.NA;

            boundaryTypeEnum =
                c.gameObject.CompareTag(tag_boundaryA) ?
                TeamEnum.A : TeamEnum.B;

            Team boundaryType = boundaryTypeEnum.Equals(TeamEnum.A) ? typeA : typeB;
            boundaryType = boundaryTypeEnum.Equals(TeamEnum.NA) ? null : boundaryType;

            //calls ballHitsBoundary()
            gameController.ballHitsBoundary(boundaryType,
                 lastCollidedWith, lastHitAgent, nextAgentTurn);

            lastCollidedWith = boundaryType.Equals(TeamEnum.A) ?
                ObjectTypeEnum.BOUNDARY_A : ObjectTypeEnum.BOUNDARY_B;
            if (boundaryType == null)
                lastCollidedWith = ObjectTypeEnum.NET;

        }

    }

    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TTConstants.tag_overNet_trigger))
        {
           // Debug.Log("over net");
            if (!lastHitAgent.Equals(TeamEnum.NA))
            {
                gameController.agentHitsBallAcrossNetReward(
                lastHitAgent.Equals(TeamEnum.A) ? typeA : typeB);
            }

        }
    }
    

    public void reset(TeamEnum serve)
    {
        var ballPos_X = serve == TeamEnum.A ? Random.Range(init_transform_ball_X_LB,
                init_transform_agent_X_RB) : Random.Range(-init_transform_ball_X_RB,
                -init_transform_agent_X_LB);

        transform.position = new Vector3(ballPos_X, init_transform_ball_Y, init_transform_ball_Z);

        ballRB.velocity = new Vector3(0f, 0f, 0f);

    }

    public void resetParameters() {
       // Debug.Log("Ball: reset parameters");
        lastHitAgent = TeamEnum.NA;
        nextAgentTurn = TeamEnum.NA;
        lastCollidedWith = ObjectTypeEnum.NA;
    }
    
}