using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Brush : Singleton<Brush>, IPointerClickHandler, IDragHandler
{
    public enum ToolType
    {
        Pen = 0,
        Bucket = 1,    
        Eraser = 2,
        Stamp = 3,
        Color = 4,
        BrushSize = 5,
        PaintBall = 6,
    }

    [Header("general")]
    public ToolType toolType;
    public Texture2D texture; 
    private Image imageComponent;
    public Color backGroundColor;
    public GameObject stampParticle;
   [Header("Properties")]
    public float brushSize; 
    public Color color;

   

    private void Start()
    {
    
        imageComponent = GetComponent<Image>();

       
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

      
        imageComponent.sprite = sprite;

        GUIManager.Instance.SetColorButton();

        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

     
        EventTrigger.Entry entry = new EventTrigger.Entry();

        entry.eventID = EventTriggerType.PointerClick;

        entry.callback.AddListener((data) => { OnPointerClick((PointerEventData)data); });

        trigger.triggers.Add(entry);

    }

    //Kayıtlı Texture yoksa CreateTexture() çağır
    public void CreateTexture()
    {
        backGroundColor = Color.white;

        int screenWidth = Screen.width;

        int screenHeight = Screen.height;

        texture = new Texture2D(screenWidth, screenHeight); 
        texture.filterMode = FilterMode.Point; 
        texture.wrapMode = TextureWrapMode.Clamp;

       
        Color[] defaultColors = new Color[texture.width * texture.height];

        for (int i = 0; i < defaultColors.Length; i++)
        {
            defaultColors[i] = Color.white;
        }
        texture.SetPixels(defaultColors);

   
        texture.Apply();

    }
    //Bucket için renk duyarlı Fonksiyon
    private void FillTexture(int startX, int startY, Color fillColor)
    {
        Color[] pixels = texture.GetPixels();
        int width = texture.width;
        int height = texture.height;
        Color targetColor = pixels[startY * width + startX];

        if (targetColor == fillColor) return;

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(new Vector2Int(startX, startY));

        while (queue.Count > 0)
        {
            Vector2Int point = queue.Dequeue();
            int x = point.x;
            int y = point.y;

            if (pixels[y * width + x] != targetColor) continue;

            pixels[y * width + x] = fillColor;

          
            if (x > 0) queue.Enqueue(new Vector2Int(x - 1, y));
            if (x < width - 1) queue.Enqueue(new Vector2Int(x + 1, y));
            if (y > 0) queue.Enqueue(new Vector2Int(x, y - 1));
            if (y < height - 1) queue.Enqueue(new Vector2Int(x, y + 1));
        }
        backGroundColor = color;
        texture.SetPixels(pixels);
    }

    public void OnPointerClick(PointerEventData eventData)
    {     
        Vector2 touchPosition = eventData.position;

        UpdateProcess(touchPosition);      
    }
    public void OnDrag(PointerEventData eventData)
    {
       
        Vector2 touchPosition = eventData.position;

        DragProcess(touchPosition, eventData.delta);
    }

    private void UpdateProcess(Vector2 texturePosition)
    {
        if (toolType == ToolType.Pen )
        {
         
          
            for (int x = (int)texturePosition.x - (int)brushSize / 2; x <= (int)texturePosition.x + (int)brushSize / 2; x++)
            {
                for (int y = (int)texturePosition.y - (int)brushSize / 2; y <= (int)texturePosition.y + (int)brushSize / 2; y++)
                {
                    if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
                    {
                        texture.SetPixel(x, y, color);
                    }
                }
            }

            
            texture.Apply();
        }
        else if (toolType == ToolType.Bucket) 
        {

            AudioManager.Instance.PlayBucketSound();
            
            Color[] pixels = texture.GetPixels();

            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            texture.SetPixels(pixels);
            backGroundColor = color;
            texture.Apply();


        }
        else if (toolType == ToolType.Stamp)
        {
            AudioManager.Instance.PlayStampSound();
                  
            Vector2 centerPosition = texturePosition;

            
         
         


            for (int x = (int)centerPosition.x - (int)brushSize / 2; x <= (int)centerPosition.x + (int)brushSize / 2; x++)
            {
                for (int y = (int)centerPosition.y - (int)brushSize / 2; y <= (int)centerPosition.y + (int)brushSize / 2; y++)
                {
                   
                    if (Vector2.Distance(new Vector2(x, y), centerPosition) <= brushSize / 2)
                    {
                        if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
                        {
                            texture.SetPixel(x, y, color);
                        }
                    }
                }
            }

            Instantiate(stampParticle, texturePosition, stampParticle.gameObject.transform.rotation);
            texture.Apply();
        }
        else if (toolType == ToolType.Eraser)
        {
          
            for (int x = (int)texturePosition.x - (int)brushSize / 2; x <= (int)texturePosition.x + (int)brushSize / 2; x++)
            {
                for (int y = (int)texturePosition.y - (int)brushSize / 2; y <= (int)texturePosition.y + (int)brushSize / 2; y++)
                {
                    if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
                    {
                        texture.SetPixel(x, y, backGroundColor);
                    }
                }
            }

        
            texture.Apply();
        }
        else if (toolType == ToolType.BrushSize)
        {
            
            GUIManager.Instance.ChangeToolButton(GUIManager.Instance.lastChooseIndx);

        }
        else if (toolType == ToolType.PaintBall)
        {
         
            DrawPaintBall(texturePosition);
        }
    }

    private void DragProcess(Vector2 texturePosition, Vector2 dragDelta)
    {
        if (toolType == ToolType.Pen)
        {
          
            for (int x = (int)texturePosition.x - (int)brushSize / 2; x <= (int)texturePosition.x + (int)brushSize / 2; x++)
            {
                for (int y = (int)texturePosition.y - (int)brushSize / 2; y <= (int)texturePosition.y + (int)brushSize / 2; y++)
                {
                    if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
                    {
                        texture.SetPixel(x, y, color);
                    }
                }
            }

        
            texture.Apply();
        }
        
        else if (toolType == ToolType.Eraser)
        {
       
            for (int x = (int)texturePosition.x - (int)brushSize / 2; x <= (int)texturePosition.x + (int)brushSize / 2; x++)
            {
                for (int y = (int)texturePosition.y - (int)brushSize / 2; y <= (int)texturePosition.y + (int)brushSize / 2; y++)
                {
                    if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
                    {
                        texture.SetPixel(x, y, backGroundColor);
                    }
                }
            }

            texture.Apply();
        }
        else if (toolType == ToolType.BrushSize)
        {

            GUIManager.Instance.ChangeToolButton(GUIManager.Instance.lastChooseIndx);

        }
    }

    private void DrawPaintBall(Vector2 centerPosition)
    {
        AudioManager.Instance.PlayStampSound();

       
        DrawCircle(centerPosition, brushSize / 2, color);

       
        int numSmallCircles = Random.Range(3, 5);
        float minRadius = brushSize / 5;
        float maxRadius = brushSize / 8;

        for (int i = 0; i < numSmallCircles; i++)
        {
            Vector2 randomPoint = centerPosition + Random.insideUnitCircle * (brushSize );
            float randomRadius = Random.Range(minRadius, maxRadius);
            DrawCircle(randomPoint, randomRadius, color); 
        }
    }

    private void DrawCircle(Vector2 center, float radius, Color color)
    {
        
        for (int x = (int)center.x - (int)radius; x <= (int)center.x + (int)radius; x++)
        {
            for (int y = (int)center.y - (int)radius; y <= (int)center.y + (int)radius; y++)
            {
                
                if (Vector2.Distance(new Vector2(x, y), center) <= radius)
                {
                    if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
                    {
                        texture.SetPixel(x, y, color);
                    }
                }
            }
        }

        
        texture.Apply();
    }


}
