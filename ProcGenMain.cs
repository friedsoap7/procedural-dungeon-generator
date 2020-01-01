using System;

/**
 * A proof of concept for procedural map generation (Roguelike dungeon generation)
 * Colin Lemarchand
 * 25 December 2019
 */

public class ProcGenMain {
    public static void Main(string[] args) {
       Map map = new Map(100, 100, 3);
       Tile[,] tiles = map.GetTiles();
       for(int y = 0; y < tiles.GetLength(0); y++) {
           for(int x = 0; x < tiles.GetLength(1); x++) {
               switch(tiles[y,x]) {
                   case Tile.WALL:
                   Console.Write("X");
                   break;
                   case Tile.FLOOR:
                   Console.Write(".");
                   break;
                   case Tile.VOID:
                   Console.Write(" ");
                   break;
                   case Tile.SPAWN:
                   Console.Write("S");
                   break;
                   case Tile.END:
                   Console.Write("E");
                   break;
                   default:
                   Console.Write("@");
                   break;
               }
               Console.Write(" ");
           }
           Console.WriteLine();
       } 
    }
}
