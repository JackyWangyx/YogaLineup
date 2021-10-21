using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStoreSetting<out TStoreData>
    where TStoreData : StoreData
{
    string SaveKey { get; }
    TStoreData CurrentSelectData { get; }
    TStoreData CurrentUnlockData { get; }

    int SelectIndex { get; set; }
    int UnlockIndex { get; set; }
    int UnlockCount { get; set; }
    int UnlockProgress { get; set; }

    void PreNextUnlock();
}