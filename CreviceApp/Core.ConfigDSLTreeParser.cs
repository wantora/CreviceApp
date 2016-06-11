﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreviceApp.Core
{
    public static class ConfigDSLTreeParser
    {
        public static IEnumerable<GestureDefinition> TreeToGestureDefinition(DSL.Root root)
        {
            List<GestureDefinition> gestureDef = new List<GestureDefinition>();
            Debug.Print("Parsing tree of GestureConfig.DSL");
            foreach (var whenElement in root.whenElements)
            {
                if (whenElement.onElements.Count == 0)
                {
                    gestureDef.Add(new GestureDefinition(whenElement.func, null));
                    continue;
                }
                foreach (var onElement in whenElement.onElements)
                {
                    if (onElement.ifButtonElements.Count == 0 && onElement.ifStrokeElements.Count == 0)
                    {
                        gestureDef.Add(new GestureDefinition(whenElement.func, onElement.button));
                        continue;
                    }
                    foreach (var ifButtonElement in onElement.ifButtonElements)
                    {
                        if (ifButtonElement.doElements.Count == 0)
                        {
                            gestureDef.Add(new ButtonGestureDefinition(whenElement.func, onElement.button, ifButtonElement.button, null));
                            continue;
                        }
                        foreach (var doElement in ifButtonElement.doElements)
                        {
                            gestureDef.Add(new ButtonGestureDefinition(whenElement.func, onElement.button, ifButtonElement.button, Helper.Convert(doElement.func)));
                        }
                    }
                    foreach (var ifStrokeElement in onElement.ifStrokeElements)
                    {
                        var stroke = Helper.Convert(ifStrokeElement.moves);
                        if (ifStrokeElement.doElements.Count == 0)
                        {
                            gestureDef.Add(new StrokeGestureDefinition(whenElement.func, onElement.button, stroke, null));
                            continue;
                        }
                        foreach (var doElement in ifStrokeElement.doElements)
                        {
                            gestureDef.Add(new StrokeGestureDefinition(whenElement.func, onElement.button, stroke, Helper.Convert(doElement.func)));
                        }
                    }
                }
            }
            Debug.Print("Parse end.");
            return gestureDef; 
        }
                
        public static IEnumerable<GestureDefinition> FilterComplete(IEnumerable<GestureDefinition> gestureDef)
        {
            return gestureDef
                .Where(x => x.IsComplete)
                .ToList();
        }
    }
}
