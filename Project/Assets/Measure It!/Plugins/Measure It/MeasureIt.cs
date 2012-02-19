/*  =====================================================================
 *  MeasureIt.cs v1.09              Copyright (c) by FluffyUnderware 2011
 *  All rights reserved                           www.fluffyunderware.com
 *
 *  http://www.fluffyunderware.com/pages/unity-plugins/measure-it.php
 *  =====================================================================
 */
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Provides counting, time measurement and logging
/// </summary>
/// <remarks>Times, Counts and values are stored in slots. You create a slot by using it's name the first time</remarks>
/// <example>
/// void aMethodToProfile()
/// {
///     MeasureIt.Begin("Calculation time");
///     // do some stuff
///     MeasureIt.End("Calculation time");
/// }
/// 
/// void CountMe()
/// {
///     MeasureIt.Count("Clicks");
/// }
/// 
/// void ShowValue()
/// {
///     MeasureIt.Set("Current Event",Event.current);
/// }
/// 
/// void ConsoleLog()
/// {
///     MeasureIt.Log("A Log entry");
/// }
/// </example>
/// <para>Version: 1.09</para>
/// <para>(c) FluffyUnderware</para>
[ExecuteInEditMode]
public class MeasureIt : MonoBehaviour
{
    #region ### Inspector Properties ###

    /// <summary>
    /// Values are stored in a ring. Once StoredValues is reached, the first values will be overwritten
    /// </summary>
    public int StoredValues = 200;
    /// <summary>
    /// The time the display will be updated
    /// </summary>
    public float UpdateTime = 0.5f;
    /// <summary>
    /// Show minimum/maximum values
    /// </summary>
    public bool ShowMinMax = true;
    /// <summary>
    /// Either Ticks, Milliseconds or seconds
    /// </summary>
    public MeasureUnit Unit = MeasureUnit.Milliseconds;
    /// <summary>
    /// The upper left point on screen where the results will be shown
    /// </summary>
    public Vector2 GUIPosition=new Vector2(20,20);
    /// <summary>
    /// Mute all output?
    /// </summary>
    public bool Mute;

    /// <summary>
    /// To mute logging for a certain group, add it's name to the list
    /// </summary>
    public List<string> MuteGroups = new List<string>();
    #endregion

    #region ### Private Fields ###

    static MeasureIt _instance;
    static MeasureIt Instance
    {
        get
        {
            if (!_instance)
                _instance = GameObject.FindObjectOfType(typeof(MeasureIt)) as MeasureIt;
            if (!_instance)
                _instance = new GameObject("Measure It").AddComponent<MeasureIt>();
            return _instance;
        }
    }
    
    /// <summary>
    /// Timers store the values you produce using MeasureIt.Begin() and MeasureIt.End()
    /// </summary>
    Dictionary<string, MeasureItTimer> Timers = new Dictionary<string, MeasureItTimer>();
    /// <summary>
    /// Counters store the values you produce using MeasureIt.Count()
    /// </summary>
    Dictionary<string, int> Counters = new Dictionary<string, int>();
    /// <summary>
    /// Values store any information. Set them using MeasureIt.Set()
    /// </summary>
    Dictionary<string, object> Values = new Dictionary<string, object>();

    System.DateTime mUpdateTime;

    #endregion

    #region ### Unity Callbacks ###

    void OnEnable()
    {
        _instance = this;
    }

    void OnDisable()
    {
        _instance = null;
    }

