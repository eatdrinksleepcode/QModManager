﻿using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System.Collections.Generic;
using System.IO;

namespace QModManager
{
    internal static class AudioFixer
    {
        internal static void ChangeDisableUnityAudio(string path, bool newValue)
        {
            AssetsManager am = new AssetsManager();
            AssetsFileInstance afi = am.LoadAssetsFile(path, false);
            am.LoadClassPackage("cldb.dat");
            AssetFileInfoEx audioInfo = afi.table.getAssetInfo(4);
            AssetTypeInstance audioAti = am.GetATI(afi.file, audioInfo);
            AssetTypeValueField audioBaseField = audioAti.GetBaseField();
            audioBaseField.Get("m_DisableAudio").GetValue().Set(false);
            byte[] audioAsset;
            using (MemoryStream memStream = new MemoryStream())
            using (AssetsFileWriter writer = new AssetsFileWriter(memStream))
            {
                writer.bigEndian = false;
                audioBaseField.Write(writer);
                audioAsset = memStream.ToArray();
            }
            List<AssetsReplacer> rep = new List<AssetsReplacer>() { new AssetsReplacerFromMemory(0, 4, 0x0B, 0xFFFF, audioAsset) };
            using (MemoryStream memStream = new MemoryStream())
            using (AssetsFileWriter writer = new AssetsFileWriter(memStream))
            {
                afi.file.Write(writer, 0, rep.ToArray(), 0);
                File.WriteAllBytes(path, memStream.ToArray());
            }
        }
    }
}