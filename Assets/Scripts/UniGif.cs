using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class UniGif
{
	public static IEnumerator GetTextureListCoroutine(byte[] bytes, Action<List<UniGif.GifTexture>, int, int, int> callback, FilterMode filterMode = FilterMode.Bilinear, TextureWrapMode wrapMode = TextureWrapMode.Clamp, bool debugLog = false)
	{
		int loopCount = -1;
		int width = 0;
		int height = 0;
		UniGif.GifData gifData = default(UniGif.GifData);
		if (!UniGif.SetGifData(bytes, ref gifData, debugLog))
		{
			UnityEngine.Debug.LogError("GIF file data set error.");
			if (callback != null)
			{
				callback(null, loopCount, width, height);
			}
			yield break;
		}
		List<UniGif.GifTexture> gifTexList = null;
		yield return UniGif.DecodeTextureCoroutine(gifData, delegate(List<UniGif.GifTexture> result)
		{
			gifTexList = result;
		}, filterMode, wrapMode);
		if (gifTexList == null || gifTexList.Count <= 0)
		{
			UnityEngine.Debug.LogError("GIF texture decode error.");
			if (callback != null)
			{
				callback(null, loopCount, width, height);
			}
			yield break;
		}
		loopCount = gifData.m_appEx.loopCount;
		width = (int)gifData.m_logicalScreenWidth;
		height = (int)gifData.m_logicalScreenHeight;
		if (callback != null)
		{
			callback(gifTexList, loopCount, width, height);
		}
		yield break;
	}

	private static IEnumerator DecodeTextureCoroutine(UniGif.GifData gifData, Action<List<UniGif.GifTexture>> callback, FilterMode filterMode, TextureWrapMode wrapMode)
	{
		if (gifData.m_imageBlockList == null || gifData.m_imageBlockList.Count < 1)
		{
			yield break;
		}
		List<UniGif.GifTexture> gifTexList = new List<UniGif.GifTexture>(gifData.m_imageBlockList.Count);
		List<ushort> disposalMethodList = new List<ushort>(gifData.m_imageBlockList.Count);
		int imgIndex = 0;
		for (int i = 0; i < gifData.m_imageBlockList.Count; i++)
		{
			byte[] decodedData = UniGif.GetDecodedData(gifData.m_imageBlockList[i]);
			UniGif.GraphicControlExtension? graphicCtrlEx = UniGif.GetGraphicCtrlExt(gifData, imgIndex);
			int transparentIndex = UniGif.GetTransparentIndex(graphicCtrlEx);
			disposalMethodList.Add(UniGif.GetDisposalMethod(graphicCtrlEx));
			Color bgColor;
			List<byte[]> colorTable = UniGif.GetColorTableAndSetBgColor(gifData, gifData.m_imageBlockList[i], transparentIndex, out bgColor);
			yield return 0;
			bool filledTexture;
			Texture2D tex = UniGif.CreateTexture2D(gifData, gifTexList, imgIndex, disposalMethodList, bgColor, filterMode, wrapMode, out filledTexture);
			yield return 0;
			int dataIndex = 0;
			for (int j = tex.height - 1; j >= 0; j--)
			{
				UniGif.SetTexturePixelRow(tex, j, gifData.m_imageBlockList[i], decodedData, ref dataIndex, colorTable, bgColor, transparentIndex, filledTexture);
			}
			tex.Apply();
			yield return 0;
			float delaySec = UniGif.GetDelaySec(graphicCtrlEx);
			gifTexList.Add(new UniGif.GifTexture(tex, delaySec));
			imgIndex++;
		}
		if (callback != null)
		{
			callback(gifTexList);
		}
		yield break;
	}

	private static byte[] GetDecodedData(UniGif.ImageBlock imgBlock)
	{
		List<byte> list = new List<byte>();
		for (int i = 0; i < imgBlock.m_imageDataList.Count; i++)
		{
			for (int j = 0; j < imgBlock.m_imageDataList[i].m_imageData.Length; j++)
			{
				list.Add(imgBlock.m_imageDataList[i].m_imageData[j]);
			}
		}
		int needDataSize = (int)(imgBlock.m_imageHeight * imgBlock.m_imageWidth);
		byte[] array = UniGif.DecodeGifLZW(list, (int)imgBlock.m_lzwMinimumCodeSize, needDataSize);
		if (imgBlock.m_interlaceFlag)
		{
			array = UniGif.SortInterlaceGifData(array, (int)imgBlock.m_imageWidth);
		}
		return array;
	}

	private static List<byte[]> GetColorTableAndSetBgColor(UniGif.GifData gifData, UniGif.ImageBlock imgBlock, int transparentIndex, out Color bgColor)
	{
		List<byte[]> list = (!imgBlock.m_localColorTableFlag) ? ((!gifData.m_globalColorTableFlag) ? null : gifData.m_globalColorTable) : imgBlock.m_localColorTable;
		if (list != null)
		{
			byte[] array = list[(int)gifData.m_bgColorIndex];
			bgColor = new Color(array[0], array[1], array[2], (transparentIndex != (int)gifData.m_bgColorIndex) ? byte.MaxValue : 0);
		}
		else
		{
			bgColor = Color.black;
		}
		return list;
	}

	private static UniGif.GraphicControlExtension? GetGraphicCtrlExt(UniGif.GifData gifData, int imgBlockIndex)
	{
		if (gifData.m_graphicCtrlExList != null && gifData.m_graphicCtrlExList.Count > imgBlockIndex)
		{
			return new UniGif.GraphicControlExtension?(gifData.m_graphicCtrlExList[imgBlockIndex]);
		}
		return null;
	}

	private static int GetTransparentIndex(UniGif.GraphicControlExtension? graphicCtrlEx)
	{
		int result = -1;
		if (graphicCtrlEx != null && graphicCtrlEx.Value.m_transparentColorFlag)
		{
			result = (int)graphicCtrlEx.Value.m_transparentColorIndex;
		}
		return result;
	}

	private static float GetDelaySec(UniGif.GraphicControlExtension? graphicCtrlEx)
	{
		float num = (graphicCtrlEx == null) ? 0.0166666675f : ((float)graphicCtrlEx.Value.m_delayTime / 100f);
		if (num <= 0f)
		{
			num = 0.1f;
		}
		return num;
	}

	private static ushort GetDisposalMethod(UniGif.GraphicControlExtension? graphicCtrlEx)
	{
		return (graphicCtrlEx == null) ? (ushort)2 : graphicCtrlEx.Value.m_disposalMethod;
	}

	private static Texture2D CreateTexture2D(UniGif.GifData gifData, List<UniGif.GifTexture> gifTexList, int imgIndex, List<ushort> disposalMethodList, Color32 bgColor, FilterMode filterMode, TextureWrapMode wrapMode, out bool filledTexture)
	{
		filledTexture = false;
		Texture2D texture2D = new Texture2D((int)gifData.m_logicalScreenWidth, (int)gifData.m_logicalScreenHeight, TextureFormat.ARGB32, false);
		texture2D.filterMode = filterMode;
		texture2D.wrapMode = wrapMode;
        int num = (imgIndex <= 0) ? 2 : disposalMethodList[imgIndex - 1];
		int num2 = -1;
		if (num != 0)
		{
			if (num == 1)
			{
				num2 = imgIndex - 1;
			}
			else if (num == 2)
			{
				filledTexture = true;
				Color32[] array = new Color32[texture2D.width * texture2D.height];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = bgColor;
				}
				texture2D.SetPixels32(array);
				texture2D.Apply();
			}
			else if (num == 3)
			{
				for (int j = imgIndex - 1; j >= 0; j--)
				{
					if (disposalMethodList[j] == 0 || disposalMethodList[j] == 1)
					{
						num2 = j;
						break;
					}
				}
			}
		}
		if (num2 >= 0)
		{
			filledTexture = true;
			Color32[] pixels = gifTexList[num2].m_texture2d.GetPixels32();
			texture2D.SetPixels32(pixels);
			texture2D.Apply();
		}
		return texture2D;
	}

	private static void SetTexturePixelRow(Texture2D tex, int y, UniGif.ImageBlock imgBlock, byte[] decodedData, ref int dataIndex, List<byte[]> colorTable, Color32 bgColor, int transparentIndex, bool filledTexture)
	{
		int num = tex.height - 1 - y;
		for (int i = 0; i < tex.width; i++)
		{
			int num2 = i;
			if (num < (int)imgBlock.m_imageTopPosition || num >= (int)(imgBlock.m_imageTopPosition + imgBlock.m_imageHeight) || num2 < (int)imgBlock.m_imageLeftPosition || num2 >= (int)(imgBlock.m_imageLeftPosition + imgBlock.m_imageWidth))
			{
				if (!filledTexture)
				{
					tex.SetPixel(i, y, bgColor);
				}
			}
			else if (dataIndex >= decodedData.Length)
			{
				if (!filledTexture)
				{
					tex.SetPixel(i, y, bgColor);
					if (dataIndex == decodedData.Length)
					{
						UnityEngine.Debug.LogError(string.Concat(new object[]
						{
							"dataIndex exceeded the size of decodedData. dataIndex:",
							dataIndex,
							" decodedData.Length:",
							decodedData.Length,
							" y:",
							y,
							" x:",
							i
						}));
					}
				}
				dataIndex++;
			}
			else
			{
				byte b = decodedData[dataIndex];
				if (colorTable == null || colorTable.Count <= (int)b)
				{
					if (!filledTexture)
					{
						tex.SetPixel(i, y, bgColor);
						if (colorTable == null)
						{
							UnityEngine.Debug.LogError("colorIndex exceeded the size of colorTable. colorTable is null. colorIndex:" + b);
						}
						else
						{
							UnityEngine.Debug.LogError(string.Concat(new object[]
							{
								"colorIndex exceeded the size of colorTable. colorTable.Count:",
								colorTable.Count,
								" colorIndex:",
								b
							}));
						}
					}
					dataIndex++;
				}
				else
				{
					byte[] array = colorTable[(int)b];
					int b2 = (transparentIndex < 0 || transparentIndex != (int)b) ? byte.MaxValue : 0;
					if (!filledTexture || b2 != 0)
					{
						Color c = new Color(array[0], array[1], array[2], b2);
						tex.SetPixel(i, y, c);
					}
					dataIndex++;
				}
			}
		}
	}

	private static byte[] DecodeGifLZW(List<byte> compData, int lzwMinimumCodeSize, int needDataSize)
	{
		int num = 0;
		int num2 = 0;
		Dictionary<int, string> dictionary = new Dictionary<int, string>();
		int num3 = 0;
		UniGif.InitDictionary(dictionary, lzwMinimumCodeSize, out num3, out num, out num2);
		byte[] bytes = compData.ToArray();
		BitArray bitArray = new BitArray(bytes);
		byte[] array = new byte[needDataSize];
		int num4 = 0;
		string text = null;
		bool flag = false;
		int i = 0;
		while (i < bitArray.Length)
		{
			if (flag)
			{
				UniGif.InitDictionary(dictionary, lzwMinimumCodeSize, out num3, out num, out num2);
				flag = false;
			}
			int numeral = bitArray.GetNumeral(i, num3);
			if (numeral == num)
			{
				flag = true;
				i += num3;
				text = null;
			}
			else
			{
				if (numeral == num2)
				{
					UnityEngine.Debug.LogWarning(string.Concat(new object[]
					{
						"early stop code. bitDataIndex:",
						i,
						" lzwCodeSize:",
						num3,
						" key:",
						numeral,
						" dic.Count:",
						dictionary.Count
					}));
					break;
				}
				string text2;
				if (dictionary.ContainsKey(numeral))
				{
					text2 = dictionary[numeral];
				}
				else
				{
					if (numeral < dictionary.Count)
					{
						UnityEngine.Debug.LogWarning(string.Concat(new object[]
						{
							"It is strange that come here. bitDataIndex:",
							i,
							" lzwCodeSize:",
							num3,
							" key:",
							numeral,
							" dic.Count:",
							dictionary.Count
						}));
						i += num3;
						continue;
					}
					if (text == null)
					{
						UnityEngine.Debug.LogWarning(string.Concat(new object[]
						{
							"It is strange that come here. bitDataIndex:",
							i,
							" lzwCodeSize:",
							num3,
							" key:",
							numeral,
							" dic.Count:",
							dictionary.Count
						}));
						i += num3;
						continue;
					}
					text2 = text + text[0];
				}
				byte[] bytes2 = Encoding.Unicode.GetBytes(text2);
				for (int j = 0; j < bytes2.Length; j++)
				{
					if (j % 2 == 0)
					{
						array[num4] = bytes2[j];
						num4++;
					}
				}
				if (num4 >= needDataSize)
				{
					break;
				}
				if (text != null)
				{
					dictionary.Add(dictionary.Count, text + text2[0]);
				}
				text = text2;
				i += num3;
				if (num3 == 3 && dictionary.Count >= 8)
				{
					num3 = 4;
				}
				else if (num3 == 4 && dictionary.Count >= 16)
				{
					num3 = 5;
				}
				else if (num3 == 5 && dictionary.Count >= 32)
				{
					num3 = 6;
				}
				else if (num3 == 6 && dictionary.Count >= 64)
				{
					num3 = 7;
				}
				else if (num3 == 7 && dictionary.Count >= 128)
				{
					num3 = 8;
				}
				else if (num3 == 8 && dictionary.Count >= 256)
				{
					num3 = 9;
				}
				else if (num3 == 9 && dictionary.Count >= 512)
				{
					num3 = 10;
				}
				else if (num3 == 10 && dictionary.Count >= 1024)
				{
					num3 = 11;
				}
				else if (num3 == 11 && dictionary.Count >= 2048)
				{
					num3 = 12;
				}
				else if (num3 == 12 && dictionary.Count >= 4096)
				{
					int numeral2 = bitArray.GetNumeral(i, num3);
					if (numeral2 != num)
					{
						flag = true;
					}
				}
			}
		}
		return array;
	}

	private static void InitDictionary(Dictionary<int, string> dic, int lzwMinimumCodeSize, out int lzwCodeSize, out int clearCode, out int finishCode)
	{
		int num = (int)Math.Pow(2.0, (double)lzwMinimumCodeSize);
		clearCode = num;
		finishCode = clearCode + 1;
		dic.Clear();
		for (int i = 0; i < num + 2; i++)
		{
			dic.Add(i, ((char)i).ToString());
		}
		lzwCodeSize = lzwMinimumCodeSize + 1;
	}

	private static byte[] SortInterlaceGifData(byte[] decodedData, int xNum)
	{
		int num = 0;
		int num2 = 0;
		byte[] array = new byte[decodedData.Length];
		for (int i = 0; i < array.Length; i++)
		{
			if (num % 8 == 0)
			{
				array[i] = decodedData[num2];
				num2++;
			}
			if (i != 0 && i % xNum == 0)
			{
				num++;
			}
		}
		num = 0;
		for (int j = 0; j < array.Length; j++)
		{
			if (num % 8 == 4)
			{
				array[j] = decodedData[num2];
				num2++;
			}
			if (j != 0 && j % xNum == 0)
			{
				num++;
			}
		}
		num = 0;
		for (int k = 0; k < array.Length; k++)
		{
			if (num % 4 == 2)
			{
				array[k] = decodedData[num2];
				num2++;
			}
			if (k != 0 && k % xNum == 0)
			{
				num++;
			}
		}
		num = 0;
		for (int l = 0; l < array.Length; l++)
		{
			if (num % 8 != 0 && num % 8 != 4 && num % 4 != 2)
			{
				array[l] = decodedData[num2];
				num2++;
			}
			if (l != 0 && l % xNum == 0)
			{
				num++;
			}
		}
		return array;
	}

	private static bool SetGifData(byte[] gifBytes, ref UniGif.GifData gifData, bool debugLog)
	{
		if (debugLog)
		{
			UnityEngine.Debug.Log("SetGifData Start.");
		}
		if (gifBytes == null || gifBytes.Length <= 0)
		{
			UnityEngine.Debug.LogError("bytes is nothing.");
			return false;
		}
		int num = 0;
		if (!UniGif.SetGifHeader(gifBytes, ref num, ref gifData))
		{
			UnityEngine.Debug.LogError("GIF header set error.");
			return false;
		}
		if (!UniGif.SetGifBlock(gifBytes, ref num, ref gifData))
		{
			UnityEngine.Debug.LogError("GIF block set error.");
			return false;
		}
		if (debugLog)
		{
			gifData.Dump();
			UnityEngine.Debug.Log("SetGifData Finish.");
		}
		return true;
	}

	private static bool SetGifHeader(byte[] gifBytes, ref int byteIndex, ref UniGif.GifData gifData)
	{
		if (gifBytes[0] != 71 || gifBytes[1] != 73 || gifBytes[2] != 70)
		{
			UnityEngine.Debug.LogError("This is not GIF image.");
			return false;
		}
		gifData.m_sig0 = gifBytes[0];
		gifData.m_sig1 = gifBytes[1];
		gifData.m_sig2 = gifBytes[2];
		if ((gifBytes[3] != 56 || gifBytes[4] != 55 || gifBytes[5] != 97) && (gifBytes[3] != 56 || gifBytes[4] != 57 || gifBytes[5] != 97))
		{
			UnityEngine.Debug.LogError("GIF version error.\nSupported only GIF87a or GIF89a.");
			return false;
		}
		gifData.m_ver0 = gifBytes[3];
		gifData.m_ver1 = gifBytes[4];
		gifData.m_ver2 = gifBytes[5];
		gifData.m_logicalScreenWidth = BitConverter.ToUInt16(gifBytes, 6);
		gifData.m_logicalScreenHeight = BitConverter.ToUInt16(gifBytes, 8);
		gifData.m_globalColorTableFlag = ((gifBytes[10] & 128) == 128);
		int num = (int)(gifBytes[10] & 112);
		if (num != 16)
		{
			if (num != 32)
			{
				if (num != 48)
				{
					if (num != 64)
					{
						if (num != 80)
						{
							if (num != 96)
							{
								if (num != 112)
								{
									gifData.m_colorResolution = 1;
								}
								else
								{
									gifData.m_colorResolution = 8;
								}
							}
							else
							{
								gifData.m_colorResolution = 7;
							}
						}
						else
						{
							gifData.m_colorResolution = 6;
						}
					}
					else
					{
						gifData.m_colorResolution = 5;
					}
				}
				else
				{
					gifData.m_colorResolution = 4;
				}
			}
			else
			{
				gifData.m_colorResolution = 3;
			}
		}
		else
		{
			gifData.m_colorResolution = 2;
		}
		gifData.m_sortFlag = ((gifBytes[10] & 8) == 8);
		int num2 = (int)((gifBytes[10] & 7) + 1);
		gifData.m_sizeOfGlobalColorTable = (int)Math.Pow(2.0, (double)num2);
		gifData.m_bgColorIndex = gifBytes[11];
		gifData.m_pixelAspectRatio = gifBytes[12];
		byteIndex = 13;
		if (gifData.m_globalColorTableFlag)
		{
			gifData.m_globalColorTable = new List<byte[]>();
			for (int i = byteIndex; i < byteIndex + gifData.m_sizeOfGlobalColorTable * 3; i += 3)
			{
				gifData.m_globalColorTable.Add(new byte[]
				{
					gifBytes[i],
					gifBytes[i + 1],
					gifBytes[i + 2]
				});
			}
			byteIndex += gifData.m_sizeOfGlobalColorTable * 3;
		}
		return true;
	}

	private static bool SetGifBlock(byte[] gifBytes, ref int byteIndex, ref UniGif.GifData gifData)
	{
		try
		{
			int num = 0;
			for (;;)
			{
				int num2 = byteIndex;
				if (gifBytes[num2] == 44)
				{
					UniGif.SetImageBlock(gifBytes, ref byteIndex, ref gifData);
				}
				else if (gifBytes[num2] == 33)
				{
					byte b = gifBytes[num2 + 1];
					if (b != 254)
					{
						if (b != 255)
						{
							if (b != 1)
							{
								if (b == 249)
								{
									UniGif.SetGraphicControlExtension(gifBytes, ref byteIndex, ref gifData);
								}
							}
							else
							{
								UniGif.SetPlainTextExtension(gifBytes, ref byteIndex, ref gifData);
							}
						}
						else
						{
							UniGif.SetApplicationExtension(gifBytes, ref byteIndex, ref gifData);
						}
					}
					else
					{
						UniGif.SetCommentExtension(gifBytes, ref byteIndex, ref gifData);
					}
				}
				else if (gifBytes[num2] == 59)
				{
					break;
				}
				if (num == num2)
				{
					goto Block_9;
				}
				num = num2;
			}
			gifData.m_trailer = gifBytes[byteIndex];
			byteIndex++;
			return true;
			Block_9:
			UnityEngine.Debug.LogError("Infinite loop error.");
			return false;
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError(ex.Message);
			return false;
		}
		return true;
	}

	private static void SetImageBlock(byte[] gifBytes, ref int byteIndex, ref UniGif.GifData gifData)
	{
		UniGif.ImageBlock item = default(UniGif.ImageBlock);
		item.m_imageSeparator = gifBytes[byteIndex];
		byteIndex++;
		item.m_imageLeftPosition = BitConverter.ToUInt16(gifBytes, byteIndex);
		byteIndex += 2;
		item.m_imageTopPosition = BitConverter.ToUInt16(gifBytes, byteIndex);
		byteIndex += 2;
		item.m_imageWidth = BitConverter.ToUInt16(gifBytes, byteIndex);
		byteIndex += 2;
		item.m_imageHeight = BitConverter.ToUInt16(gifBytes, byteIndex);
		byteIndex += 2;
		item.m_localColorTableFlag = ((gifBytes[byteIndex] & 128) == 128);
		item.m_interlaceFlag = ((gifBytes[byteIndex] & 64) == 64);
		item.m_sortFlag = ((gifBytes[byteIndex] & 32) == 32);
		int num = (int)((gifBytes[byteIndex] & 7) + 1);
		item.m_sizeOfLocalColorTable = (int)Math.Pow(2.0, (double)num);
		byteIndex++;
		if (item.m_localColorTableFlag)
		{
			item.m_localColorTable = new List<byte[]>();
			for (int i = byteIndex; i < byteIndex + item.m_sizeOfLocalColorTable * 3; i += 3)
			{
				item.m_localColorTable.Add(new byte[]
				{
					gifBytes[i],
					gifBytes[i + 1],
					gifBytes[i + 2]
				});
			}
			byteIndex += item.m_sizeOfLocalColorTable * 3;
		}
		item.m_lzwMinimumCodeSize = gifBytes[byteIndex];
		byteIndex++;
		for (;;)
		{
			byte b = gifBytes[byteIndex];
			byteIndex++;
			if (b == 0)
			{
				break;
			}
			UniGif.ImageBlock.ImageDataBlock item2 = default(UniGif.ImageBlock.ImageDataBlock);
			item2.m_blockSize = b;
			item2.m_imageData = new byte[(int)item2.m_blockSize];
			for (int j = 0; j < item2.m_imageData.Length; j++)
			{
				item2.m_imageData[j] = gifBytes[byteIndex];
				byteIndex++;
			}
			if (item.m_imageDataList == null)
			{
				item.m_imageDataList = new List<UniGif.ImageBlock.ImageDataBlock>();
			}
			item.m_imageDataList.Add(item2);
		}
		if (gifData.m_imageBlockList == null)
		{
			gifData.m_imageBlockList = new List<UniGif.ImageBlock>();
		}
		gifData.m_imageBlockList.Add(item);
	}

	private static void SetGraphicControlExtension(byte[] gifBytes, ref int byteIndex, ref UniGif.GifData gifData)
	{
		UniGif.GraphicControlExtension item = default(UniGif.GraphicControlExtension);
		item.m_extensionIntroducer = gifBytes[byteIndex];
		byteIndex++;
		item.m_graphicControlLabel = gifBytes[byteIndex];
		byteIndex++;
		item.m_blockSize = gifBytes[byteIndex];
		byteIndex++;
		int num = (int)(gifBytes[byteIndex] & 28);
		if (num != 4)
		{
			if (num != 8)
			{
				if (num != 12)
				{
					item.m_disposalMethod = 0;
				}
				else
				{
					item.m_disposalMethod = 3;
				}
			}
			else
			{
				item.m_disposalMethod = 2;
			}
		}
		else
		{
			item.m_disposalMethod = 1;
		}
		item.m_transparentColorFlag = ((gifBytes[byteIndex] & 1) == 1);
		byteIndex++;
		item.m_delayTime = BitConverter.ToUInt16(gifBytes, byteIndex);
		byteIndex += 2;
		item.m_transparentColorIndex = gifBytes[byteIndex];
		byteIndex++;
		item.m_blockTerminator = gifBytes[byteIndex];
		byteIndex++;
		if (gifData.m_graphicCtrlExList == null)
		{
			gifData.m_graphicCtrlExList = new List<UniGif.GraphicControlExtension>();
		}
		gifData.m_graphicCtrlExList.Add(item);
	}

	private static void SetCommentExtension(byte[] gifBytes, ref int byteIndex, ref UniGif.GifData gifData)
	{
		UniGif.CommentExtension item = default(UniGif.CommentExtension);
		item.m_extensionIntroducer = gifBytes[byteIndex];
		byteIndex++;
		item.m_commentLabel = gifBytes[byteIndex];
		byteIndex++;
		for (;;)
		{
			byte b = gifBytes[byteIndex];
			byteIndex++;
			if (b == 0)
			{
				break;
			}
			UniGif.CommentExtension.CommentDataBlock item2 = default(UniGif.CommentExtension.CommentDataBlock);
			item2.m_blockSize = b;
			item2.m_commentData = new byte[(int)item2.m_blockSize];
			for (int i = 0; i < item2.m_commentData.Length; i++)
			{
				item2.m_commentData[i] = gifBytes[byteIndex];
				byteIndex++;
			}
			if (item.m_commentDataList == null)
			{
				item.m_commentDataList = new List<UniGif.CommentExtension.CommentDataBlock>();
			}
			item.m_commentDataList.Add(item2);
		}
		if (gifData.m_commentExList == null)
		{
			gifData.m_commentExList = new List<UniGif.CommentExtension>();
		}
		gifData.m_commentExList.Add(item);
	}

	private static void SetPlainTextExtension(byte[] gifBytes, ref int byteIndex, ref UniGif.GifData gifData)
	{
		UniGif.PlainTextExtension item = default(UniGif.PlainTextExtension);
		item.m_extensionIntroducer = gifBytes[byteIndex];
		byteIndex++;
		item.m_plainTextLabel = gifBytes[byteIndex];
		byteIndex++;
		item.m_blockSize = gifBytes[byteIndex];
		byteIndex++;
		byteIndex += 2;
		byteIndex += 2;
		byteIndex += 2;
		byteIndex += 2;
		byteIndex++;
		byteIndex++;
		byteIndex++;
		byteIndex++;
		for (;;)
		{
			byte b = gifBytes[byteIndex];
			byteIndex++;
			if (b == 0)
			{
				break;
			}
			UniGif.PlainTextExtension.PlainTextDataBlock item2 = default(UniGif.PlainTextExtension.PlainTextDataBlock);
			item2.m_blockSize = b;
			item2.m_plainTextData = new byte[(int)item2.m_blockSize];
			for (int i = 0; i < item2.m_plainTextData.Length; i++)
			{
				item2.m_plainTextData[i] = gifBytes[byteIndex];
				byteIndex++;
			}
			if (item.m_plainTextDataList == null)
			{
				item.m_plainTextDataList = new List<UniGif.PlainTextExtension.PlainTextDataBlock>();
			}
			item.m_plainTextDataList.Add(item2);
		}
		if (gifData.m_plainTextExList == null)
		{
			gifData.m_plainTextExList = new List<UniGif.PlainTextExtension>();
		}
		gifData.m_plainTextExList.Add(item);
	}

	private static void SetApplicationExtension(byte[] gifBytes, ref int byteIndex, ref UniGif.GifData gifData)
	{
		gifData.m_appEx.m_extensionIntroducer = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_extensionLabel = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_blockSize = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appId1 = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appId2 = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appId3 = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appId4 = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appId5 = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appId6 = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appId7 = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appId8 = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appAuthCode1 = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appAuthCode2 = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appAuthCode3 = gifBytes[byteIndex];
		byteIndex++;
		for (;;)
		{
			byte b = gifBytes[byteIndex];
			byteIndex++;
			if (b == 0)
			{
				break;
			}
			UniGif.ApplicationExtension.ApplicationDataBlock item = default(UniGif.ApplicationExtension.ApplicationDataBlock);
			item.m_blockSize = b;
			item.m_applicationData = new byte[(int)item.m_blockSize];
			for (int i = 0; i < item.m_applicationData.Length; i++)
			{
				item.m_applicationData[i] = gifBytes[byteIndex];
				byteIndex++;
			}
			if (gifData.m_appEx.m_appDataList == null)
			{
				gifData.m_appEx.m_appDataList = new List<UniGif.ApplicationExtension.ApplicationDataBlock>();
			}
			gifData.m_appEx.m_appDataList.Add(item);
		}
	}

	public class GifTexture
	{
		public GifTexture(Texture2D texture2d, float delaySec)
		{
			this.m_texture2d = texture2d;
			this.m_delaySec = delaySec;
		}

		public Texture2D m_texture2d;

		public float m_delaySec;
	}

	private struct GifData
	{
		public string signature
		{
			get
			{
				char[] value = new char[]
				{
					(char)this.m_sig0,
					(char)this.m_sig1,
					(char)this.m_sig2
				};
				return new string(value);
			}
		}

		public string version
		{
			get
			{
				char[] value = new char[]
				{
					(char)this.m_ver0,
					(char)this.m_ver1,
					(char)this.m_ver2
				};
				return new string(value);
			}
		}

		public void Dump()
		{
			UnityEngine.Debug.Log("GIF Type: " + this.signature + "-" + this.version);
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Image Size: ",
				this.m_logicalScreenWidth,
				"x",
				this.m_logicalScreenHeight
			}));
			UnityEngine.Debug.Log("Animation Image Count: " + this.m_imageBlockList.Count);
			UnityEngine.Debug.Log("Animation Loop Count (0 is infinite): " + this.m_appEx.loopCount);
			if (this.m_graphicCtrlExList != null && this.m_graphicCtrlExList.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder("Animation Delay Time (1/100sec)");
				for (int i = 0; i < this.m_graphicCtrlExList.Count; i++)
				{
					stringBuilder.Append(", ");
					stringBuilder.Append(this.m_graphicCtrlExList[i].m_delayTime);
				}
				UnityEngine.Debug.Log(stringBuilder.ToString());
			}
			UnityEngine.Debug.Log("Application Identifier: " + this.m_appEx.applicationIdentifier);
			UnityEngine.Debug.Log("Application Authentication Code: " + this.m_appEx.applicationAuthenticationCode);
		}

		public byte m_sig0;

		public byte m_sig1;

		public byte m_sig2;

		public byte m_ver0;

		public byte m_ver1;

		public byte m_ver2;

		public ushort m_logicalScreenWidth;

		public ushort m_logicalScreenHeight;

		public bool m_globalColorTableFlag;

		public int m_colorResolution;

		public bool m_sortFlag;

		public int m_sizeOfGlobalColorTable;

		public byte m_bgColorIndex;

		public byte m_pixelAspectRatio;

		public List<byte[]> m_globalColorTable;

		public List<UniGif.ImageBlock> m_imageBlockList;

		public List<UniGif.GraphicControlExtension> m_graphicCtrlExList;

		public List<UniGif.CommentExtension> m_commentExList;

		public List<UniGif.PlainTextExtension> m_plainTextExList;

		public UniGif.ApplicationExtension m_appEx;

		public byte m_trailer;
	}

	private struct ImageBlock
	{
		public byte m_imageSeparator;

		public ushort m_imageLeftPosition;

		public ushort m_imageTopPosition;

		public ushort m_imageWidth;

		public ushort m_imageHeight;

		public bool m_localColorTableFlag;

		public bool m_interlaceFlag;

		public bool m_sortFlag;

		public int m_sizeOfLocalColorTable;

		public List<byte[]> m_localColorTable;

		public byte m_lzwMinimumCodeSize;

		public List<UniGif.ImageBlock.ImageDataBlock> m_imageDataList;

		public struct ImageDataBlock
		{
			public byte m_blockSize;

			public byte[] m_imageData;
		}
	}

	private struct GraphicControlExtension
	{
		public byte m_extensionIntroducer;

		public byte m_graphicControlLabel;

		public byte m_blockSize;

		public ushort m_disposalMethod;

		public bool m_transparentColorFlag;

		public ushort m_delayTime;

		public byte m_transparentColorIndex;

		public byte m_blockTerminator;
	}

	private struct CommentExtension
	{
		public byte m_extensionIntroducer;

		public byte m_commentLabel;

		public List<UniGif.CommentExtension.CommentDataBlock> m_commentDataList;

		public struct CommentDataBlock
		{
			public byte m_blockSize;

			public byte[] m_commentData;
		}
	}

	private struct PlainTextExtension
	{
		public byte m_extensionIntroducer;

		public byte m_plainTextLabel;

		public byte m_blockSize;

		public List<UniGif.PlainTextExtension.PlainTextDataBlock> m_plainTextDataList;

		public struct PlainTextDataBlock
		{
			public byte m_blockSize;

			public byte[] m_plainTextData;
		}
	}

	private struct ApplicationExtension
	{
		public string applicationIdentifier
		{
			get
			{
				char[] value = new char[]
				{
					(char)this.m_appId1,
					(char)this.m_appId2,
					(char)this.m_appId3,
					(char)this.m_appId4,
					(char)this.m_appId5,
					(char)this.m_appId6,
					(char)this.m_appId7,
					(char)this.m_appId8
				};
				return new string(value);
			}
		}

		public string applicationAuthenticationCode
		{
			get
			{
				char[] value = new char[]
				{
					(char)this.m_appAuthCode1,
					(char)this.m_appAuthCode2,
					(char)this.m_appAuthCode3
				};
				return new string(value);
			}
		}

		public int loopCount
		{
			get
			{
				if (this.m_appDataList == null || this.m_appDataList.Count < 1 || this.m_appDataList[0].m_applicationData.Length < 3 || this.m_appDataList[0].m_applicationData[0] != 1)
				{
					return 0;
				}
				return (int)BitConverter.ToUInt16(this.m_appDataList[0].m_applicationData, 1);
			}
		}

		public byte m_extensionIntroducer;

		public byte m_extensionLabel;

		public byte m_blockSize;

		public byte m_appId1;

		public byte m_appId2;

		public byte m_appId3;

		public byte m_appId4;

		public byte m_appId5;

		public byte m_appId6;

		public byte m_appId7;

		public byte m_appId8;

		public byte m_appAuthCode1;

		public byte m_appAuthCode2;

		public byte m_appAuthCode3;

		public List<UniGif.ApplicationExtension.ApplicationDataBlock> m_appDataList;

		public struct ApplicationDataBlock
		{
			public byte m_blockSize;

			public byte[] m_applicationData;
		}
	}
}
