using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VillagersUtilities
{
    public class PrefabHandler
    {
        static readonly string _ChickenResourcePrefab = @"Prefabs\ChickenCooked";
        public static STATUS_CODE LoadPrefab(ref GameObject a_prefab, AnimalType a_animalType)
        {
            STATUS_CODE status = STATUS_CODE.IS_GOOD;

            GameObject outObject = null;
            try
            {
                switch (a_animalType)
                {
                    case AnimalType.CHICKEN:
                        {
                            outObject = Resources.Load(_ChickenResourcePrefab, typeof(GameObject)) as GameObject;
                            break;
                        }
                    default:
                        {
                            outObject = null;
                            status = STATUS_CODE.BAD_ANIMAL;
                            break;
                        }
                }
            }
            catch (System.Exception exc)
            {
                Debug.LogError(exc.ToString());
                status = STATUS_CODE.IS_BAD;
            }
            a_prefab = outObject;
            return status;
        }

    }

}

public enum AnimalType
{
    NONE,
    CHICKEN
}
public enum STATUS_CODE
{
    IS_GOOD = 0,
    IS_BAD = 1,
    BAD_ANIMAL = 2
}