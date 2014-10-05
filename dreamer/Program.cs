using System;

namespace dreamer
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (frmLogo frm = new frmLogo())
            {
                frm.Show();
                using (Game_Main game = new Game_Main())
                {
                    game.Run();
                }
            }
        }
    }
#endif
}

