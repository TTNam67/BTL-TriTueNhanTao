using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using DG.Tweening;
using Dasis.Extensions;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

[HideMonoScript]
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public static readonly string NULL_NAME = "Null"; // Never names Audio like this

    [Searchable(FilterOptions = SearchFilterOptions.ISearchFilterableInterface)]
    [SerializeField]
    [ListDrawerSettings(AlwaysAddDefaultValue = true, NumberOfItemsPerPage = 5, DraggableItems = false, IsReadOnly = true)]
    private List<Audio> audios;

    [Space]
    [InfoBox("Limit number of the audio players that can be played at the same time")]
    [PropertyOrder(Order = 1)]
    [SerializeField]
    [Range(1, 20)]
    private int soundPlayerLimit = 10; // Recommended

    [PropertyOrder(Order = 1)]
    [SerializeField]
    [Range(1, 5)]
    private int musicPlayerLimit = 2; // Recommended

    [Space]
    [InfoBox("Define where to load Audio and where to generate Audio Dictionary")]
    [PropertyOrder(Order = 1)]
    [SerializeField]
    [FolderPath(ParentFolder = "Assets/Resources")]
    private string audioFolder;

    [PropertyOrder(Order = 1)]
    [SerializeField]
    [FolderPath]
    private string audioDictionaryFolder;

    [SerializeField]
    [ListDrawerSettings(NumberOfItemsPerPage = 5, IsReadOnly = true)]
    private List<AudioClip> clips = new List<AudioClip>();

    private readonly List<Audio.Player> soundPlayers = new List<Audio.Player>();
    private readonly List<Audio.Player> musicPlayers = new List<Audio.Player>();
    private readonly int[] sourceIndex = new int[2];
    private readonly float[] masterVolume = new float[] { 1, 1 };

    [SerializeField] private AudioMixerGroup _masterAudioMixerGroup, _musicAudioMixer, _sfxAudioMixerGroup;

    #region Player Data
    public bool IS_SOUND_ON
    {
        get { return PlayerPrefs.GetInt("EnableSound", 1) == 1; }
        set { PlayerPrefs.SetInt("EnableSound", value ? 1 : 0); }
    }

    public bool IS_MUSIC_ON
    {
        get { return PlayerPrefs.GetInt("EnableMusic", 1) == 1; }
        set { PlayerPrefs.SetInt("EnableMusic", value ? 1 : 0); }
    }
    #endregion

    #region Initialize
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
            DontDestroyOnLoad(this);
            return;
        }
        if (Instance == this)
        {
            return;
        }
        Destroy(gameObject);
    }

    public void Initialize()
    {
        for (int i = 0; i < soundPlayerLimit; i++)
        {
            soundPlayers.Add(new Audio.Player(gameObject.AddComponent<AudioSource>()));
            soundPlayers[i].source.enabled = false;
            soundPlayers[i].source.playOnAwake = false;
            soundPlayers[i].source.outputAudioMixerGroup = _sfxAudioMixerGroup;
        }
        for (int i = 0; i < musicPlayerLimit; i++)
        {
            musicPlayers.Add(new Audio.Player(gameObject.AddComponent<AudioSource>()));
            musicPlayers[i].source.enabled = false;
            musicPlayers[i].source.playOnAwake = false;
            musicPlayers[i].source.outputAudioMixerGroup = _musicAudioMixer;
        }
    }
    #endregion

    #region Optimization
    public void SortAudio()
    {
        audios.Sort((a, b) =>
        {
            return a.name.CompareTo(b.name);
        });

        // For testing
        for (int i = 0; i < audios.Count; i++)
        {
            audios[i].audioFolder = audioFolder;
        }
    }

    /// <summary>
    /// Taking advantage of ordered dictionary, this uses binary seacrh to optimize searching performance.
    /// </summary>
    public static int FindAudioIndex(string name)
    {
        int left = 0, right = AudioDictionary.names.Count - 1;
        int middle;
        int compare;
        while (left <= right)
        {
            middle = (left + right) / 2;
            compare = name.CompareTo(AudioDictionary.names[middle]);
            if (left == right)
            {
                if (compare == 0)
                    return middle;
                break;
            }
            if (compare == 0)
            {
                return middle;
            }
            if (compare > 0)
            {
                left = middle + 1;
            }
            if (compare < 0)
            {
                right = middle - 1;
            }
        }
        Debug.LogWarning("Can not find audio with the given name!");
        return -1;
    }

    /// <summary>
    /// Find an audio source that is free (disable). If can not, return the oldest one.
    /// </summary>
    public Audio.Player GetAudioPlayer(Audio.Type type)
    {
        int iterations = 0;
        int typeIndex = (int)type;
        var sources = (type == Audio.Type.Sound) ? soundPlayers : musicPlayers;
        while (sources[sourceIndex[typeIndex]].source.enabled)
        {
            iterations++;
            sourceIndex[typeIndex] = (sourceIndex[typeIndex] + 1) % sources.Count;
            if (iterations >= sources.Count)
            {
                //Debug.LogWarning("Source limit has been reached! Considering increase the limit to make sure the audio play correctly.");
                break;
            }
        }
        sources[sourceIndex[typeIndex]].source.enabled = true;
        return sources[sourceIndex[typeIndex]];
    }
    #endregion

    #region Audio Controls
    public static float GetAudioLength(string name)
    {
        for (int i = 0; i < AudioDictionary.names.Count; i++)
        {
            if (AudioDictionary.names[i].Equals(name))
            {
                return AudioDictionary.lengths[i];
            }
        }
        return 0;
    }

    public void SetSourceProperties(AudioSource source, AudioClip clip, Audio audio)
    {
        source.clip = clip;
        source.volume = audio.volume * GetMasterVolume(audio.type);
        source.pitch = audio.pitch;
        source.loop = audio.loop;
    }

    public float GetMasterVolume(Audio.Type type)
    {
        float volume = masterVolume[(int)type];
        if (!IS_MUSIC_ON && type == Audio.Type.Music)
            volume = 0;
        if (!IS_SOUND_ON && type == Audio.Type.Sound)
            volume = 0;
        return volume;
    }

    /// <summary>
    /// Play audio with default properties (search by name)
    /// </summary>
    public void Play(string name, bool unstoppable = false)
    {
        if (name.Equals(NULL_NAME)) return;
        int index = FindAudioIndex(name);
        if (index == -1) return;
        Audio audio = audios[index];
        SetPlayerAndPlay(audio, index, unstoppable);
    }

    /// <summary>
    /// Play audio with default properties (search by enumName)
    /// </summary>
    public void Play(AudioEnum enumName, bool unstoppable = false)
    {
        int index = (int)enumName;
        Audio audio = audios[index];
        SetPlayerAndPlay(audio, index, unstoppable);
    }

    /// <summary>
    /// Play audio with overrided properties
    /// </summary>
    public void Play(Audio audio, bool unstoppable = false)
    {
        int index = FindAudioIndex(audio.name);
        if (index == -1) return;
        SetPlayerAndPlay(audio, index, unstoppable);
    }

    public void SetPlayerAndPlay(Audio audio, int index, bool unstoppable)
    {
        Audio.Player player = GetAudioPlayer(audio.type);
        player.originVolume = audio.volume;
        player.unstoppable = unstoppable;
        SetSourceProperties(player.source, clips[index], audio);
        player.source.Play();
        DisablePlayerWhenPlayCompleted(player);
    }

    public async void DisablePlayerWhenPlayCompleted(Audio.Player player)
    {
        if (player.source.loop) return;
        while (player.source != null && player.source.isPlaying)
        {
            await Task.Yield();
        }
        if (player.source != null)
            player.source.enabled = false;
    }

    public void ChangeVolume(Audio.Type type, float targetVolume, bool smooth = true)
    {
        masterVolume[(int)type] = targetVolume;
        float actualMasterVolume = GetMasterVolume(type);
        var players = (type == Audio.Type.Music) ? musicPlayers : soundPlayers;
        foreach (var player in players)
        {
            if (smooth)
            {
                DOVirtual.Float(player.source.volume, player.originVolume * actualMasterVolume, 0.2f,
                    volume => player.source.volume = volume);
                continue;
            }
            player.source.volume = player.originVolume * actualMasterVolume;
        }
    }

    public float GetVolumeData(AudioEnum audioEnum)
    {
        return audios[(int)audioEnum].volume;
    }

    public void ChangeVolumeData(AudioEnum audioEnum, float volume)
    {
        Audio audio = audios[(int)audioEnum];
        audio.volume = volume;
    }

    public void Mute(Audio.Type type, bool smooth = true)
    {
        ChangeVolume(type, 0, smooth);
    }

    public void MuteAll()
    {
        Mute(Audio.Type.Music, false);
        Mute(Audio.Type.Sound, false);
    }

    public void ResetVolume(Audio.Type type, bool smooth = true)
    {
        ChangeVolume(type, 1, smooth);
    }

    public void ResetVolumeAll()
    {
        ResetVolume(Audio.Type.Music, false);
        ResetVolume(Audio.Type.Sound, false);
    }

    public void Pause(Audio.Type type)
    {
        var players = (type == Audio.Type.Music) ? musicPlayers : soundPlayers;
        foreach (var player in players)
        {
            player.source.Pause();
        }
    }

    public void PauseAll()
    {
        Pause(Audio.Type.Music);
        Pause(Audio.Type.Sound);
    }

    public void UnPause(Audio.Type type)
    {
        var players = (type == Audio.Type.Music) ? musicPlayers : soundPlayers;
        foreach (var player in players)
        {
            player.source.UnPause();
        }
    }

    public void UnPauseAll()
    {
        UnPause(Audio.Type.Music);
        UnPause(Audio.Type.Sound);
    }

    public void StopAll(Audio.Type type)
    {
        var players = (type == Audio.Type.Music) ? musicPlayers : soundPlayers;
        foreach (var player in players)
        {
            if (player.unstoppable) continue;
            player.source.Stop();
            player.source.enabled = false;
        }
    }
    #endregion

    #region Audio Settings
    public void LoadClips()
    {
        Object[] resouceClips = Resources.LoadAll(audioFolder, typeof(AudioClip));
        if (resouceClips == null) return;
        clips.Clear();
        for (int i = 0; i < resouceClips.Length; i++)
        {
            clips.Add((AudioClip)resouceClips[i]);
        }
        clips.Sort((a, b) =>
        {
            return a.name.CompareTo(b.name);
        });
    }

    [PropertyOrder(Order = -1)]
    [ButtonGroup("Top")]
    [Button(Icon = SdfIconType.MusicNoteList, ButtonHeight = 50, Name = "")]
    [GUIColor(.6f, 1, .6f, 1)]
    public void LoadAudio()
    {
        LoadClips();
        List<AudioClip> unloadedClips = new List<AudioClip>();
        foreach (var clip in clips)
        {
            bool isLoaded = false;
            for (int i = 0; i < audios.Count; i++)
            {
                if (audios[i].name.Equals(clip.name))
                {
                    isLoaded = true;
                    break;
                }
            }
            if (isLoaded) continue;
            unloadedClips.Add(clip);
        }

        for (int i = 0; i < unloadedClips.Count; i++)
        {
            Audio audio = new Audio
            {
                name = unloadedClips[i].name,
                testSource = GetComponent<AudioSource>(),
                insideManager = true,
            };
            audios.Add(audio);
        }
        SortAudio();
    }

    // Constant stuffs, please don't change at all cost.
    private readonly string className = "AudioDictionary";
    private readonly string enumName = "AudioEnum";
    //

    [PropertyOrder(Order = -1)]
    [ButtonGroup("Top")]
    [Button(Icon = SdfIconType.Stack, Name = "")]
    [GUIColor(.6f, .6f, 1, 1)]
    public void GenerateDictionary()
    {
        audios.Sort((a, b) =>
        {
            return a.name.CompareTo(b.name);
        });

        string classContent = $"public static class {className}" + "\n{\n";
        classContent += "\tpublic static List<string> names = new List<string> {\n";
        string enumContent = $"public enum {enumName}" + "\n{\n";
        foreach (var audio in audios)
        {
            classContent += $"\t\t\"{audio.name}\",\n";
            string enumName = Stringify.CapitalizeEachWord(audio.name);
            if (!char.IsLetter(enumName[0]) && !enumName[0].Equals("_"))
            {
                enumName = $"_{enumName}";
            }
            enumContent += $"\t{enumName.Replace(" ", string.Empty)},\n";
        }
        classContent += $"\t\t\"{NULL_NAME}\",\n" + "\t};\n\n";
        enumContent += $"\t{NULL_NAME},\n" + "}";

        classContent += "\tpublic static List<float> lengths = new List<float> {\n";
        foreach (var clip in clips)
        {
            classContent += $"\t\t{clip.length}f,\n";
        }
        classContent += $"\t\t{0}f,\n" + "\t};\n}\n\n";

        var path = audioDictionaryFolder + "/" + className + ".cs";

        var scriptFile = new StreamWriter(path);
        scriptFile.Write("using System.Collections.Generic;\n\n");
        scriptFile.Write(classContent);
        scriptFile.Write(enumContent);
        scriptFile.Close();
#if UNITY_EDITOR
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
        AssetDatabase.Refresh();
#endif
    }

    [ButtonGroup("Top")]
    [PropertyOrder(Order = -1)]
    [Button(Icon = SdfIconType.Trash, Name = "")]
    [GUIColor(1, .8f, .8f, 1)]
    public void ClearNullAudio()
    {
        for (int i = audios.Count - 1; i >= 0; i--)
        {
            if (audios[i].IsNull)
            {
                audios.RemoveAt(i);
            }
        }
    }

    [ButtonGroup("Bottom")]
    [Button(Icon = SdfIconType.Trash)]
    [GUIColor(1, .6f, .6f, 1)]
    public void ClearAll()
    {
        audios.Clear();
        clips.Clear();
    }
    #endregion
}

