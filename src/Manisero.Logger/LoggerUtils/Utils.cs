using System;
using System.Diagnostics;

namespace Manisero.Logger.LoggerUtils
{
    internal static class Utils
    {
        public static Type GetCallingType()
        {
            var frame = new StackFrame(2);
            return frame.GetMethod().DeclaringType;
        }

        public static bool IsConsoleAvailable()
        {
            try
            {
                var height = Console.WindowHeight;
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}
