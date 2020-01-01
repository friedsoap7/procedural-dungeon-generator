public class Room {

    public int[] bounds; //NESW
    public int width, height;
    public bool openNorth, openEast, openSouth, openWest;
    public double expansionChance;
    public bool isCorridor;

    public const int MIN_SIZE = 5;
    public const int SIZE_RANGE = 15;

    public Room(bool openNorth, bool openEast, bool openSouth,
            bool openWest, int[] bounds) {
        this.openNorth = openNorth;
        this.openEast = openEast;
        this.openWest = openWest;
        this.openSouth = openSouth;
        this.bounds = bounds;
        width = bounds[1] - bounds[3];
        height = bounds[2] - bounds[0];
        expansionChance = 0.5;
    }

    public Room(int[] bounds) : this (false, false, false, false, bounds) {}

    public bool IsLegal(Tile[,] map) {
        if(bounds[0] < 0 || bounds[2] >= map.GetLength(0)) return false;
        if(bounds[3] < 0 || bounds[1] >= map.GetLength(1)) return false;
        for(int y = bounds[0]; y <= bounds[2]; y++) {
            for(int x = bounds[3]; x <= bounds[1]; x++) {
                if(map[y,x] == Tile.FLOOR) return false;
            }
        }
        return true;
    }

    public void PlaceTiles(Tile[,] map) {
        for(int x = bounds[3]; x <= bounds[1]; x++) {
            map[bounds[0],x] = Tile.WALL;
            map[bounds[2],x] = Tile.WALL;
        }
        for(int y = bounds[0]; y <= bounds[2]; y++) {
            map[y,bounds[3]] = Tile.WALL;
            map[y,bounds[1]] = Tile.WALL;
        }
        for(int y = bounds[0] + 1; y < bounds[2]; y++) {
            for(int x = bounds[3] + 1; x < bounds[1]; x++) {
                map[y,x] = Tile.FLOOR;
            }
        }
    }
}