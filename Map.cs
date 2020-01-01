using System;
using System.Collections.Generic;
public class Map {
    private Tile[,] map;
    private List<Room> rooms;
    private Room startRoom;
    private Room endRoom;
    private Random random = new Random();

    /**
     * Creates a Map object of width sizeX and height sizeY.
     * @param sizeX the map's width in Tiles
     * @param sizeY the map's height in Tiles
     * @param epochs the number of times to run the generation algorithm
     */
    public Map(int sizeX, int sizeY, int epochs) {
        map = new Tile[sizeY, sizeX];
        for(int y = 0; y < sizeY; y++) {
            for(int x = 0; x < sizeX; x++) {
                map[y,x] = Tile.VOID;
            }
        }

        rooms = new List<Room>();

        generate(epochs);
    }

    private void generateStartRoom() {
        int startRoomWidth = generateRoomSize();
        int startRoomHeight = generateRoomSize();
        int startRoomX;
        int startRoomY;
        bool IsLegal = false;

        do {
            startRoomX = random.Next(map.GetLength(1));
            startRoomY = random.Next(map.GetLength(0));
            if(startRoomX + startRoomWidth > map.GetLength(1)) {
                continue;
            }
            if(startRoomY + startRoomHeight > map.GetLength(0)) {
                continue;
            }
            IsLegal = true;
        } while(!IsLegal);

        int[] startRoomBounds = new int[4];
        startRoomBounds[0] = startRoomY;
        startRoomBounds[1] = startRoomX + startRoomWidth - 1;
        startRoomBounds[2] = startRoomY + startRoomHeight - 1;
        startRoomBounds[3] = startRoomX;

        startRoom = new Room(startRoomBounds);
        rooms.Add(startRoom);
        startRoom.PlaceTiles(map);
    }

    private void generate(int epochs) {
        generateStartRoom();
        for(int i = 0; i < epochs; i++) {
            for(int j = 0; j < rooms.Count; j++) {
                Room room = rooms[j];
                considerExpansionNorth(room);
                considerExpansionEast(room);
                considerExpansionSouth(room);
                considerExpansionWest(room);
            }
        }
    }

    private bool considerExpansionNorth(Room room) {
        if(room.openNorth) return false;
        if(random.NextDouble() > room.expansionChance) return false;

        int expansionRoomWidth = generateRoomSize();
        int expansionRoomHeight = generateRoomSize();

        int doorX = random.Next(room.width - 2) + 1;
        int expansionDoorX = random.Next(expansionRoomWidth - 2) + 1;

        doorX += room.bounds[3];

        int expansionRoomX = doorX - expansionDoorX;
        int expansionRoomY = room.bounds[0] - expansionRoomHeight + 1;

        int[] expansionRoomBounds = new int[4];
        expansionRoomBounds[0] = expansionRoomY;
        expansionRoomBounds[1] = expansionRoomX + expansionRoomWidth - 1;
        expansionRoomBounds[2] = expansionRoomY + expansionRoomHeight - 1;
        expansionRoomBounds[3] = expansionRoomX;

        Room expansionRoom = new Room(false, false, true, false, expansionRoomBounds);
        if(expansionRoom.IsLegal(map)) {
            rooms.Add(expansionRoom);
            expansionRoom.PlaceTiles(map);
            room.openNorth = true;
            map[room.bounds[0],doorX] = Tile.FLOOR;
            return true;
        }
        return false;
    }

    private bool considerExpansionSouth(Room room) {
        if(room.openSouth) return false;
        if(random.NextDouble() > room.expansionChance) return false;

        int expansionRoomWidth = generateRoomSize();
        int expansionRoomHeight = generateRoomSize();

        int doorX = random.Next(room.width - 2) + 1;
        int expansionDoorX = random.Next(expansionRoomWidth - 2) + 1;

        doorX += room.bounds[3];

        int expansionRoomX = doorX - expansionDoorX;
        int expansionRoomY = room.bounds[2];

        int[] expansionRoomBounds = new int[4];
        expansionRoomBounds[0] = expansionRoomY;
        expansionRoomBounds[1] = expansionRoomX + expansionRoomWidth - 1;
        expansionRoomBounds[2] = expansionRoomY + expansionRoomHeight - 1;
        expansionRoomBounds[3] = expansionRoomX;

        Room expansionRoom = new Room(true, false, false, false, expansionRoomBounds);
        if(expansionRoom.IsLegal(map)) {
            rooms.Add(expansionRoom);
            expansionRoom.PlaceTiles(map);
            room.openSouth = true;
            map[room.bounds[2],doorX] = Tile.FLOOR;
            return true;
        }
        return false;
    }

    private bool considerExpansionEast(Room room) {
        if(room.openEast) return false;
        if(random.NextDouble() > room.expansionChance) return false;

        int expansionRoomWidth = generateRoomSize();
        int expansionRoomHeight = generateRoomSize();

        int doorY = random.Next(room.height - 2) + 1;
        int expansionDoorY = random.Next(expansionRoomHeight - 2) + 1;

        doorY += room.bounds[0];

        int expansionRoomX = room.bounds[1];
        int expansionRoomY = doorY - expansionDoorY;

        int[] expansionRoomBounds = new int[4];
        expansionRoomBounds[0] = expansionRoomY;
        expansionRoomBounds[1] = expansionRoomX + expansionRoomWidth - 1;
        expansionRoomBounds[2] = expansionRoomY + expansionRoomHeight - 1;
        expansionRoomBounds[3] = expansionRoomX;

        Room expansionRoom = new Room(false, false, false, true, expansionRoomBounds);
        if(expansionRoom.IsLegal(map)) {
            rooms.Add(expansionRoom);
            expansionRoom.PlaceTiles(map);
            room.openEast = true;
            map[doorY,room.bounds[1]] = Tile.FLOOR;
            return true;
        }
        return false;
    }

    private bool considerExpansionWest(Room room) {
        if(room.openWest) return false;
        if(random.NextDouble() > room.expansionChance) return false;

        int expansionRoomWidth = generateRoomSize();
        int expansionRoomHeight = generateRoomSize();

        int doorY = random.Next(room.height - 2) + 1;
        int expansionDoorY = random.Next(expansionRoomHeight - 2) + 1;

        doorY += room.bounds[0];

        int expansionRoomX = room.bounds[3] - expansionRoomWidth + 1;
        int expansionRoomY = doorY - expansionDoorY;

        int[] expansionRoomBounds = new int[4];
        expansionRoomBounds[0] = expansionRoomY;
        expansionRoomBounds[1] = expansionRoomX + expansionRoomWidth - 1;
        expansionRoomBounds[2] = expansionRoomY + expansionRoomHeight - 1;
        expansionRoomBounds[3] = expansionRoomX;

        Room expansionRoom = new Room(false, true, false, false, expansionRoomBounds);
        if(expansionRoom.IsLegal(map)) {
            rooms.Add(expansionRoom);
            expansionRoom.PlaceTiles(map);
            room.openWest = true;
            map[doorY,room.bounds[3]] = Tile.FLOOR;
            return true;
        }
        return false;
    }

    private int generateRoomSize() {
        return random.Next(Room.SIZE_RANGE) + Room.MIN_SIZE;
    }

    public Tile[,] GetTiles() {
        return map;
    }

}
