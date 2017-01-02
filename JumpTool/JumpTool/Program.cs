using System;
using System.Diagnostics;
using System.IO;

namespace JumpTool
{
   internal static class Program
   {
      private static void Main( string[] args )
      {
         if ( args.Length == 0 )
         {
            Console.WriteLine( "Syntax: jump <command> <arguments>" );
            Console.WriteLine();
            Console.WriteLine( "Commands:" );
            Console.WriteLine( "  build       - Compiles the map" );
            Console.WriteLine( "  pack        - Creates the file manifest and packs content into the .bsp" );
            Environment.Exit( 1 );
         }

         if ( args[0].ToLower() == "pack" )
         {
            Pack();
         }
         else
         {
            Console.WriteLine( $"Unknown command: {args[0]}" );
            Environment.Exit( 1 );
         }

         Environment.Exit( 1 );
      }

      private static void Pack()
      {
         //string currentDirectory = Environment.CurrentDirectory;
         string currentDirectory = @"C:\TF2\Maps\jump_lotus";

         string mapName = Path.GetFileName( currentDirectory );
         Console.WriteLine( $"Detected map name: {mapName}" );

         // Get content

         string contentDirectory = Path.Combine( currentDirectory, "Content" );

         if ( !Directory.Exists( contentDirectory ) )
         {
            Console.WriteLine( "Content directory not found" );
            Environment.Exit( 1 );
         }

         // Grok all content files

         var allFiles = Directory.GetFiles( contentDirectory, "*", SearchOption.AllDirectories );

         // Write a manifest

         string fileList = Path.Combine( currentDirectory, mapName + "_files.txt" );

         using ( var streamWriter = new StreamWriter( fileList ) )
         {
            foreach ( string file in allFiles )
            {
               string bspFile = file.Replace( contentDirectory, string.Empty ).Substring( 1 );

               streamWriter.WriteLine( bspFile );
               streamWriter.WriteLine( file );
            }
         }

         // Pack

         const string bspZipPath = @"C:\Program Files (x86)\Steam\steamapps\common\team fortress 2\bin\bspzip.exe";
         string unpackedBspPath = Path.Combine( currentDirectory, mapName + ".bsp" );
         string packedBspPath = Path.Combine( currentDirectory, "Output", mapName + ".bsp" );

         var process = Process.Start( bspZipPath, $"-addorupdatelist {unpackedBspPath} {fileList} {packedBspPath}" );
         process.WaitForExit();

         // Delete the manifest

         File.Delete( fileList );
      }
   }
}
