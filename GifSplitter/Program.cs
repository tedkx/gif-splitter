using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace GifSplitter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 0)
                ExitWithMessage("No gif file supplied");
            if(!File.Exists(args[0]))
                ExitWithMessage("File not found");

            // Load image
            Image image = null;
            try
            {
                image = Image.FromFile(args[0]);
            }
            catch(Exception ex)
            {
                ExitWithMessage(string.Format("Could not load image file: {0}", ex.Message), 2);
            }

            // Split and save frames
            string directory = args[0].Substring(0, args[0].Length - 4);
            try { Directory.CreateDirectory(directory); }
            catch (Exception ex) { ExitWithMessage(string.Format("Could not create directory {0}: {1}", directory, ex.Message), 3); }
            Image[] frames = null;

            try
            {
                int numFrames = image.GetFrameCount(FrameDimension.Time);
                frames = new Image[numFrames];

                for (int i = 0; i < numFrames; i++)
                {
                    image.SelectActiveFrame(FrameDimension.Time, i);
                    image.Save(Path.Combine(directory, string.Format("frame_{0}.jpg", i)), ImageFormat.Jpeg);
                }
            } 
            catch (Exception ex)
            {
                ExitWithMessage(string.Format("Could not save frames: {0}", ex.Message), 4);
            }

            Console.WriteLine("Saved in directory " + directory);
            Console.ReadLine();
        }

        static void ExitWithMessage(string message, int exitCode = 1)
        {
            Console.WriteLine(message);
            Console.ReadLine();
            Environment.Exit(exitCode);
        }
    }
}
