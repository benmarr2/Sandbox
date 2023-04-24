using Godot;
using System;
using dPlatormer.scripts;
using static dPlatormer.scripts.PlayerState;

public partial class player : CharacterBody2D
{
    public const float Speed = 300.0f;
    public const float JumpVelocity = -600.0f;
    private bool _canDoubleJump;
    
    public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    
    private AnimatedSprite2D _playerAnimations;
    private InputHandler _inputHandler;
    private PlayerState _currentState;
    public override void _Ready()
   {
       _playerAnimations = GetNode<AnimatedSprite2D>("Player");
       _inputHandler = new InputHandler();
   }

    public override void _PhysicsProcess(double delta)
    {
        ApplyGravity(delta);
        Vector2 velocity = Velocity;

        _inputHandler.Update();
        // Handle player input
        
        if (_inputHandler.Jump && IsOnFloor())
        {
            _currentState = PlayerState.Jump;
        }
        else if (!IsOnFloor() && Velocity.Y >= 0f)
        {
            _currentState = PlayerState.Fall;
        }
        else if (_inputHandler.Left || _inputHandler.Right)
        {
            _currentState = PlayerState.Run;
        }
        else if (IsOnFloor())
        {
            _currentState = PlayerState.Idle;
        }

        // Handle player states
        switch (_currentState)
        {
            case PlayerState.Idle:
                HandleIdleState();
                break;
            case PlayerState.Jump:
                HandleJumpState();
                break;
            case PlayerState.Run:
                HandleRunState();
                break;
            case PlayerState.Fall:
                HandleFallState();
                break;
        }

        MoveAndSlide();
    }

    private void ApplyGravity(double delta)
    {
        Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity * (float)delta);
    }
    public void HandleIdleState()
    {
        _playerAnimations.Play("idle");
        GD.Print("IDLE");
        Velocity = new Vector2(0, Velocity.Y);
        GD.Print(Velocity.Y);
    }

    public void HandleJumpState()
    {
        _playerAnimations.Play("jump");
        GD.Print("JUMPING");
        Velocity = new Vector2(Velocity.X, JumpVelocity);
    }

    public void HandleFallState()
    {
        _playerAnimations.Play("fall");
        GD.Print("FALLING");
        Velocity = new Vector2(Velocity.X, Gravity);
    }

    public void HandleRunState()
    {
        _playerAnimations.Play("run");
        GD.Print("RUNNING");
        
        float direction = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        if (direction <= 0)
        {
            _playerAnimations.FlipH = true;
        }
        else
        {
            _playerAnimations.FlipH = false;
        }
        
        Velocity = new Vector2(direction * Speed, Velocity.Y);
    }
}
