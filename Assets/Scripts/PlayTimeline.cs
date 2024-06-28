// Source: https://learn.unity.com/tutorial/starting-timeline-through-a-c-script-2019-3#5ff8d183edbc2a0020996601

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Playables;

// public class TimelinePlayer : MonoBehaviour
// {
//     private PlayableDirector director;
//     public GameObject controlPanel;

//     private void Awake()
//     {
//         director = GetComponent<PlayableDirector>();
//         director.played += Director_Played;
//         director.stopped += Director_Stopped;
//     }

//     private void Director_Stopped(PlayableDirector obj)
//     {
//         controlPanel.SetActive(true);
//     }

//     private void Director_Played(PlayableDirector obj)
//     {
//         controlPanel.SetActive(false);
//     }

//     public void StartTimeline()
//     {
//         director.Play();
//     }
// } 
