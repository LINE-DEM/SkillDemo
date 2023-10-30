/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 随机数工具

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using System;

public static class RandomUtils {
    private static Random random;

    public static void InitRandom(int seed) {
        random = new Random(seed);
    }

    /// <summary>
    /// 返回的随机数包含上限与下限
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int GetRandom(int min, int max) {
        return random.Next(min, max + 1);
    }
}
