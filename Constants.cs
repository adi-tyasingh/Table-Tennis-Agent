using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTConstants
{
    public static float init_transform_agent_X_LB = 0.07f;
    public static float init_transform_agent_X_RB = 1.3f;
    public static float init_transform_agent_Y_LB = 0.13f;
    public static float init_transform_agent_Y_UB = 1f;

    public static float init_transform_ball_X_LB = 0.15f;
    public static float init_transform_ball_X_RB = 1.145f;
    public static float init_transform_ball_Y = 1f;
    public static float init_transform_ball_Z = 0f;

    public static float init_velocity_X = 5f;
    public static float init_velocity_Y = 2f;
    public static float init_velocity_Z = 5f;

    public static float init_rotate_agent_X = -55f;
    public static float init_rotate_agent_Y = 90f;
    public static float init_rotate_agent_Z = 180f;

    public static string tag_ball = "ball";
    public static string tag_agentA = "agentA";
    public static string tag_agentB = "agentB";

    public static string tag_floorA = "floorA";
    public static string tag_floorB = "floorB";

    public static string tag_boundaryA = "boundaryA";
    public static string tag_boundaryB = "boundaryB";

    public static string tag_net = "net";
    public static string tag_overNet = "overNet";
    public static string tag_overNet_trigger = "overNet_trigger";

    public static string tag_canvas = "canvas";
    public static string tag_scoreA = "scoreA";
    public static string tag_scoreB = "scoreB";
    public static string tag_episode= "episode";
    public static string tag_step_count = "stepCount";

    public static string tag_game_controller = "GameController";
    public static string tag_game_textA = "gameA";
    public static string tag_game_textB = "gameB";
    public static string tag_game_count_text = "gameCount";

    public static string env_max_academy_steps = "max_academy_steps";
    public static string env_reward_ball_hit = "reward_ball_hit";
    public static string env_reward_ball_over_net = "reward_ball_over_net";
    public static string env_reward_agent = "reward_agent";


    public enum ObjectTypeEnum {
        BALL, AGENT_A, AGENT_B, NET, BOUNDARY_A, BOUNDARY_B,
        FLOOR_A, FLOOR_B, NA
    }

    public enum TeamEnum
    {
        A, B, NA
    }

    public class Team{

        public TeamEnum teamEnum;

        public Team(TeamEnum type) {
            this.teamEnum = type;
        }
        public bool isA()
        {
            if (teamEnum.Equals(TeamEnum.A))
                return true;
            else return false;
        }

        public bool isB()
        {
            if (teamEnum.Equals(TeamEnum.B))
                return true;
            else return false;
        }

        public ObjectTypeEnum getAgent()
        {
            return isA() ? ObjectTypeEnum.AGENT_A : ObjectTypeEnum.AGENT_B;
        }

        public ObjectTypeEnum getBoundary()
        {
            return isA() ? ObjectTypeEnum.BOUNDARY_A : ObjectTypeEnum.BOUNDARY_B;
        }

        public ObjectTypeEnum getFloor()
        {
            return isA() ? ObjectTypeEnum.FLOOR_A : ObjectTypeEnum.FLOOR_B;
        }

        public TeamEnum getTeam()
        {
            return teamEnum;
        }
        public TeamEnum getOpponentTeam()
        {
            if (teamEnum == TeamEnum.NA) return TeamEnum.NA;
            return isA() ? TeamEnum.B : TeamEnum.A;
        }
    }
}
