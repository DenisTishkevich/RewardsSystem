namespace RewardsSystem.Common.Extensions;

public static class NumericExtensions
{
    public static bool IsWithin<T>(this T value, T min, T max) where T : IComparable<T> => value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
}
