/**************************************************************************/
/*! @file   ScreenUtility.cs
@brief  プラットフォーム間依存を吸収する解像度ユーティリティ
***************************************************************************/
using UnityEngine;


public static class ScreenUtility
{
    public static readonly int UI_WIDTH = 1200;
    public static readonly int UI_HEIGHT = 750;
    public static readonly float UI_RATIO = (float)UI_HEIGHT / (float)UI_WIDTH;
    public static readonly int UI_BG_WIDTH = 1334;
    public static readonly int UI_BG_HEIGHT = 750;
    public static readonly float UI_BG_RATIO = (float)UI_BG_HEIGHT / (float)UI_BG_WIDTH;

    public static readonly int DEFAULT_WIDTH = Screen.width;
    public static readonly int DEFAULT_HEIGHT = Screen.height;

    public static readonly float RESOLUTION_LOW = 0.55f;
    public static readonly float RESOLUTION_HIGH = 1.0f;

    public static Rect GetSafeArea()
    {
        // レイアウト調整する場合はデフォルトのサイズにする（非実行時）
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            Rect dummy_area = new Rect(0.0f, 0.0f, UI_WIDTH, UI_HEIGHT);
            return dummy_area;
        }
#endif //UNITY_EDITOR


        var area = Screen.safeArea;

#if UNITY_EDITOR

        // iPhoneXをエディタでシュミレート
        bool emulateIPhoneX = false;
        if (WIDTH == 1125 && HEIGHT == 2436 || WIDTH == 2436 && HEIGHT == 1125)
        {
            emulateIPhoneX = true;
        }

        if (emulateIPhoneX)
        {
            Vector2 positionOffset;
            Vector2 sizeOffset;
            //縦持ち
            if (WIDTH < HEIGHT)
            {
                positionOffset = new Vector2(0f, area.size.y * 34f / 812f);
                sizeOffset = positionOffset + new Vector2(0f, area.size.y * 44f / 812f);
            }
            //横持ち
            else
            {
                positionOffset = new Vector2(area.size.x * 44f / 812f, area.size.y * 21f / 375f);
                sizeOffset = positionOffset + new Vector2(area.size.x * 44f / 812f, 0f);
            }
            area.position = area.position + positionOffset;
            area.size = area.size - sizeOffset;
        }

#endif

        //// セーフエリア適応
        //if (area.width != WIDTH || area.height != HEIGHT)
        //{
        //    enable = true;
        //}

        return area;
    }

    public static void SetResolution(float scale)
    {
        if (scale > 1.0f || scale <= 0.0f)
        {
            Debug.LogError(">>>>> ScalseResolution(): scale[" + scale + "]");
            return;
        }

#if !UNITY_EDITOR
            Screen.SetResolution((int)((float)DEFAULT_WIDTH * scale), (int)((float)DEFAULT_HEIGHT * scale), true);
#endif // UNITY_EDITOR
    }



#if (!UNITY_EDITOR && UNITY_STANDALONE_WIN)
        static float            mDefaultAspectRatio;
        static int              mDefaultScreenWidth;
        static int              mDefaultScreenHeight;
        
        public static float     AcpectRatio             { get { return mDefaultAspectRatio;             } }
        public static int       WIDTH                   { get { return mDefaultScreenWidth;             } }
        public static int       HEIGHT                  { get { return mDefaultScreenHeight;            } }
        
        public static int       DefaultScreenWidth      { get { return mDefaultScreenWidth; } }
        public static int       DefaultScreenHeight     { get { return mDefaultScreenHeight; } }
        public static float     ScreenWidthScale        { get { return (float)mDefaultScreenWidth / Screen.width; } }
        public static float     ScreenHeightScale       { get { return (float)mDefaultScreenHeight / Screen.height; } }
        
        // Windows版専用プロパティ
        public const int        MIN_WINDOW_WIDTH        = 480;       // ウィンドウの最小サイズ(16 : 9)
        public const int        MIN_WINDOW_HEIGHT       = 270;
        public const int        DEFAULT_WINDOW_WIDTH    = 1920;  // ウィンドウの最大サイズ(16 : 9)
        public const int        DEFAULT_WINDOW_HEIGHT   = 1080;
        public const float      ASPECT_RATIO            = (float)MIN_WINDOW_WIDTH / (float)MIN_WINDOW_HEIGHT;
        
        static ScreenUtility( )
        {
            mDefaultScreenWidth = DEFAULT_WINDOW_WIDTH;
            mDefaultScreenHeight = DEFAULT_WINDOW_HEIGHT;
            mDefaultAspectRatio = (float)mDefaultScreenWidth / (float)mDefaultScreenHeight;
        }
        
        public static void SetResolution(int w, int h)
        {
            Screen.SetResolution( w, h, false );
        }
#else

    //public static float     AcpectRatio             { get { return (float)Width / (float)Height;    } }
    public static int WIDTH { get { return Screen.width; } }
    public static int HEIGHT { get { return Screen.height; } }

#endif

    public static float RATIO { get { return (float)HEIGHT / (float)WIDTH; } }
}

