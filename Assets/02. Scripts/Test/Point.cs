
// 2차원 공간의 점
public class Point
{
    public float x;
    public float z;
    public string monsterName; // 몬스터 고유 식별자
    
    public Point(float x, float z, string monsterName)
    {
        this.x = x;
        this.z = z;
        this.monsterName = monsterName;
    }
}
