using Godot;

public class FadeBlock : GDKnyttBaseObject
{
    private float pos;

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        pos += delta * (random.Next(5) - 1) * 50 * Mathf.Pi / 90;
        Modulate = new Color(1, 1, 1, 0.16f + 0.16f * Mathf.Sin(pos));
    }
}
