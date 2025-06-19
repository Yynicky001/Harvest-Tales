using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class hoe : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.CompareTag("soil")&&EquipSystem.Instance.IsPlayerHoldinghoe())
                {
                    Transform hitTransform = hit.collider.transform;
                    ClearChildren(hitTransform);
                }
            }
        }
    }
     public void ClearChildren(Transform hitTransform)
    {
        // 遍历当前物体的所有子物体
        for (int i = hitTransform.childCount - 1; i >= 0; i--)
        {
            Transform child = hitTransform.GetChild(i);
            // 运行时使用 Destroy 销毁子物体（延迟一帧处理）
            Destroy(child.gameObject);
        }
    }
}
