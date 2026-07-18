/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 18 июля 2026 07:27:13
 * Version: 1.0.102
 */

namespace Lizerium.BINI.Converter.Models
{
    public sealed record BiniDocument(IReadOnlyList<BiniSection> Sections);
}
