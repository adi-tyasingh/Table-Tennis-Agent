using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using static TTConstants;

public class GameController : MonoBehaviour {
    
    public Ball ball;
    public TTAgent agentA;
    public TTAgent agentB;
    Rigidbody ballRB;

    int resetTimer = 0;
    float maxEnvironmentSteps;
    float ballHitReward;
    float ballOverNetReward;
    float agentReward;
    EnvironmentParameters environmentParameters;

    public void Start() {
        Debug.Log("game controller called");
        ballRB = ball.GetComponent<Rigidbody>();
        environmentParameters = Academy.Instance.EnvironmentParameters;

        maxEnvironmentSteps = environmentParameters.GetWithDefault(TTConstants.env_max_academy_steps, 10000);
        ballHitReward = environmentParameters.GetWithDefault(TTConstants.env_reward_ball_hit, 0);
        ballOverNetReward = environmentParameters.GetWithDefault(TTConstants.env_reward_ball_over_net, 0);
        agentReward = environmentParameters.GetWithDefault(TTConstants.env_reward_agent, 1);
        //matchReset();
    }

    void agentScores(TTConstants.TeamEnum agent)
    {
        float rewardA = (agent.Equals(TTConstants.TeamEnum.A) ? +1 : -1) * environmentParameters.GetWithDefault(TTConstants.env_reward_agent, 1); ;
        float rewardB = -rewardA;

        agentA.SetReward(rewardA);
        agentB.SetReward(rewardB);

        if (agent.Equals(TTConstants.TeamEnum.A))
            agentA.addScore(1);
        else
            agentB.addScore(1);

        //Debug.Log("agent scores: " + agent.ToString());
        //Debug.Log("CR: " + agentA.GetCumulativeReward());

        episodeReset();
    }

    void agentHitsBallReward(TTConstants.Team agent) {
        Debug.Log("Agent hits ball reward: " + agent.teamEnum.ToString()
        + ", reward: " + environmentParameters.GetWithDefault(TTConstants.env_reward_ball_hit, 0));

        if (agent.isA())
            agentA.AddReward(environmentParameters.GetWithDefault(TTConstants.env_reward_ball_hit, 0));
        else
            agentB.AddReward(environmentParameters.GetWithDefault(TTConstants.env_reward_ball_hit, 0));
    }

    public void agentHitsBallAcrossNetReward(TTConstants.Team agent)
    {
        //Debug.Log("Agent hits ball across net reward: " + agent.teamEnum.ToString());

        if (agent.isA())
            agentA.AddReward(environmentParameters.GetWithDefault(TTConstants.env_reward_ball_over_net, 0));
        else
            agentB.AddReward(environmentParameters.GetWithDefault(TTConstants.env_reward_ball_over_net, 0));

    }
    public void ballHitsAgent(TTConstants.Team agent,
        TTConstants.ObjectTypeEnum lastCollidedWith,
        TTConstants.TeamEnum lastHitAgentTeam,
        TTConstants.TeamEnum nextAgentTurn) {

        //agent didn't let the ball bounce on his side (check serve case)
        //or agent hits the ball twice

        // Debug.Log("GC ball hits agent: " + agent.teamEnum.ToString());

        agentHitsBallReward(agent);
       
        if (lastCollidedWith != ObjectTypeEnum.NA
            && lastCollidedWith != agent.getFloor())
                agentScores(agent.getOpponentTeam());

        else if (lastHitAgentTeam == agent.getTeam())
            agentScores(agent.getOpponentTeam());

    }

    public void ballHitsFloor(TTConstants.Team floor,
        TTConstants.ObjectTypeEnum lastCollidedWith,
        TTConstants.TeamEnum lastHitAgentTeam,
        TTConstants.TeamEnum nextAgentTurn)
    {
        //hits on his own court side?
        //or if agent doesn't serve 
        //agent let ball bounce twice on his court

        //Debug.Log("GC: ball hits floor: " + floor.teamEnum.ToString());
       
        if (lastHitAgentTeam == floor.getTeam()
            || lastCollidedWith == TTConstants.ObjectTypeEnum.NA
            || lastCollidedWith == floor.getFloor())
        {
            agentScores(floor.getOpponentTeam());

        }
    }

    public void ballHitsBoundary(TTConstants.Team boundary,
        TTConstants.ObjectTypeEnum lastCollidedWith,
        TTConstants.TeamEnum lastHitAgentTeam,
        TTConstants.TeamEnum nextAgentTurn)
    {
        //Debug.Log("GC ball hits boundary: " + boundary.teamEnum.ToString());

        if (nextAgentTurn != TTConstants.TeamEnum.NA)
            agentScores(nextAgentTurn == TTConstants.TeamEnum.A ?
                TTConstants.TeamEnum.B : TTConstants.TeamEnum.A);

        else if (lastHitAgentTeam == TTConstants.TeamEnum.A)
            agentScores(TTConstants.TeamEnum.B);
        else if (lastHitAgentTeam == TTConstants.TeamEnum.B)
            agentScores(TTConstants.TeamEnum.A);
        else
        {
            Debug.Log("ball hits boundary edge case");
            episodeReset();
        }
    }

    public void agentHitsNet(TTConstants.Team agent)
    {
        agentScores(agent.getOpponentTeam());
    }

    void FixedUpdate()
    {
        resetTimer += 1;
        if (resetTimer >= maxEnvironmentSteps && maxEnvironmentSteps > 0)
        {
            Debug.Log("reset timer exceeded max steps");
            agentA.EpisodeInterrupted();
            agentB.EpisodeInterrupted();
            episodeReset();
        }
        
       /*
       var ballVelocity = ballRB.velocity;
       if (ballVelocity.x == 0 && ballVelocity.y == 0 && ballVelocity.z == 0)
       {
           // Debug.Log("velocity = 0");
            episodeReset();
       }*/
       
        // ballRB.velocity = new Vector3(Mathf.Clamp(rgV.x, -9f, 9f), Mathf.Clamp(rgV.y, -9f, 9f), rgV.z);
    }

    void episodeReset()
    {
        resetTimer = 0;
        //Debug.Log("Resetting episode");
        //TODO whose turn to serve
        var flip = Random.Range(0, 2);
        var serve = (flip < 1) ? TTConstants.TeamEnum.A
                        : TTConstants.TeamEnum.B;

        agentA.EndEpisode();
        agentB.EndEpisode();
        agentA.resetRacket();
        agentB.resetRacket();
        //TODO whose turn is it when an episode ends
        ball.reset(serve);
        ball.resetParameters();
        //Debug.Log("CR: " + agentA.GetCumulativeReward());
    }

    public void matchReset()
    {
        Debug.Log("Resetting match");
        episodeReset();
        agentA.resetScore();
        agentB.resetScore();
    }

}