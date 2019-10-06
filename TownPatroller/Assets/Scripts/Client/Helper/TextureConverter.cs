using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TownPatroller.Client.Helper
{
    public static class TextureConverter
    {
        public static Texture2D Base64ToTexture2D(byte[] imageData)
        {

            int width, height;
            GetImageSize(imageData, out width, out height);

            Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false, true);

            texture.hideFlags = HideFlags.HideAndDontSave;
            texture.filterMode = FilterMode.Point;
            texture.LoadImage(imageData);
            texture.Apply();

            return texture;
        }
        private static void GetImageSize(byte[] imageData, out int width, out int height)
        {
            width = ReadInt(imageData, 3 + 15);
            height = ReadInt(imageData, 3 + 15 + 2 + 2);
        }

        private static int ReadInt(byte[] imageData, int offset)
        {
            return (imageData[offset] << 8 | imageData[offset + 1]);
        }
    }
}
