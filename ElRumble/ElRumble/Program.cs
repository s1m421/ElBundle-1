﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace ElRumble
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Rumble.OnLoad;
        }
    }
}