#region Structure
[System.Serializable]
public class Audio : ISearchFilterable
{
    [DisableIf("@insideManager")]
    [ValueDropdown("@AudioDictionary.names")]
    [GUIColor(0, 1, 1, 1)]
    public string name;
    public Type type;
    [Range(0f, 1f)]
    public float volume = 1;
    [Range(.5f, 2f)]
    public float pitch = 1;

    [HorizontalGroup]
    public bool loop;

    [HideInInspector] public string audioFolder;
    [HideInInspector] public AudioSource testSource;
    [HideInInspector] public bool insideManager = false;

    private string clipName;
    private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

    public bool IsPlaying
    {
        get
        {
            if (testSource == null)
                return false;
            if (testSource.clip == null)
                return false;
            if (testSource.clip.name.Equals(clipName))
                return true;
            return false;
        }
    }

    public bool IsNull
    {
        get
        {
            Object clipObject = Resources.Load($"{audioFolder}/{name}", typeof(AudioClip));
            if (clipObject == null)
            {
                return true;
            }
            return false;
        }
    }

    [HorizontalGroup]
    [Button(Icon = SdfIconType.ArrowCounterclockwise)]
    [HideLabel]
    [PropertyTooltip("Reset Settings")]
    public void ResetSettings()
    {
        type = Type.Sound;
        volume = 1;
        pitch = 1;
        loop = false;
    }

