namespace QuickMods.quick;

public interface IModsBase
{
    bool Initialized();

    void Start();

    void OnDestroy();

    void Update();
}