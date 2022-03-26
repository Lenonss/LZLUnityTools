using System;
using System.Collections;
using System.Collections.Generic;
using LZLUnityTool.Plugins.CommonPlugin;
using Sirenix.OdinInspector;
using UnityEngine;

public class DailyRewardManager : SingletonMono<DailyRewardManager>
{
    /// <summary>
    /// 距离获得奖励剩余时间
    /// </summary>
    public TimeSpan TimeUntilReward
    {
        get
        {
            return CalculateTimeUntil(NextRewardTime);
        }
        set
        {
            TimeUntilReward = value;
        }
    }

    [Header("是否开启不销毁状态")]
    public bool openDonDestroy = false;
    
    [Header("每日奖励时间间隔配置")]
    [Tooltip("Number of hours between 2 rewards")]
    public int rewardIntervalHours = 24;
    [Tooltip("Number of minues between 2 rewards")]
    public int rewardIntervalMinutes = 0;
    [Tooltip("Number of seconds between 2 rewards")]
    public int rewardIntervalSeconds = 0;
    

    /// <summary>
    /// 下一次获取奖励的时间
    /// </summary>
    private DateTime NextRewardTime
    {
        get
        {
            return GetNextRewardTime();
        }
    }

    /// <summary>
    /// 下次奖励时间的存储键值
    /// </summary>
    private const string NextRewardTimeKey = "NEXT_DAILY_REWARD_TIME";
    
    protected void Awake()
    {
        if (openDonDestroy)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// 计算距离获得奖励还差多长时间
    /// </summary>
    /// <param name="time">下次获得奖励的日期</param>
    /// <returns></returns>
    private TimeSpan CalculateTimeUntil(DateTime time)
    {
        return time.Subtract(DateTime.Now);
    }

    /// <summary>
    /// 保存下次获得奖励的时间日期
    /// </summary>
    /// <param name="time"></param>
    private void StoreNextRewardTime(DateTime time)
    {
        PlayerPrefs.SetString(NextRewardTimeKey, time.ToBinary().ToString());
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 获得下次获得奖励的时间日期
    /// </summary>
    /// <returns></returns>
    private DateTime GetNextRewardTime()
    {
        string storedTime = PlayerPrefs.GetString(NextRewardTimeKey, string.Empty);

        if (!string.IsNullOrEmpty(storedTime))
            return DateTime.FromBinary(Convert.ToInt64(storedTime));
        else
            return DateTime.Now;
    }

    /// <summary>
    /// 设置下次获得奖励的时间日期
    /// </summary>
    /// <param name="hours">间隔小时</param>
    /// <param name="minutes">间隔分钟</param>
    /// <param name="seconds">间隔秒钟</param>
    public void SetNextRewardTime(int hours, int minutes, int seconds)
    {
        var nextRewardTime = DateTime.Now.Add(new TimeSpan(hours, minutes, seconds));
        StoreNextRewardTime(nextRewardTime);
    }
    
    /// <summary>
    /// 设置下次奖励时间
    /// </summary>
    public void SetNextRewardTime()
    {
        SetNextRewardTime(rewardIntervalHours, rewardIntervalMinutes, rewardIntervalSeconds);
    }

    [Button("输出下次奖励时间信息")]
    public void PrintNextRewardTime()
    {
        string printStr = string.Format("距离下次奖励日期: {0:00}:{1:00}:{2:00}", TimeUntilReward.Hours, TimeUntilReward.Minutes,
            TimeUntilReward.Seconds);
        var nextRewardDate = GetNextRewardTime();
        
        string printstr2 = "下次奖励日期："+nextRewardDate.Year+"年, "+nextRewardDate.Month+"月，"
                           +nextRewardDate.Day+"号，"+nextRewardDate.Hour+"小时，"+nextRewardDate.Minute+"分，"+
                           nextRewardDate.Second+"秒";
        Debug.Log(printStr);
        Debug.Log(printstr2);
    }
    
    public string TimeRemainingTillNextReward()
    {
        TimeSpan timeToReward = DailyRewardManager.Instance.TimeUntilReward;

        if (timeToReward <= TimeSpan.Zero)
        {
            return "DAILY REWARD\nAVAILABLE!";
        }
        else
        {
            return string.Format("REWARD IN {0:00}:{1:00}:{2:00}", timeToReward.Hours, timeToReward.Minutes, timeToReward.Seconds);
        }
    }
}
