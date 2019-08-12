using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class MMOTileSelector : Agent
{
    [SerializeField] Transform pointer  = null;
    [SerializeField] MMOMasterAgent masterAgent = null;
    LevelGenerator level = null;

    Building currentTarget = null;
    


    public override void AgentReset()
    {

    }

    public override void CollectObservations()
    {
        //Camera in use
        if(currentTarget != null)
            AddVectorObs(currentTarget.Observe());// +3 for each Resource one for Target
        else
            AddVectorObs(new Resource().Observe());// +3 for each Resource one for Target

        AddVectorObs(masterAgent.Observe());// +3 for each Resource one in Stock




        
    }
    private void SetMask()
    {

    }

    public override void InitializeAgent()
    {
        level = MMOAcademy.instance.LevelGenerator;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        int x = (int)(Remap01(vectorAction[0]) * level.WorldSetting.SizeX);
        int y = (int)(Remap01(vectorAction[1]) * level.WorldSetting.SizeY);

        ClampToLast(ref x, level.WorldSetting.SizeX);
        ClampToLast(ref y, level.WorldSetting.SizeY);

        pointer.transform.position = level.GetTile(x, y).transform.position;
        masterAgent.ResourceStock += level.GetTile(x, y).GetComponent<WorldTile>().GetResource();
    }

    float Remap01(float inFloat) // from -1 ... 1 to 0 .. 1
    {
        return (inFloat + 1) / 2;
    }

    void ClampToLast(ref int value, int last)
    {
        if (value == last)
            value--;
    }
}
