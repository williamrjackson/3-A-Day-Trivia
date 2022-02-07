using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wrj;

public class Party : MonoBehaviour
{
    public float duration = .5f;
    public float scaleSpeed = 1f;
    public float rowChangeSpeed = 1f;
    public float yOffset = -1f;
    public float loopDuration = 1f;
    private List<MeshRenderer> childCubes;
    private float columnSpacingExtremes;
    private float rowSpacingExtremes;
    private Color RandomBright => Color.HSVToRGB(Random.value, 1f, 1f);
    private CurvedGridLayout3d grid;

    void Start()
    {
        childCubes = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
        grid = GetComponent<CurvedGridLayout3d>();
        columnSpacingExtremes = grid.columnSpacing;
        rowSpacingExtremes = grid.rowSpacing;
        StartCoroutine(PartyRoutine());
        Utils.MapToCurve.Linear.Rotate(transform, Vector3.zero.With(y: 360f, x: 180f), loopDuration, loop: -1, shortestPath: false);
    }
    private void Update()
    {
        float spacingx = Mathf.PingPong(Time.time * scaleSpeed, 1f).Remap(0f, 1f, -rowSpacingExtremes, rowSpacingExtremes);
        grid.rowSpacing = spacingx;
        float spacingy = Mathf.PingPong(Time.time - yOffset * scaleSpeed, 1f).Remap(0f, 1f, -columnSpacingExtremes, columnSpacingExtremes);
        grid.columnSpacing = spacingy;
        grid.columns = Mathf.RoundToInt(Mathf.PingPong(Time.time * rowChangeSpeed, 20f));

    }

    private IEnumerator PartyRoutine()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            Utils.MapToCurve.Ease.ChangeColor(childCubes.GetRandom().transform, RandomBright, duration, matColorReference: "_BaseColor");
        }
    }

}
