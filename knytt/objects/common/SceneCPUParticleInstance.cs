using Godot;

public class SceneCPUParticleInstance : Node2D
{
    public Vector2 Velocity;
    public float Mass;
    public Vector2 Force;
    public Vector2 Gravity;
    public float Drag;

    public OpenSimplexNoise _noise;

    public SceneCPUParticles parent;

    bool stopped = false;

    bool _brownian;
    public bool BrownianMotion
    {
        get { return _brownian; }
        set
        {
            if (value)
            {
                if (_noise == null)
                {
                    _noise = new OpenSimplexNoise();
                    _noise.Seed = GDKnyttDataStore.random.Next();
                }
            }
            _brownian = value;
        }
    }

    public Vector2 BrownianForce;
    public Vector2 BrownianSpeed;
    public float BrownianExponent;
    protected Vector2 _brownian_t;

    public float Lifetime;

    public string Params;

    protected Vector2 _acceleration;
    protected float _time;

    public override void _Ready()
    {
        _brownian_t = new Vector2(0f, 999999f);
    }

    public override void _PhysicsProcess(float delta)
    {
        if (stopped) { return; }

        base._PhysicsProcess(delta);
        _time += delta;
        if (_time >= Lifetime) { parent?.returnParticle(this); }

        if (_brownian) { calcBrownianForces(delta); }

        // Simple Euler Integration
        _acceleration = Force / Mass;
        _acceleration += Gravity;
        Velocity += (_acceleration - (Drag * Velocity)) * delta;
        Translate(Velocity * delta);

        Force = Vector2.Zero;
    }

    private void calcBrownianForces(float delta)
    {
        _brownian_t += BrownianSpeed * delta;

        var bm = new Vector2(_noise.GetNoise1d(_brownian_t.x), _noise.GetNoise1d(_brownian_t.y)) * BrownianForce;
        bm.x = Mathf.Pow(Mathf.Abs(bm.x), BrownianExponent) * Mathf.Sign(bm.x);
        bm.y = Mathf.Pow(Mathf.Abs(bm.y), BrownianExponent) * Mathf.Sign(bm.y);
        Force += bm;
    }

    public void stop()
    {
        stopped = true;
    }

    public void hide()
    {
        Visible = false;
        stopped = true;
        _time = 0;
        _acceleration = Vector2.Zero;
        Velocity = Vector2.Zero;
        Force = Vector2.Zero;
        _brownian_t = new Vector2(0f, 999999f);
    }

    public void renew()
    {
        Visible = true;
        Position = Vector2.Zero;
        stopped = false;
    }
}
