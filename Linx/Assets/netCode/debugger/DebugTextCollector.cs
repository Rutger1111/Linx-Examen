using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTextCollector
{
    private static DebugTextCollector debugTextCollector;
    public List<string> texts = new List<string>();
    private DebugTextCollector(){}
    public static DebugTextCollector GetTextCollector(){
        if (DebugTextCollector.debugTextCollector == null){
            DebugTextCollector.debugTextCollector = new DebugTextCollector();
        }
        return DebugTextCollector.debugTextCollector;
    }
    public void AddDebugText(string textToAdd){
        texts.Add(textToAdd);
    }
}
    