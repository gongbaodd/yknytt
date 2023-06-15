using Godot;

public class FlyBot : GDKnyttBaseObject
{
    PathFollow2D path;

    public override void _Ready()
    {
        base._Ready();
        path = GetNode<PathFollow2D>("Path2D/PathFollow2D");
    }

    public override void _PhysicsProcess(float delta)
    {
        path.Offset += 80f * delta;
    }
}
