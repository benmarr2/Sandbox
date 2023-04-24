using Godot;

namespace dPlatormer.scripts;

public class InputHandler
{
    public bool Left { get; private set; }
    public bool Right { get; private set; }
    public bool Jump{ get; private set; }

    public float Horizontal => Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");

    public void Update()
    {
        Left = Input.IsActionPressed("ui_left");
        Right = Input.IsActionPressed("ui_right");
        Jump = Input.IsActionPressed("ui_accept");
    }
}