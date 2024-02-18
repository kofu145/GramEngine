using System.Collections.Concurrent;
using GramEngine.Core;
using SFML.Audio;

namespace GramEngine.ECS.Components;

public class Sound : Component
{
    private ConcurrentDictionary<string, StoredSound> sounds;
    public string CurrentSound;
    public float GlobalVolume;

    public bool isPlaying
    {
        get => sounds[CurrentSound].sound.Status == SoundStatus.Playing;
    }
    
    public Sound(float globalVolume = 20)
    {
        sounds = new ConcurrentDictionary<string, StoredSound>();
        GlobalVolume = globalVolume;
    }
    public Sound(string filename, string soundName, float globalVolume = 20)
    {
        sounds = new ConcurrentDictionary<string, StoredSound>();
        var soundBuffer = new SoundBuffer(filename);
        var sound = new SFML.Audio.Sound(soundBuffer);
        //sound.Attenuation = 0;
        CurrentSound = soundName;
        sounds.TryAdd(soundName, new StoredSound(soundBuffer, sound));
        GlobalVolume = globalVolume;
    }

    public void AddSound(string filename, string soundName)
    {
        var soundBuffer = new SoundBuffer(filename);
        var sound = new SFML.Audio.Sound(soundBuffer);
        //sound.Attenuation = 0;
        sounds.TryAdd(soundName, new StoredSound(soundBuffer, sound));
    }
    
    public float Volume
    {
        get => sounds[CurrentSound].sound.Volume;
        set => sounds[CurrentSound].sound.Volume = value;
    }
    
    public float Attenuation
    {
        get => sounds[CurrentSound].sound.Attenuation;
        set => sounds[CurrentSound].sound.Attenuation = value;
    }
    public float Pitch
    {
        get => sounds[CurrentSound].sound.Pitch;
        set => sounds[CurrentSound].sound.Pitch = value;
    }
    
    public bool Loop
    {
        get => sounds[CurrentSound].sound.Loop;
        set => sounds[CurrentSound].sound.Loop = value;
    }
    
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Dispose()
    {
        foreach (var sound in sounds.Values)
        {
            sound.sound.Stop();
            sound.sound.Dispose();
            sound.soundBuffer.Dispose();
        }
    }
    
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public void Play()
    {
        Volume = GlobalVolume;
        sounds[CurrentSound].sound.Play();
    }

    public void Play(string soundName)
    {
        CurrentSound = soundName;
        Play();
    }

    public void Pause()
    {
        sounds[CurrentSound].sound.Pause();
    }

    public void Stop()
    {
        sounds[CurrentSound].sound.Stop();
    }

    internal struct StoredSound
    {
        internal SoundBuffer soundBuffer;
        internal SFML.Audio.Sound sound;

        public StoredSound(SoundBuffer soundBuffer, SFML.Audio.Sound sound)
        {
            this.soundBuffer = soundBuffer;
            this.sound = sound;
        }
    }

}