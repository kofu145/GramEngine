﻿using System.Numerics;
using GramEngine.Core;
using GramEngine.Core.Input;

namespace GramEngine.ECS.Components.UI;

public class Button : Component
{
    public event OnPress OnButtonDown;
    public event OnDepress OnButtonUp;

    public event OnStartHover OnHover;
    public event OnEndHover OnStopHover;

    public delegate void OnPress();
    public delegate void OnDepress();
    public delegate void OnStartHover();
    public delegate void OnEndHover();

    public bool isHovered = false;
    public bool isPressed = false;
    public int Width;
    public int Height;
    
    private Sprite parentSprite;
    
    public Button(int width, int height)
    {
        Width = width;
        Height = height;
    }
    public override void Initialize()
    {
        parentSprite = ParentEntity.GetComponent<Sprite>();
    }
    public override void Update(GameTime gameTime)
    {
        var pos = ParentEntity.Transform.Position;
        Vector2 mousePos = InputManager.MouseWorldPos;
        // check if mouse in bounds
        if (mousePos.X > pos.X && mousePos.X < pos.X + Width &&
            mousePos.Y > pos.Y && mousePos.Y < pos.Y + Height)
        {
            // set to hovered if in bounds and not already hovered
            if (!isHovered)
            {
                isHovered = true;
                OnHover?.Invoke();
                //StartHover();
            }
            // check if mouse button is down
            if (InputManager.GetMousePress(MouseButton.Left))
            {
                // if is down and not pressed = false then call mousepressed
                if (!isPressed)
                {
                isPressed = true;
                OnButtonDown?.Invoke();
                //StartPress();
                }

            }
        }
        // if mouse is not in bounds
        else
        {
            // set hovered to false if it is true
            if (isHovered)
            {
                isHovered = false;
                OnStopHover?.Invoke();
                //StopHover();
            }
        }
        // check if mouse is not pressed 
        if (!InputManager.GetMousePress(MouseButton.Left))
        {
            // if not pressed and flag is currently set true then call not pressed function
            if (isPressed)
            {
                isPressed = false;
                OnButtonUp?.Invoke();
                //StopPress();
            } 
        }
    }
    // made these private, because I don't see many reasons you'd force them to trigger?
    // If you do, sounds like bad architectural design on user end

    private void StartHover()
    {
        //animation.SetTexture(1);
        Console.WriteLine("hovering!");
    }
    private void StopHover()
    {
        //var animation = ParentEntity.GetComponent<Animation>();
        //animation.SetTexture(0);
        Console.WriteLine("stopped hovering!");

    }
    private void StartPress()
    {
        //var animation = ParentEntity.GetComponent<Animation>();
        //animation.SetTexture(2);
        Console.WriteLine("pressing!!");

    }
    private void StopPress()
    {
        var animation = ParentEntity.GetComponent<Animation>();
        if (isHovered)
        {
            //animation.SetTexture(1);
            Console.WriteLine("stoped ppressing!!");

        }
        else
        {
            //animation.SetTexture(0);
            Console.WriteLine("stoped ppressing!! not hovering!");

        }
    }
}