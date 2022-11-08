/**************************************************************************/
/*! @file   DebugUtility.cs
    @brief  デバッグユーティリティ
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using UnityObject = UnityEngine.Object;

//=========================================================================
//. デバッグユーティリティ
//=========================================================================

    public static class DebugUtility
    {
        // ログタグ
        public static string            LOGTAG_API              = "API";
        public static string            LOGTAG_API_REQUEST      = "API_REQUEST";
        public static string            LOGTAG_API_RESPONSE     = "API_RESPONSE";
        public static string            LOGTAG_VIEW             = "VIEW";
        public static string            LOGTAG_ASSETMANAGER     = "ASSET";
        public static string            LOGTAG_SOUNDMANAGER     = "SOUND";
        public static string            LOGTAG_THREADMANAGER    = "THREAD";
        public static string            LOGTAG_ERROR            = "ERROR";

        //=========================================================================
        //. メンバ
        //=========================================================================

        // レイキャストオーダー
        public static int               RAYCASTER_ORDER_MENU    = 32700;
        public static int               RAYCASTER_ORDER_WINDOW  = 32710;
        
        // デバッグ用の画面サイズ
        public static float             GUISCREEN_WIDTH         = 960;
        public static float             GUISCREEN_HEIGHT        = 640;
        
        //=========================================================================
        //. GUIレイアウト
        //=========================================================================
        #region GUIレイアウト
        
        public static Matrix4x4 GUI_matrix      = Matrix4x4.identity;
        public static Matrix4x4 GUI_matrixInv   = Matrix4x4.identity;
        
        /// ***********************************************************************
        /// <summary>
        /// GUIレイアウトを指定サイズに合わせる
        /// </summary>
        /// ***********************************************************************
        public static void BeingScreenSize( float width, float height, float scale = 1.0f )
        {
            Vector2 screen      = new Vector2( ScreenUtility.WIDTH, ScreenUtility.HEIGHT );
            Vector2 guiScreen   = new Vector2( width, height );
            guiScreen.x = width;
            guiScreen.y = guiScreen.x * ( screen.y / screen.x );
            if( ( guiScreen.y - height ) * 0.5f < 0 )
            {
                guiScreen.x = height * ( screen.x / screen.y );
                guiScreen.y = height;
            }
            float ratio = ( (float)ScreenUtility.WIDTH / guiScreen.x );
            float ratio_h = ( (float)ScreenUtility.HEIGHT / guiScreen.y );
            Vector2 pos = Vector2.zero;
            if( guiScreen.x > width )
            {
                pos.x = ( screen.x - ( width * ratio ) ) * 0.5f;
            }
            if( guiScreen.y > height )
            {
                pos.y = ( screen.y - ( height * ratio_h ) ) * 0.5f;
            }
            GUI.matrix = Matrix4x4.TRS( pos, Quaternion.identity, new Vector3( ratio, ratio_h, 1f ) * scale );
            GUI_matrix = GUI.matrix;
            GUI_matrixInv = GUI.matrix.inverse;
        }
        
        /// ***********************************************************************
        /// <summary>
        /// GUIレイアウト元に戻す
        /// </summary>
        /// ***********************************************************************
        public static void EndScreenSize( )
        {
            GUI.matrix = Matrix4x4.identity;
            GUI_matrix = Matrix4x4.identity;
        }
        
        #endregion
        
        //=========================================================================
        //. 静的メンバ
        //=========================================================================
        #region 静的メンバ

        public static IDebugLogEmitter LogEmitter = null;
        private static bool g_LogDisable = false;
        // static DebugMenu m_DebugMenu = null;
        // public static DebugMenu debugMenu { get { return m_DebugMenu; } }

        /// ***********************************************************************
        /// <summary>
        /// デバッグログ出力するかどうか(Warning,Error以外のログ)
        /// </summary>
        /// ***********************************************************************
        public static bool isLogDisable
        {
            set
            {
                g_LogDisable = value;
            }
            get
            {
                return g_LogDisable;
            }
        }

        /// ***********************************************************************
        /// <summary>
        /// デバッグビルドか確認する
        /// </summary>
        /// ***********************************************************************
        public static bool isDebugBuild
        {
            get
            {
                #if UNITY_EDITOR
                return true;
                #else
                return UnityEngine.Debug.isDebugBuild;
                #endif
            }
        }

        static string PrependTag(string tag, string text)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return text;
            }

            return ColorizeTag(tag) + text;
        }

        public static string ColorizeTag(string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return "";
            }

            var color = 0;  // 0xrrggbb
            for (var i = 0; i < tag.Length; ++i)
            {
                var j = i % 6 * 4;
                var m = 0xf << j;
                var c = (color & m) >> j;
                c = (c + (int)tag[i]) & 0xf;
                color = (color & ~m) | (c << j);
            }
            for (var i = 0; i < 3; ++i)
            {
                var j = i * 8;
                var m = 0xff << j;
                var c = (color & m) >> j;
                c += 0x40;
                if (c > 0xff) c = 0xff;
                color = (color & ~m) | (c << j);
            }
            return string.Format("<color=#{0:X6}>[{1}]</color> ", color, tag);
        }

        /// ***********************************************************************
        /// <summary>
        /// ログ
        /// </summary>
        /// ***********************************************************************
        [Conditional("DEBUG_BUILD")]
        public static void Log(string tag, string text)
        {
            if( isLogDisable ) return;

            #if UNITY_EDITOR
            if (IsSuppressedMessage(text))
            {
                return;
            }
            #endif

            if (LogEmitter == null)
            {
                UnityEngine.Debug.Log(PrependTag(tag, text));
            }
            else
            {
                LogEmitter.Log(tag, text);
            }
        }

        /// ***********************************************************************
        /// <summary>
        /// ログ
        /// </summary>
        /// ***********************************************************************
        [Conditional("DEBUG_BUILD")]
        public static void Log(string text, UnityObject o)
        {
            if( isLogDisable ) return;

            #if UNITY_EDITOR
            if (IsSuppressedMessage(text))
            {
                return;
            }
            #endif

            var name = o == null ? "" : o.GetType().Name;
            if (LogEmitter == null)
            {
                UnityEngine.Debug.Log(PrependTag(name, text), o);
            }
            else
            {
                LogEmitter.Log(name, text);
            }
        }

        static readonly char[] DirSep = new char[] { '/', '\\' };

        static string GetNameFromPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return "";

            var i = path.LastIndexOfAny(DirSep);
            var j = path.LastIndexOf('.');
            if (i == -1)
            {
                if (j == -1)
                {
                    return path;
                }

                return path.Substring(0, j);
            }

            if (j == -1)
            {
                return path.Substring(i + 1);
            }

            return path.Substring(i + 1, j - i - 1);
        }

        [Conditional("DEBUG_BUILD")]
        public static void Log( string text, [CallerLineNumber] int _line = -1, [CallerFilePath] string _file = "" )
        {
            if( isLogDisable ) return;

            Log(GetNameFromPath(_file), text);
        }

        [Conditional("DEBUG_BUILD")]
        public static void LogWithoutTrace( string text )
        {
            if( isLogDisable ) return;

            var bak = Application.GetStackTraceLogType(LogType.Log);
            try
            {
                Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
                Log("", text);
            }
            finally
            {
                Application.SetStackTraceLogType(LogType.Log, bak);
            }
        }

        public static void LogSystem(string tag, string text)
        {
            if( LogEmitter == null )
            {
                UnityEngine.Debug.Log(text);
            }
            else
            {
                LogEmitter.LogSystem(tag, text);
            }
        }

        public static void LogWarning( string tag, string text )
        {
            if( LogEmitter == null )
            {
                UnityEngine.Debug.LogWarning(PrependTag(tag, text));
            }
            else
            {
                LogEmitter.LogWarning(tag, text);
            }
        }

        public static void LogWarning( string text, UnityObject o )
        {
            var name = o == null ? "" : o.GetType().Name;
            if( LogEmitter == null )
            {
                UnityEngine.Debug.LogWarning(PrependTag(name, text), o);
            }
            else
            {
                LogEmitter.LogWarning(name, text);
            }
        }

        public static void LogWarning( string text, [CallerLineNumber] int _line = -1, [CallerFilePath] string _file = "" )
        {
            LogWarning(GetNameFromPath(_file), text);
        }

        public static void LogError( string tag, string text )
        {
            if( LogEmitter == null )
            {
                UnityEngine.Debug.LogError(PrependTag(tag, text));
            }
            else
            {
                LogEmitter.LogError(tag, text);
            }
        }

        public static void LogError( string text, UnityObject o )
        {
            var name = o == null ? "" : o.GetType().Name;
            if( LogEmitter == null )
            {
                UnityEngine.Debug.LogError(PrependTag(name, text), o);
            }
            else
            {
                LogEmitter.LogError(name, text);
            }
        }

        public static void LogError( string text, [CallerLineNumber] int _line = -1, [CallerFilePath] string _file = "" )
        {
            LogError(GetNameFromPath(_file), text);
        }

        public static void LogError( System.Exception e, [CallerLineNumber] int _line = -1, [CallerFilePath] string _file = "" )
        {
            var text = e.ToString() + "\n" + e.StackTrace;
            LogError(GetNameFromPath(_file), text);
        }

        public static void LogRandom( string tag, string text )
        {
            if( isLogDisable ) return;

            if( LogEmitter == null )
            {
                UnityEngine.Debug.Log(PrependTag(tag, text));
            }
            else
            {
                LogEmitter.LogRandom(tag, text);
            }
        }

        public static void LogRandom( string text )
        {
            if( isLogDisable ) return;

            LogRandom("", text);
        }

        // ネットワークの場合は部分的に情報抜き出し：リクエスト
        public static void LogNetwork(string tag, string text)
        {
            if( LogEmitter == null )
            {
                UnityEngine.Debug.Log(PrependTag(tag, text));
            }
            else
            {
                LogEmitter.LogNetwork(tag, text);
            }
        }

        #endregion

        //=========================================================================
        //. ビルド共通
        //=========================================================================

        static Application.LogCallback m_LogCallbacks;

        /// ***********************************************************************
        /// <summary>
        /// 例外
        /// </summary>
        /// ***********************************************************************
        public static void Assert<T>( bool cond, string format, T arg )
        {
            if( !cond )
            {
                Assert( string.Format(format, arg) );
            }
        }

        public static void Assert<T1, T2>( bool cond, string format, T1 arg1, T2 arg2 )
        {
            if( !cond )
            {
                Assert( string.Format(format, arg1, arg2) );
            }
        }

        public static void Assert(bool cond, string msg )
        {
            if( !cond )
            {
                Assert( msg );
            }
        }

       
        public static void Assert( string msg )
        {
            throw new System.Exception( "[Assertion Failed] " + msg );
        }

        public static void AssertWarning<T>( bool cond, string format, T arg )
        {
            if( !cond )
            {
                LogWarning("[Assertion Failed] " + string.Format(format, arg));
            }
        }

        public static void AssertWarning( bool cond, string msg, [CallerFilePath] string _file = "", [CallerLineNumber] int _line = -1 )
        {
            if( !cond )
            {
                var location = " @" + Path.GetFileName(_file) + ":" + _line.ToString();
                LogWarning("[Assertion Failed] " + msg + location);
            }
        }

        public static void NotNull<T>(  T target, string name, [CallerFilePath] string _file = "", [CallerLineNumber] int _line = -1 )
            where T : class
        {
            if (target == null)
            {
                var location = "@" + Path.GetFileName(_file) + ":" + _line.ToString();
                LogError($"[NotNull Assertion Failed] {name} :{typeof(T)} {location}");
            }
        }

        [Conditional("DEBUG_BUILD")]
        public static void AssertDebug( bool cond, string msg, [CallerFilePath] string _file = "", [CallerLineNumber] int _line = -1 )
        {
            if( !cond )
            {
                var location = " @" + Path.GetFileName(_file) + ":" + _line.ToString();
                LogError("[Assertion Failed] " + msg + location);
            }
        }

        [Conditional("DEBUG_BUILD")]
        public static void AssertDebug<T>( bool cond, string msg, T arg, [CallerFilePath] string _file = "", [CallerLineNumber] int _line = -1 )
        {
            if( !cond )
            {
                var location = " @" + Path.GetFileName(_file) + ":" + _line.ToString();
                LogError("[Assertion Failed] " + string.Format(msg, arg) + location);
            }
        }

        /// ***********************************************************************
        /// <summary>
        /// ログコールバック
        /// </summary>
        /// ***********************************************************************
        static void HandleLog( string logString, string stackTrace, LogType type )
        {
            if( m_LogCallbacks != null )
            {
                m_LogCallbacks.Invoke( logString, stackTrace, type );
            }
        }
        // ログコールバック登録
        public static void RegisterLogCallback( Application.LogCallback callback )
        {
            if( m_LogCallbacks != null )
            {
                m_LogCallbacks += callback;
            }
            else
            {
                m_LogCallbacks = callback;
            }
            Application.logMessageReceived += HandleLog;
        }
        // ログコールバック解除
        public static void UnregisterLogCallback( Application.LogCallback callback )
        {
            m_LogCallbacks -= callback;
        }

        #if UNITY_EDITOR
        static string[] suppressedPatterns = null;

        static bool IsSuppressedMessage(string message)
        {
            if (suppressedPatterns == null)
            {
                var filePath = Application.dataPath + "/../TempEditor/suppress_messages.txt";
                if (File.Exists(filePath))
                {
                    var list = new List<string>();
                    foreach (var s in File.ReadAllText(filePath).Split(new[] {'\r', '\n'}))
                    {
                        if (!string.IsNullOrEmpty(s)) list.Add(s);
                    }
                    suppressedPatterns = list.ToArray();
                }
                else
                {
                    suppressedPatterns = new string[0];
                }
            }
            foreach (var pattern in suppressedPatterns)
            {
                if (message.StartsWith(pattern))
                {
                    return true;
                }
            }
            return false;
        }
        #endif
    }

    public interface IDebugLogEmitter
    {
        void Log(string tag, string text);
        void LogWarning(string tag, string text);
        void LogError(string tag, string text);
        void LogNetwork(string tag, string text);
        void LogRandom(string tag, string text);
        void LogSystem(string tag, string text);
    }
