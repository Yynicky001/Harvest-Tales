using UnityEngine;

public class TerrHideTerrainMaterial : MonoBehaviour
{
    void Start()
    {
        // 获取地形组件
        Terrain terrain = GetComponent<Terrain>();
        if (terrain != null)
        {
            // 获取地形数据
            TerrainData terrainData = terrain.terrainData;
            if (terrainData != null)
            {
                // 创建一个透明材质
                Material transparentMaterial = new Material(Shader.Find("Standard"));
                transparentMaterial.color = new Color(0, 0, 0, 0);

                // 应用透明材质到地形
                terrain.materialTemplate = transparentMaterial;
            }
        }
    }
}
