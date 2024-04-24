using System;
using System.Collections;
using UnityEngine;

public class SaveManagerGame : Singleton<SaveManagerGame>
{
    public GameData myData = new GameData();

    public void Start()
    {


        LoadGame();
    }
    //For Debugging 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveGame();
            Debug.Log("Kayıt dosyası oluşturuldu");

        }
    }

    public void SaveGame()
    {
        SaveManager.CurrentSaveData.currentCanvas = EncodeTextureToBase64(Brush.Instance.texture);
        SaveManager.CurrentSaveData.background_Color = Brush.Instance.backGroundColor;
        SaveManager.CurrentSaveData.bursh_Size = Brush.Instance.brushSize;
        SaveManager.SaveGame();
    }

    public void LoadGame()
    {
        SaveManager.LoadGame();
        myData = SaveManager.CurrentSaveData;
        if (myData.currentCanvas != null)
        {
            byte[] textureBytes = Convert.FromBase64String(myData.currentCanvas);
            Brush.Instance.texture = new Texture2D(Screen.width, Screen.height);

            Brush.Instance.texture.LoadImage(textureBytes);

            Brush.Instance.backGroundColor = SaveManager.CurrentSaveData.background_Color;
            Brush.Instance.brushSize = SaveManager.CurrentSaveData.bursh_Size;


        }
        else
        {
            Brush.Instance.CreateTexture();

        }

    }

    private string EncodeTextureToBase64(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        return Convert.ToBase64String(bytes);
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveGame();
        }

    }


}




