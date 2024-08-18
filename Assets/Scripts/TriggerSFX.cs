using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSFX : MonoBehaviour
{
    public AudioSource breakBlocks;
    public AudioSource invalidPlacement;
    public AudioSource invalidPlacement2;
    public AudioSource invalidPlacement3;
    public AudioSource placement;
    public AudioSource placement2;
    public AudioSource placement3;
    public AudioSource timerTick;
    public AudioSource timerHitZero;

    public enum SoundType
    {
        break_block,
        invalid_placement,
        invalid_placement2,
        invalid_placement3,
        placement,
        placement2,
        placement3,
        timer_tick,
        timer_zero,
    }
    
    public void PlaySound(SoundType sound_name)
    {
        Dictionary<SoundType, AudioSource> soundMapping = new()
        {
            {SoundType.break_block, breakBlocks},
            {SoundType.invalid_placement, invalidPlacement},
            {SoundType.invalid_placement2, invalidPlacement2},
            {SoundType.invalid_placement3, invalidPlacement3},
            {SoundType.placement, placement},
            {SoundType.placement2, placement2},
            {SoundType.placement3, placement3},
            {SoundType.timer_tick, timerTick},
            {SoundType.timer_zero, timerHitZero},
        };
        soundMapping[sound_name].Play();
    }
}