    void OnGUI() 
    {
        if ((System.DateTime.Now-mUpdateTime).Milliseconds > UpdateTime*1000) {
            mUpdateTime = System.DateTime.Now.AddMilliseconds(UpdateTime*1000);
            CalcStats();
        }

        if (Mute) return;

        Vector2 p = GUIPosition;


        foreach (string ktimer in Timers.Keys) {
            MeasureItTimer T = Timers[ktimer];
                if (ShowMinMax)
                    GUI.Label(new Rect(p.x, p.y, 500, 20), string.Format("{0} Avg= {1:0.00} (Min={2:0.00}, Max={3:0.00})", ktimer, T.Avg, T.Min, T.Max));
                else
                    GUI.Label(new Rect(p.x, p.y, 500, 20), string.Format("{0} Avg= {1:0.00}", ktimer, T.Avg));
                p.y += 20;
        }
        foreach (string kcounter in Counters.Keys) {
            GUI.Label(new Rect(p.x, p.y, 500, 20), string.Format("{0} # {1}", kcounter, Counters[kcounter]));
            p.y += 20;
        }

        foreach (string kval in Values.Keys) {
            GUI.Label(new Rect(p.x, p.y, 500, 20), string.Format("{0}: {1}", kval, (Values[kval] == null) ? "Null" : Values[kval].ToString()));
            p.y += 20;
        }

    }

    #endregion

    #region ### Public Methods and Properties ###

    /// <summary>
    /// Begin measurement for a given slot. If the slot doesn't exist, it will be created
    /// </summary>
    /// <param name="slotName">Name of the slot</param>
    /// <remarks>Slot names are case sensitive</remarks>
    static public void Begin(string slotName)
    {
        MeasureItTimer slot;
        if (!Instance.Timers.ContainsKey(slotName)) {
            slot = new MeasureItTimer(Instance.StoredValues);
            Instance.Timers.Add(slotName, slot);
            slot.Watch.Start();
        }
        else {
            slot = Instance.Timers[slotName];
            slot.Watch.Reset();
            slot.Watch.Start();
        }
    }
    /// <summary>
    /// Begin measurement for a given slot. If the slot doesn't exist, it will be created
    /// </summary>
    /// <param name="slotName">Name of the slot</param>
    /// <param name="group">Groupname</param>
    /// <remarks>Slot names are case sensitive</remarks>
    /// <remarks>You can mute output by adding it's group to <see cref=" MuteGroups"/></remarks>
    static public void Begin(string slotname, string group)
    {
        if (!Instance.MuteGroups.Contains(group))
            Begin(slotname);
    }

    /// <summary>
    /// Remove a given slot regardless of it's type (Timer or Counter)
    /// </summary>
    /// <param name="slotName">Name of the slot</param>
    /// <remarks>Slot names are case sensitive</remarks>
    static public void Clear(string slotName)
    {
        if (Instance.Timers.ContainsKey(slotName))
            Instance.Timers.Remove(slotName);
        if (Instance.Counters.ContainsKey(slotName))
            Instance.Counters.Remove(slotName);
        if (Instance.Values.ContainsKey(slotName))
            Instance.Values.Remove(slotName);
    }

    /// <summary>
    /// Remove all slots
    /// </summary>
    static public void Clear()
    {
        Instance.Timers.Clear();
        Instance.Counters.Clear();
        Instance.Values.Clear();
    }

    /// <summary>
    /// Increase the counter for a given slot by one. If the slot doesn't exist, it will be created.
    /// </summary>
    /// <param name="slotName">Name of the slot</param>
    /// <remarks>Slot names are case sensitive</remarks>
    /// <returns>The current count</returns>
    static public int Count(string slotName)
    {
        if (!Instance.Counters.ContainsKey(slotName))
            Instance.Counters.Add(slotName, 1);
        else
            Instance.Counters[slotName]++;
        return Instance.Counters[slotName];
    }
    /// <summary>
    /// Increase the counter for a given slot by one. If the slot doesn't exist, it will be created.
    /// </summary>
    /// <param name="slotName">Name of the slot</param>
    /// <param name="group">Groupname</param>
    /// <returns>The current count</returns>
    /// <remarks>Slot names are case sensitive</remarks>
    /// <remarks>You can omit counting by adding it's group to <see cref=" MuteGroups"/></remarks>
    static public int Count(string slotName, string group)
    {
        if (!Instance.MuteGroups.Contains(group))
            return Count(slotName);
        else 
            return 0;
    }



