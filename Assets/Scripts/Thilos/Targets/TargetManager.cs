using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager
{
    public static List<ITarget> targetsUpper = new List<ITarget>();
    public static List<ITarget> targetsLower = new List<ITarget>();
    public static List<ITarget> getTargets(Vector3 pos, int layer, float range)
    {
        List<ITarget> targets = new List<ITarget>();

        if(layer == 0)
        {
            for(int i = 0; i < targetsUpper.Count; i++)
            {
                Vector3 tPos = targetsUpper[i].getPosition();
                tPos.z = 0;
                if (Vector3.Distance(pos, tPos) < range)
                    targets.Add(targetsUpper[i]);
            }
        }
        else
        {
            for (int i = 0; i < targetsLower.Count; i++)
            {
                Vector3 tPos = targetsLower[i].getPosition();
                tPos.z = 0;
                if (Vector3.Distance(pos, tPos) < range)
                    targets.Add(targetsLower[i]);
            }
        }

        return targets;
    }

    public static void addUpperTarget(ITarget target)
    {
        if (targetsUpper.Contains(target))
            return;
        targetsUpper.Add(target);

    }
    public static void remUpperTarget(ITarget target)
    {
        targetsUpper.Remove(target);
    }
    public static void addLowerTarget(ITarget target)
    {
        if (targetsLower.Contains(target))
            return;
        targetsLower.Add(target);
    }
    public static void remLowerTarget(ITarget target)
    {
        targetsLower.Remove(target);
    }

    public static void moveFromLowerToUpper(ITarget target)
    {
        remLowerTarget(target);
        addUpperTarget(target);
    }
    public static void moveFromUpperToLower(ITarget target)
    {
        addLowerTarget(target);
        remUpperTarget(target);
    }
}