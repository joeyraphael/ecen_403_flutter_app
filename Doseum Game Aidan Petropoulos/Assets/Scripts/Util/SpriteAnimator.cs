using System.Collections; // not really using this but whatever
using System.Collections.Generic;
using UnityEngine;

// super simple animator that cycles thru a list of sprites
public class SpriteAnimator
{
    SpriteRenderer spriteRenderer; // what image i'm changing
    List<Sprite> frames; // the animation frames
    float frameRate; // how fast to swap frames

    int currentFrame; // which frame i'm on
    float timer; // time since last frame swap

    public SpriteAnimator(List<Sprite> frames, SpriteRenderer spriteRenderer, float frameRate = 0.16f)
    {
        this.frames = frames; // store frames
        this.spriteRenderer = spriteRenderer; // store renderer
        this.frameRate = frameRate; // store rate
    }

    public void Start()
    {
        currentFrame = 0; // start on frame 0
        timer = 0f; // reset timer
        spriteRenderer.sprite = frames[0]; // show first sprite
    }

    public void HandleUpdate()
    {
        timer += Time.deltaTime; // add time each frame

        // time to swap sprite?
        if (timer > frameRate)
        {
            currentFrame = (currentFrame + 1) % frames.Count; // move to next looped frame
            spriteRenderer.sprite = frames[currentFrame]; // change sprite

            timer -= frameRate; // reset timer a bit
        }
    }

    public List<Sprite> Frames
    {
        get { return frames; } // give back frames if i need them anywhere else
    }
}