    /// <summary>
    /// End measurement for a given slot and store the time difference. If the slot doesn't exist, it will be ignored.
    /// </summary>
    /// <param name="slotName">Name of the slot</param>
    /// <remarks>Slot names are case sensitive</remarks>
    /// <returns>The measured time</returns>
    static public float End(string slotName)
    {
        if (!Instance.Timers.ContainsKey(slotName)) return -1;
        MeasureItTimer slot = Instance.Timers[slotName];
        if (!slot.Watch.IsRunning) return -1;
        slot.Watch.Stop();
        switch (Instance.Unit) {
            case MeasureUnit.Ticks:
                slot.Times[slot.Idx] = slot.Watch.ElapsedTicks;
                break;
            case MeasureUnit.Milliseconds:
                slot.Times[slot.Idx] = slot.Watch.ElapsedMilliseconds;
                break;
            case MeasureUnit.Seconds:
                slot.Times[slot.Idx] = slot.Watch.ElapsedMilliseconds * 0.001f;
                break;
        }
		float v=slot.Times[slot.Idx];
        if (++slot.Idx == slot.Times.Length)
            slot.Idx = 0;
        return v;
    }

    /// <summary>
    /// True if this is a high resolution timer.
    /// </summary>
    static public bool IsHighResolutionTimer
    {
        get { return System.Diagnostics.Stopwatch.IsHighResolution; }
    }

    /// <summary>
    /// Logs a message
    /// </summary>
    /// <param name="message">everything that can be parsed to a string</param>
    static public void Log(object message)
    {
        Log(message, null, LogType.Log, "");
    }
    /// <summary>
    /// Logs a message
    /// </summary>
    /// <param name="message">everything that can be parsed to a string</param>
    /// <param name="group">Group of this log entry</param>
    /// <remarks>You can mute logs by adding it's group to <see cref=" MuteGroups"/></remarks>
    static public void Log(object message, string group)
    {
        Log(message, null, LogType.Log, group);
    }
    /// <summary>
    /// Logs a message
    /// </summary>
    /// <param name="message">everything that can be parsed to a string</param>
    /// <param name="context">a Unity object</param>
    /// <param name="group">Group of this log entry</param>
    /// <remarks>You can omit logging by adding it's group to <see cref=" MuteGroups"/></remarks>
    static public void Log(object message, Object context, string group)
    {
        Log(message, context, LogType.Log, group);
    }
    /// <summary>
    /// Logs a message
    /// </summary>
    /// <param name="message">everything that can be parsed to a string</param>
    /// <param name="context">a Unity object</param>
    /// <param name="type">Type of this log entry</param>
    /// <param name="group">Group of this log entry</param>
    /// <remarks>You can omit logging by adding it's group to <see cref=" MuteGroups"/></remarks>
    static public void Log(object message, Object context, LogType type, string group)
    {
        if (!Instance.Mute && !Instance.MuteGroups.Contains(group)) {
            if (group.Length > 0)
                message = string.Format("[{0}] {1}", group, message);
            switch (type) {
                case LogType.Warning: Debug.LogWarning(message, context); break;
                case LogType.Error: Debug.LogError(message, context); break;
                default: Debug.Log(message, context); break;
            }
        }
    }

    /// <summary>
    /// Logs a warning message
    /// </summary>
    /// <param name="message">everything that can be parsed to a string</param>
    static public void LogWarning(object message)
    {
        Log(message, null, LogType.Warning, "");
    }
    /// <summary>
    /// Logs a warning message
    /// </summary>
    /// <param name="message">everything that can be parsed to a string</param>
    /// <param name="group">Group of this log entry</param>
    /// <remarks>You can omit logging by adding it's group to <see cref=" MuteGroups"/></remarks>
    static public void LogWarning(object message, string group)
    {
        Log(message, null, LogType.Warning, group);
    }
    /// <summary>
    /// Logs a warning message
    /// </summary>
    /// <param name="message">everything that can be parsed to a string</param>
    /// <param name="context">a Unity object</param>
    /// <param name="group">Group of this log entry</param>
    /// <remarks>You can omit logging by adding it's group to <see cref=" MuteGroups"/></remarks>
    static public void LogWarning(object message, Object context, string group)
    {
        Log(message, context, LogType.Warning, group);
    }

