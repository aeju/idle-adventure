
// 쿼드트리에서 사용되는 사각형 영역
public class Rectangle
{
    // 중심점 (centerX, centerZ), 너비 width, 높이 length
    public float centerX;
    public float centerZ;
    public float width;
    public float length;
    
    public Rectangle(float x, float z, float w, float l)
    {
        centerX = x;
        centerZ = z;
        width = w;
        length = l;
    }

    // 특정 포인트가 사각형 영역 내에 포함되는지 판단 (Quadtree - Insert에서 사용)
    public bool Contains(Point point)
    {
        bool contains = (point.x > centerX - width 
                         && point.x < centerX + width 
                         && point.z > centerZ - length 
                         && point.z < centerZ + length);
        return contains;
    }
    
    // 영역이 다른 영역과 겹치는지 판단 (Quadtree - Query에서 사용) 
    public bool Intersects(Rectangle range)
    {
        return !(centerX + width < range.centerX - range.width ||
                 centerX - width > range.centerX + range.width ||
                 centerZ + length < range.centerZ - range.length ||
                 centerZ - length > range.centerZ + range.length);
    }
}
