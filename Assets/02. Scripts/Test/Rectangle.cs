
// 쿼드트리에서 사용되는 사각형 영역
using UnityEngine;

public class Rectangle
{
    // 중심점 (centerX, centerY), 너비 width, 높이 height
    // 중심점 (centerX, centerZ), 너비 width, 높이 height
    public float centerX;
    //public float centerY;
    public float centerZ;
    public float width;
    public float height;

    //public Rectangle(float x, float y, float w, float h)
    public Rectangle(float x, float z, float w, float h)
    {
        centerX = x;
        //centerY = y;
        centerZ = z;
        width = w;
        height = h;
    }

    // 특정 포인트가 사각형 영역 내에 포함되는지 판단
    //public bool Contains(GameObject objPoint)
    public bool Contains(Point point)
    {
        bool contains = (point.x > centerX - width 
                         && point.x < centerX + width 
                         //&& point.y > centerY - height 
                         && point.z > centerZ - height 
                         //&& point.y < centerY + height);
                         && point.z < centerZ + height);
        return contains;
        
        /*
        Transform objPoint = obj.transform;

        bool contains = (objPoint.position.x > centerX - width
                         && objPoint.position.x < centerX + width
                         && objPoint.position.y > centerY - height
                         && objPoint.position.y < centerY + height);
        return contains;
        */
    }
}
