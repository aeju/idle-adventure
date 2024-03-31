
// 쿼드트리에서 사용되는 사각형 영역
public class Rectangle
{
    // 중심점 (centerX, centerZ), 너비 width, 높이 height
    public float centerX;
    public float centerZ;
    public float width;
    public float height;
    
    public Rectangle(float x, float z, float w, float h)
    {
        centerX = x;
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
                         && point.z > centerZ - height 
                         && point.z < centerZ + height);
        return contains;
    }
}
