using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class isoConv
{
    
    public static Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

    public static Vector3 ToIso(this Vector3 input)
    {
        return isoMatrix.MultiplyPoint3x4(input);
    }

}
