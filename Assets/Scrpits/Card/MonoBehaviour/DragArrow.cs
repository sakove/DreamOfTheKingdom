using System;
using UnityEngine;
using UnityEngine.UIElements;

public class DragArrow : MonoBehaviour
{
    private LineRenderer line;
    private Vector3 mousePos;
    
    public int pointsCount;
    public float arcModifier;//描绘贝塞尔曲线形状

    private void Awake()
    {
        line=GetComponent<LineRenderer>();
    }

    private void Update()
    {
        mousePos=Camera.main.ScreenToWorldPoint(new(Input.mousePosition.x,Input.mousePosition.y,10f));

        SetArrowPosition();
    }

    private void SetArrowPosition()
    {
        Vector3 cardPos = transform.position;
        Vector3 direction=mousePos - cardPos;
        Vector3 normalizedDirection = direction.normalized;//归一化方向
        
        //计算垂直于卡牌到鼠标方向的向量
        Vector3 perpendicular=new(-normalizedDirection.y,normalizedDirection.x,normalizedDirection.z);
        
        //设置控制点的偏移值
        Vector3 offset=perpendicular*arcModifier;//改变曲线的形状
        
        Vector3 controlPoint=(cardPos+mousePos)/2+offset;
        
        line.positionCount=pointsCount;

        for (int i = 0; i < pointsCount; i++)
        {
            float t=i/(float)(pointsCount-1);
            Vector3 point=CalculateQuadrationBezierPoint(t,cardPos,controlPoint,mousePos);
            line.SetPosition(i,point);
        }
    }

    //计算二次贝塞尔曲线点
    private Vector3 CalculateQuadrationBezierPoint(float t,Vector3 p0,Vector3 p1,Vector3 p2)
    {
        float u=1f-t;
        float tt = t * t;
        float uu = u * u;
        
        Vector3 p=uu*p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        
        return p;
    }
}
