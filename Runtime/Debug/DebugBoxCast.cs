using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugBoxCast
{
    public static void Draw(Vector3 center, Vector3 halfExtents, Vector3 direction, Color col){
        Quaternion lookRot = Quaternion.LookRotation( direction.normalized, Vector3.up);
        var boxRight = lookRot * (Vector3.right*halfExtents.x); 
        var boxLeft = lookRot * (-Vector3.right*halfExtents.x); 
        var boxTop = lookRot * (Vector3.up*halfExtents.y); 
        var boxBottom = lookRot * (-Vector3.up*halfExtents.y);

        var topRight = boxRight + boxTop + center; 
        var topLeft = boxLeft + boxTop + center; 
        var bottomLeft = boxLeft + boxBottom + center; 
        var bottomRight = boxRight + boxBottom + center; 

        Debug.DrawRay(topLeft, topRight-topLeft, col);
        Debug.DrawRay(topLeft, bottomLeft-topLeft, col);
        Debug.DrawRay(bottomRight, bottomLeft-bottomRight, col);
        Debug.DrawRay(bottomRight, topRight-bottomRight, col);
        
        Debug.DrawRay(topLeft + direction, topRight-topLeft, col);
        Debug.DrawRay(topLeft + direction, bottomLeft-topLeft, col);
        Debug.DrawRay(bottomRight + direction, bottomLeft-bottomRight, col);
        Debug.DrawRay(bottomRight + direction, topRight-bottomRight, col);
        
        Debug.DrawRay(topLeft, direction, col);
        Debug.DrawRay(topRight, direction, col);
        Debug.DrawRay(bottomRight, direction, col);
        Debug.DrawRay(bottomLeft, direction, col);
    }
}
