using System;

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
            Console.WriteLine( "  pack        - Creates the file manifest and packs content into the .bsp" );
            Environment.Exit( 1 );
         }
      }
   }
}
