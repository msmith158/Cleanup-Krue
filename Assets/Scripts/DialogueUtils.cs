using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogueUtils
{
    private static string savedTag = "";
    
    
    public static void ProcessInlineTag(string tag)
    {
        if (savedTag != null) savedTag = "";
        savedTag = tag;
    }
}