    [ShowIf("@!IsPlaying && insideManager")]
    [HorizontalGroup]
    [Button(Icon = SdfIconType.Play)]
    [HideLabel]
    [PropertyTooltip("Play Audio")]
    public async void PlayAudio()
    {
        Object clipObject = Resources.Load($"{audioFolder}/{name}", typeof(AudioClip));
        if (clipObject == null)
        {
            Debug.LogWarning("Can not find audio with the given name!");
            return;
        }

        AudioClip clip = (AudioClip)clipObject;
        clipName = clip.name;
        testSource.clip = clip;
        testSource.volume = volume;
        testSource.pitch = pitch;
        testSource.loop = loop;
        testSource.Play();

        if (loop) return;
        int audioLength = Mathf.RoundToInt(1000 * testSource.clip.length / testSource.pitch);
        try
        {
            await Task.Delay(audioLength, tokenSource.Token);
            if (testSource.clip != null && testSource.clip.name.Equals(clip.name))
            {
                testSource.clip = null;
            }
        }
        catch
        {
            return;
        }
    }

    [ShowIf("@IsPlaying && insideManager")]
    [HorizontalGroup]
    [Button(Icon = SdfIconType.Stop)]
    [HideLabel]
    [PropertyTooltip("Pause Audio")]
    public void StopAudio()
    {
        testSource.Stop();
        testSource.clip = null;
        tokenSource.Cancel();
    }

    public bool IsMatch(string searchString)
    {
        return name.ToLower().Contains(searchString.ToLower());
    }

    public enum Type
    {
        Sound,
        Music,
    }

    public class Player
    {
        public AudioSource source;
        public float originVolume;
        public bool unstoppable = false;

        public Player(AudioSource audioSource, float volume = 1)
        {
            source = audioSource;
            originVolume = volume;
        }
    }
}

#endregion