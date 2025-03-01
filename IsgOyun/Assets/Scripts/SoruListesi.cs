using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "YeniSoruListesi", menuName = "Soru Verisi")]
public class SoruListesi : ScriptableObject
{
    public List<Soru> sorular = new List<Soru>();
}