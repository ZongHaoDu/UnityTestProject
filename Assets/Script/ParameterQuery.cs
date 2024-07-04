using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterQuery : MonoBehaviour
{

    public static int QueryParameters(GameObject target, string parameterName)
    {
        int tagValue;
        bool isNumericTag = int.TryParse(target.tag, out tagValue);

        if (!isNumericTag)
        {
            Debug.LogError("Tag is not numeric: " + target.tag);
            return 0;
        }

        if (parameterName == "attack")
        {
            switch (tagValue)
            {
                case 0: 
                    return 10;
                case 1:
                    return 20;
                default:
                    return 0;
            }
        }

        if (parameterName == "blood")
        {
            switch (tagValue)
            {
                case 0: 
                    return 100;
                case 1:
                    return 200;
                default:
                    return 0;
            }
        }
        if (parameterName == "time")
        {
            switch (tagValue)
            {
                case 0: 
                    return 10;
                case 1:
                    return 20;
                default:
                    return 0;
            }
        }
        return 0; 
    }
}
