/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 13 июля 2026 06:52:22
 * Version: 1.0.97
 */

namespace Lizerium.BINI.Converter.Models
{
    public sealed record BiniDocument(IReadOnlyList<BiniSection> Sections);
}
