namespace QuickMods.quick;

public interface IModsBase
{
    bool Initialized();

    bool Enabled();

    bool Active();

    string Name();

    void Start();

    void OnDestroy();

    void Update();
}