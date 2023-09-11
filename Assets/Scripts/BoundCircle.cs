using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundCircle : MonoBehaviour
{
    public GamePlaySettingConfig settingConfig;
    public static float radius;
    public float Radius;
    public int segments;
    public LineRenderer lineRenderer;
    private bool needToShrink = false;
    [SerializeField] float minRadius = 20f;
    public float delayTimeToShrink = 10f;
    public float shrinkSpeed = 2;
    public void OnStartGame()
    {
        ApplyConfig();
        var players = GameObject.FindObjectsByType<Player>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var player in players)
        {
            Player p = (Player)player;
            p.OnPlayerMovement -= OnPlayerMoverment;
            p.OnPlayerMovement += OnPlayerMoverment;
        }
        DOVirtual.DelayedCall(delayTimeToShrink,() =>
       {
           needToShrink = true;
       });
        radius = Radius;
        // Set the Line Renderer's width and material
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        // Update the Line Renderer's positions 
        UpdatePositions();
        gameObject.SetActive(true);
    }

    private void ApplyConfig()
    {
        if (settingConfig == null) return;
        minRadius = settingConfig.minRadius;
        shrinkSpeed = settingConfig.srinkSpeed;
        delayTimeToShrink = settingConfig.delayTimeToSrink;
    }

    void UpdatePositions()
    {
        // Calculate the center of the circle
        Vector3 center = transform.position;

        // Create an array to hold the positions around the circumference of the circle
        Vector3[] positions = new Vector3[segments + 1];

        // Calculate the positions around the circumference of the circle
        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            positions[i] = center + new Vector3(x, 0, z);
        }

        // Add the first position to the end of the array to close the circle
        positions[segments] = positions[0];

        // Update the Line Renderer's positions
        lineRenderer.positionCount = segments + 1;
        lineRenderer.SetPositions(positions);
    }
    void OnPlayerMoverment(Transform playerPos)
    {
        float playerPosX = playerPos.position.x;
        float playerPosZ = playerPos.position.z;
        float distance = Mathf.Sqrt((playerPosX * playerPosX) + (playerPosZ * playerPosZ));
        float boundOffset = 0.5f;
        var bound = BoundCircle.radius - boundOffset;
        if (distance >= bound)
        {
            var angle = Mathf.Atan2(playerPosZ, playerPosX);
            var newPosX = bound * Mathf.Cos(angle);
            var newPosZ = bound * Mathf.Sin(angle);
            playerPos.position = new Vector3(newPosX, 0, newPosZ);
        }
    }
    void Update()
    {
        if (!needToShrink) return;
        radius -= shrinkSpeed * Time.deltaTime;
        if (radius <= minRadius) needToShrink = false;
        // Update the Line Renderer's positions every frame
        UpdatePositions();
    }
}
