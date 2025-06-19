using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManage : MonoBehaviour
{
    public static SoundManage Instance { get; set; }

    [SerializeField]
    public AudioSource dropItemSound;

    [SerializeField]
    public AudioSource startingZoneBGMusic; // 背景音乐

    private void Awake()
    {
        // 仅在游戏运行时执行单例逻辑
        if (Application.isPlaying)
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject); // 防止在场景切换时被销毁

            // 初始化背景音乐设置
            if (startingZoneBGMusic != null)
            {
                startingZoneBGMusic.loop = true; // 设置为循环播放
            }
        }
    }

    public void PlayDropSound()
    {
        if (dropItemSound != null)
            dropItemSound.Play();
    }

    // 播放背景音乐的方法
    public void PlayBackgroundMusic()
    {
        if (startingZoneBGMusic != null && !startingZoneBGMusic.isPlaying)
        {
            startingZoneBGMusic.Play();
        }
    }

    // 停止背景音乐的方法
    public void StopBackgroundMusic()
    {
        if (startingZoneBGMusic != null && startingZoneBGMusic.isPlaying)
        {
            startingZoneBGMusic.Stop();
        }
    }
}
