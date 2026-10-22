using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class SmoothMotion
{
    public float RemainingDuration { get; private set; }
    
    private float _elapsedTime;

    public void Stop()
    {
        _elapsedTime = 0f;
    }
    
    public Vector2 StoppableInterpolateVector(Vector2 startVector, Vector2 endVector, float duration)
    {
        if (_elapsedTime < duration)
        {
            var t = CoefficientT(duration);
            
            return Vector2.Lerp(startVector, endVector, t);
        }

        return endVector;
    }

    public Vector2 ParabolicInterpolateVector(Vector2 startVector, Vector2 endVector, float height, float duration)
    {
        if (_elapsedTime < duration)
        {
            var t = CoefficientT(duration);
            
            return ParabolicPosition(startVector, endVector, height, t);
        }
        return endVector;
    }

    private Vector2 ParabolicPosition(Vector2 startVector, Vector2 endVector, float height, float t)
    {
        var interpolatedPosition = Vector2.Lerp(startVector, endVector, t);

        var parabolicHeight = 4 * t * (1 - t) * height;
        
        return interpolatedPosition + new Vector2(0, parabolicHeight);
    }

    private float CoefficientT(float duration)
    {
        _elapsedTime += Time.deltaTime;
        RemainingDuration = duration - _elapsedTime;
        
        return _elapsedTime / duration;
    }
}
