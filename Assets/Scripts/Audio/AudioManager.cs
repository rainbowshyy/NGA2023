using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public enum audioEvent { CubertIntensity, EnemyIntensity, EnemyIntensitySet, SirKelIntensity, TreasureIntensity, CubertDie, SirKelDie, TreasureDie, Laser, Energy, Damage, Health}
public enum audioState { Planning ,Combat, Victory, Loss }
public enum beatEvent { Laser, Energy, Damage, Health}

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Dictionary<musicLayer, Dictionary<audioState, Dictionary<int, Dictionary<int, MusicElement>>>> musicMap;

    private Dictionary<beatEvent, int> beatEvents;
    private Dictionary<musicLayer, int> intensities;
    private int bpm;

    public static System.Action<audioEvent, int> onAudioEvent;
    public static System.Action onBeat;

    private double timePerLoop;
    private double nextLoop;
    private double timePerBeat;
    private List<double> nextBeat;
    private audioState state;
    private bool combatMusicStarted = false;
    private bool planningMusicStarted;
    private int enemyIntensity = 0;
    private int beatNumber;
    private int setBPM = 0;

    [SerializeField] private int[] enemyMusicIntensityCutoffs;

    private bool[] agentAlive;


    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        musicMap = new Dictionary<musicLayer, Dictionary<audioState, Dictionary<int, Dictionary<int, MusicElement>>>>();
        foreach (MusicElement element in Resources.LoadAll<MusicElement>("MusicElements/"))
        {
            for (int i = 0; i < 2; i++)
            {
                element.audioSource[i] = gameObject.AddComponent<AudioSource>();
                element.audioSource[i].clip = element.clip;
                element.audioSource[i].volume = 0;
                element.audioSource[i].pitch = element.pitch;
            }
            if (!musicMap.ContainsKey(element.layer))
            {
                musicMap.Add(element.layer, new Dictionary<audioState, Dictionary<int, Dictionary<int, MusicElement>>>());
                musicMap[element.layer].Add(element.state, new Dictionary<int, Dictionary<int, MusicElement>>());
                musicMap[element.layer][element.state].Add(element.bpm, new Dictionary<int, MusicElement>());
                musicMap[element.layer][element.state][element.bpm].Add(element.intensity, element);
            }
            else
            {
                if (!musicMap[element.layer].ContainsKey(element.state))
                {
                    musicMap[element.layer].Add(element.state, new Dictionary<int, Dictionary<int, MusicElement>>());
                    musicMap[element.layer][element.state].Add(element.bpm, new Dictionary<int, MusicElement>());
                    musicMap[element.layer][element.state][element.bpm].Add(element.intensity, element);
                }
                else
                {
                    if (!musicMap[element.layer][element.state].ContainsKey(element.bpm))
                    {
                        musicMap[element.layer][element.state].Add(element.bpm, new Dictionary<int, MusicElement>());
                        musicMap[element.layer][element.state][element.bpm].Add(element.intensity, element);
                    }
                    else
                    {
                        musicMap[element.layer][element.state][element.bpm].Add(element.intensity, element);
                    }
                }
            }
        }

        intensities = new Dictionary<musicLayer, int>();
        foreach (musicLayer m in System.Enum.GetValues(typeof(musicLayer)))
        {
            intensities.Add(m, 0);
        }

        beatEvents = new Dictionary<beatEvent, int>();
        foreach (beatEvent b in System.Enum.GetValues(typeof(beatEvent)))
        {
            beatEvents.Add(b, -1);
        }

        nextBeat = new List<double>();
        SetBPM(100);
        agentAlive = new bool[3] { true, true, true };

        intensities[musicLayer.Enemy] = 1;
        musicMap[musicLayer.Enemy][audioState.Planning][bpm][1].audioSource[0].volume = 1;
        musicMap[musicLayer.Enemy][audioState.Planning][bpm][1].audioSource[1].volume = 1;
    }

    private void OnEnable()
    {
        onAudioEvent += AudioEvent;
        CodeBlockManager.onStartProgram += StartCombat;
        GameManager.onRoundEnd += EndCombat;
    }

    private void OnDisable()
    {
        onAudioEvent -= AudioEvent;
        CodeBlockManager.onStartProgram -= StartCombat;
        GameManager.onRoundEnd -= EndCombat;
    }

    private void Start()
    {
        planningMusicStarted = true;
        SetNextLoopTimeToBeats(4);
        PlayStateScheduled(audioState.Planning, nextLoop);
    }

    private void Update()
    {
        if (AudioSettings.dspTime > nextBeat[0])
        {
            ResetNextBeatTime();
            if (setBPM != 0)
            {
                SetBPMRuntime(setBPM);
            }
            switch (state)
            {
                case audioState.Combat:
                    onBeat?.Invoke();
                    DoBeatEvents();
                    if (!combatMusicStarted)
                    {
                        Stop(musicMap[musicLayer.Cubert][audioState.Planning][bpm][0]);
                        Stop(musicMap[musicLayer.SirKel][audioState.Planning][bpm][0]);
                        Stop(musicMap[musicLayer.Treasure][audioState.Planning][bpm][0]);
                        StopEnemy(audioState.Planning);
                        PlayState(audioState.Combat);
                        SetNextLoopTime();
                        combatMusicStarted = true;
                        PlayStateScheduled(audioState.Combat, nextLoop);
                        EnemyIntensityPlanningToCombat();
                    }
                    else if (AudioSettings.dspTime > nextLoop)
                    {
                        SetNextLoopTime();
                        PlayStateScheduled(audioState.Combat, nextLoop);
                    }
                    break;

                case audioState.Victory:
                    Stop(musicMap[musicLayer.SirKel][audioState.Combat][bpm][0]);
                    Stop(musicMap[musicLayer.Cubert][audioState.Combat][bpm][0]);
                    Stop(musicMap[musicLayer.Treasure][audioState.Combat][bpm][0]);
                    Stop(musicMap[musicLayer.Laser][audioState.Combat][bpm][0]);
                    Stop(musicMap[musicLayer.Energy1][audioState.Combat][bpm][0]);
                    Stop(musicMap[musicLayer.Energy2][audioState.Combat][bpm][0]);
                    Stop(musicMap[musicLayer.Attack1][audioState.Combat][bpm][0]);
                    Stop(musicMap[musicLayer.Attack2][audioState.Combat][bpm][0]);
                    Stop(musicMap[musicLayer.Health1][audioState.Combat][bpm][0]);
                    Stop(musicMap[musicLayer.Health2][audioState.Combat][bpm][0]);
                    StopEnemy(audioState.Combat);
                    SetNextLoopTimeToBeats(4);
                    SetState(audioState.Planning);
                    break;
                case audioState.Loss:
                    Stop(musicMap[musicLayer.Cubert][audioState.Combat][bpm][0]);
                    Stop(musicMap[musicLayer.SirKel][audioState.Combat][bpm][0]);
                    Stop(musicMap[musicLayer.Treasure][audioState.Combat][bpm][0]);
                    Stop(musicMap[musicLayer.Laser][audioState.Combat][bpm][0]);
                    Stop(musicMap[musicLayer.Energy1][audioState.Combat][bpm][0]);
                    Stop(musicMap[musicLayer.Energy2][audioState.Combat][bpm][0]);
                    Stop(musicMap[musicLayer.Attack1][audioState.Combat][bpm][0]);
                    Stop(musicMap[musicLayer.Attack2][audioState.Combat][bpm][0]);
                    Stop(musicMap[musicLayer.Health1][audioState.Combat][bpm][0]);
                    Stop(musicMap[musicLayer.Health2][audioState.Combat][bpm][0]);
                    StopEnemy(audioState.Combat);
                    SetNextLoopTimeToBeats(2);
                    SetState(audioState.Planning);
                    break;
                case audioState.Planning:
                    if (AudioSettings.dspTime > nextLoop)
                    {
                        if (!planningMusicStarted)
                        {
                            agentAlive[0] = true;
                            agentAlive[1] = true;
                            agentAlive[2] = true;
                            PlayState(audioState.Planning);
                            planningMusicStarted = true;
                            UpdateEnemyIntensity(true);
                        }
                        SetNextLoopTime();
                        PlayStateScheduled(audioState.Planning, nextLoop);
                    }
                    break;
            }
        }
    }

    private void Stop(MusicElement music)
    {
        music.audioSource[0].Stop();
        music.audioSource[1].Stop();
    }

    private void StopEnemy(audioState state)
    {
        for (int i = 1; i < 5; i++)
        {
            Stop(musicMap[musicLayer.Enemy][state][bpm][i]);
        }
    }

    private void Play(MusicElement music)
    {
        music.toggle = 1 - music.toggle;
        music.audioSource[music.toggle].time = 0f;
        music.audioSource[music.toggle].Play();
    }

    private void PlayScheduled(MusicElement music, double time)
    {
        music.toggle = 1 - music.toggle;
        music.audioSource[music.toggle].time = 0f;
        music.audioSource[music.toggle].PlayScheduled(time);
    }

    private void PlayState(audioState state)
    {
        Play(musicMap[musicLayer.Cubert][state][bpm][0]);
        Play(musicMap[musicLayer.SirKel][state][bpm][0]);
        Play(musicMap[musicLayer.Treasure][state][bpm][0]);
        if (state == audioState.Planning || state == audioState.Combat)
        {
            for (int i = 1; i < 5; i++)
            {
                Play(musicMap[musicLayer.Enemy][state][bpm][i]);
            }
            if (state == audioState.Combat)
            {
                Play(musicMap[musicLayer.Laser][state][bpm][0]);
                Play(musicMap[musicLayer.Energy1][state][bpm][0]);
                Play(musicMap[musicLayer.Energy2][state][bpm][0]);
                Play(musicMap[musicLayer.Attack1][state][bpm][0]);
                Play(musicMap[musicLayer.Attack2][state][bpm][0]);
                Play(musicMap[musicLayer.Health1][state][bpm][0]);
                Play(musicMap[musicLayer.Health2][state][bpm][0]);
            }
        }
    }

    private void PlayStateScheduled(audioState state, double time)
    {
        PlayScheduled(musicMap[musicLayer.Cubert][state][bpm][0], time);
        PlayScheduled(musicMap[musicLayer.SirKel][state][bpm][0], time);
        PlayScheduled(musicMap[musicLayer.Treasure][state][bpm][0], time);
        if (state == audioState.Planning || state == audioState.Combat)
        {
            for (int i = 1; i < 5; i++)
            {
                PlayScheduled(musicMap[musicLayer.Enemy][state][bpm][i], time);
            }
            if (state == audioState.Combat)
            {
                PlayScheduled(musicMap[musicLayer.Laser][state][bpm][0], time);
                PlayScheduled(musicMap[musicLayer.Energy1][state][bpm][0], time);
                PlayScheduled(musicMap[musicLayer.Energy2][state][bpm][0], time);
                PlayScheduled(musicMap[musicLayer.Attack1][state][bpm][0], time);
                PlayScheduled(musicMap[musicLayer.Attack2][state][bpm][0], time);
                PlayScheduled(musicMap[musicLayer.Health1][state][bpm][0], time);
                PlayScheduled(musicMap[musicLayer.Health2][state][bpm][0], time);
            }
        }
    }

    private void SetStateTime(audioState state, float time)
    {
        musicMap[musicLayer.Cubert][state][bpm][0].audioSource[musicMap[musicLayer.Cubert][state][bpm][0].toggle].time = time;
        musicMap[musicLayer.SirKel][state][bpm][0].audioSource[musicMap[musicLayer.SirKel][state][bpm][0].toggle].time = time;
        musicMap[musicLayer.Treasure][state][bpm][0].audioSource[musicMap[musicLayer.Treasure][state][bpm][0].toggle].time = time;
        for (int i = 1; i < 5; i++)
        {
            musicMap[musicLayer.Enemy][state][bpm][i].audioSource[musicMap[musicLayer.Enemy][state][bpm][i].toggle].time = time;
        }
        if (state == audioState.Combat)
        {
            musicMap[musicLayer.Laser][state][bpm][0].audioSource[musicMap[musicLayer.Laser][state][bpm][0].toggle].time = time;
            musicMap[musicLayer.Energy1][state][bpm][0].audioSource[musicMap[musicLayer.Energy1][state][bpm][0].toggle].time = time;
            musicMap[musicLayer.Energy2][state][bpm][0].audioSource[musicMap[musicLayer.Energy2][state][bpm][0].toggle].time = time;
            musicMap[musicLayer.Attack1][state][bpm][0].audioSource[musicMap[musicLayer.Attack1][state][bpm][0].toggle].time = time;
            musicMap[musicLayer.Attack2][state][bpm][0].audioSource[musicMap[musicLayer.Attack2][state][bpm][0].toggle].time = time;
            musicMap[musicLayer.Health1][state][bpm][0].audioSource[musicMap[musicLayer.Health1][state][bpm][0].toggle].time = time;
            musicMap[musicLayer.Health2][state][bpm][0].audioSource[musicMap[musicLayer.Health2][state][bpm][0].toggle].time = time;
        }
    }

    private void AudioEvent(audioEvent audio, int param)
    {
        switch (audio)
        {
            case audioEvent.CubertIntensity:
                SetIntensity(musicLayer.Cubert, param);
                SetVolumeForPlayerAgent(agentType.Cubert);
                break;
            case audioEvent.EnemyIntensity:

                enemyIntensity += param;
                if (enemyIntensity < 0)
                {
                    enemyIntensity = 0;
                }
                UpdateEnemyIntensity(false);
                break;
            case audioEvent.EnemyIntensitySet:
                enemyIntensity = param;
                break;
            case audioEvent.SirKelIntensity:
                SetIntensity(musicLayer.SirKel, param);
                SetVolumeForPlayerAgent(agentType.SirKel);
                break;
            case audioEvent.TreasureIntensity:
                SetIntensity(musicLayer.Treasure, param);
                SetVolumeForPlayerAgent(agentType.Treasure);
                break;
            case audioEvent.CubertDie:
                SetTempVolumeForPlayerAgent(agentType.Cubert, 0f);
                break;
            case audioEvent.SirKelDie:
                SetTempVolumeForPlayerAgent(agentType.SirKel, 0f);
                break;
            case audioEvent.TreasureDie:
                SetTempVolumeForPlayerAgent(agentType.Treasure, 0f);
                break;
            case audioEvent.Laser:
                beatEvents[beatEvent.Laser] = param;
                break;
            case audioEvent.Energy:
                beatEvents[beatEvent.Energy] = param;
                break;
            case audioEvent.Damage:
                beatEvents[beatEvent.Damage] = param;
                break;
            case audioEvent.Health:
                beatEvents[beatEvent.Health] = param;
                break;
        }
    }

    private void DoBeatEvents()
    {
        foreach (var key in beatEvents.Keys.ToList())
        {
            float volume = beatEvents[key] + 1;
            beatEvents[key] = -1;
            switch (key)
            {
                case beatEvent.Laser:
                    if (volume > 0)
                    {
                        volume = Mathf.Log10(1.3f * volume + 0.5f) + 0.6f;
                        if (volume > 1.4f)
                        {
                            volume = 1.4f;
                        }
                    }
                    musicMap[musicLayer.Laser][audioState.Combat][bpm][0].audioSource[0].volume = volume;
                    musicMap[musicLayer.Laser][audioState.Combat][bpm][0].audioSource[1].volume = volume;
                    break;
                case beatEvent.Energy:
                    if (beatNumber % 2 == 0)
                    {
                        musicMap[musicLayer.Energy1][audioState.Combat][bpm][0].audioSource[0].volume = volume * 0.8f;
                        musicMap[musicLayer.Energy1][audioState.Combat][bpm][0].audioSource[1].volume = volume * 0.8f;
                        musicMap[musicLayer.Energy2][audioState.Combat][bpm][0].audioSource[0].volume = 0;
                        musicMap[musicLayer.Energy2][audioState.Combat][bpm][0].audioSource[1].volume = 0;
                    }
                    else
                    {
                        musicMap[musicLayer.Energy2][audioState.Combat][bpm][0].audioSource[0].volume = volume * 0.8f;
                        musicMap[musicLayer.Energy2][audioState.Combat][bpm][0].audioSource[1].volume = volume * 0.8f;
                        musicMap[musicLayer.Energy1][audioState.Combat][bpm][0].audioSource[0].volume = 0;
                        musicMap[musicLayer.Energy1][audioState.Combat][bpm][0].audioSource[1].volume = 0;
                    }
                    break;
                case beatEvent.Damage:
                    if (volume > 0)
                    {
                        volume = (Mathf.Log10(1.3f * volume + 0.5f) + 0.3f) * 0.6f;
                        if (volume > 1.2f)
                        {
                            volume = 1.2f;
                        }
                    }
                    if (beatNumber % 2 == 0)
                    {
                        musicMap[musicLayer.Attack1][audioState.Combat][bpm][0].audioSource[0].volume = volume;
                        musicMap[musicLayer.Attack1][audioState.Combat][bpm][0].audioSource[1].volume = volume;
                        musicMap[musicLayer.Attack2][audioState.Combat][bpm][0].audioSource[0].volume = 0;
                        musicMap[musicLayer.Attack2][audioState.Combat][bpm][0].audioSource[1].volume = 0;
                    }
                    else
                    {
                        musicMap[musicLayer.Attack2][audioState.Combat][bpm][0].audioSource[0].volume = volume;
                        musicMap[musicLayer.Attack2][audioState.Combat][bpm][0].audioSource[1].volume = volume;
                        musicMap[musicLayer.Attack1][audioState.Combat][bpm][0].audioSource[0].volume = 0;
                        musicMap[musicLayer.Attack1][audioState.Combat][bpm][0].audioSource[1].volume = 0;
                    }
                    break;
                case beatEvent.Health:
                    if (beatNumber % 2 == 0)
                    {
                        musicMap[musicLayer.Health1][audioState.Combat][bpm][0].audioSource[0].volume = volume * 0.8f;
                        musicMap[musicLayer.Health1][audioState.Combat][bpm][0].audioSource[1].volume = volume * 0.8f;
                        musicMap[musicLayer.Health2][audioState.Combat][bpm][0].audioSource[0].volume = 0;
                        musicMap[musicLayer.Health2][audioState.Combat][bpm][0].audioSource[1].volume = 0;
                    }
                    else
                    {
                        musicMap[musicLayer.Health2][audioState.Combat][bpm][0].audioSource[0].volume = volume * 0.8f;
                        musicMap[musicLayer.Health2][audioState.Combat][bpm][0].audioSource[1].volume = volume * 0.8f;
                        musicMap[musicLayer.Health1][audioState.Combat][bpm][0].audioSource[0].volume = 0;
                        musicMap[musicLayer.Health1][audioState.Combat][bpm][0].audioSource[1].volume = 0;
                    }
                    break;
            }
        }
    }

    private void SetIntensity(musicLayer layer, int param)
    {
        intensities[layer] = param;
    }

    private void SetVolumeForPlayerAgent(agentType agent)
    {
        switch (agent)
        {
            case agentType.Cubert:
                if (!agentAlive[0])
                {
                    return;
                }
                musicMap[musicLayer.Cubert][state][bpm][0].audioSource[0].volume = GetVolumeFromIntensity(intensities[musicLayer.Cubert]);
                musicMap[musicLayer.Cubert][state][bpm][0].audioSource[1].volume = GetVolumeFromIntensity(intensities[musicLayer.Cubert]);
                break;
            case agentType.SirKel:
                if (!agentAlive[1])
                {
                    return;
                }
                musicMap[musicLayer.SirKel][state][bpm][0].audioSource[0].volume = GetVolumeFromIntensity(intensities[musicLayer.SirKel]);
                musicMap[musicLayer.SirKel][state][bpm][0].audioSource[1].volume = GetVolumeFromIntensity(intensities[musicLayer.SirKel]);
                break;
            case agentType.Treasure:
                if (!agentAlive[2])
                {
                    return;
                }
                musicMap[musicLayer.Treasure][state][bpm][0].audioSource[0].volume = GetVolumeFromIntensity(intensities[musicLayer.Treasure]);
                musicMap[musicLayer.Treasure][state][bpm][0].audioSource[1].volume = GetVolumeFromIntensity(intensities[musicLayer.Treasure]);
                break;
        }
    }

    private void SetTempVolumeForPlayerAgent(agentType agent, float volume)
    {
        foreach (int currentBpm in new int[3] {100, 120, 140})
            switch (agent)
            {
                case agentType.Cubert:
                    musicMap[musicLayer.Cubert][state][currentBpm][0].audioSource[0].volume = volume;
                    musicMap[musicLayer.Cubert][state][currentBpm][0].audioSource[1].volume = volume;
                    agentAlive[0] = false;
                    break;
                case agentType.SirKel:
                    musicMap[musicLayer.SirKel][state][currentBpm][0].audioSource[0].volume = volume;
                    musicMap[musicLayer.SirKel][state][currentBpm][0].audioSource[1].volume = volume;
                    agentAlive[1] = false;
                    break;
                case agentType.Treasure:
                    musicMap[musicLayer.Treasure][state][currentBpm][0].audioSource[0].volume = volume;
                    musicMap[musicLayer.Treasure][state][currentBpm][0].audioSource[1].volume = volume;
                    agentAlive[2] = false;
                    break;
            }
    }

    private float GetVolumeFromIntensity(int param)
    {
        float volume = Mathf.Log10(0.5f * (float)param + 1f);
        if (volume > 1f)
        {
            volume = 1f;
        }
        return volume;
    }

    private void SetBPM(int bpm)
    {
        this.bpm = bpm;
        //timePerLoop = (double)musicMap[musicLayer.Cubert][audioState.Planning][bpm][0].audioSource[0].clip.samples / musicMap[musicLayer.Cubert][audioState.Planning][bpm][0].audioSource[0].clip.frequency + 0.01;
        timePerLoop = 957.5d / (double)bpm; //matte sier 960, men noen lyver
        timePerBeat = 0.0625d * timePerLoop;
    }

    private void SetBPMRuntime(int bpmParam)
    {
        double samples = musicMap[musicLayer.Cubert][state][bpm][0].audioSource[1 - musicMap[musicLayer.Cubert][state][bpm][0].toggle].timeSamples;
        double freq = musicMap[musicLayer.Cubert][state][bpm][0].audioSource[musicMap[musicLayer.Cubert][state][bpm][0].toggle].clip.frequency;
        double time = samples / freq;
        double ratio = (double)bpm / (double)bpmParam;
        Debug.Log("time : " + time + " ratio : " + ratio);

        if (state == audioState.Combat)
        {
            Stop(musicMap[musicLayer.Laser][audioState.Combat][bpm][0]);
            Stop(musicMap[musicLayer.Energy1][audioState.Combat][bpm][0]);
            Stop(musicMap[musicLayer.Energy2][audioState.Combat][bpm][0]);
            Stop(musicMap[musicLayer.Attack1][audioState.Combat][bpm][0]);
            Stop(musicMap[musicLayer.Attack2][audioState.Combat][bpm][0]);
            Stop(musicMap[musicLayer.Health1][audioState.Combat][bpm][0]);
            Stop(musicMap[musicLayer.Health2][audioState.Combat][bpm][0]);
            StopEnemy(audioState.Combat);
        }
        else if (state == audioState.Planning)
        {
            StopEnemy(audioState.Planning);
        }
        Stop(musicMap[musicLayer.Cubert][state][bpm][0]);
        Stop(musicMap[musicLayer.SirKel][state][bpm][0]);
        Stop(musicMap[musicLayer.Treasure][state][bpm][0]);

        SetBPM(bpmParam);

        nextBeat.Clear();
        for (int i = 1; i < 19; i++)
        {
            nextBeat.Add((double)i * 0.0625d * timePerLoop + AudioSettings.dspTime);
        }
        SetState(state);
        PlayState(state);
        UpdateEnemyIntensity(true);

        if (beatNumber < 16)
        {
            time = time * ratio;

            Debug.Log("newTime : " + time);

            SetStateTime(state, (float)time);

            samples = musicMap[musicLayer.Cubert][state][bpm][0].audioSource[musicMap[musicLayer.Cubert][state][bpm][0].toggle].clip.samples;

            double untilNext = (samples / freq) - (musicMap[musicLayer.Cubert][state][bpm][0].audioSource[musicMap[musicLayer.Cubert][state][bpm][0].toggle].timeSamples / freq);

            nextLoop = AudioSettings.dspTime + untilNext;

            PlayStateScheduled(state, nextLoop);
        }

        setBPM = 0;
    }

    public void BPMButton(int bpmParam)
    {
        if (bpmParam != bpm)
        {
            setBPM = bpmParam;
        }
        else
        {
            setBPM = 0;
        }
    }

    private void SetNextLoopTime()
    {
        beatNumber = 0;
        nextLoop = AudioSettings.dspTime + timePerLoop;
        nextBeat.Clear();
        for (int i = 1; i < 19; i++)
        {
            nextBeat.Add((double)i * 0.0625d * timePerLoop + AudioSettings.dspTime);
        }
    }

    private void SetNextLoopTimeToBeats(int beats)
    {
        nextLoop = AudioSettings.dspTime + timePerBeat * (double) beats;
        nextBeat.Clear();
        for (int i = 0; i < 18; i++)
        {
            nextBeat.Add((double)i * 0.0625d * timePerLoop + AudioSettings.dspTime);
        }
    }

    private void ResetNextBeatTime()
    {
        nextBeat.RemoveAt(0);
        beatNumber++;
    }

    private void StartCombat()
    {
        SetState(audioState.Combat);
        combatMusicStarted = false;
        planningMusicStarted = false;
    }

    private void EndCombat(bool win)
    {
        if (win)
        {
            SetState(audioState.Victory);
            PlayState(audioState.Victory);
        }
        else
        {
            SetState(audioState.Loss);
            PlayState(audioState.Loss);
        }
        StopEnemy(audioState.Combat);
        //nextBeat[0] = 0;
    }

    private void SetState(audioState state)
    {
        this.state = state;
        SetVolumeForPlayerAgent(agentType.Cubert);
        SetVolumeForPlayerAgent(agentType.SirKel);
        SetVolumeForPlayerAgent(agentType.Treasure);
    }

    private void UpdateEnemyIntensity(bool force)
    {
        int newIntensity;
        if (enemyIntensity >= enemyMusicIntensityCutoffs[2])
        {
            newIntensity = 4;
        }
        else if (enemyIntensity >= enemyMusicIntensityCutoffs[1])
        {
            newIntensity = 3;
        }
        else if (enemyIntensity >= enemyMusicIntensityCutoffs[0])
        {
            newIntensity = 2;
        }
        else
        {
            newIntensity = 1;
        }

        if ((newIntensity != intensities[musicLayer.Enemy] && enemyIntensity > 0) || force)
        {
            musicMap[musicLayer.Enemy][state][100][intensities[musicLayer.Enemy]].audioSource[0].volume = 0;
            musicMap[musicLayer.Enemy][state][100][intensities[musicLayer.Enemy]].audioSource[1].volume = 0;
            musicMap[musicLayer.Enemy][state][100][newIntensity].audioSource[0].volume = 1;
            musicMap[musicLayer.Enemy][state][100][newIntensity].audioSource[1].volume = 1;
            musicMap[musicLayer.Enemy][state][120][intensities[musicLayer.Enemy]].audioSource[0].volume = 0;
            musicMap[musicLayer.Enemy][state][120][intensities[musicLayer.Enemy]].audioSource[1].volume = 0;
            musicMap[musicLayer.Enemy][state][120][newIntensity].audioSource[0].volume = 1;
            musicMap[musicLayer.Enemy][state][120][newIntensity].audioSource[1].volume = 1;
            musicMap[musicLayer.Enemy][state][140][intensities[musicLayer.Enemy]].audioSource[0].volume = 0;
            musicMap[musicLayer.Enemy][state][140][intensities[musicLayer.Enemy]].audioSource[1].volume = 0;
            musicMap[musicLayer.Enemy][state][140][newIntensity].audioSource[0].volume = 1;
            musicMap[musicLayer.Enemy][state][140][newIntensity].audioSource[1].volume = 1;
            intensities[musicLayer.Enemy] = newIntensity;
        }
    }

    private void EnemyIntensityPlanningToCombat()
    {
        musicMap[musicLayer.Enemy][audioState.Planning][bpm][intensities[musicLayer.Enemy]].audioSource[0].volume = 0;
        musicMap[musicLayer.Enemy][audioState.Planning][bpm][intensities[musicLayer.Enemy]].audioSource[1].volume = 0;
        musicMap[musicLayer.Enemy][audioState.Combat][bpm][intensities[musicLayer.Enemy]].audioSource[0].volume = 1;
        musicMap[musicLayer.Enemy][audioState.Combat][bpm][intensities[musicLayer.Enemy]].audioSource[1].volume = 1;

    }
}
