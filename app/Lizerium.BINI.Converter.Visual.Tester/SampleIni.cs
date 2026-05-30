/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 30 мая 2026 15:43:02
 * Version: 1.0.52
 */

namespace Lizerium.BINI.Converter.Visual.Tester;

internal static class SampleIni
{
    public const string Text = """"
                               ; comments are discarded
                               [Commodities]
                               iron = 1.42, 300, icons\iron.bmp, "+1", -0

                               [Weapons]
                               red_laser_beam = 255, 0, 0, "Laser Beam, ""Red"""
                               "==SPECIAL==" = 1.0
                               """";
}
