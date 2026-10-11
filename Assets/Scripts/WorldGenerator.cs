using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    public int width = 100;
    public int height = 100;
    private float[,] landmassNoise;
    public float noiseScale = 20f;

    private float[,] temperatureNoise;
    public float temperatureNoiseOffset = 6767f;

    private float[,] humidityNoise;
    public float humidityNoiseOffset = 676.67f;

    public Tilemap tilemap;
    public TileBase water;

    public TileBase[] polar;                  
    public TileBase[] tundra;                 
    public TileBase[] taiga;                  
    public TileBase[] stepe;                  
    public TileBase[] vidMiskas;              
    public TileBase[] vidLietingasisMiskas;   
    public TileBase[] karstojiDykuma;         
    public TileBase[] savanna;                
    public TileBase[] atograzuMiskas;         

    void Start()
    {
        GenerateLandmass();
        GenerateTemperature();
        GenerateHumidity();

        GenerateBiomes();
    }

    void GenerateLandmass()
    {
        landmassNoise = new float[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float noiseX = i / noiseScale;
                float noiseY = j / noiseScale;
                float2 noisePosition = new float2(noiseX, noiseY);

                float landnoise = (noise.snoise(noisePosition) + 1) / 2;
                landmassNoise[i, j] = landnoise;
            }
        }
    }

    void GenerateTemperature()
    {
        temperatureNoise = new float[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float noiseX = i / noiseScale + temperatureNoiseOffset;
                float noiseY = j / noiseScale + temperatureNoiseOffset;
                float2 noisePosition = new float2(noiseX, noiseY);

                float tempnoise = (noise.snoise(noisePosition) + 1) / 2;
                temperatureNoise[i, j] = tempnoise;
            }
        }
    }

    void GenerateHumidity()
    {
        humidityNoise = new float[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float noiseX = i / noiseScale + humidityNoiseOffset;
                float noiseY = j / noiseScale + humidityNoiseOffset;
                float2 noisePosition = new float2(noiseX, noiseY);

                float humidnoise = (noise.snoise(noisePosition) + 1) / 2;
                humidityNoise[i, j] = humidnoise;
            }
        }
    }

    void GenerateBiomes()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float land = landmassNoise[i, j];
                float temperature = temperatureNoise[i, j];
                float humidity = humidityNoise[i, j];
                Vector3Int position = new Vector3Int(i, j, 0);

                if (land < 0.1f)
                {
                    tilemap.SetTile(position, water);
                }
                else
                {
                    if (temperature < 0.33f)
                    {
                        if (humidity < 0.33f) tilemap.SetTile(position, polar[UnityEngine.Random.Range(0, polar.Length)]);
                        else if (humidity < 0.66f) tilemap.SetTile(position, tundra[UnityEngine.Random.Range(0, tundra.Length)]);
                        else tilemap.SetTile(position, taiga[UnityEngine.Random.Range(0, taiga.Length)]);
                    }
                    else if (temperature < 0.66f)
                    {
                        if (humidity < 0.33f) tilemap.SetTile(position, stepe[UnityEngine.Random.Range(0, stepe.Length)]);
                        else if (humidity < 0.66f) tilemap.SetTile(position, vidMiskas[UnityEngine.Random.Range(0, vidMiskas.Length)]);
                        else tilemap.SetTile(position, vidLietingasisMiskas[UnityEngine.Random.Range(0, vidLietingasisMiskas.Length)]);
                    }
                    else
                    {
                        if (humidity < 0.33f) tilemap.SetTile(position, karstojiDykuma[UnityEngine.Random.Range(0, karstojiDykuma.Length)]);
                        else if (humidity < 0.66f) tilemap.SetTile(position, savanna[UnityEngine.Random.Range(0, savanna.Length)]);
                        else tilemap.SetTile(position, atograzuMiskas[UnityEngine.Random.Range(0, atograzuMiskas.Length)]);
                    }
                }
            }
        }
    }
}