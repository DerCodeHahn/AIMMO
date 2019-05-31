using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System;

public class MMOAgent : Agent
{
    public enum MovementAction
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }
    MMOAcademy academy;
    float foodLevel;
    float waterLevel;

    [SerializeField] bool MaskActions = true;
    [SerializeField] float UseFoodEachStep = 0.1f;
    [SerializeField] float UseWaterEachStep = 0.1f;

    WorldTile currentWorldTile;

    public WorldTile CurrentWorldTile { get => currentWorldTile; }

    public float FoodLevel { get => foodLevel; set => foodLevel = value; }
    public float WaterLevel { get => waterLevel; set => waterLevel = value; }

    public override void AgentReset()
    {
        foodLevel = 1;
        waterLevel = 1;
        transform.position = Vector3.zero;
    }
    public override void InitializeAgent()
    {

    }

    private void Start()
    {
        academy = MMOAcademy.instance;
    }

    public override void CollectObservations()
    {
        FoodLevel -= UseFoodEachStep;
        WaterLevel -= UseWaterEachStep;

        AddVectorObs(foodLevel); // Vec ops + 1
        AddVectorObs(waterLevel); // Vec ops + 1
        GameObject currentTileGameObject = academy.LevelGenerator.GetTileFromWorldPos(transform.position);
        currentWorldTile = currentTileGameObject.GetComponent<WorldTile>();
        currentWorldTile.ActionOnMMOAgent(this);

        if (MaskActions)
        {
            SetMask();
        }
    }

    private void FixedUpdate()
    {

    }

    /// <summary>
    /// Applies the mask for the agents action to disallow unnecessary actions.
    /// https://github.com/Unity-Technologies/ml-agents/issues/1482
    /// </summary>
    private void SetMask()
    {
        // Prevents the agent from picking an action that would make it collide with a wall
        if (currentWorldTile.x <= 0)
        {
            SetActionMask(0, (int)MovementAction.Left);
        }

        if (currentWorldTile.x >= academy.LevelGenerator.WorldSetting.SizeX - 1)
        {
            SetActionMask(0, (int)MovementAction.Right);
        }

        if (currentWorldTile.y <= 0)
        {
            SetActionMask(0, (int)MovementAction.Down);
        }

        if (currentWorldTile.y >= academy.LevelGenerator.WorldSetting.SizeY - 1)
        {
            SetActionMask(0, (int)MovementAction.Up);
        }

    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (foodLevel <= 0 || waterLevel <= 0)
        {
            string deathCause = foodLevel <= 0 ? "FoodDeath" : "WaterDeath";
            string step = MMOAcademy.instance.GetStepCount() + " ";
            Debug.Log(step + deathCause);
            Die();
            return;
        }

        AddReward(0.001f);

        int movement = Mathf.FloorToInt(vectorAction[0]);

        switch ((MovementAction)movement)
        {
            case MovementAction.None:
                // do nothing
                break;
            case MovementAction.Right:
                Move(Vector3.right);
                break;
            case MovementAction.Left:
                Move(Vector3.left);
                break;
            case MovementAction.Up:
                Move(Vector3.up);
                break;
            case MovementAction.Down:
                Move(Vector3.down);
                break;
            default:
                throw new ArgumentException("Invalid action value");
        }
    }

    public void Die()
    {
        Done();
        academy.Done();
        SetReward(-1f);
    }

    private void Move(Vector3 direction)
    {
        transform.position += direction;
    }

}
