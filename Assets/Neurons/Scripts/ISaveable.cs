
public interface ISaveable
{
    /// <summary>
    /// Dispose is called before any loading.
    /// Allowing asset cleanup and managment before loading. 
    /// </summary>
    void Dispose();

    /// <summary>
    /// Saves the current data
    /// </summary>
    /// <returns></returns>
    void SaveState(ref SaveSystem.SaveData savedata);

    /// <summary>
    /// Applies the saved data
    /// </summary>
    /// <param name="gameState"></param>
    void LoadState(SaveSystem.SaveData gameState, int index);

    /// <summary>
    /// Called when all Load states have completed.
    /// Used for establishing reference to other objects in data
    /// </summary>
    void PostLoadState();

    /// <summary>
    /// The game object this ISaveable 
    /// </summary>
    /// <returns></returns>   
}