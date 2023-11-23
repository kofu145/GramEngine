using GramEngine.Core;
using SFML.Audio;

namespace GramEngine.ECS.Components;

public class Sound : Component
{
    private SoundBuffer soundBuffer;
    private SFML.Audio.Sound sound;
    
    public float Volume
    {
        get => sound.Volume;
        set => sound.Volume = value;
    }
    
    public float Attenuation
    {
        get => sound.Attenuation;
        set => sound.Attenuation = value;
    }
    public float Pitch
    {
        get => sound.Pitch;
        set => sound.Pitch = value;
    }
    
    public bool Loop
    {
        get => sound.Loop;
        set => sound.Loop = value;
    }

    public Sound(string filename)
    {
        soundBuffer = new SoundBuffer(filename);
        sound = new SFML.Audio.Sound(soundBuffer);
        sound.Attenuation = 0;
    }
    
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Dispose()
    {
        sound.Dispose();
        soundBuffer.Dispose();
    }
    
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public void Play()
    {
        sound.Play();
    }

    public void Pause()
    {
        sound.Pause();
    }

    public void Stop()
    {
        sound.Stop();
    }
    
}