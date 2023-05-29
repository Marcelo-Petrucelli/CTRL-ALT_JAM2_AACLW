﻿using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using System;
using System.Collections;

public class AudioController:MonoBehaviour
{
    [SerializeField, BoxGroup("References")] Transform dubParent;
    
    internal static AudioController Instance = null;
    [ShowNonSerializedField] internal bool isMusicMuted = false;
    [ShowNonSerializedField] internal bool isSFXMuted = false;
    [ShowNonSerializedField] internal bool isDubMuted = false;

    private List<FMODUnity.StudioEventEmitter> musicsEmitter;
    private List<FMODUnity.StudioEventEmitter> dubs;

    void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
        this.musicsEmitter = new List<FMODUnity.StudioEventEmitter>(this.GetComponents<FMODUnity.StudioEventEmitter>());
        this.dubs = new List<FMODUnity.StudioEventEmitter>(this.dubParent.GetComponentsInChildren<FMODUnity.StudioEventEmitter>());
    }

    public void PlayDub(int index, float wait = 0f) {
        if(wait == 0) {
            this.dubs[index].Play();
        } else {
            this.ExecuteAfter(() => {
                this.dubs[index].Play();
            }, wait);
        }
    }

    public void EnterBattle() {
        this.musicsEmitter[1].SetParameter("Finish battle", 0f);
        this.musicsEmitter[1].SetParameter("Battle", 1f);
    }

    public void ExitBattle() {
        this.musicsEmitter[1].SetParameter("Battle", 0f);
        this.musicsEmitter[1].SetParameter("Finish battle", 1f);
    }

    public void GameStartedMusic() {
        this.musicsEmitter[0].SetParameter("FINISH INTRO", 1.2f);
        this.ExecuteAfter(() => {
            this.musicsEmitter[1].Play();
            this.musicsEmitter[1].SetParameter("Transition", 1.1f); //1~1.5
        }, 3.3f);
    }

    public void FirstAdvanceMusic() {
        this.musicsEmitter[1].SetParameter("Transition", 2.1f); //2~2.5
    }

    public void SecondAdvanceMusic() {
        this.musicsEmitter[1].SetParameter("Transition", 3.1f); //3~3.5
    }

    public void FinalizeNormalAndStartWOWMusic() {
        this.musicsEmitter[1].SetParameter("Transition", 4.1f); //4~4.5
    }

    public void StartEUFOMusic() {
        this.musicsEmitter[1].SetParameter("Transition", 5.1f); //5~5.5
    }

    public void FoundIslandMusicEnding() {
        this.musicsEmitter[1].SetParameter("Transition", 6.1f); //5~5.5
    }

    public void ReturnedToMenuFromWinningCredits() {
        this.musicsEmitter[1].SetParameter("Transition", 7.1f); //7~7.1
    }

    private void ExecuteAfter(Action stuff, float waitingTime) {
        StartCoroutine(this.ExecuteAfterCR(stuff, waitingTime));
    }

    private IEnumerator ExecuteAfterCR(Action stuff, float waitingTime) {
        yield return new WaitForSeconds(waitingTime);
        stuff?.Invoke();
    }
}
