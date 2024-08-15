using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Saveable : MonoBehaviour
{
    /// <summary>
    /// Dispose is called before any loading.
    /// Allowing asset cleanup and managment before loading. 
    /// </summary>
    public abstract void Dispose();
    /// <summary>
    /// Saves the current data
    /// </summary>
    /// <returns></returns>
    public abstract void SaveState(ref SaveSystem.DataContainer savedata);
    /// <summary>
    /// Applies the saved data
    /// </summary>
    /// <param name="gameState"></param>
    public abstract void LoadState(SaveableTypeBase gameState);
    /// <summary>
    /// Called when all Load states have completed.
    /// Used for establishing reference to other objects in data
    /// </summary>
    public abstract void PostLoadState();


}
