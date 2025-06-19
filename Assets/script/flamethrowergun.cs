using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class flamethrowergun : MonoBehaviour
{
    public float range = 10f;
    public float radius = 1f;
    public float baseDamage = 10;
    public float fireTime = 2f;
    public float addFireDuration = 2f;
    public float addFireDamage = 2f;
    public float Firetime = 1f;
    public float Freshtime = 0f;
    public VisualEffect particle;

    private Dictionary<GameObject, List<float>> affectedEnemies = new Dictionary<GameObject, List<float>>();
    private Dictionary<GameObject, bool> processedParents = new Dictionary<GameObject, bool>();
    void Update()
    {
        if (Input.GetMouseButton(0))
        {

            if (Time.time % fireTime < 0.01f)
            {
                ShootFlame();
                particle.GetComponent<VisualEffect>().Play();
            }
        }
        else
        {
            particle.GetComponent<VisualEffect>().Stop();
        }

        // 清理过期的受击记录
        List<GameObject> keysToRemove = new List<GameObject>();
        foreach (var enemy in affectedEnemies)
        {
            var go = enemy.Key;
            var hitTimes = enemy.Value;
            for (int i = 0; i < hitTimes.Count; i++)
            {
                if (Time.time - hitTimes[i] > addFireDuration)
                {
                    hitTimes.RemoveAt(i);
                    i--;
                }
            }
            if (hitTimes.Count == 0)
            {
                keysToRemove.Add(go);
            }

            // 根据有效攻击次数计算并施加额外伤害
            if (Time.time - Freshtime > Firetime)
            {
                if (affectedEnemies.ContainsKey(go))
                {
                    int validHits = affectedEnemies[go].Count;
                    var enemyComponent = go.GetComponent<Animal>();
                    if (enemyComponent != null)
                    {
                        float extraDamage = (validHits - 1) * addFireDamage;
                        enemyComponent.TakeDamage(extraDamage);
                    }
                }
                Freshtime = Time.time;
            }
        }

        foreach (var key in keysToRemove)
        {
            affectedEnemies.Remove(key);
        }
    }

    void ShootFlame()
    {
        Vector3 flameEndPosition = transform.position + transform.forward * range;

        // 检测火焰喷射末端周围一定半径内的所有碰撞体
        Collider[] colliders = Physics.OverlapSphere(flameEndPosition, radius);
        Transform parentTransform = transform.parent;

        foreach (var collider in colliders)
        {
            GameObject targetObject = collider.gameObject;
            if (targetObject.transform.parent != null)
            {
                targetObject = targetObject.transform.parent.gameObject;
            }
            if ((targetObject.transform == parentTransform))
            {
                continue; // 跳过本体或本体的子对象
            }
            if (!processedParents.ContainsKey(targetObject))
            {
                if (targetObject.gameObject.CompareTag("enemy"))
                {
                    var enemyComponent = targetObject.gameObject.GetComponent<Animal>();
                    if (enemyComponent != null)
                    {
                        enemyComponent.TakeDamage(baseDamage);
                        if (!affectedEnemies.ContainsKey(targetObject.gameObject))
                        {
                            affectedEnemies[targetObject.gameObject] = new List<float> { Time.time };
                            affectedEnemies[targetObject.gameObject].Add(Time.time);
                        }
                        else
                        {
                            affectedEnemies[targetObject.gameObject].Add(Time.time);
                        }
                    }
                }
                processedParents[targetObject] = true;
            }

        }
        processedParents.Clear();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * range);
    }
}
