using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAgentConstants
{
    public static float init_transform_agent_X_LB = 0.07f;
    public static float init_transform_agent_X_RB = 1.3f;
    public static float init_transform_agent_Y_LB = 0.13f;
    public static float init_transform_agent_Y_UB = 1f;

    public static float init_transform_ball_X_LB = 0.15f;
    public static float init_transform_ball_X_RB = 1.145f;

    public static float init_transform_ball_Y_LB = 0.25f;
    public static float init_transform_ball_Y_UB = 0.35f;

    public static float init_transform_ball_Z_LB = 0.53f;
    public static float init_transform_ball_Z_UB = -0.53f;

    public static float init_transform_ball_Z = 0f;

    public static float init_velocity_X = 5f;
    public static float init_velocity_Y = 2f;
    public static float init_velocity_Z = 5f;

    public static float ball_velocity_X = 20f;
    public static float ball_velocity_Y = 30f;
    public static float ball_velocity_Z = 0f;

    public static float init_rotate_agent_X = -55f;
    public static float init_rotate_agent_Y = 90f;
    public static float init_rotate_agent_Z = 180f;

    public static string tag_ball = "singleAgent_ball";
    public static string tag_agent = "singleAgent_bat";
    public static string tag_bot = "singleAgent_bot";

    public static string tag_floorA = "singleAgent_floorA";
    public static string tag_floorB = "singleAgent_floorB";

    public static string tag_boundaryA = "singleAgent_boundaryA";
    public static string tag_boundaryB = "singleAgent_boundaryB";

    public static string tag_net = "singleAgent_net";
    public static string tag_overNet = "singleAgent_overNet";
    public static string tag_overNet_trigger = "singleAgent_overNet_trigger";

    public static string tag_canvas = "singleAgent_canvas";
    public static string tag_scoreA = "singleAgent_scoreA";
    public static string tag_scoreB = "singleAgent_scoreB";
    public static string tag_episode = "singleAgent_episode";
    public static string tag_step_count = "singleAgent_stepCount";

    public static string tag_gameController = "singleAgent_gameController";
    public static string tag_game_textA = "singleAgent_gameA";
    public static string tag_game_textB = "singleAgent_gameB";
    public static string tag_game_count_text = "singleAgent_gameCount";

    public static string env_max_academy_steps = "max_academy_steps";
    public static string env_reward_ball_hit = "singleAgent_reward_ball_hit";
    public static string env_reward_ball_over_net = "singleAgent_reward_ball_over_net";
    public static string env_reward_ball_hit_table = "singleAgent_reward_ball_hit_table";
    public static string env_penalty_foul_hit = "singleAgent_penalty_foul_hit";
    public static string env_penalty_no_hit = "singleAgent_penalty_no_hit";
    public static string env_penalty_foul_floor = "singleAgent_rpenalty_foul_floor";
    public static string env_penalty_boundary_hit = "singleAgent_penalty_boundary_hit";
    public static string env_penalty_net_hit = "singleAgent_penalty_net_hit";


    public enum RewardType {
        AGENT_HITS_BALL, AGENT_HITS_BALL_ACROSS_NET, AGENT_HITS_BALL_ONTO_TABLE,
        AGENT_FOUL_HIT, AGENT_FLOOR_FOUL, AGENT_DOESNT_HIT, AGENT_HITS_BOUNDARY, AGENT_HITS_NET
    }
    public enum ObjectTypeEnum
    {
        BALL, AGENT, BOT, NET, BOUNDARY_A, BOUNDARY_B,
        FLOOR_A, FLOOR_B, NA
    }

    public enum TeamEnum
    {
        AGENT, BOT, NA
    }

    public class Team
    {

        public TeamEnum teamEnum;

        public Team(TeamEnum type)
        {
            this.teamEnum = type;
        }
        public bool isAgent()
        {
            if (teamEnum.Equals(TeamEnum.AGENT))
                return true;
            else return false;
        }

        public bool isBot()
        {
            if (teamEnum.Equals(TeamEnum.BOT))
                return true;
            else return false;
        }

        public ObjectTypeEnum getAgent()
        {
            return isAgent() ? ObjectTypeEnum.AGENT : ObjectTypeEnum.BOT;
        }

        public ObjectTypeEnum getBoundary()
        {
            return isAgent() ? ObjectTypeEnum.BOUNDARY_A : ObjectTypeEnum.BOUNDARY_B;
        }

        public ObjectTypeEnum getFloor()
        {
            return isAgent() ? ObjectTypeEnum.FLOOR_A : ObjectTypeEnum.FLOOR_B;
        }

        public TeamEnum getTeam()
        {
            return teamEnum;
        }
        public TeamEnum getOpponentTeam()
        {
            if (teamEnum == TeamEnum.NA) return TeamEnum.NA;
            return isAgent() ? TeamEnum.BOT : TeamEnum.AGENT;
        }
    }
}
