using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Territory : MonoBehaviour
{
    [SerializeField] Transform window; // Fereastra de cumpărare
    [SerializeField] int crystalsAmount; // Costul pentru a debloca teritoriu
    [SerializeField] private Tilemap territoryTilemap; // Tilemap-ul unde se află teritoriul
    [SerializeField] private TileBase territoryTile; // Tile-ul care marchează teritoriul ocupat (după cumpărare)

    private bool isBlocked = true; // Verifică dacă teritoriul este blocat
    private BoundsInt area; // Zona calculată automat

    void Start()
    {
        window.gameObject.SetActive(false); // Ascunde fereastra la început
        CalculateArea(); // Calculează automat zona `area`
        isBlocked = true;
    }

    private void CalculateArea()
    {
        //Debug.Log("Calculăm zona...");

        BoundsInt bounds = territoryTilemap.cellBounds;
        List<Vector3Int> occupiedTilePositions = new List<Vector3Int>();

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                TileBase tile = territoryTilemap.GetTile(tilePosition);

                if (tile != null)
                {
                    occupiedTilePositions.Add(tilePosition);
                }
            }
        }

        if (occupiedTilePositions.Count > 0)
        {
            Vector3Int minPosition = new Vector3Int(
                occupiedTilePositions.Min(pos => pos.x),
                occupiedTilePositions.Min(pos => pos.y),
                0
            );

            Vector3Int maxPosition = new Vector3Int(
                occupiedTilePositions.Max(pos => pos.x),
                occupiedTilePositions.Max(pos => pos.y),
                0
            );

            int width = maxPosition.x - minPosition.x + 1;
            int height = maxPosition.y - minPosition.y + 1;

            area = new BoundsInt(minPosition.x, minPosition.y, 0, width, height, 1);
            //Debug.Log($"Area calculată: Position: {area.position}, Size: {area.size}");
        }
        else
        {
            Debug.LogError("Nu s-au găsit tile-uri ocupate.");
            area = new BoundsInt(0, 0, 0, 0, 0, 0); // Setează o zonă invalidă pentru a evita utilizarea sa mai departe
        }
    }


    private void OnMouseDown() // Triggered when the territory is clicked
    {

        if (isBlocked)
        {

            ShowWindow(); // Afișează fereastra pentru cumpărare
        }
    }

    private void ShowWindow()
    {
        window.gameObject.SetActive(true); // Afișează fereastra
        window.Find("Amount Text").GetComponent<TextMeshProUGUI>().text = crystalsAmount.ToString(); // Setează textul costului
        PanZoom.current.Focus(transform.position);

        window.Find("Close Button").GetComponent<Button>().onClick.RemoveAllListeners();
        window.Find("Close Button").GetComponent<Button>().onClick.AddListener(delegate
        {
            window.gameObject.SetActive(false); // Închide fereastra
        });

        window.Find("Buy Button").GetComponent<Button>().onClick.RemoveAllListeners();
        window.Find("Buy Button").GetComponent<Button>().onClick.AddListener(delegate
        {
            EventManager.Instance.AddListenerOnce<EnoughCurrencyGameEvent>(OnEnoughCurrency);
            EventManager.Instance.AddListenerOnce<NotEnoughCurrencyGameEvent>(OnNotEnoughCurrency);

            CurrencyChangeGameEvent info = new CurrencyChangeGameEvent(-crystalsAmount, CurrencyType.Crystals);
            EventManager.Instance.QueueEvent(info);

            window.gameObject.SetActive(false);
        });
    }

    private void UnlockTerritory()
    {
        // Folosim BuildingSystem pentru a debloca zona pe `territoryTilemap`
        BuildingSystem.current.UnlockTerritory(area, territoryTile, territoryTilemap);
        isBlocked = false; // Setează teritoriul ca deblocat
    }

    private void OnEnoughCurrency(EnoughCurrencyGameEvent info)
    {
        Debug.LogError("Are bani , a cumparat");
        UnlockTerritory(); // Deblochează teritoriul dacă sunt suficiente resurse
        EventManager.Instance.RemoveListener<NotEnoughCurrencyGameEvent>(OnNotEnoughCurrency);
    }

    private void OnNotEnoughCurrency(NotEnoughCurrencyGameEvent info)
    {
        EventManager.Instance.RemoveListener<EnoughCurrencyGameEvent>(OnEnoughCurrency);
    }

}
