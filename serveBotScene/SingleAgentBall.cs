using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using static SingleAgentConstants;

public class SingleAgentBall : MonoBehaviour
{
   
    Rigidbody ballRB;

    public SingleAgentGameController gameController;

    ObjectTypeEnum lastCollidedWith;
    TeamEnum lastHitAgent;
    TeamEnum nextAgentTurn;

    Team typeA;
    Team typeB;

    void Start()
    {
        Debug.Log("ball starts");

        ballRB = GetComponent<Rigidbody>();
        typeA = new Team(TeamEnum.AGENT);
        typeB = new Team(TeamEnum.BOT);

        resetParameters();
    }

    void OnCollisionEnter(Collision c)
    {
        /*************** if ball collides with agent **********/
        if (c.gameObject.CompareTag(tag_agent))
        {
            // Debug.Log("ball hits agent: " + c.gameObject.tag);

            TeamEnum agentTypeEnum =
                c.gameObject.CompareTag(tag_agent) ?
                TeamEnum.AGENT : TeamEnum.BOT;

            Team agentType = agentTypeEnum.Equals(TeamEnum.AGENT) ? typeA : typeB;

            //calls ballHitsAgent()
            gameController.ballHitsAgent(agentType, lastCollidedWith,
                lastHitAgent, nextAgentTurn);

            //setting parameters
            lastHitAgent = agentTypeEnum;

            lastCollidedWith = agentTypeEnum.Equals(TeamEnum.AGENT) ?
                ObjectTypeEnum.AGENT : ObjectTypeEnum.BOT;

            nextAgentTurn = TeamEnum.NA;
        }

        /*************** if ball collides with floor **********/

        else if (c.gameObject.CompareTag(tag_floorA)
            || c.gameObject.CompareTag(tag_floorB))
        {
            //  Debug.Log("ball hits floor: " + c.gameObject.tag);

            TeamEnum floorTypeEnum =
                c.gameObject.CompareTag(tag_floorA) ?
                TeamEnum.AGENT : TeamEnum.BOT;

            Team floorType = floorTypeEnum.Equals(TeamEnum.AGENT) ? typeA : typeB;

            //calls ballHitsFloor()
            gameController.ballHitsFloor(floorType, lastCollidedWith,
                lastHitAgent, nextAgentTurn);

            lastCollidedWith = floorTypeEnum.Equals(TeamEnum.AGENT) ?
                ObjectTypeEnum.FLOOR_A : ObjectTypeEnum.FLOOR_B;

            nextAgentTurn = floorTypeEnum;
        }

        /*************** if ball collides with net or boundary **********/

        else if (c.gameObject.CompareTag(tag_net)
            || c.gameObject.CompareTag(tag_boundaryA)
            || c.gameObject.CompareTag(tag_boundaryB))
        {

            // Debug.Log("ball hits boundary: " + c.gameObject.tag);

            TeamEnum boundaryTypeEnum;
            if (c.gameObject.CompareTag(tag_net))
                boundaryTypeEnum = TeamEnum.NA;
            else
            {
                boundaryTypeEnum =
                c.gameObject.CompareTag(tag_boundaryA) ?
                TeamEnum.AGENT : TeamEnum.BOT;
            }

            Team boundaryType = boundaryTypeEnum.Equals(TeamEnum.AGENT) ? typeA : typeB;
            boundaryType = boundaryTypeEnum.Equals(TeamEnum.NA) ? null : boundaryType;

            //calls ballHitsBoundary()
            gameController.ballHitsBoundary(boundaryType,
                 lastCollidedWith, lastHitAgent, nextAgentTurn);

            if (boundaryType == null)
                lastCollidedWith = ObjectTypeEnum.NET;

            else {
                lastCollidedWith = boundaryType.Equals(TeamEnum.AGENT) ?
                ObjectTypeEnum.BOUNDARY_A : ObjectTypeEnum.BOUNDARY_B;
            }
            
        }

        else
            gameController.ballCollidesEdgeCase();
       
    }

    // rewards agent for hitting ball over net
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(tag_overNet_trigger))
        {
            // Debug.Log("over net");
            if (!lastHitAgent.Equals(TeamEnum.NA))
            {
                gameController.agentReward(lastHitAgent, RewardType.AGENT_HITS_BALL_ACROSS_NET);
            }

        }
    }


    public void resetParameters()
    {
        // Debug.Log("Ball: reset parameters");
        lastHitAgent = TeamEnum.NA;
        nextAgentTurn = TeamEnum.NA;
        lastCollidedWith = ObjectTypeEnum.NA;
    }

    public void setLastHitAgent(TeamEnum team) {
        lastHitAgent = team;
    }

}