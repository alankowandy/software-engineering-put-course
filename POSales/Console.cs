﻿using System;
using System.IO;
using System.Runtime.InteropServices;

public static class ConsoleHelper
{
    public static void CreateConsole()
    {
        AllocConsole();

        // stdout's handle seems to always be equal to 7
        IntPtr defaultStdout = new IntPtr(1);
        IntPtr currentStdout = GetStdHandle(StdOutputHandle);

            SetStdHandle(StdOutputHandle, defaultStdout);

        // reopen stdout
        TextWriter writer = new StreamWriter(Console.OpenStandardOutput())
        { AutoFlush = true };
        Console.SetOut(writer);
    }

    // P/Invoke required:
    private const UInt32 StdOutputHandle = 0xFFFFFFF5;
    [DllImport("kernel32.dll")]
    private static extern IntPtr GetStdHandle(UInt32 nStdHandle);
    [DllImport("kernel32.dll")]
    private static extern void SetStdHandle(UInt32 nStdHandle, IntPtr handle);
    [DllImport("kernel32")]
    static extern bool AllocConsole();
}