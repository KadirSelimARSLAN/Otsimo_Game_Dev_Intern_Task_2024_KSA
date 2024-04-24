using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GUIManager : Singleton<GUIManager>
{
    public Brush brush;

    public Image ColorImage;

    public GameObject colorPanel;
    public GameObject brushSizePanel;
    public GameObject buttonPanel;
    public Slider brushSizeSlider;
    public TextMeshProUGUI brushSizeText;
    public TextMeshProUGUI brushSizeButtonText;
    public int lastChooseIndx = 0;
    private void Start()
    {
        ChangeToolButton(0);
        brushSizeSlider.value = brush.brushSize;
        brushSizeText.text = "BRUSH SIZE :" + brush.brushSize.ToString() + "px";
        brushSizeButtonText.text = brushSizeSlider.value.ToString(".0") + "px";
    }
    public void SetColorButton()
    {
        ColorImage.color = brush.color;

    }
    public void BrushSizeSlider()
    {
        brush.brushSize = brushSizeSlider.value;
        brushSizeText.text = "BRUSH SIZE :" + brushSizeSlider.value.ToString() + "px";
        brushSizeButtonText.text = brushSizeSlider.value.ToString(".0") + "px";
    }
   public void ChangeToolButton(int indx)
    {
        AudioManager.Instance.PlayButtonClickSound();

        if(indx == 0)
        {
            brush.toolType = Brush.ToolType.Pen;
            colorPanel.SetActive(false);
            brushSizePanel.SetActive(false);
            lastChooseIndx = 0;
        }
        else if(indx == 1)
        {
            brush.toolType = Brush.ToolType.Bucket;
            colorPanel.SetActive(false);
            brushSizePanel.SetActive(false);
            lastChooseIndx = 1;
        }
        else if (indx == 2)
        {
            brush.toolType = Brush.ToolType.Eraser;
            colorPanel.SetActive(false);
            brushSizePanel.SetActive(false);
            lastChooseIndx = 2;
        }
        else if (indx == 3)
        {
            brush.toolType = Brush.ToolType.Stamp;
            colorPanel.SetActive(false);
            brushSizePanel.SetActive(false);
            lastChooseIndx = 3;
        }
        else if (indx == 4)
        {
            if (!colorPanel.activeInHierarchy)
            {
                colorPanel.SetActive(true);
                brushSizePanel.SetActive(false);
                brush.toolType = Brush.ToolType.Color;
            }
            else
            {
                colorPanel.SetActive(false);
                brushSizePanel.SetActive(false);
                ChangeToolButton(lastChooseIndx);
                return;
            }
           
        }
        else if (indx == 5)
        {
            brushSizeButtonText.text = brushSizeSlider.value.ToString(".0") + "px";
            if (!brushSizePanel.activeInHierarchy)
            {
                brushSizePanel.SetActive(true);
                colorPanel.SetActive(false);
                brush.toolType = Brush.ToolType.BrushSize;
            }
            else
            {
                brushSizePanel.SetActive(false);
                colorPanel.SetActive(false);
                ChangeToolButton(lastChooseIndx);
                return;
            }

        }
        else if (indx == 6)
        {
            brush.toolType = Brush.ToolType.PaintBall;
            colorPanel.SetActive(false);
            brushSizePanel.SetActive(false);
            lastChooseIndx = 6;
        }
        for (int i = 0; i < buttonPanel.transform.childCount; i++)
            {
                if (indx != i)
                {
                    buttonPanel.transform.GetChild(i).GetComponent<Image>().color = Color.white;
                }
                else
                {
                    buttonPanel.transform.GetChild(i).GetComponent<Image>().color = Color.gray;
                }

            } 
    }

    public void ColorButton(Image colorImage)
    {
        brush.color = colorImage.color;
        SetColorButton();
        ChangeToolButton(lastChooseIndx);
        colorPanel.SetActive(false);

    }
}
