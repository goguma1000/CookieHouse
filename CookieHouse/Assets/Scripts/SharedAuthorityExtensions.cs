using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Fusion;

public static class SharedAuthorityExtensions
{
    public static async Task<bool> WaitForStateAuthority(this NetworkObject o, float maxWaitTime = 8)
    {
        float waitStartTime = Time.time;
        o.RequestStateAuthority();
        while(!o.HasStateAuthority && (Time.time - waitStartTime) < maxWaitTime)
        {
            await Task.Delay(1);
        }
        return o.HasStateAuthority;
    }
}