    /// <summary>
    /// Logs an error message
    /// </summary>
    /// <param name="message">everything that can be parsed to a string</param>
    static public void LogError(object message)
    {
        Log(message, null, LogType.Error, "");
    }
    /// <summary>
    /// Logs an error message
    /// </summary>
    /// <param name="message">everything that can be parsed to a string</param>
    /// <param name="group">Group of this log entry</param>
    /// <remarks>You can omit logging by adding it's group to <see cref=" MuteGroups"/></remarks>
    static public void LogError(object message, string group)
    {
        Log(message, null, LogType.Error, group);
    }
    /// <summary>
    /// Logs an error message
    /// </summary>
    /// <param name="message">everything that can be parsed to a string</param>
    /// <param name="context">a Unity object</param>
    /// <param name="group">Group of this log entry</param>
    /// <remarks>You can omit logging by adding it's group to <see cref=" MuteGroups"/></remarks>
    static public void LogError(object message, Object context, string group)
    {
        Log(message, context, LogType.Error, group);
    }

    /// <summary>
    /// Mute a group
    /// </summary>
    /// <param name="groupName">the name of the group you want to mute</param>
    static public void MuteGroup(string groupName)
    {
        if (!Instance.MuteGroups.Contains(groupName))
            Instance.MuteGroups.Add(groupName);
    }

    /// <summary>
    /// Clear mute state for all groups
    /// </summary>
    static public void UnMuteAllGroups()
    {
        Instance.MuteGroups.Clear();
    }

    /// <summary>
    /// Sets a value to display.
    /// </summary>
    /// <param name="slotName">Name of slot</param>
    /// <param name="obj">an object. It's ToString() method will be used for display</param>
    static public void Set(string slotName, object obj)
    {
        if (!Instance.Values.ContainsKey(slotName))
            Instance.Values.Add(slotName, obj);
        else
            Instance.Values[slotName] = obj;
    }
    /// <summary>
    /// Sets a value to display.
    /// </summary>
    /// <param name="slotName">Name of slot</param>
    /// <param name="obj">an object. It's ToString() method will be used for display</param>
    /// <param name="group">Groupname</param>
    /// <remarks>You can omit value setting by adding it's group to <see cref=" MuteGroups"/></remarks>
    static public void Set(string slotName, object obj, string group)
    {
        if (!Instance.MuteGroups.Contains(group))
            Set(slotName, obj);
    }

    /// <summary>
    /// The resolution of the timer in ticks in Nanoseconds
    /// </summary>
    static public long ResolutionNs
    {
        get { return (1000L * 1000L * 1000L) / System.Diagnostics.Stopwatch.Frequency; }
    }

    #endregion

    #region ### Privates ###

    void CalcStats()
    {
        foreach (string ktimer in Timers.Keys) {
            int v = 0;
            MeasureItTimer T = Timers[ktimer];
            T.Avg = 0;
            T.Min = float.MaxValue;
            T.Max = 0;
            foreach (float d in T.Times) {
                if (d >= 0) {
                    v++;
                    T.Avg += d;
                    T.Min = System.Math.Min(T.Min, d);
                    T.Max = System.Math.Max(T.Max, d);
                }
            }
            T.Avg = (v > 0) ? T.Avg / v : 0;
        }

    }

    #endregion


}
/// <summary>
/// Storing statistics used by Begin()/End()
/// </summary>
internal class MeasureItTimer
{
    internal System.Diagnostics.Stopwatch Watch;
    internal float[] Times;
    internal int Idx;
    internal float Min;
    internal float Max;
    internal float Avg;

    public MeasureItTimer(int size)
    {
        Times = new float[size];
        for (int i = 0; i < size; i++)
            Times[i] = -1;
        Watch = new System.Diagnostics.Stopwatch();
    }
}

/// <summary>
/// Determine the unit of time presentation
/// </summary>
public enum MeasureUnit
{
    /// <summary>
    /// Time values are shown as ticks
    /// </summary>
    Ticks = 0,
    /// <summary>
    /// Time values are shown as milliseconds
    /// </summary>
    Milliseconds = 1,
    /// <summary>
    /// Time values are shown as seconds
    /// </summary>
    Seconds = 2
}

