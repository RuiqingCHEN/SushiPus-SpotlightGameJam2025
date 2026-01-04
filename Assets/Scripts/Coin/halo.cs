using UnityEngine;
// 创建一个简单的材质脚本
public class ParticleHighlight : MonoBehaviour
{
    private ParticleSystem highlightParticles;

    void Start()
    {
        // 创建简单的粒子系统
        GameObject particleObj = new GameObject("HighlightParticles");
        particleObj.transform.SetParent(transform);
        particleObj.transform.localPosition = Vector3.zero;

        highlightParticles = particleObj.AddComponent<ParticleSystem>();
        var main = highlightParticles.main;
        main.loop = true;
        main.startLifetime = 1f;
        main.startSpeed = 0f;
        main.startSize = 0.5f;
        main.startColor = new Color(1, 1, 0, 0.3f);

        var emission = highlightParticles.emission;
        emission.rateOverTime = 10f;

        var shape = highlightParticles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 1.2f;

        // 初始禁用
        highlightParticles.Stop();
    }

    public void EnableHighlight()
    {
        highlightParticles.Play();
    }

    public void DisableHighlight()
    {
        highlightParticles.Stop();
    }
}