using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSFX : MonoBehaviour
{
    public AudioSource breakBlocks;
    public AudioSource invalidPlacement;
    public AudioSource placement;
    public AudioSource timerTick;
    public AudioSource timerHitZero;

    public enum SoundType
    {
        break_block,
        invalid_placement,
        placement,
        timer_tick,
        timer_zero,
    }
    
    public void PlaySound(SoundType sound_name)
    {
        Dictionary<SoundType, AudioSource> soundMapping = new()
        {
            {SoundType.break_block, breakBlocks},
            {SoundType.invalid_placement, invalidPlacement},
            {SoundType.placement, placement},
            {SoundType.timer_tick, timerTick},
            {SoundType.timer_zero, timerHitZero},
        };
        soundMapping[sound_name].Play();
    }
}
