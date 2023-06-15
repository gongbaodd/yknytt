using Godot;

public class TrapFire : GDKnyttBaseObject
{
    private AnimatedSprite sprite;
    private Timer shotDelayTimer;

    public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite>("AnimatedSprite");
        shotDelayTimer = GetNode<Timer>("ShotDelayTimer");

        GDArea.Selector.Register(this);
        GDArea.Bullets.RegisterEmitter(this, "FireBullet2",
            (p, i) =>
            {
                p.Translate(new Vector2(12, 12));
                p.VelocityMMF2 = 50;
                p.Direction = getDirection();
                //p.DirectionMMF2 = Mathf.Round(p.DirectionMMF2);
            });
    }

    private float getDirection()
    {
        return Mathf.Atan2(GlobalPosition.y - Juni.ApparentPosition.y + 10, GlobalPosition.x - Juni.ApparentPosition.x);
    }

    private async void _on_ShotTimer_timeout_ext()
    {
        // TODO: different FPS in the original game
        sprite.Play("shoot");
        await ToSignal(sprite, "animation_finished");

        var wait_time = GDArea.Selector.GetIndex(this) * 0.03f;
        if (wait_time > 0)
        {
            shotDelayTimer.Start(wait_time);
            await ToSignal(shotDelayTimer, "timeout");
        }

        GDArea.Bullets.Emit(this);
        GetNode<AudioStreamPlayer2D>("ShotPlayer").Play();
        sprite.Play("default");
    }
}
