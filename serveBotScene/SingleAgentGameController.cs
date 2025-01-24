using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using static SingleAgentConstants;

public class SingleAgentGameController : MonoBehaviour
{

    public SingleAgentBall ball;
    public SingleAgentBat agent;
    public ServeBot bot;
    Rigidbody ballRB;

    int resetTimer = 0;
    float maxEnvironmentSteps;
    float rewardBallHit;
    float rewardBallOverNet;
    float rewardBallOnTable;
    float penaltyFoulHit;
    float penaltyNoHit;
    float penaltyFloorFoul;
    float penaltyBoundaryHit;
    float penaltyNetHit;

    EnvironmentParameters environmentParameters;

    public void Start()
    {
        Debug.Log("game controller called");
        ballRB = ball.GetComponent<Rigidbody>();
        environmentParameters = Academy.Instance.EnvironmentParameters;

        maxEnvironmentSteps = environmentParameters.GetWithDefault(env_max_academy_steps, 10000);
        rewardBallHit = environmentParameters.GetWithDefault(env_reward_ball_hit, 0.2f);
        rewardBallOverNet = environmentParameters.GetWithDefault(env_reward_ball_over_net, 0.3f);
        rewardBallOnTable = environmentParameters.GetWithDefault(env_reward_ball_hit_table, 0.5f);

        penaltyFoulHit = environmentParameters.GetWithDefault(env_penalty_foul_hit, 0);
        penaltyNoHit = environmentParameters.GetWithDefault(env_penalty_no_hit, 0);
        penaltyFloorFoul = environmentParameters.GetWithDefault(env_penalty_foul_floor, 0);
        penaltyBoundaryHit = environmentParameters.GetWithDefault(env_penalty_boundary_hit, 0);
        penaltyNetHit = environmentParameters.GetWithDefault(env_penalty_net_hit, 0);

    }

    void agentScores(TeamEnum team)
    {
        Debug.Log("agent scores: " + team.ToString());
        if (team.Equals(TeamEnum.AGENT))
            agent.addScore(1);
        else
            bot.addScore(1);

        Debug.Log("agent scores: calling reset episode");
       episodeReset();

    }

    public void agentReward(TeamEnum team, RewardType rewardType) {
        
        Debug.Log("reward: " + rewardType.ToString() + ", team: " + team.ToString());

        if (!team.Equals(TeamEnum.AGENT))
            return;

        switch (rewardType) {
            case RewardType.AGENT_HITS_BALL:
                this.agent.AddReward(rewardBallHit);
                break;
            case RewardType.AGENT_HITS_BALL_ACROSS_NET:
                this.agent.AddReward(rewardBallOverNet);
                break;
            case RewardType.AGENT_HITS_BALL_ONTO_TABLE:
                this.agent.AddReward(rewardBallOnTable);
                break;
            case RewardType.AGENT_FOUL_HIT:
                this.agent.AddReward(penaltyFoulHit);
                break;
            case RewardType.AGENT_FLOOR_FOUL:
                this.agent.AddReward(penaltyFloorFoul);
                break;
            case RewardType.AGENT_DOESNT_HIT:
                this.agent.AddReward(penaltyNoHit);
                break;
            case RewardType.AGENT_HITS_BOUNDARY:
                this.agent.AddReward(penaltyBoundaryHit);
                break;
            case RewardType.AGENT_HITS_NET:
                this.agent.AddReward(penaltyBoundaryHit);
                break;

        }

    }

    public void ballHitsAgent(Team team,
       ObjectTypeEnum lastCollidedWith,
       TeamEnum lastHitAgentTeam,
        TeamEnum nextAgentTurn)
    {

        //agent didn't let the ball bounce on his side (check serve case)
        //or agent hits the ball twice

        agentReward(team.teamEnum, RewardType.AGENT_HITS_BALL);
       // agentHitsBallReward(team);

        //negative reward for hitting before ball hits floor or hitting ball twice
        if (lastCollidedWith != team.getFloor()
            || lastHitAgentTeam == team.getTeam())
            
        {
            agentReward(team.teamEnum, RewardType.AGENT_FOUL_HIT);
            //agentPenalty(team.getTeam());
            agentScores(team.getOpponentTeam());

        }

    }

    public void ballHitsFloor(Team floor,
       ObjectTypeEnum lastCollidedWith,
       TeamEnum lastHitAgentTeam,
       TeamEnum nextAgentTurn)
    {
        //hits on his own court side?
        //or if agent doesn't serve 
        //agent let ball bounce twice on his court

        if (lastHitAgentTeam == floor.getTeam()
            || lastCollidedWith == floor.getFloor())
        {
            agentReward(floor.getTeam(), RewardType.AGENT_FLOOR_FOUL);
            //agentPenalty(floor.getTeam());
            agentScores(floor.getOpponentTeam());

        }
        else if (lastHitAgentTeam == floor.getOpponentTeam()) {
            agentReward(lastHitAgentTeam, RewardType.AGENT_HITS_BALL_ONTO_TABLE);
            //agentHitsOpponentTableReward(lastHitAgentTeam);
        }
    }

    public void ballHitsBoundary(Team boundary,
        ObjectTypeEnum lastCollidedWith,
        TeamEnum lastHitAgentTeam,
        TeamEnum nextAgentTurn)
    {
        Debug.Log("ball hits boundary call: ");
        if(boundary==null) Debug.Log("net");
        else Debug.Log(boundary.teamEnum.ToString());
       
        Debug.Log("lastCollidedWith: " + lastCollidedWith.ToString()
            + ", lastHitAgentTeam: " + lastHitAgentTeam.ToString()
            + ", nextAgentTurn: " + nextAgentTurn.ToString());

        if (nextAgentTurn != TeamEnum.NA)
        {
            agentReward(nextAgentTurn, RewardType.AGENT_DOESNT_HIT);
            //agentPenalty(nextAgentTurn);
            agentScores(nextAgentTurn == TeamEnum.AGENT ?
                  TeamEnum.BOT : TeamEnum.AGENT);

        }
        else if (lastHitAgentTeam == TeamEnum.AGENT) {
            agentReward(TeamEnum.AGENT, RewardType.AGENT_HITS_BOUNDARY);
            agentScores(TeamEnum.BOT);

            //agentPenalty(TeamEnum.AGENT);
        }
        else if (lastHitAgentTeam == TeamEnum.BOT) {
            Debug.Log("bot threw ball onto boundary: ");
            if(boundary==null) Debug.Log("net");
            else Debug.Log(boundary.teamEnum.ToString());
            agentScores(TeamEnum.AGENT);
            //TODO 

        }
        else
        {
            Debug.Log("ball hits boundary edge case");
            episodeReset();
        }
    }

    public void agentHitsNet(Team team)
    {
        agentReward(team.teamEnum, RewardType.AGENT_HITS_NET);
        agentScores(team.getOpponentTeam());
    }

    public void ballCollidesEdgeCase() {
        Debug.Log("Ball collides edge case");
        episodeReset();

    }

    void FixedUpdate()
    {
        resetTimer += 1;
        if (resetTimer >= maxEnvironmentSteps && maxEnvironmentSteps > 0)
        {
            Debug.Log("reset timer exceeded max steps");
            agent.EpisodeInterrupted();
            episodeReset();
        }

    }

   
    void episodeReset()
    {
        Debug.Log("GC: Resetting episode");
        resetTimer = 0;
        agent.EndEpisode();
        agent.resetRacket();
        ball.resetParameters();
        bot.serveBall();
    }

    public void matchReset()
    {
        Debug.Log("Resetting match");
        episodeReset();
        agent.resetScore();
        bot.resetScore();
    }

}