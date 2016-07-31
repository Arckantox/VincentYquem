using UnityEngine;
using System.Collections;

[System.Serializable]
public class CastlePiece : System.Object    // Class contenant les données des pièces du chateau, utilisé pour les SAVE / LOAD (Position, Rotation et ID)
{
    public int tabSize;
    public float[] positionX;    // Position stocké pour SAVE / LOAD
    public float[] positionY;
    public float[] rotation;      // Rotation sauvée
    public int[] ID;              // ID de la pièce par rapport au tableau de pièces TabShapes
}
