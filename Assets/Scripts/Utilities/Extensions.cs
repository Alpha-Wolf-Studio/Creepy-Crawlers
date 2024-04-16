using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// Inverse of Evaluate()
    /// </summary>
    /// <param name="curve">normalized AnimationCurve (time goes from 0 to 1)</param>
    /// <param name="value">value to search</param>
    /// <returns>time at which we have the closest value not exceeding it</returns>
    public static float InverseEvaluate(this AnimationCurve curve, float value, int decimals = 6)
    {
        // Retrieve the closest decimal and then go down
        float time = 0.1f;
        float step = 0.1f;
        float evaluate = curve.Evaluate(time);
        while (decimals > 0)
        {
            // Loop until we pass our value
            while (evaluate < value)
            {
                time += step;
                evaluate = curve.Evaluate(time);
            }

            // Go one step back and increase precision of the step by one decimal
            time -= step;
            evaluate = curve.Evaluate(time);
            step /= 10f;
            decimals--;
        }

        return time;
    }
}
