using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LZLUnityTool.Plugins.UIPlugin.NewerGuid
{
    [RequireComponent(typeof(EventPenetrate))]
    public class CircleShaderController : MonoBehaviour
{
    public enum  ChangeType
    {
        Smooth,EveryReduce
    }
    [Title("动画变化方式")]
    public ChangeType changeType;
    
    [ShowIf("changeType",ChangeType.EveryReduce)]
    public float unitReduce;

    [Title("自定义画布")]
    public bool customCanvas;
    [InfoBox("自定义选择的画布",InfoMessageType.None),ShowIf("customCanvas",true)]
    public Canvas targetCanvas;
    
    /// <summary>
    /// 要高亮显示的目标
    /// </summary>
    private Image Target;

    private EventPenetrate ev;
    private int currentIndex ;
    public Image[] Targets;
    /// <summary>
    /// 区域范围缓存
    /// </summary>
    private Vector3[] _corners = new Vector3[4];

    /// <summary>
    /// 镂空区域圆心
    /// </summary>
    private Vector4 _center;

    /// <summary>
    /// 镂空区域半径
    /// </summary>
    private float _radius;

    /// <summary>
    /// 遮罩材质
    /// </summary>
    private Material _material;

    /// <summary>
    /// 当前高亮区域的半径
    /// </summary>
    private float _currentRadius;

    /// <summary>
    /// 高亮区域缩放的动画时间
    /// </summary>
    private float _shrinkTime = 0.5f;

    /// <summary>
    /// 世界坐标向画布坐标转换
    /// </summary>
    /// <param name="canvas">画布</param>
    /// <param name="world">世界坐标</param>
    /// <returns>返回画布上的二维坐标</returns>
    private Vector2 WorldToCanvasPos(Canvas canvas, Vector3 world)
    {
        Vector2 position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
            world, canvas.GetComponent<Camera>(), out position);
        return position;
    }

    public void ChangeTarget()
    {
        if (currentIndex >= Targets.Length)
            currentIndex = 0;
        Target = Targets[currentIndex];
        ev.SetTargetImage(Target);
        currentIndex++;
        Canvas canvas = targetCanvas ? targetCanvas : GameObject.Find("Canvas").GetComponent<Canvas>();
        //获取高亮区域的四个顶点的世界坐标
        Target.rectTransform.GetWorldCorners(_corners);
        //计算最终高亮显示区域的半径
        
        _radius = Vector2.Distance(WorldToCanvasPos(canvas, _corners[0]),
                      WorldToCanvasPos(canvas, _corners[2])) / 2f;
        //计算高亮显示区域的圆心
        float x = _corners[0].x + ((_corners[3].x - _corners[0].x) / 2f);
        float y = _corners[0].y + ((_corners[1].y - _corners[0].y) / 2f);
        Vector3 centerWorld = new Vector3(x, y, 0);
        Vector2 center = WorldToCanvasPos(canvas, centerWorld);

        //适配参数
        float deltay = Screen.height - canvas.GetComponent<CanvasScaler>().referenceResolution.y;
        float deltax=Screen.width - canvas.GetComponent<CanvasScaler>().referenceResolution.x;
        deltax /= 4;
        deltay /= 4;
        //设置遮罩材料中的圆心变量
        Vector4 centerMat = new Vector4(center.x, center.y+deltay, 0, 0);
       _material = GetComponent<Image>().material;
        _material.SetVector("_Center", centerMat);
        //计算当前高亮显示区域的半径
        RectTransform canRectTransform = canvas.transform as RectTransform;
        if (canRectTransform != null)
        {
            //获取画布区域的四个顶点
            canRectTransform.GetWorldCorners(_corners);
            //将画布顶点距离高亮区域中心最远的距离作为当前高亮区域半径的初始值
            foreach (Vector3 corner in _corners)
            {
                _currentRadius = Mathf.Max(Vector3.Distance(WorldToCanvasPos(canvas, corner), center),
                    _currentRadius);
            }
        }
        _material.SetFloat("_Slider", _currentRadius);
    }
    private void Awake()
    {
        currentIndex = 0;
        ev = transform.GetComponent<EventPenetrate>();
        //获取画布
        ChangeTarget();
    }

    /// <summary>
    /// 收缩速度
    /// </summary>
    private float _shrinkVelocity = 0f;

    private void Update()
    {
        switch (changeType)
        {
            case  ChangeType.Smooth:
                //从当前半径到目标半径差值显示收缩动画
                float value = Mathf.SmoothDamp(_currentRadius, _radius, ref _shrinkVelocity, _shrinkTime);
                if (!Mathf.Approximately(value, _currentRadius))
                {
                    _currentRadius = value;
                    _material.SetFloat("_Slider", _currentRadius);
                }
                break;
            case ChangeType.EveryReduce:
                if (_currentRadius > _radius)
                {
                    _currentRadius -= unitReduce;
                }
                _material.SetFloat("_Slider", _currentRadius);
                break;
        }
    }
}
}
