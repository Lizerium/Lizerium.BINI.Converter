/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 27 апреля 2026 09:41:16
 * Version: 1.0.9
 */

namespace Lizerium.BINI.Converter.Models;

public readonly struct BiniValue
{
    private BiniValue(BiniValueType type, uint integer, float floating, string? text)
    {
        Type = type;
        Integer = integer;
        Floating = floating;
        Text = text;
    }

    public BiniValueType Type { get; }

    public uint Integer { get; }

    public float Floating { get; }

    public string? Text { get; }

    public static BiniValue FromInteger(int value) => new(BiniValueType.Integer, unchecked((uint)value), 0, null);

    public static BiniValue FromUnsignedInteger(uint value) => new(BiniValueType.Integer, value, 0, null);

    public static BiniValue FromFloat(float value) => new(BiniValueType.Float, 0, value, null);

    public static BiniValue FromString(string value) => new(BiniValueType.String, 0, 0, value);
}
