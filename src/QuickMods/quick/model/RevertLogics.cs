using KSP.Game;

namespace QuickMods.quick.model;

public class RevertLogics
{
    private byte[] _revertToLaunchSaveGame;
    private byte[] _revertToVabSaveGame;
    private string _lastVesselCreatedName;
    private string _lastLaunchedAssemblyName;

    public bool CanRevert()
    {
        var gameStateRevTracker = GameManager.Instance.Game.stateRevTracker;
        return gameStateRevTracker.HasValidReversionState && gameStateRevTracker.IsLastLaunchedVesselTheActiveVessel() ||
               gameStateRevTracker.HasValidVABReversionState && gameStateRevTracker.IsLastLaunchedAssemblyTheActiveVessel();
    }

    public bool RevertIsEmpty()
    {
        var gameStateRevTracker = GameManager.Instance.Game.stateRevTracker;
        return gameStateRevTracker._saveGameData == null && gameStateRevTracker._lastVesselCreatedName == null;
    }

    public bool RevertSaved()
    {
        return _revertToLaunchSaveGame != null && _lastVesselCreatedName != null;
    }

    public void Reset()
    {
        _revertToVabSaveGame = null;
        _revertToVabSaveGame = null;
        _lastVesselCreatedName = null;
        _lastLaunchedAssemblyName = null;

        var gameStateRevTracker = GameManager.Instance.Game.stateRevTracker;
        gameStateRevTracker._saveVABGameData = null;
        gameStateRevTracker._saveGameData = null;
        gameStateRevTracker._lastLaunchedAssemblyName = null;
        gameStateRevTracker._lastVesselCreatedName = null;
    }

    public void Save()
    {
        var gameStateRevTracker = GameManager.Instance.Game.stateRevTracker;
        _revertToVabSaveGame = gameStateRevTracker._saveVABGameData;
        _revertToLaunchSaveGame = gameStateRevTracker._saveGameData;
        _lastLaunchedAssemblyName = gameStateRevTracker._lastLaunchedAssemblyName;
        _lastVesselCreatedName = gameStateRevTracker._lastVesselCreatedName;
    }

    public void Restore()
    {
        var gameStateRevTracker = GameManager.Instance.Game.stateRevTracker;
        gameStateRevTracker._saveVABGameData = _revertToVabSaveGame;
        gameStateRevTracker._saveGameData = _revertToLaunchSaveGame;
        gameStateRevTracker._lastLaunchedAssemblyName = _lastLaunchedAssemblyName;
        gameStateRevTracker._lastVesselCreatedName = _lastVesselCreatedName;
    }
}