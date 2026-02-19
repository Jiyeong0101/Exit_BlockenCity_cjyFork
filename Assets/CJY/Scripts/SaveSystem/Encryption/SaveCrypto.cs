using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class SaveCrypto
{
    // 키 생성 (기기별)
    private static byte[] GetKey()
    {
        string keySource =
            Application.companyName +
            Application.productName +
            SystemInfo.deviceUniqueIdentifier;

        return SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(keySource));
    }

    // AES 암호화
    public static byte[] Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = GetKey();
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor();
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        // IV + 암호문 결합
        byte[] result = new byte[aes.IV.Length + encrypted.Length];
        Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
        Buffer.BlockCopy(encrypted, 0, result, aes.IV.Length, encrypted.Length);

        return result;
    }

    // AES 복호화
    public static string Decrypt(byte[] cipherData)
    {
        using var aes = Aes.Create();
        aes.Key = GetKey();

        byte[] iv = new byte[aes.BlockSize / 8];
        byte[] encrypted = new byte[cipherData.Length - iv.Length];

        Buffer.BlockCopy(cipherData, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(cipherData, iv.Length, encrypted, 0, encrypted.Length);

        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        byte[] plainBytes = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);

        return Encoding.UTF8.GetString(plainBytes);
    }

    // 무결성 해시
    public static string ComputeHash(byte[] data)
    {
        using var sha = SHA256.Create();
        return Convert.ToBase64String(sha.ComputeHash(data));
    }
}
