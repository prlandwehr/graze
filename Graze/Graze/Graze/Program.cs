using System;

namespace Graze
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //process command line options
            if (args.Length >= 1)
            {
                for (int index = 0; index < args.Length; index++)
                {
                    if (args[index] == "-lerp")
                    {
                        using (GRGame game = new GRGame(true))
                        {
                            game.Run();
                        }
                    }
                }
            }
            //otherwise run default
            else
            {
                using (GRGame game = new GRGame())
                {
                    game.Run();
                }
            }
        }
    }
#endif
}

