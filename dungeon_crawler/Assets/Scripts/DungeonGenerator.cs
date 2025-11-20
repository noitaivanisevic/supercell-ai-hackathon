using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [System.Serializable]
    public class DungeonRoom
    {
        public int x, y;           // Bottom-left corner
        public int width, height;  // Size
        public Vector2Int center => new Vector2Int(x + width/2, y + height/2);
        
        public DungeonRoom(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
        
        public bool Overlaps(DungeonRoom other, int padding = 2)
        {
            return !(x - padding > other.x + other.width + padding ||
                     x + width + padding < other.x - padding ||
                     y - padding > other.y + other.height + padding ||
                     y + height + padding < other.y - padding);
        }
    }
    
    [Header("Generation Settings")]
    public int minRoomSize = 5;
    public int maxRoomSize = 12;
    public int roomCount = 8;
    public int mapWidth = 60;
    public int mapHeight = 60;
    public int maxAttempts = 100;
    
    [Header("Prefabs - DRAG AND DROP HERE")]
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject doorPrefab;
    
    [Header("Gameplay Spawning")]
    public GameObject enemyPrefab;
    public GameObject treasurePrefab;
    public GameObject exitPrefab;
    public GameObject playerObject;  // Drag your player here
    
    [Header("Testing")]
    public bool generateOnStart = true;
    public int startingFloor = 1;
    
    private List<DungeonRoom> rooms = new List<DungeonRoom>();
    private HashSet<Vector2Int> floorTiles = new HashSet<Vector2Int>();
    private GameObject dungeonParent;
    private int currentFloor = 1;
    
    void Start()
    {
        if (generateOnStart)
        {
            GenerateDungeon(startingFloor);
        }
    }
    
    public void GenerateDungeon(int floorNumber)
    {
        currentFloor = floorNumber;
        ClearDungeon();
        
        // Create parent object for organization
        dungeonParent = new GameObject($"Dungeon_Floor_{floorNumber}");
        
        Debug.Log($"=== Generating Floor {floorNumber} ===");
        
        // Step 1: Generate rooms
        GenerateRooms();
        Debug.Log($"Generated {rooms.Count} rooms");
        
        // Step 2: Connect rooms with corridors
        ConnectRooms();
        Debug.Log("Rooms connected");
        
        // Step 3: Create walls around everything
        CreateWalls();
        Debug.Log("Walls created");
        
        // Step 4: Spawn gameplay elements
        SpawnGameplayElements(floorNumber);
        Debug.Log("Gameplay elements spawned");
        
        Debug.Log($"=== Floor {floorNumber} Complete! ===");
    }
    
    // ============================================
    // STEP 1: GENERATE ROOMS
    // ============================================
    void GenerateRooms()
    {
        rooms.Clear();
        
        for (int i = 0; i < roomCount; i++)
        {
            DungeonRoom newRoom = null;
            int attempts = 0;
            
            // Try to place a room that doesn't overlap
            while (attempts < maxAttempts)
            {
                int width = Random.Range(minRoomSize, maxRoomSize);
                int height = Random.Range(minRoomSize, maxRoomSize);
                int x = Random.Range(1, mapWidth - width - 1);
                int y = Random.Range(1, mapHeight - height - 1);
                
                newRoom = new DungeonRoom(x, y, width, height);
                
                // Check if room overlaps with existing rooms
                bool overlaps = false;
                foreach (DungeonRoom existingRoom in rooms)
                {
                    if (newRoom.Overlaps(existingRoom))
                    {
                        overlaps = true;
                        break;
                    }
                }
                
                if (!overlaps)
                {
                    rooms.Add(newRoom);
                    CreateRoomFloor(newRoom);
                    break;
                }
                
                attempts++;
            }
            
            if (attempts >= maxAttempts)
            {
                Debug.LogWarning($"Failed to place room {i} after {maxAttempts} attempts");
            }
        }
    }
    
    void CreateRoomFloor(DungeonRoom room)
    {
        for (int x = room.x; x < room.x + room.width; x++)
        {
            for (int y = room.y; y < room.y + room.height; y++)
            {
                Vector2Int tile = new Vector2Int(x, y);
                if (!floorTiles.Contains(tile))
                {
                    floorTiles.Add(tile);
                    
                    if (floorPrefab != null)
                    {
                        GameObject floorTile = Instantiate(floorPrefab, new Vector3(x, y, 0), Quaternion.identity);
                        floorTile.transform.parent = dungeonParent.transform;
                        floorTile.name = $"Floor_{x}_{y}";
                    }
                }
            }
        }
    }
    
    // ============================================
    // STEP 2: CONNECT ROOMS
    // ============================================
    void ConnectRooms()
    {
        // Connect each room to the next one
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            DungeonRoom roomA = rooms[i];
            DungeonRoom roomB = rooms[i + 1];
            
            CreateCorridor(roomA.center, roomB.center);
        }
        
        // Optionally: Add extra connections for interesting layouts
        if (rooms.Count > 3)
        {
            // Connect first room to a middle room
            CreateCorridor(rooms[0].center, rooms[rooms.Count / 2].center);
        }
    }
    
    void CreateCorridor(Vector2Int start, Vector2Int end)
    {
        // L-shaped corridor: horizontal first, then vertical
        int x = start.x;
        int y = start.y;
        
        // Horizontal corridor
        while (x != end.x)
        {
            CreateFloorTile(x, y);
            x += (x < end.x) ? 1 : -1;
        }
        
        // Vertical corridor
        while (y != end.y)
        {
            CreateFloorTile(x, y);
            y += (y < end.y) ? 1 : -1;
        }
    }
    
    void CreateFloorTile(int x, int y)
    {
        Vector2Int tile = new Vector2Int(x, y);
        if (!floorTiles.Contains(tile))
        {
            floorTiles.Add(tile);
            
            if (floorPrefab != null)
            {
                GameObject floorTile = Instantiate(floorPrefab, new Vector3(x, y, 0), Quaternion.identity);
                floorTile.transform.parent = dungeonParent.transform;
                floorTile.name = $"Corridor_{x}_{y}";
            }
        }
    }
    
    // ============================================
    // STEP 3: CREATE WALLS
    // ============================================
    void CreateWalls()
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        
        // For each floor tile, check surrounding positions
        foreach (Vector2Int floorTile in floorTiles)
        {
            // Check all 8 directions
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    
                    Vector2Int neighbor = new Vector2Int(floorTile.x + dx, floorTile.y + dy);
                    
                    // If neighbor is not a floor tile, it should be a wall
                    if (!floorTiles.Contains(neighbor))
                    {
                        wallPositions.Add(neighbor);
                    }
                }
            }
        }
        
        // Place walls
        foreach (Vector2Int wallPos in wallPositions)
        {
            if (wallPrefab != null)
            {
                GameObject wall = Instantiate(wallPrefab, new Vector3(wallPos.x, wallPos.y, 0), Quaternion.identity);
                wall.transform.parent = dungeonParent.transform;
                wall.name = $"Wall_{wallPos.x}_{wallPos.y}";
            }
        }
    }
    
    // ============================================
    // STEP 4: SPAWN GAMEPLAY ELEMENTS
    // ============================================
    void SpawnGameplayElements(int floorNumber)
    {
        if (rooms.Count == 0)
        {
            Debug.LogWarning("No rooms to spawn gameplay elements in!");
            return;
        }
        
        // Spawn player in first room
        SpawnPlayer(rooms[0]);
        
        // Spawn enemies (more on higher floors)
        int enemyCount = 3 + (floorNumber * 2);
        SpawnEnemies(enemyCount, floorNumber);
        
        // Spawn treasures
        int treasureCount = Mathf.Max(1, rooms.Count / 3);
        SpawnTreasures(treasureCount);
        
        // Spawn exit in last room
        SpawnExit(rooms[rooms.Count - 1]);
    }
    
    void SpawnPlayer(DungeonRoom room)
    {
        Vector3 spawnPos = new Vector3(room.center.x, room.center.y, 0);
        
        if (playerObject != null)
        {
            playerObject.transform.position = spawnPos;
            Debug.Log($"Player spawned at {spawnPos}");
        }
        else
        {
            Debug.LogWarning("Player object not assigned! Drag your player into the Inspector.");
        }
    }
    
    void SpawnEnemies(int count, int floorNumber)
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("Enemy prefab not assigned!");
            return;
        }
        
        // Don't spawn enemies in first room (player spawn)
        List<DungeonRoom> validRooms = new List<DungeonRoom>(rooms);
        if (validRooms.Count > 0) validRooms.RemoveAt(0);
        
        for (int i = 0; i < count && validRooms.Count > 0; i++)
        {
            DungeonRoom room = validRooms[Random.Range(0, validRooms.Count)];
            Vector2Int spawnPos = GetRandomPositionInRoom(room);
            
            GameObject enemy = Instantiate(enemyPrefab, 
                new Vector3(spawnPos.x, spawnPos.y, 0), 
                Quaternion.identity);
            enemy.transform.parent = dungeonParent.transform;
            enemy.name = $"Enemy_{i}";
            
            // Scale difficulty with floor number
            // Note: You'll need to create EnemyController script for this to work
            // For now, just spawning the enemy
        }
    }
    
    void SpawnTreasures(int count)
    {
        if (treasurePrefab == null)
        {
            Debug.LogWarning("Treasure prefab not assigned!");
            return;
        }
        
        for (int i = 0; i < count && rooms.Count > 1; i++)
        {
            // Skip first and last room
            DungeonRoom room = rooms[Random.Range(1, rooms.Count - 1)];
            Vector2Int spawnPos = GetRandomPositionInRoom(room);
            
            GameObject treasure = Instantiate(treasurePrefab, 
                new Vector3(spawnPos.x, spawnPos.y, 0), 
                Quaternion.identity);
            treasure.transform.parent = dungeonParent.transform;
            treasure.name = $"Treasure_{i}";
        }
    }
    
    void SpawnExit(DungeonRoom room)
    {
        if (exitPrefab == null)
        {
            Debug.LogWarning("Exit prefab not assigned!");
            return;
        }
        
        Vector3 exitPos = new Vector3(room.center.x, room.center.y, 0);
        GameObject exit = Instantiate(exitPrefab, exitPos, Quaternion.identity);
        exit.transform.parent = dungeonParent.transform;
        exit.name = "Exit_Stairs";
    }
    
    Vector2Int GetRandomPositionInRoom(DungeonRoom room)
    {
        int x = Random.Range(room.x + 1, room.x + room.width - 1);
        int y = Random.Range(room.y + 1, room.y + room.height - 1);
        return new Vector2Int(x, y);
    }
    
    // ============================================
    // UTILITY
    // ============================================
    void ClearDungeon()
    {
        rooms.Clear();
        floorTiles.Clear();
        
        // Destroy previous dungeon
        if (dungeonParent != null)
        {
            Destroy(dungeonParent);
        }
    }
    
    // ============================================
    // EDITOR VISUALIZATION (Gizmos)
    // ============================================
    void OnDrawGizmos()
    {
        if (rooms == null || rooms.Count == 0) return;
        
        // Draw room outlines in Scene view
        Gizmos.color = Color.green;
        foreach (DungeonRoom room in rooms)
        {
            Vector3 center = new Vector3(room.center.x, room.center.y, 0);
            Vector3 size = new Vector3(room.width, room.height, 0);
            Gizmos.DrawWireCube(center, size);
        }
        
        // Draw connections
        Gizmos.color = Color.yellow;
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            Vector3 start = new Vector3(rooms[i].center.x, rooms[i].center.y, 0);
            Vector3 end = new Vector3(rooms[i + 1].center.x, rooms[i + 1].center.y, 0);
            Gizmos.DrawLine(start, end);
        }
    }
}