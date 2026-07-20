/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 20 июля 2026 12:10:08
 * Version: 1.0.104
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
