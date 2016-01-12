namespace Client
{
    using Client.Targeting;
    using Microsoft.DirectX.Direct3D;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class Renderer
    {
        private const int A_255 = -16777216;
        private const int A_FULL = -16777216;
        public static int blockHeight = 7;
        public static int blockWidth = 7;
        public static int cellHeight = (blockHeight << 3);
        public static int cellWidth = (blockWidth << 3);
        public const int CF_STRETCH = 1;
        public static int eOffsetX;
        public static int eOffsetY;
        public const int FALSE = 0;
        private static bool m_AAEnable;
        public static int m_ActFrames;
        public static int m_Alpha;
        public static bool m_AlphaEnable;
        private static int m_AlphaStateCount;
        private static ArrayList m_AlphaStates = new ArrayList();
        private static bool m_AlphaTestEnable;
        private static int m_AlwaysHighlight;
        private static DrawBlendType m_BlendType;
        private static bool m_CanAADependent;
        private static bool m_CanAAEdges;
        private static bool m_CanAAIndependent;
        private static bool m_CanAntiAlias;
        private static bool m_CanCullCW;
        private static bool m_CanCullNone;
        private static int m_CharX = -1024;
        private static int m_CharY = -1024;
        private static int m_CharZ = -1024;
        private static bool m_ColorAlphaEnable;
        private static int m_Count;
        private static int[] m_Counts = new int[8];
        private static bool m_CullEnable = true;
        private static bool m_CullLand;
        private static bool m_CurAlphaTest;
        private static DrawBlendType m_CurBlendType;
        public static bool m_Dead;
        public static bool m_DeathOverride;
        private static bool m_DrawFPS;
        private static bool m_DrawGrid;
        private static bool m_DrawPCount;
        private static bool m_DrawPing;
        private static bool m_EdgeAAEnable;
        public static float m_fAlpha;
        public static bool m_FilterEnable;
        public static Rectangle m_FoliageCheck = new Rectangle((Engine.ScreenWidth / 2) - 0x16, (Engine.ScreenHeight / 2) - 60, 0x2c, 0x52);
        public static int m_Frames;
        private static CustomVertex.TransformedColoredTextured[] m_GeoPool;
        public static IHue m_hGray;
        private static bool m_Invalidate;
        private static Mobile m_LastEnemy;
        private static ICell m_LastFind;
        private static Hues.HTemperedHue m_LightBlue;
        private static Hues.HTemperedHue m_LightRed;
        private static byte[] m_LineByteBuffer;
        private static ArrayList[] m_Lists = new ArrayList[8];
        private static bool m_MiniHealth = true;
        private static Queue m_MiniHealthQueue;
        private static System.Drawing.Point m_MousePoint = System.Drawing.Point.Empty;
        private static System.Drawing.Point[] m_PointPool = new System.Drawing.Point[4];
        private static byte[] m_QuadByteBuffer;
        private static ArrayList m_RectsList;
        public static string m_ScreenShot;
        public static Client.Texture m_TextSurface;
        public static ArrayList m_TextToDraw;
        private static ArrayList m_TextToDrawList;
        private static Client.Texture m_Texture;
        private static Queue m_ToUpdateQueue;
        private static Queue m_TransDrawQueue;
        private static bool m_Transparency;
        public static int m_Version = 0;
        public static CustomVertex.TransformedColoredTextured[][][] m_VertexPool;
        private static BufferedVertexStream m_VertexStream;
        private static CustomVertex.TransformedColoredTextured[] m_vFriendPool = VertexConstructor.Create();
        private static CustomVertex.TransformedColoredTextured[] m_vHaloPool = VertexConstructor.Create();
        private static CustomVertex.TransformedColoredTextured[] m_vMultiPool;
        public static Client.VertexCache m_vTextCache;
        private static CustomVertex.TransformedColoredTextured[] m_vTransDrawPool;
        private static bool m_WasDead;
        public static int m_xBaseLast;
        public static int m_xScroll;
        private static int m_xServerEnd;
        private static int m_xServerStart;
        public static int m_xWorld;
        public static int m_yBaseLast;
        public static int m_yScroll;
        private static int m_yServerEnd;
        private static int m_yServerStart;
        public static int m_yWorld;
        public static int m_zWorld;
        private const double r21 = 0.047619047619047616;
        private static System.Type tCorpseCell = typeof(CorpseCell);
        private static System.Type tDynamicItem = typeof(DynamicItem);
        private static System.Type tLandTile = typeof(LandTile);
        private static System.Type tMobileCell = typeof(MobileCell);
        public const int TRUE = 1;
        private static System.Type tStaticItem = typeof(StaticItem);
        public const int VertexBufferLength = 0x8000;
        private static int xLast;
        private static int xwLast;
        private static int yLast;
        private static int ywLast;
        private static int zwLast;

        [DllImport("gdi32")]
        private static extern int BitBlt(IntPtr hDestDC, int x, int y, int w, int h, IntPtr hSrcDC, int xSrc, int ySrc, int raster);

        [DllImport("User32")]
        private static extern int ClientToScreen(IntPtr Handle, ref int X, ref int Y);

        public static void Draw()
        {
            try
            {
                DrawUnsafe();
            }
            catch (Exception exception)
            {
                Debug.Error(exception);
            }
        }

        public static void DrawColoredIcon(int x, int y, int color, bool formation, bool target)
        {
            if (!m_AlphaEnable)
            {
                SetAlphaEnablePrecalc(true);
            }
            if (m_FilterEnable)
            {
                SetFilterEnablePrecalc(false);
            }
            SetAlpha(1f);
            ColorAlphaEnable = true;
            Client.Texture texture = target ? Engine.m_TargetImage : Engine.m_Friend;
            if (formation)
            {
                int num = x;
                int num2 = y;
                int num3 = (m_Frames * 3) % 80;
                if (num3 > 40)
                {
                    num3 = 40 - (num3 - 40);
                }
                texture.DrawRotated(num, (num2 - 40) + num3, 0.0, color);
                texture.DrawRotated(num - ((int)((40 - num3) * 0.39073112848927377)), num2 - ((int)((40 - num3) * 0.92050485345244037)), 157.0, color);
                texture.DrawRotated(num + ((int)((40 - num3) * 0.39073112848927377)), num2 - ((int)((40 - num3) * 0.92050485345244037)), -157.0, color);
            }
            else
            {
                int num4 = y - (texture.Height / (target ? 2 : 1));
                texture.DrawGame(x - (texture.Width >> 1), num4, color, m_vFriendPool);
            }
            ColorAlphaEnable = false;
        }

        public static unsafe void DrawLine(CustomVertex.TransformedColoredTextured v1, CustomVertex.TransformedColoredTextured v2, int color)
        {
            CustomVertex.TransformedColoredTextured[] texturedArray = new CustomVertex.TransformedColoredTextured[] { v1, v2 };
            texturedArray[0].Color = texturedArray[1].Color = GetQuadColor(color);
            fixed (CustomVertex.TransformedColoredTextured* texturedRef = texturedArray)
            {
                PushLineStrip(texturedRef, 2);
            }
        }

        public static unsafe void DrawLine(int X1, int Y1, int X2, int Y2)
        {
            CustomVertex.TransformedColoredTextured[] texturedArray = GeoPool(2);
            texturedArray[0].X = X1;
            texturedArray[0].Y = Y1;
            texturedArray[1].X = X2;
            texturedArray[1].Y = Y2;
            fixed (CustomVertex.TransformedColoredTextured* texturedRef = texturedArray)
            {
                PushLineStrip(texturedRef, 2);
            }
        }

        public static unsafe void DrawLine(int X1, int Y1, int X2, int Y2, int color)
        {
            CustomVertex.TransformedColoredTextured[] texturedArray = GeoPool(2);
            texturedArray[0].X = X1;
            texturedArray[0].Y = Y1;
            texturedArray[1].X = X2;
            texturedArray[1].Y = Y2;
            texturedArray[0].Color = texturedArray[1].Color = GetQuadColor(color);
            fixed (CustomVertex.TransformedColoredTextured* texturedRef = texturedArray)
            {
                PushLineStrip(texturedRef, 2);
            }
        }

        public static unsafe void DrawLines(CustomVertex.TransformedColoredTextured[] v)
        {
            v[0].Color = v[1].Color = v[2].Color = v[3].Color = v[4].Color = GetQuadColor(v[0].Color);
            fixed (CustomVertex.TransformedColoredTextured* texturedRef = v)
            {
                PushLineStrip(texturedRef, v.Length);
            }
        }

        public static void DrawMapLine(LandTile[,] landTiles, int bx, int by, int x, int y, int x2, int y2)
        {
            SetTexture(null);
            if (m_AlphaEnable)
            {
                SetAlphaEnablePrecalc(false);
            }
            if (m_FilterEnable)
            {
                SetFilterEnablePrecalc(false);
            }
            AlphaTestEnable = false;
            DrawLine(bx + 0x16, by - (landTiles[x, y].m_Z << 2), (bx + 0x16) + ((x2 - y2) * 0x16), (by + 0x16) - (landTiles[x + x2, y + y2].m_Z << 2), 0x40ff40);
        }

        public static unsafe void DrawPoints(params Client.Point[] points)
        {
            int length = points.Length;
            CustomVertex.TransformedColoredTextured[] texturedArray = GeoPool(length);
            for (int i = 0; i < length; i++)
            {
                texturedArray[i].X = 0.5f + points[i].X;
                texturedArray[i].Y = 0.5f + points[i].Y;
            }
            fixed (CustomVertex.TransformedColoredTextured* texturedRef = texturedArray)
            {
                PushPointList(texturedRef, length);
            }
        }

        public static unsafe void DrawQuadPrecalc(CustomVertex.TransformedColoredTextured[] v)
        {
            fixed (CustomVertex.TransformedColoredTextured* texturedRef = v)
            {
                PushQuad(texturedRef);
            }
        }

        public static unsafe void DrawQuadPrecalc(CustomVertex.TransformedColoredTextured* pVertex)
        {
            PushQuad(pVertex);
        }

        public static unsafe void DrawUnsafe()
        {
            if ((Engine.m_Device != null) && Validate())
            {
                Stats.Reset();
                Engine.m_Device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, Color.Black, 1f, 0);
                Queue toUpdateQueue = m_ToUpdateQueue;
                if (toUpdateQueue == null)
                {
                    toUpdateQueue = m_ToUpdateQueue = new Queue();
                }
                else if (toUpdateQueue.Count > 0)
                {
                    toUpdateQueue.Clear();
                }
                Mobile mobile = null;
                Engine.m_Device.BeginScene();
                try
                {
                    int num = 0;
                    int num2 = 0;
                    int num3 = 0;
                    bool preserveHue = false;
                    bool flag2 = false;
                    m_xWorld = m_yWorld = m_zWorld = 0;
                    Mobile player = World.Player;
                    if (player != null)
                    {
                        preserveHue = player.Ghost;
                        flag2 = player.Flags[MobileFlag.Warmode];
                        m_xWorld = num = player.X;
                        m_yWorld = num2 = player.Y;
                        m_zWorld = num3 = player.Z;
                    }
                    m_Dead = preserveHue;
                    m_xScroll = 0;
                    m_yScroll = 0;
                    ArrayList textToDrawList = m_TextToDrawList;
                    if (textToDrawList == null)
                    {
                        textToDrawList = m_TextToDrawList = new ArrayList();
                    }
                    else if (textToDrawList.Count > 0)
                    {
                        textToDrawList.Clear();
                    }
                    m_TextToDraw = textToDrawList;
                    Queue miniHealthQueue = m_MiniHealthQueue;
                    if (miniHealthQueue == null)
                    {
                        miniHealthQueue = m_MiniHealthQueue = new Queue();
                    }
                    else if (miniHealthQueue.Count > 0)
                    {
                        miniHealthQueue.Clear();
                    }
                    eOffsetX = 0;
                    eOffsetY = 0;
                    if (Engine.m_Ingame)
                    {
                        if (GDesktopBorder.Instance != null)
                        {
                            GDesktopBorder.Instance.DoRender();
                        }
                        SetViewport(Engine.GameX, Engine.GameY, Engine.GameWidth, Engine.GameHeight);
                        if (m_VertexPool == null)
                        {
                            m_VertexPool = new CustomVertex.TransformedColoredTextured[cellWidth][][];
                            for (int m = 0; m < cellWidth; m++)
                            {
                                m_VertexPool[m] = new CustomVertex.TransformedColoredTextured[cellHeight][];
                                for (int n = 0; n < cellHeight; n++)
                                {
                                    m_VertexPool[m][n] = VertexConstructor.Create();
                                }
                            }
                        }
                        Map.Lock();
                        MapPackage package = Map.GetMap((num >> 3) - (blockWidth >> 1), (num2 >> 3) - (blockHeight >> 1), blockWidth, blockHeight);
                        int num6 = ((num >> 3) - (blockWidth >> 1)) << 3;
                        int num7 = ((num2 >> 3) - (blockHeight >> 1)) << 3;
                        ArrayList[,] cells = package.cells;
                        LandTile[,] landTiles = package.landTiles;
                        byte[,] buffer = package.flags;
                        int[,] colorMap = package.colorMap;
                        int num8 = num & 7;
                        int num9 = num2 & 7;
                        int num10 = ((blockWidth / 2) * 8) + num8;
                        int num11 = ((blockHeight / 2) * 8) + num9;
                        int num12 = 0;
                        int num13 = 0;
                        num12 = Engine.GameWidth >> 1;
                        num12 -= 0x16;
                        num12 += (4 - num8) * 0x16;
                        num12 -= (4 - num9) * 0x16;
                        num13 += (4 - num8) * 0x16;
                        num13 += (4 - num9) * 0x16;
                        num13 += num3 << 2;
                        num13 += ((((Engine.GameHeight >> 1) - ((num10 + num11) * 0x16)) - ((4 - num9) * 0x16)) - ((4 - num8) * 0x16)) - 0x16;
                        num12--;
                        num13--;
                        num12 += Engine.GameX;
                        num13 += Engine.GameY;
                        Mobile mobile3 = World.Player;
                        bool flag3 = false;
                        m_xScroll = m_yScroll = 0;
                        if ((player != null) && (player.Walking.Count > 0))
                        {
                            WalkAnimation animation = (WalkAnimation)player.Walking.Peek();
                            int xOffset = 0;
                            int yOffset = 0;
                            int fOffset = 0;
                            if (!animation.Snapshot(ref xOffset, ref yOffset, ref fOffset))
                            {
                                if (!animation.Advance)
                                {
                                    xOffset = animation.xOffset;
                                    yOffset = animation.yOffset;
                                }
                                else
                                {
                                    xOffset = 0;
                                    yOffset = 0;
                                }
                            }
                            num12 -= xOffset;
                            num13 -= yOffset;
                            m_xScroll = xOffset;
                            m_yScroll = yOffset;
                        }
                        if (Engine.m_Quake)
                        {
                            Random random = new Random();
                            num12 += -2 + random.Next(5);
                            num13 += -2 + random.Next(5);
                        }
                        bool flag4 = (((!m_Invalidate && !Engine.m_Quake) && ((m_CharX == num) && (m_CharY == num2))) && (((m_CharZ == num3) && (m_WasDead == preserveHue)) && (m_xBaseLast == num12))) && (m_yBaseLast == num13);
                        m_xBaseLast = num12;
                        m_yBaseLast = num13;
                        m_Invalidate = false;
                        m_WasDead = preserveHue;
                        m_CharX = num;
                        m_CharY = num2;
                        m_CharZ = num3;
                        int z = 0x7fffffff;
                        int num18 = 0x7fffffff;
                        ArrayList list2 = new ArrayList();
                        int count = cells[num10 + 1, num11 + 1].Count;
                        for (int i = 0; i < count; i++)
                        {
                            ICell cell = (ICell)cells[num10 + 1, num11 + 1][i];
                            System.Type cellType = cell.CellType;
                            if ((cellType == tStaticItem) || (cellType == tDynamicItem))
                            {
                                ITile tile = (ITile)cell;
                                if ((Map.m_ItemFlags[tile.ID & 0x3fff][TileFlag.Roof] && (tile.Z >= (num3 + 15))) && (tile.Z < z))
                                {
                                    z = tile.Z;
                                }
                            }
                        }
                        count = cells[num10, num11].Count;
                        for (int j = 0; j < count; j++)
                        {
                            ICell cell2 = (ICell)cells[num10, num11][j];
                            System.Type type2 = cell2.CellType;
                            if (((type2 == tStaticItem) || (type2 == tDynamicItem)) || (type2 == tLandTile))
                            {
                                ITile tile2 = (ITile)cell2;
                                if (!Map.GetTileFlags(tile2.ID)[TileFlag.Roof])
                                {
                                    int num22 = (type2 == tLandTile) ? tile2.SortZ : tile2.Z;
                                    if (num22 >= (num3 + 15))
                                    {
                                        if (type2 == tLandTile)
                                        {
                                            if (num22 < z)
                                            {
                                                z = num22;
                                            }
                                            if ((num3 + 0x10) < num18)
                                            {
                                                num18 = num3 + 0x10;
                                            }
                                        }
                                        else if (num22 < z)
                                        {
                                            z = num22;
                                        }
                                    }
                                }
                            }
                        }
                        m_CullLand = num18 < 0x7fffffff;
                        IHue hue = preserveHue ? Hues.Grayscale : Hues.Default;
                        m_hGray = hue;
                        Queue transDrawQueue = m_TransDrawQueue;
                        if (transDrawQueue == null)
                        {
                            transDrawQueue = m_TransDrawQueue = new Queue();
                        }
                        else if (transDrawQueue.Count > 0)
                        {
                            transDrawQueue.Clear();
                        }
                        bool multiPreview = Engine.m_MultiPreview;
                        int num23 = 0;
                        int num24 = 0;
                        int num25 = 0;
                        int num26 = 0;
                        int num27 = 0;
                        int num28 = 0;
                        int num29 = 0;
                        int num30 = 0;
                        int num31 = 0;
                        int num32 = 0;
                        int num33 = 0;
                        int num34 = 0;
                        int num35 = 0;
                        IHue lightRed = null;
                        if (multiPreview)
                        {
                            if (Gumps.IsWorldAt(Engine.m_xMouse, Engine.m_yMouse, true))
                            {
                                short tileX = 0;
                                short tileY = 0;
                                ICell cell3 = FindTileFromXY(Engine.m_xMouse, Engine.m_yMouse, ref tileX, ref tileY, false);
                                if (cell3 == null)
                                {
                                    multiPreview = false;
                                }
                                else if ((cell3.CellType == tLandTile) || (cell3.CellType == tStaticItem))
                                {
                                    num23 = tileX - num6;
                                    num24 = tileY - num7;
                                    num25 = cell3.Z + ((cell3.CellType == tStaticItem) ? cell3.Height : 0);
                                    num27 = Engine.m_MultiList.Count;
                                    num23 -= Engine.m_xMultiOffset;
                                    num24 -= Engine.m_yMultiOffset;
                                    num25 -= Engine.m_zMultiOffset;
                                    num28 = num23 + Engine.m_MultiMinX;
                                    num29 = num24 + Engine.m_MultiMinY;
                                    num30 = num23 + Engine.m_MultiMaxX;
                                    num31 = num24 + Engine.m_MultiMaxY;
                                }
                                else
                                {
                                    multiPreview = false;
                                }
                            }
                            else
                            {
                                multiPreview = false;
                            }
                        }
                        else if (((Control.ModifierKeys & (Keys.Control | Keys.Shift)) == (Keys.Control | Keys.Shift)) && (((Gumps.LastOver is GSpellIcon) && (((GSpellIcon)Gumps.LastOver).m_SpellID == 0x39)) || ((Gumps.LastOver is GContainerItem) && ((((GContainerItem)Gumps.LastOver).Item.ID & 0x3fff) == 0x1f65))))
                        {
                            int num38 = 1 + ((int)(Engine.Skills[SkillName.Magery].Value / 15f));
                            if (m_LightRed == null)
                            {
                                m_LightRed = new Hues.HTemperedHue(Engine.C32216(0xff0000), 80);
                            }
                            lightRed = m_LightRed;
                            num32 = (player.X - num6) - num38;
                            num33 = (player.Y - num7) - num38;
                            num34 = (player.X - num6) + num38;
                            num35 = (player.Y - num7) + num38;
                        }
                        else if (Engine.TargetHandler is ServerTargetHandler)
                        {
                            ServerTargetHandler targetHandler = (ServerTargetHandler)Engine.TargetHandler;
                            IHue lightBlue = null;
                            int num39 = -1;
                            bool flag6 = false;
                            if ((targetHandler.Action == TargetAction.MeteorSwarm) || (targetHandler.Action == TargetAction.ChainLightning))
                            {
                                if (m_LightRed == null)
                                {
                                    m_LightRed = new Hues.HTemperedHue(Engine.C32216(0xff0000), 80);
                                }
                                lightBlue = m_LightRed;
                                num39 = 2;
                            }
                            else if (targetHandler.Action == TargetAction.MassCurse)
                            {
                                if (m_LightRed == null)
                                {
                                    m_LightRed = new Hues.HTemperedHue(Engine.C32216(0xff0000), 80);
                                }
                                lightBlue = m_LightRed;
                                num39 = 3;
                            }
                            else if (targetHandler.Action == TargetAction.Reveal)
                            {
                                if (m_LightBlue == null)
                                {
                                    m_LightBlue = new Hues.HTemperedHue(Engine.C32216(0x99ccff), 80);
                                }
                                lightBlue = m_LightBlue;
                                num39 = 1 + ((int)(Engine.Skills[SkillName.Magery].Value / 20f));
                            }
                            else if (targetHandler.Action == TargetAction.DetectHidden)
                            {
                                if (m_LightBlue == null)
                                {
                                    m_LightBlue = new Hues.HTemperedHue(Engine.C32216(0x99ccff), 80);
                                }
                                lightBlue = m_LightBlue;
                                num39 = (int)(Engine.Skills[SkillName.DetectingHidden].Value / 10f);
                            }
                            else if (targetHandler.Action == TargetAction.ArchProtection)
                            {
                                if (m_LightBlue == null)
                                {
                                    m_LightBlue = new Hues.HTemperedHue(Engine.C32216(0x99ccff), 80);
                                }
                                lightBlue = m_LightBlue;
                                num39 = Engine.Features.AOS ? 2 : 3;
                            }
                            else if (targetHandler.Action == TargetAction.ArchCure)
                            {
                                if (m_LightBlue == null)
                                {
                                    m_LightBlue = new Hues.HTemperedHue(Engine.C32216(0x99ccff), 80);
                                }
                                lightBlue = m_LightBlue;
                                num39 = 3;
                            }
                            else if (targetHandler.Action == TargetAction.WallOfStone)
                            {
                                if (m_LightRed == null)
                                {
                                    m_LightRed = new Hues.HTemperedHue(Engine.C32216(0xff0000), 80);
                                }
                                lightBlue = m_LightRed;
                                num39 = 1;
                                flag6 = true;
                            }
                            else if (((targetHandler.Action == TargetAction.EnergyField) || (targetHandler.Action == TargetAction.FireField)) || ((targetHandler.Action == TargetAction.ParalyzeField) || (targetHandler.Action == TargetAction.PoisonField)))
                            {
                                if (m_LightRed == null)
                                {
                                    m_LightRed = new Hues.HTemperedHue(Engine.C32216(0xff0000), 80);
                                }
                                lightBlue = m_LightRed;
                                num39 = 2;
                                flag6 = true;
                            }
                            if ((lightBlue != null) && Gumps.IsWorldAt(Engine.m_xMouse, Engine.m_yMouse, true))
                            {
                                short num40 = 0;
                                short num41 = 0;
                                ICell cell4 = FindTileFromXY(Engine.m_xMouse, Engine.m_yMouse, ref num40, ref num41, false);
                                if ((cell4 != null) && (((cell4.CellType == tLandTile) || (cell4.CellType == tStaticItem)) || ((cell4.CellType == tMobileCell) || (cell4.CellType == tDynamicItem))))
                                {
                                    lightRed = lightBlue;
                                    if (num39 >= 0)
                                    {
                                        if (flag6)
                                        {
                                            bool flag7;
                                            int num42 = player.X - num40;
                                            int num43 = player.Y - num41;
                                            int num44 = num42 - num43;
                                            int num45 = num42 + num43;
                                            if ((num44 >= 0) && (num45 >= 0))
                                            {
                                                flag7 = false;
                                            }
                                            else if (num44 >= 0)
                                            {
                                                flag7 = true;
                                            }
                                            else if (num45 >= 0)
                                            {
                                                flag7 = true;
                                            }
                                            else
                                            {
                                                flag7 = false;
                                            }
                                            if (flag7)
                                            {
                                                num32 = (num40 - num6) - num39;
                                                num34 = (num40 - num6) + num39;
                                                num33 = num41 - num7;
                                                num35 = num41 - num7;
                                            }
                                            else
                                            {
                                                num32 = num40 - num6;
                                                num34 = num40 - num6;
                                                num33 = (num41 - num7) - num39;
                                                num35 = (num41 - num7) + num39;
                                            }
                                        }
                                        else
                                        {
                                            num32 = (num40 - num6) - num39;
                                            num33 = (num41 - num7) - num39;
                                            num34 = (num40 - num6) + num39;
                                            num35 = (num41 - num7) + num39;
                                        }
                                    }
                                }
                            }
                        }
                        int id = 0;
                        int num47 = 0;
                        IHue hue4 = null;
                        bool flag8 = false;
                        bool flag9 = false;
                        DynamicItem item = null;
                        StaticItem item2 = null;
                        Item owner = null;
                        bool xDouble = false;
                        ArrayList equip = null;
                        int num48 = (cellWidth < cellHeight) ? (cellWidth - 1) : (cellHeight - 1);
                        if (num18 < 0x7fffffff)
                        {
                            colorMap = package.frameColors;
                            int[,] realColors = package.realColors;
                            for (int num49 = -1; num49 < (cellHeight - 1); num49++)
                            {
                                for (int num50 = -1; num50 < (cellWidth - 1); num50++)
                                {
                                    colorMap[num50 + 1, num49 + 1] = realColors[num50 + 1, num49 + 1];
                                    if ((num50 >= 0) && (num49 >= 0))
                                    {
                                        LandTile tile3 = landTiles[num50, num49];
                                        if (tile3.m_FoldLeftRight)
                                        {
                                            bool flag11 = ((landTiles[num50, num49].Z <= num18) && (landTiles[num50, num49 + 1].Z <= num18)) && (landTiles[num50 + 1, num49 + 1].Z <= num18);
                                            bool flag12 = ((landTiles[num50, num49].Z <= num18) && (landTiles[num50 + 1, num49].Z <= num18)) && (landTiles[num50 + 1, num49 + 1].Z <= num18);
                                            if (!flag11 && !flag12)
                                            {
                                                colorMap[num50, num49] = 0;
                                                colorMap[num50 + 1, num49] = 0;
                                                colorMap[num50 + 1, num49 + 1] = 0;
                                                colorMap[num50, num49 + 1] = 0;
                                            }
                                            else if (!flag11)
                                            {
                                                colorMap[num50, num49] = 0;
                                                colorMap[num50 + 1, num49 + 1] = 0;
                                                colorMap[num50, num49 + 1] = 0;
                                            }
                                            else if (!flag12)
                                            {
                                                colorMap[num50, num49] = 0;
                                                colorMap[num50 + 1, num49 + 1] = 0;
                                                colorMap[num50 + 1, num49] = 0;
                                            }
                                        }
                                        else
                                        {
                                            bool flag13 = ((landTiles[num50, num49].Z <= num18) && (landTiles[num50, num49 + 1].Z <= num18)) && (landTiles[num50 + 1, num49].Z <= num18);
                                            bool flag14 = ((landTiles[num50 + 1, num49].Z <= num18) && (landTiles[num50, num49 + 1].Z <= num18)) && (landTiles[num50 + 1, num49 + 1].Z <= num18);
                                            if (!flag13 && !flag14)
                                            {
                                                colorMap[num50, num49] = 0;
                                                colorMap[num50 + 1, num49] = 0;
                                                colorMap[num50 + 1, num49 + 1] = 0;
                                                colorMap[num50, num49 + 1] = 0;
                                            }
                                            else if (!flag13)
                                            {
                                                colorMap[num50, num49] = 0;
                                                colorMap[num50 + 1, num49] = 0;
                                                colorMap[num50, num49 + 1] = 0;
                                            }
                                            else if (!flag14)
                                            {
                                                colorMap[num50 + 1, num49 + 1] = 0;
                                                colorMap[num50 + 1, num49] = 0;
                                                colorMap[num50, num49 + 1] = 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        for (int k = 0; k < ((num48 * 2) - 1); k++)
                        {
                            int y = k;
                            int num53 = 0;
                            if (y >= num48)
                            {
                                num53 = (k - num48) + 1;
                                y = num48 - 1;
                            }
                            int x = num53;
                            while ((x < num48) && (y >= 0))
                            {
                                int bx = ((x - y) * 0x16) + num12;
                                int by = ((x + y) * 0x16) + num13;
                                int num57 = cells[x, y].Count;
                                for (int num58 = 0; num58 < num57; num58++)
                                {
                                    TileFlags flags;
                                    Client.Texture texture;
                                    float num69;
                                    bool flag17;
                                    int num71;
                                    int num72;
                                    ICell cell5 = (ICell)cells[x, y][num58];
                                    System.Type type3 = cell5.CellType;
                                    if ((type3 != tStaticItem) && (type3 != tDynamicItem))
                                    {
                                        goto Label_177B;
                                    }
                                    flag8 = type3 == tStaticItem;
                                    flag9 = !flag8;
                                    IItem item3 = (IItem)cell5;
                                    if (flag8)
                                    {
                                        item2 = (StaticItem)item3;
                                        id = item2.m_ID;
                                        switch (id)
                                        {
                                            case 0x4001:
                                            case 0x5796:
                                            case 0x61a4:
                                            case 0x6198:
                                            case 0x61bc:
                                            case 0x6199:
                                                {
                                                    continue;
                                                }
                                        }
                                        num47 = item2.m_Z;
                                        flags = Map.m_ItemFlags[id & 0x3fff];
                                        if ((z < 0x7fffffff) && ((num47 >= z) || flags[TileFlag.Roof]))
                                        {
                                            continue;
                                        }
                                        hue4 = item2.m_Hue;
                                        xDouble = false;
                                    }
                                    else
                                    {
                                        item = (DynamicItem)item3;
                                        id = item.m_ID;
                                        switch (id)
                                        {
                                            case 0x4001:
                                            case 0x5796:
                                            case 0x61a4:
                                            case 0x6198:
                                            case 0x61bc:
                                            case 0x6199:
                                                {
                                                    continue;
                                                }
                                        }
                                        num47 = item.m_Z;
                                        flags = Map.m_ItemFlags[id & 0x3fff];
                                        if ((z < 0x7fffffff) && ((num47 >= z) || flags[TileFlag.Roof]))
                                        {
                                            continue;
                                        }
                                        hue4 = item.m_Hue;
                                        owner = item.m_Item;
                                        id = Map.GetDispID(id, (ushort)owner.Amount, ref xDouble);
                                    }
                                    bool flag15 = false;
                                    if ((((lightRed != null) && (x >= num32)) && ((y >= num33) && (x <= num34))) && (y <= num35))
                                    {
                                        hue4 = lightRed;
                                        flag15 = true;
                                    }
                                    if ((flag4 && flag8) && !flag15)
                                    {
                                        StaticItem item5 = item2;
                                        if (item5.m_bInit)
                                        {
                                            if (item5.m_bDraw)
                                            {
                                                if (item5.m_bAlpha)
                                                {
                                                    if (!m_AlphaEnable)
                                                    {
                                                        m_AlphaEnable = true;
                                                    }
                                                    m_fAlpha = ((float)item5.m_vAlpha) / 255f;
                                                    m_Alpha = item5.m_vAlpha << 0x18;
                                                }
                                                else if (m_AlphaEnable)
                                                {
                                                    m_AlphaEnable = false;
                                                }
                                                if (!m_AlphaTestEnable)
                                                {
                                                    m_AlphaTestEnable = true;
                                                }
                                                if (m_FilterEnable)
                                                {
                                                    SetFilterEnablePrecalc(false);
                                                }
                                                m_Texture = item5.m_sDraw;
                                                fixed (CustomVertex.TransformedColoredTextured* texturedRef = item5.m_vPool)
                                                {
                                                    PushQuad(texturedRef);
                                                }
                                            }
                                            continue;
                                        }
                                    }
                                    if ((!flag8 && (owner != null)) && (owner.ID == 0x2006))
                                    {
                                        IHue hue5;
                                        int amount = owner.Amount;
                                        CorpseTableEntry entry = (CorpseTableEntry)CorpseTable.m_Entries[amount];
                                        if ((entry != null) && (BodyConverter.GetFileSet(amount) == 1))
                                        {
                                            amount = entry.m_OldID;
                                            hue4 = Hues.Load(entry.m_NewHue ^ 0x8000);
                                        }
                                        int animDirection = Engine.GetAnimDirection(owner.Direction);
                                        int actionID = Engine.m_Animations.ConvertAction(amount, owner.Serial, owner.X, owner.Y, animDirection, GenericAction.Die, null);
                                        int num63 = Engine.m_Animations.GetFrameCount(amount, actionID, animDirection);
                                        int xCenter = bx + 0x17;
                                        int yCenter = (by - (num47 << 2)) + 20;
                                        if (preserveHue)
                                        {
                                            hue5 = hue;
                                        }
                                        else
                                        {
                                            hue5 = hue4;
                                        }
                                        int textureX = 0;
                                        int textureY = 0;
                                        if (m_AlphaEnable)
                                        {
                                            SetAlphaEnablePrecalc(false);
                                        }
                                        if (!m_AlphaTestEnable)
                                        {
                                            m_AlphaTestEnable = true;
                                        }
                                        if (m_FilterEnable)
                                        {
                                            SetFilterEnablePrecalc(false);
                                        }
                                        Frame frame = Engine.m_Animations.GetFrame(owner, amount, actionID, animDirection, num63 - 1, xCenter, yCenter, hue5, ref textureX, ref textureY, preserveHue);
                                        owner.DrawGame(frame.Image, textureX, textureY);
                                        owner.MessageX = (textureX + frame.Image.xMin) + ((frame.Image.xMax - frame.Image.xMin) / 2);
                                        owner.MessageY = textureY;
                                        owner.BottomY = textureY + frame.Image.yMax;
                                        owner.MessageFrame = m_ActFrames;
                                        for (int num68 = 0; num68 < owner.CorpseEquip.Count; num68++)
                                        {
                                            EquipEntry entry2 = (EquipEntry)owner.CorpseEquip[num68];
                                            if (owner.Items.Contains(entry2.m_Item))
                                            {
                                                if (!preserveHue)
                                                {
                                                    hue5 = Hues.Load(entry2.m_Item.Hue);
                                                }
                                                frame = Engine.m_Animations.GetFrame(entry2.m_Item, entry2.m_Animation, actionID, animDirection, num63 - 1, xCenter, yCenter, hue5, ref textureX, ref textureY, preserveHue);
                                                entry2.m_Item.DrawGame(frame.Image, textureX, textureY);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        IHue grayscale;
                                        texture = null;
                                        num69 = 1f;
                                        flag17 = false;
                                        int num70 = id & 0x3fff;
                                        bool flag18 = false;
                                        if (preserveHue)
                                        {
                                            grayscale = hue;
                                        }
                                        else if (flag8)
                                        {
                                            grayscale = hue4;
                                        }
                                        else if ((owner != null) && owner.Flags[ItemFlag.Hidden])
                                        {
                                            grayscale = Hues.Grayscale;
                                            flag17 = true;
                                            num69 = 0.5f;
                                        }
                                        else if ((owner != null) && ((owner.Hue & 0x4000) != 0))
                                        {
                                            grayscale = Hues.Load(0xbb8);
                                            flag17 = true;
                                            num69 = 1f;
                                            flag18 = true;
                                        }
                                        else
                                        {
                                            grayscale = hue4;
                                        }
                                        AnimData anim = Map.GetAnim(num70);
                                        if ((anim.frameCount == 0) || !flags[TileFlag.Animation])
                                        {
                                            texture = item3.GetItem(grayscale, (short)id);
                                        }
                                        else
                                        {
                                            texture = item3.GetItem(grayscale, (short)(id + anim[(m_Frames / (anim.frameInterval + 1)) % anim.frameCount]));
                                        }
                                        if ((texture != null) && !texture.IsEmpty())
                                        {
                                            num71 = (bx + 0x16) - (texture.Width >> 1);
                                            num72 = ((by - (num47 << 2)) + 0x2b) - texture.Height;
                                            if (flag3 && flags[TileFlag.Foliage])
                                            {
                                                Rectangle rectangle = new Rectangle(num71 + texture.xMin, num72 + texture.yMin, texture.xMax, texture.yMax);
                                                if (rectangle.IntersectsWith(m_FoliageCheck))
                                                {
                                                    flag17 = true;
                                                    num69 *= 0.4f;
                                                }
                                            }
                                            else if (flags[TileFlag.Translucent])
                                            {
                                                flag17 = true;
                                                num69 *= 0.9f;
                                            }
                                            if (m_AlphaEnable != flag17)
                                            {
                                                SetAlphaEnablePrecalc(flag17);
                                            }
                                            if (flag17)
                                            {
                                                SetAlpha(num69);
                                            }
                                            if (!m_AlphaTestEnable)
                                            {
                                                m_AlphaTestEnable = true;
                                            }
                                            if (m_FilterEnable)
                                            {
                                                SetFilterEnablePrecalc(false);
                                            }
                                            if (!flag9)
                                            {
                                                goto Label_170C;
                                            }
                                            if (owner != null)
                                            {
                                                if (flag18)
                                                {
                                                    SetBlendType(DrawBlendType.BlackTransparency);
                                                }
                                                owner.DrawGame(texture, num71, num72);
                                                if (xDouble)
                                                {
                                                    owner.DrawGame(texture, num71 + 5, num72 + 5);
                                                }
                                                if (flag18)
                                                {
                                                    SetBlendType(DrawBlendType.Normal);
                                                }
                                                goto Label_16A2;
                                            }
                                            texture.DrawGame(num71, num72);
                                            if (xDouble)
                                            {
                                                texture.DrawGame(num71 + 5, num72 + 5);
                                            }
                                        }
                                    }
                                    continue;
                                Label_16A2:
                                    if (m_Transparency)
                                    {
                                        transDrawQueue.Enqueue(TransparentDraw.PoolInstance(texture, num71, num72, flag17, num69, xDouble));
                                    }
                                    owner.MessageX = (num71 + texture.xMin) + ((texture.xMax - texture.xMin) / 2);
                                    owner.MessageY = num72;
                                    owner.BottomY = num72 + texture.yMax;
                                    owner.MessageFrame = m_ActFrames;
                                    continue;
                                Label_170C:
                                    if (flags[TileFlag.Animation])
                                    {
                                        texture.DrawGame(num71, num72, item2.m_vPool);
                                    }
                                    else
                                    {
                                        item2.m_bDraw = texture.DrawGame(num71, num72, item2.m_vPool);
                                        item2.m_vAlpha = (byte)(m_Alpha >> 0x18);
                                        item2.m_bInit = !flag15;
                                        item2.m_bAlpha = flag17;
                                        item2.m_sDraw = texture;
                                    }
                                    continue;
                                Label_177B:
                                    if (type3 == tLandTile)
                                    {
                                        LandTile lt = (LandTile)cell5;
                                        num47 = lt.m_Z;
                                        int landID = lt.m_ID;
                                        if (landID != 2)
                                        {
                                            int num74 = bx;
                                            int num75 = by;
                                            int num76 = package.CellX + x;
                                            int num77 = package.CellY + y;
                                            bool flag19 = false;
                                            if ((((lightRed != null) && (x >= num32)) && ((y >= num33) && (x <= num34))) && (y <= num35))
                                            {
                                                flag19 = true;
                                            }
                                            if ((flag4 && lt.m_bInit) && !flag19)
                                            {
                                                if (lt.m_bDraw)
                                                {
                                                    AlphaTestEnable = !lt.m_bFilter;
                                                    if (m_AlphaEnable)
                                                    {
                                                        SetAlphaEnablePrecalc(false);
                                                    }
                                                    SetTexture(lt.m_sDraw);
                                                    if (m_FilterEnable != lt.m_bFilter)
                                                    {
                                                        SetFilterEnablePrecalc(!m_FilterEnable);
                                                    }
                                                    DrawQuadPrecalc(lt.m_vDraw);
                                                    if (m_DrawGrid)
                                                    {
                                                        Grid(lt, landTiles, x, y, bx, by);
                                                    }
                                                    if (((num76 == m_xServerStart) || (num76 == m_xServerEnd)) && ((num77 >= m_yServerStart) && (num77 < m_yServerEnd)))
                                                    {
                                                        DrawMapLine(landTiles, num74, num75, x, y, 0, 1);
                                                    }
                                                    else if ((x > 0) && (lt.m_Guarded != landTiles[x - 1, y].m_Guarded))
                                                    {
                                                        DrawMapLine(landTiles, num74, num75, x, y, 0, 1);
                                                    }
                                                    if (((num77 == m_yServerStart) || (num77 == m_yServerEnd)) && ((num76 >= m_xServerStart) && (num76 < m_xServerEnd)))
                                                    {
                                                        DrawMapLine(landTiles, num74, num75, x, y, 1, 0);
                                                    }
                                                    else if ((y > 0) && (lt.m_Guarded != landTiles[x, y - 1].m_Guarded))
                                                    {
                                                        DrawMapLine(landTiles, num74, num75, x, y, 1, 0);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                CustomVertex.TransformedColoredTextured[] texturedArray;
                                                IHue hue7 = flag19 ? lightRed : hue;
                                                int textureID = Map.GetTexture(landID);
                                                if ((((buffer[x, y] & 1) == 0) || (textureID == 0)) || (textureID >= 0x1000))
                                                {
                                                    Client.Texture land = hue7.GetLand(landID);
                                                    if ((land != null) && !land.IsEmpty())
                                                    {
                                                        if ((num18 < 0x7fffffff) && (num47 >= num18))
                                                        {
                                                            lt.m_bDraw = false;
                                                            lt.m_bInit = true;
                                                        }
                                                        else
                                                        {
                                                            AlphaTestEnable = true;
                                                            if (m_AlphaEnable)
                                                            {
                                                                SetAlphaEnablePrecalc(false);
                                                            }
                                                            if (m_FilterEnable)
                                                            {
                                                                SetFilterEnablePrecalc(false);
                                                            }
                                                            texturedArray = m_VertexPool[x][y];
                                                            lt.m_bDraw = land.DrawGame(num74, num75 - (num47 << 2), texturedArray);
                                                            lt.m_vDraw = texturedArray;
                                                            lt.m_bInit = !flag19;
                                                            lt.m_sDraw = land;
                                                            lt.m_bFilter = false;
                                                            if (m_DrawGrid)
                                                            {
                                                                Grid(lt, landTiles, x, y, bx, by);
                                                            }
                                                            if (((num76 == m_xServerStart) || (num76 == m_xServerEnd)) && ((num77 >= m_yServerStart) && (num77 < m_yServerEnd)))
                                                            {
                                                                DrawMapLine(landTiles, num74, num75, x, y, 0, 1);
                                                            }
                                                            else if ((x > 0) && (lt.m_Guarded != landTiles[x - 1, y].m_Guarded))
                                                            {
                                                                DrawMapLine(landTiles, num74, num75, x, y, 0, 1);
                                                            }
                                                            if (((num77 == m_yServerStart) || (num77 == m_yServerEnd)) && ((num76 >= m_xServerStart) && (num76 < m_xServerEnd)))
                                                            {
                                                                DrawMapLine(landTiles, num74, num75, x, y, 1, 0);
                                                            }
                                                            else if ((y > 0) && (lt.m_Guarded != landTiles[x, y - 1].m_Guarded))
                                                            {
                                                                DrawMapLine(landTiles, num74, num75, x, y, 1, 0);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Client.Texture texture3 = hue7.GetTexture(textureID);
                                                    if ((texture3 == null) || texture3.IsEmpty())
                                                    {
                                                        texture3 = hue7.GetTexture(1);
                                                        if ((texture3 == null) || texture3.IsEmpty())
                                                        {
                                                            continue;
                                                        }
                                                    }
                                                    if ((num74 >= (Engine.GameX + Engine.GameWidth)) || ((num74 + 0x2c) <= Engine.GameX))
                                                    {
                                                        lt.m_vDraw = null;
                                                        lt.m_bDraw = false;
                                                        lt.m_bInit = true;
                                                        lt.m_sDraw = null;
                                                        lt.m_bFilter = false;
                                                    }
                                                    else
                                                    {
                                                        texturedArray = m_VertexPool[x][y];
                                                        fixed (CustomVertex.TransformedColoredTextured* texturedRef2 = texturedArray)
                                                        {
                                                            if (lt.m_FoldLeftRight)
                                                            {
                                                                texturedRef2[1].X = num74 + 0x16;
                                                                texturedRef2[1].Y = num75 - (num47 << 2);
                                                                texturedRef2->Y = (num75 + 0x16) - (landTiles[x + 1, y].m_Z << 2);
                                                                texturedRef2->X = num74 + 0x2c;
                                                                texturedRef2[2].Y = (num75 + 0x2c) - (landTiles[x + 1, y + 1].m_Z << 2);
                                                                texturedRef2[2].X = num74 + 0x16;
                                                                texturedRef2[3].Y = (num75 + 0x16) - (landTiles[x, y + 1].m_Z << 2);
                                                                texturedRef2[3].X = num74;
                                                                if ((((texturedRef2->Y < Engine.GameY) || (texturedRef2->Y >= (Engine.GameY + Engine.GameHeight))) && ((texturedRef2[1].Y < Engine.GameY) || (texturedRef2[1].Y >= (Engine.GameY + Engine.GameHeight)))) && (((texturedRef2[2].Y < Engine.GameY) || (texturedRef2[2].Y >= (Engine.GameY + Engine.GameHeight))) && ((texturedRef2[3].Y < Engine.GameY) || (texturedRef2[3].Y >= (Engine.GameY + Engine.GameHeight)))))
                                                                {
                                                                    lt.m_vDraw = texturedArray;
                                                                    lt.m_bDraw = false;
                                                                    lt.m_bInit = true;
                                                                    lt.m_sDraw = null;
                                                                    lt.m_bFilter = false;
                                                                    continue;
                                                                }
                                                                texturedRef2[2].Color = colorMap[x + 1, y + 1];
                                                                texturedRef2->Color = colorMap[x + 1, y];
                                                                texturedRef2[3].Color = colorMap[x, y + 1];
                                                                texturedRef2[1].Color = colorMap[x, y];
                                                                if (texture3.Width == 0x2c)
                                                                {
                                                                    texturedRef2[1].Tu = 0.5f * texture3.MaxTU;
                                                                    texturedRef2[1].Tv = 0f;
                                                                    texturedRef2->Tu = 1f * texture3.MaxTU;
                                                                    texturedRef2->Tv = 0.5f * texture3.MaxTV;
                                                                    texturedRef2[2].Tu = 0.5f * texture3.MaxTU;
                                                                    texturedRef2[2].Tv = 1f * texture3.MaxTV;
                                                                    texturedRef2[3].Tu = 0f;
                                                                    texturedRef2[3].Tv = 0.5f * texture3.MaxTV;
                                                                    texturedRef2->Color = ((texturedRef2->Color & 0xff) * 0xff) / 210;
                                                                    texturedRef2[1].Color = ((texturedRef2[1].Color & 0xff) * 0xff) / 210;
                                                                    texturedRef2[2].Color = ((texturedRef2[2].Color & 0xff) * 0xff) / 210;
                                                                    texturedRef2[3].Color = ((texturedRef2[3].Color & 0xff) * 0xff) / 210;
                                                                    if (texturedRef2->Color < 0)
                                                                    {
                                                                        texturedRef2->Color = 0;
                                                                    }
                                                                    else if (texturedRef2->Color > 0xff)
                                                                    {
                                                                        texturedRef2->Color = 0xffffff;
                                                                    }
                                                                    else
                                                                    {
                                                                        //IntPtr ptr1 = (IntPtr) texturedRef2;
                                                                        //ptr1.Color *= 0x10101;
                                                                        texturedRef2->Color *= 0x10101;
                                                                    }
                                                                    if (texturedRef2[1].Color < 0)
                                                                    {
                                                                        texturedRef2[1].Color = 0;
                                                                    }
                                                                    else if (texturedRef2[1].Color > 0xff)
                                                                    {
                                                                        texturedRef2[1].Color = 0xffffff;
                                                                    }
                                                                    else
                                                                    {
                                                                        //IntPtr ptr2 = (IntPtr)(texturedRef2 + 1);
                                                                        //ptr2.Color *= 0x10101;
                                                                        (texturedRef2 + 1)->Color *= 0x10101;
                                                                    }
                                                                    if (texturedRef2[2].Color < 0)
                                                                    {
                                                                        texturedRef2[2].Color = 0;
                                                                    }
                                                                    else if (texturedRef2[2].Color > 0xff)
                                                                    {
                                                                        texturedRef2[2].Color = 0xffffff;
                                                                    }
                                                                    else
                                                                    {
                                                                        //IntPtr ptr3 = (IntPtr) (texturedRef2 + 2);
                                                                        //ptr3.Color *= 0x10101;
                                                                        (texturedRef2 + 2)->Color *= 0x10101;
                                                                    }
                                                                    if (texturedRef2[3].Color < 0)
                                                                    {
                                                                        texturedRef2[3].Color = 0;
                                                                    }
                                                                    else if (texturedRef2[3].Color > 0xff)
                                                                    {
                                                                        texturedRef2[3].Color = 0xffffff;
                                                                    }
                                                                    else
                                                                    {
                                                                        //IntPtr ptr4 = (IntPtr) (texturedRef2 + 3);
                                                                        //ptr4.Color *= 0x10101;
                                                                        (texturedRef2 + 3)->Color *= 0x10101;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    texturedRef2->Tv = 0f;
                                                                    texturedRef2[1].Tv = 0f;
                                                                    texturedRef2[1].Tu = 0f;
                                                                    texturedRef2[3].Tu = 0f;
                                                                    texturedRef2->Tu = 1f;
                                                                    texturedRef2[2].Tu = 1f;
                                                                    texturedRef2[2].Tv = 1f;
                                                                    texturedRef2[3].Tv = 1f;
                                                                }
                                                                if (num18 < 0x7fffffff)
                                                                {
                                                                    bool flag20 = ((landTiles[x, y].Z <= num18) && (landTiles[x, y + 1].Z <= num18)) && (landTiles[x + 1, y + 1].Z <= num18);
                                                                    bool flag21 = ((landTiles[x, y].Z <= num18) && (landTiles[x + 1, y].Z <= num18)) && (landTiles[x + 1, y + 1].Z <= num18);
                                                                    if (!flag20 && !flag21)
                                                                    {
                                                                        lt.m_vDraw = texturedArray;
                                                                        lt.m_bDraw = false;
                                                                        lt.m_bInit = true;
                                                                        lt.m_sDraw = null;
                                                                        lt.m_bFilter = false;
                                                                        continue;
                                                                    }
                                                                    if (!flag20)
                                                                    {
                                                                        texturedRef2[1].Color = texturedRef2[2].Color = 0;
                                                                        texturedRef2[3] = texturedRef2[1];
                                                                    }
                                                                    else if (!flag21)
                                                                    {
                                                                        texturedRef2[1].Color = texturedRef2[2].Color = 0;
                                                                        //texturedRef2 = *((CustomVertex.TransformedColoredTextured**) (texturedRef2 + 1));
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                texturedRef2[3].X = num74 + 0x16;
                                                                texturedRef2[3].Y = num75 - (num47 << 2);
                                                                texturedRef2[1].Y = (num75 + 0x16) - (landTiles[x + 1, y].m_Z << 2);
                                                                texturedRef2[1].X = num74 + 0x2c;
                                                                texturedRef2->Y = (num75 + 0x2c) - (landTiles[x + 1, y + 1].m_Z << 2);
                                                                texturedRef2->X = num74 + 0x16;
                                                                texturedRef2[2].Y = (num75 + 0x16) - (landTiles[x, y + 1].m_Z << 2);
                                                                texturedRef2[2].X = num74;
                                                                if ((((texturedRef2->Y < Engine.GameY) || (texturedRef2->Y >= (Engine.GameY + Engine.GameHeight))) && ((texturedRef2[1].Y < Engine.GameY) || (texturedRef2[1].Y >= (Engine.GameY + Engine.GameHeight)))) && (((texturedRef2[2].Y < Engine.GameY) || (texturedRef2[2].Y >= (Engine.GameY + Engine.GameHeight))) && ((texturedRef2[3].Y < Engine.GameY) || (texturedRef2[3].Y >= (Engine.GameY + Engine.GameHeight)))))
                                                                {
                                                                    lt.m_vDraw = texturedArray;
                                                                    lt.m_bDraw = false;
                                                                    lt.m_bInit = true;
                                                                    lt.m_sDraw = null;
                                                                    lt.m_bFilter = false;
                                                                    continue;
                                                                }
                                                                texturedRef2->Color = colorMap[x + 1, y + 1];
                                                                texturedRef2[1].Color = colorMap[x + 1, y];
                                                                texturedRef2[2].Color = colorMap[x, y + 1];
                                                                texturedRef2[3].Color = colorMap[x, y];
                                                                if (texture3.Width == 0x2c)
                                                                {
                                                                    texturedRef2[3].Tu = 0.5f * texture3.MaxTU;
                                                                    texturedRef2[3].Tv = 0f;
                                                                    texturedRef2[1].Tu = 1f * texture3.MaxTU;
                                                                    texturedRef2[1].Tv = 0.5f * texture3.MaxTV;
                                                                    texturedRef2->Tu = 0.5f * texture3.MaxTU;
                                                                    texturedRef2->Tv = 1f * texture3.MaxTV;
                                                                    texturedRef2[2].Tu = 0f;
                                                                    texturedRef2[2].Tv = 0.5f * texture3.MaxTV;
                                                                    texturedRef2->Color = ((texturedRef2->Color & 0xff) * 0xff) / 210;
                                                                    texturedRef2[1].Color = ((texturedRef2[1].Color & 0xff) * 0xff) / 210;
                                                                    texturedRef2[2].Color = ((texturedRef2[2].Color & 0xff) * 0xff) / 210;
                                                                    texturedRef2[3].Color = ((texturedRef2[3].Color & 0xff) * 0xff) / 210;
                                                                    if (texturedRef2->Color < 0)
                                                                    {
                                                                        texturedRef2->Color = 0;
                                                                    }
                                                                    else if (texturedRef2->Color > 0xff)
                                                                    {
                                                                        texturedRef2->Color = 0xffffff;
                                                                    }
                                                                    else
                                                                    {
                                                                        //IntPtr ptr5 = (IntPtr) texturedRef2;
                                                                        //ptr5.Color *= 0x10101;
                                                                        (texturedRef2 + 0)->Color *= 0x10101;
                                                                    }
                                                                    if (texturedRef2[1].Color < 0)
                                                                    {
                                                                        texturedRef2[1].Color = 0;
                                                                    }
                                                                    else if (texturedRef2[1].Color > 0xff)
                                                                    {
                                                                        texturedRef2[1].Color = 0xffffff;
                                                                    }
                                                                    else
                                                                    {
                                                                        //IntPtr ptr6 = (IntPtr) (texturedRef2 + 1);
                                                                        //ptr6.Color *= 0x10101;
                                                                        (texturedRef2 + 1)->Color *= 0x10101;
                                                                    }
                                                                    if (texturedRef2[2].Color < 0)
                                                                    {
                                                                        texturedRef2[2].Color = 0;
                                                                    }
                                                                    else if (texturedRef2[2].Color > 0xff)
                                                                    {
                                                                        texturedRef2[2].Color = 0xffffff;
                                                                    }
                                                                    else
                                                                    {
                                                                        //IntPtr ptr7 = (IntPtr) (texturedRef2 + 2);
                                                                        //ptr7.Color *= 0x10101;
                                                                        (texturedRef2 + 2)->Color *= 0x10101;
                                                                    }
                                                                    if (texturedRef2[3].Color < 0)
                                                                    {
                                                                        texturedRef2[3].Color = 0;
                                                                    }
                                                                    else if (texturedRef2[3].Color > 0xff)
                                                                    {
                                                                        texturedRef2[3].Color = 0xffffff;
                                                                    }
                                                                    else
                                                                    {
                                                                        //IntPtr ptr8 = (IntPtr) (texturedRef2 + 3);
                                                                        //ptr8.Color *= 0x10101;
                                                                        (texturedRef2 + 3)->Color *= 0x10101;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    texturedRef2[1].Tv = 0f;
                                                                    texturedRef2[2].Tu = 0f;
                                                                    texturedRef2[3].Tu = 0f;
                                                                    texturedRef2[3].Tv = 0f;
                                                                    texturedRef2->Tv = 1f;
                                                                    texturedRef2->Tu = 1f;
                                                                    texturedRef2[1].Tu = 1f;
                                                                    texturedRef2[2].Tv = 1f;
                                                                }
                                                                if (num18 < 0x7fffffff)
                                                                {
                                                                    bool flag22 = ((landTiles[x, y].Z <= num18) && (landTiles[x, y + 1].Z <= num18)) && (landTiles[x + 1, y].Z <= num18);
                                                                    bool flag23 = ((landTiles[x + 1, y].Z <= num18) && (landTiles[x, y + 1].Z <= num18)) && (landTiles[x + 1, y + 1].Z <= num18);
                                                                    if (!flag22 && !flag23)
                                                                    {
                                                                        lt.m_vDraw = texturedArray;
                                                                        lt.m_bDraw = false;
                                                                        lt.m_bInit = true;
                                                                        lt.m_sDraw = null;
                                                                        lt.m_bFilter = false;
                                                                        continue;
                                                                    }
                                                                    if (!flag22)
                                                                    {
                                                                        texturedRef2[3] = texturedRef2[1];
                                                                    }
                                                                    else if (!flag23)
                                                                    {
                                                                        //texturedRef2 = *((CustomVertex.TransformedColoredTextured**) (texturedRef2 + 1));
                                                                    }
                                                                }
                                                            }
                                                            //IntPtr ptr9 = (IntPtr) texturedRef2;
                                                            //ptr9.X -= 0.5f;
                                                            texturedRef2->X -= 0.5f;
                                                            //IntPtr ptr10 = (IntPtr) texturedRef2;
                                                            //ptr10.Y -= 0.5f;
                                                            texturedRef2->Y -= 0.5f;
                                                            //IntPtr ptr11 = (IntPtr) (texturedRef2 + 1);
                                                            //ptr11.X -= 0.5f;
                                                            (texturedRef2 + 1)->X -= 0.5f;
                                                            //IntPtr ptr12 = (IntPtr) (texturedRef2 + 1);
                                                            //ptr12.Y -= 0.5f;
                                                            (texturedRef2 + 1)->Y -= 0.5f;
                                                            //IntPtr ptr13 = (IntPtr) (texturedRef2 + 2);
                                                            //ptr13.X -= 0.5f;
                                                            (texturedRef2 + 2)->X -= 0.5f;
                                                            //IntPtr ptr14 = (IntPtr) (texturedRef2 + 2);
                                                            //ptr14.Y -= 0.5f;
                                                            (texturedRef2 + 2)->Y -= 0.5f;
                                                            //IntPtr ptr15 = (IntPtr) (texturedRef2 + 3);
                                                            //ptr15.X -= 0.5f;
                                                            (texturedRef2 + 3)->X -= 0.5f;
                                                            //IntPtr ptr16 = (IntPtr) (texturedRef2 + 3);
                                                            //ptr16.Y -= 0.5f;
                                                            (texturedRef2 + 3)->Y -= 0.5f;

                                                            if (m_AlphaEnable)
                                                            {
                                                                SetAlphaEnablePrecalc(false);
                                                            }
                                                            if (!m_FilterEnable)
                                                            {
                                                                SetFilterEnablePrecalc(true);
                                                            }
                                                            AlphaTestEnable = false;
                                                            SetTexture(texture3);
                                                            DrawQuadPrecalc(texturedRef2);
                                                        }
                                                        lt.m_vDraw = texturedArray;
                                                        lt.m_bDraw = true;
                                                        lt.m_bInit = !flag19;
                                                        lt.m_bFilter = true;
                                                        lt.m_sDraw = texture3;
                                                        if (m_DrawGrid)
                                                        {
                                                            Grid(lt, landTiles, x, y, bx, by);
                                                        }
                                                        if (((num76 == m_xServerStart) || (num76 == m_xServerEnd)) && ((num77 >= m_yServerStart) && (num77 < m_yServerEnd)))
                                                        {
                                                            DrawMapLine(landTiles, num74, num75, x, y, 0, 1);
                                                        }
                                                        else if ((x > 0) && (lt.m_Guarded != landTiles[x - 1, y].m_Guarded))
                                                        {
                                                            DrawMapLine(landTiles, num74, num75, x, y, 0, 1);
                                                        }
                                                        if (((num77 == m_yServerStart) || (num77 == m_yServerEnd)) && ((num76 >= m_xServerStart) && (num76 < m_xServerEnd)))
                                                        {
                                                            DrawMapLine(landTiles, num74, num75, x, y, 1, 0);
                                                        }
                                                        else if ((y > 0) && (lt.m_Guarded != landTiles[x, y - 1].m_Guarded))
                                                        {
                                                            DrawMapLine(landTiles, num74, num75, x, y, 1, 0);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (((type3 == tMobileCell) || (type3 == tCorpseCell)) && ((z >= 0x7fffffff) || (cell5.Z < z)))
                                    {
                                        IAnimatedCell cell6 = (IAnimatedCell)cell5;
                                        int body = 0;
                                        int direction = 0;
                                        int num81 = 0;
                                        int action = 0;
                                        int num83 = 0;
                                        bool flag24 = type3 == tMobileCell;
                                        Mobile mobile4 = flag24 ? ((MobileCell)cell5).m_Mobile : null;
                                        int frames = 0;
                                        int num85 = 0;
                                        int num86 = 0;
                                        if (flag24 && (mobile4 != null))
                                        {
                                            equip = mobile4.Equip;
                                            if (mobile4.Player)
                                            {
                                                flag3 = true;
                                            }
                                            if (mobile4.Walking.Count > 0)
                                            {
                                                WalkAnimation animation2 = (WalkAnimation)mobile4.Walking.Peek();
                                                if (!animation2.Snapshot(ref num85, ref num86, ref frames))
                                                {
                                                    if (!animation2.Advance)
                                                    {
                                                        num85 = animation2.xOffset;
                                                        num86 = animation2.yOffset;
                                                    }
                                                    else
                                                    {
                                                        num85 = 0;
                                                        num86 = 0;
                                                    }
                                                    frames = animation2.Frames;
                                                    mobile4.SetLocation((short)animation2.NewX, (short)animation2.NewY, (short)animation2.NewZ);
                                                    ((WalkAnimation)mobile4.Walking.Dequeue()).Dispose();
                                                    if (mobile4.Player)
                                                    {
                                                        if (Engine.amMoving)
                                                        {
                                                            Engine.DoWalk(Engine.movingDir, true);
                                                        }
                                                        eOffsetX += num85;
                                                        eOffsetY += num86;
                                                    }
                                                    if (mobile4.Walking.Count == 0)
                                                    {
                                                        Engine.EquipSort(mobile4, animation2.NewDir);
                                                        mobile4.Direction = (byte)animation2.NewDir;
                                                        mobile4.IsMoving = false;
                                                        mobile4.MovedTiles = 0;
                                                        mobile4.HorseFootsteps = 0;
                                                    }
                                                    else
                                                    {
                                                        ((WalkAnimation)mobile4.Walking.Peek()).Start();
                                                    }
                                                    toUpdateQueue.Enqueue(mobile4);
                                                }
                                            }
                                        }
                                        cell6.GetPackage(ref body, ref action, ref direction, ref num83, ref num81);
                                        int num87 = num83;
                                        int num88 = Engine.m_Animations.GetFrameCount(body, action, direction);
                                        if (num88 != 0)
                                        {
                                            IHue notoriety;
                                            Frame frame2;
                                            num87 = num87 % num88;
                                            int num89 = bx + 0x16;
                                            int num90 = (by - (cell6.Z << 2)) + 0x16;
                                            num89++;
                                            num90 -= 2;
                                            num89 += num85;
                                            num90 += num86;
                                            if (frames != 0)
                                            {
                                                num87 += frames;
                                                num87 = num87 % num88;
                                                num83 += frames;
                                                num83 = num83 % num88;
                                            }
                                            if (flag24 && (mobile4 != null))
                                            {
                                                if ((mobile4.Human && mobile4.IsMoving) && (mobile4.LastFrame != num83))
                                                {
                                                    int num143;
                                                    if ((equip.Count > 0) && (((EquipEntry)equip[0]).m_Layer == Layer.Mount))
                                                    {
                                                        if ((direction & 0x80) == 0x80)
                                                        {
                                                            if ((mobile4.HorseFootsteps != mobile4.MovedTiles) && ((mobile4.MovedTiles & 1) == 1))
                                                            {
                                                                mobile4.Sounds = (num143 = mobile4.Sounds) + 1;
                                                                int soundID = 0x129 + (num143 & 1);
                                                                Engine.Sounds.PlaySound(soundID, mobile4.X, mobile4.Y, mobile4.Z, 0.8f);
                                                                mobile4.HorseFootsteps = mobile4.MovedTiles;
                                                            }
                                                        }
                                                        else if ((mobile4.HorseFootsteps != mobile4.MovedTiles) && ((mobile4.MovedTiles & 1) == 1))
                                                        {
                                                            mobile4.Sounds = (num143 = mobile4.Sounds) + 1;
                                                            int num92 = 0x12b + (num143 & 1);
                                                            Engine.Sounds.PlaySound(num92, mobile4.X, mobile4.Y, mobile4.Z, 0.8f);
                                                            mobile4.HorseFootsteps = mobile4.MovedTiles;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        switch (num83)
                                                        {
                                                            case 1:
                                                            case 6:
                                                                {
                                                                    mobile4.Sounds = (num143 = mobile4.Sounds) + 1;
                                                                    int num93 = 0x12b + (num143 & 1);
                                                                    Engine.Sounds.PlaySound(num93, mobile4.X, mobile4.Y, mobile4.Z, 0.65f);
                                                                    break;
                                                                }
                                                        }
                                                    }
                                                }
                                                mobile4.LastFrame = num83;
                                            }
                                            bool ghost = false;
                                            bool flag26 = false;
                                            IHue hue9 = null;
                                            bool enable = false;
                                            bool flag28 = false;
                                            float alpha = 1f;
                                            if (preserveHue)
                                            {
                                                notoriety = hue;
                                                flag26 = true;
                                                hue9 = notoriety;
                                                if (flag24 && (mobile4 != null))
                                                {
                                                    ghost = mobile4.Ghost;
                                                }
                                            }
                                            else if ((((lightRed != null) && (x >= num32)) && ((y >= num33) && (x <= num34))) && (y <= num35))
                                            {
                                                notoriety = lightRed;
                                                flag26 = true;
                                                hue9 = notoriety;
                                                flag28 = false;
                                            }
                                            else if (flag24)
                                            {
                                                if (mobile4 != null)
                                                {
                                                    ghost = mobile4.Ghost;
                                                    if ((mobile4.Flags.Value & -223) != 0)
                                                    {
                                                        notoriety = Hues.Load(0x8485);
                                                        flag26 = true;
                                                        hue9 = notoriety;
                                                        flag28 = false;
                                                    }
                                                    else if (mobile4.Flags[MobileFlag.Hidden])
                                                    {
                                                        notoriety = Hues.Grayscale;
                                                        flag26 = true;
                                                        hue9 = notoriety;
                                                    }
                                                    else if (((Engine.m_Highlight == mobile4) || (m_AlwaysHighlight == mobile4.Serial)) && !mobile4.Player)
                                                    {
                                                        notoriety = Hues.GetNotoriety(mobile4.Notoriety);
                                                        flag26 = true;
                                                        hue9 = notoriety;
                                                        flag28 = true;
                                                    }
                                                    else if (mobile4.Ignored)
                                                    {
                                                        notoriety = Hues.Load(0x8035);
                                                        flag26 = true;
                                                        hue9 = notoriety;
                                                        flag28 = true;
                                                    }
                                                    else if (mobile4.Bonded)
                                                    {
                                                        notoriety = Hues.Grayscale;
                                                        flag26 = true;
                                                        hue9 = notoriety;
                                                        flag28 = true;
                                                    }
                                                    else if ((mobile4.Hue & 0x4000) != 0)
                                                    {
                                                        notoriety = Hues.Load(0x8bb8);
                                                    }
                                                    else
                                                    {
                                                        notoriety = Hues.Load(num81);
                                                    }
                                                }
                                                else
                                                {
                                                    notoriety = Hues.Load(num81);
                                                }
                                            }
                                            else
                                            {
                                                notoriety = Hues.Load(num81);
                                            }
                                            int num95 = 0;
                                            int num96 = 0;
                                            try
                                            {
                                                frame2 = Engine.m_Animations.GetFrame(mobile4, body, action, direction, num87, num89, num90, notoriety, ref num95, ref num96, flag26);
                                            }
                                            catch
                                            {
                                                frame2 = Engine.m_Animations.GetFrame(mobile4, body, action, direction, num87, num89, num90, notoriety, ref num95, ref num96, flag26);
                                            }
                                            bool flag29 = false;
                                            bool alphaEnable = false;
                                            bool flag31 = false;
                                            float fAlpha = 1f;
                                            int num98 = -1;
                                            int num99 = -1;
                                            Client.Texture t = null;
                                            if ((frame2.Image != null) && !frame2.Image.IsEmpty())
                                            {
                                                if (flag24 && (mobile4 != null))
                                                {
                                                    mobile4.MessageFrame = m_ActFrames;
                                                    mobile4.ScreenX = mobile4.MessageX = num89;
                                                    mobile4.ScreenY = num90;
                                                    mobile4.MessageY = num96;
                                                    if (mobile4.Player)
                                                    {
                                                        m_FoliageCheck = new Rectangle(num95, num96, frame2.Image.Width, frame2.Image.Height);
                                                    }
                                                }
                                                if (((flag2 && flag24) && ((mobile4 != null) && !mobile4.Player)) && (((mobile4.Notoriety >= Notoriety.Innocent) && (mobile4.Notoriety <= Notoriety.Vendor)) && World.CharData.Halos))
                                                {
                                                    if (!m_AlphaEnable)
                                                    {
                                                        SetAlphaEnablePrecalc(true);
                                                    }
                                                    if (m_FilterEnable)
                                                    {
                                                        SetFilterEnablePrecalc(false);
                                                    }
                                                    SetAlpha(1f);
                                                    ColorAlphaEnable = true;
                                                    Engine.m_Halo.DrawGame(num89 - (Engine.m_Halo.Width >> 1), num90 - (Engine.m_Halo.Height >> 1), Engine.C16232(Hues.GetNotorietyData(mobile4.Notoriety).colors[0x2f]), m_vHaloPool);
                                                    ColorAlphaEnable = false;
                                                }
                                                bool flag32 = false;
                                                if (flag24 && (mobile4 != null))
                                                {
                                                    if ((mobile4.Flags[MobileFlag.Hidden] || (body == 970)) || (mobile4.Bonded || mobile4.Ghost))
                                                    {
                                                        if (!m_AlphaEnable)
                                                        {
                                                            SetAlphaEnablePrecalc(true);
                                                        }
                                                        SetAlpha(0.5f);
                                                        enable = true;
                                                        alpha = 0.5f;
                                                    }
                                                    else if ((mobile4.Hue & 0x4000) != 0)
                                                    {
                                                        if (!m_AlphaEnable)
                                                        {
                                                            SetAlphaEnablePrecalc(true);
                                                        }
                                                        SetAlpha(1f);
                                                        alpha = 1f;
                                                        flag32 = true;
                                                    }
                                                    else if (m_AlphaEnable)
                                                    {
                                                        SetAlphaEnablePrecalc(false);
                                                    }
                                                }
                                                else if (body == 970)
                                                {
                                                    if (!m_AlphaEnable)
                                                    {
                                                        SetAlphaEnablePrecalc(true);
                                                    }
                                                    SetAlpha(0.5f);
                                                    enable = true;
                                                    alpha = 0.5f;
                                                }
                                                else if (m_AlphaEnable)
                                                {
                                                    SetAlphaEnablePrecalc(false);
                                                }
                                                flag29 = type3 == tMobileCell;
                                                if (flag29)
                                                {
                                                    if (((mobile4 != null) && mobile4.HumanOrGhost) && ((equip.Count > 0) && (((EquipEntry)equip[0]).m_Layer == Layer.Mount)))
                                                    {
                                                        alphaEnable = m_AlphaEnable;
                                                        fAlpha = m_fAlpha;
                                                        flag31 = flag32;
                                                        t = frame2.Image;
                                                        num98 = num95;
                                                        num99 = num96;
                                                    }
                                                    else
                                                    {
                                                        flag29 = false;
                                                    }
                                                }
                                                if (m_FilterEnable)
                                                {
                                                    SetFilterEnablePrecalc(false);
                                                }
                                                AlphaTestEnable = true;
                                                if (!ghost && !flag29)
                                                {
                                                    if ((((num96 >= ((Engine.GameY + Engine.GameHeight) + 30)) || ((num96 + frame2.Image.Height) <= (Engine.GameY - 30))) || (num95 >= ((Engine.GameX + Engine.GameWidth) + 30))) || ((num95 + frame2.Image.Width) <= (Engine.GameX - 30)))
                                                    {
                                                        continue;
                                                    }
                                                    if (flag32)
                                                    {
                                                        SetBlendType(DrawBlendType.BlackTransparency);
                                                    }
                                                    if (flag24 && (mobile4 != null))
                                                    {
                                                        mobile4.DrawGame(frame2.Image, num95, num96);
                                                    }
                                                    else
                                                    {
                                                        frame2.Image.DrawGame(num95, num96);
                                                    }
                                                    if (flag32)
                                                    {
                                                        SetBlendType(DrawBlendType.Normal);
                                                    }
                                                }
                                            }
                                            if (type3 == tCorpseCell)
                                            {
                                                Item item6 = World.FindItem(((CorpseCell)cell5).Serial);
                                                if ((item6 != null) && (Engine.m_Animations.GetBodyType(body) == BodyType.Human))
                                                {
                                                    int num100 = item6.Equip.Count;
                                                    for (int num101 = 0; num101 < num100; num101++)
                                                    {
                                                        EquipEntry entry3 = (EquipEntry)item6.Equip[num101];
                                                        int bodyID = entry3.m_Animation;
                                                        if (!flag26)
                                                        {
                                                            notoriety = Hues.GetItemHue(item6.ID, entry3.m_Item.Hue);
                                                        }
                                                        int num103 = num83;
                                                        int num104 = Engine.m_Animations.GetFrameCount(bodyID, action, direction);
                                                        if (num104 == 0)
                                                        {
                                                            num103 = 0;
                                                        }
                                                        else
                                                        {
                                                            num103 = num103 % num104;
                                                        }
                                                        if (m_FilterEnable)
                                                        {
                                                            SetFilterEnablePrecalc(false);
                                                        }
                                                        if (m_AlphaEnable != enable)
                                                        {
                                                            SetAlphaEnablePrecalc(enable);
                                                        }
                                                        if (enable)
                                                        {
                                                            SetAlpha(alpha);
                                                        }
                                                        frame2 = Engine.m_Animations.GetFrame(null, bodyID, action, direction, num103, num89, num90, notoriety, ref num95, ref num96, flag26);
                                                        if ((frame2.Image != null) && !frame2.Image.IsEmpty())
                                                        {
                                                            AlphaTestEnable = true;
                                                            frame2.Image.DrawGame(num95, num96);
                                                        }
                                                    }
                                                }
                                            }
                                            else if ((type3 == tMobileCell) && (mobile4 != null))
                                            {
                                                int num105 = 0;
                                                int num106 = ((by - (cell5.Z << 2)) + 0x12) - (num105 = Engine.m_Animations.GetHeight(body, action, direction));
                                                int num107 = bx + 0x16;
                                                num107 += num85;
                                                num106 += num86;
                                                if (((m_MiniHealth && mobile4.OpenedStatus) && ((mobile4.StatusBar == null) && !mobile4.Ghost)) && (mobile4.HPMax > 0))
                                                {
                                                    miniHealthQueue.Enqueue(MiniHealthEntry.PoolInstance(num107, (num106 + 4) + num105, mobile4));
                                                }
                                                if (mobile4.HumanOrGhost)
                                                {
                                                    int num108 = equip.Count;
                                                    bool flag33 = false;
                                                    bool flag34 = false;
                                                    for (int num109 = 0; num109 < num108; num109++)
                                                    {
                                                        EquipEntry entry4 = (EquipEntry)equip[num109];
                                                        if (!ghost || (entry4.m_Layer == Layer.OuterTorso))
                                                        {
                                                            int num110 = entry4.m_Animation;
                                                            int num111 = action;
                                                            int num112 = num83;
                                                            if (ghost)
                                                            {
                                                                num110 = 970;
                                                            }
                                                            if (entry4.m_Layer == Layer.Mount)
                                                            {
                                                                flag33 = true;
                                                                if (mobile4.IsMoving)
                                                                {
                                                                    num111 = ((direction & 0x80) == 0) ? 0 : 1;
                                                                }
                                                                else if (mobile4.Animation == null)
                                                                {
                                                                    num111 = 2;
                                                                }
                                                                else if (num111 == 0x17)
                                                                {
                                                                    num111 = 0;
                                                                }
                                                                else if (num111 == 0x18)
                                                                {
                                                                    num111 = 1;
                                                                }
                                                                else
                                                                {
                                                                    if ((num111 < 0x19) || (num111 > 0x1d))
                                                                    {
                                                                        goto Label_3A65;
                                                                    }
                                                                    num111 = 2;
                                                                }
                                                            }
                                                            else if (flag33)
                                                            {
                                                                if (mobile4.IsMoving)
                                                                {
                                                                    num111 = 0x17 + ((direction & 0x80) >> 7);
                                                                }
                                                                else if (mobile4.Animation == null)
                                                                {
                                                                    num111 = 0x19;
                                                                }
                                                            }
                                                            bool flag35 = enable;
                                                            float num113 = alpha;
                                                            int num114 = entry4.m_Item.Hue;
                                                            if (entry4.m_Layer == Layer.Mount)
                                                            {
                                                                int num115 = num110;
                                                                Engine.m_Animations.Translate(ref num115, ref num114);
                                                            }
                                                            bool flag36 = false;
                                                            if ((!flag26 || ((entry4.m_Layer == Layer.Mount) && flag28)) && ((num114 & 0x4000) != 0))
                                                            {
                                                                flag35 = true;
                                                                num113 = 1f;
                                                                flag36 = true;
                                                                notoriety = Hues.Load(0x8bb8);
                                                                flag34 = true;
                                                            }
                                                            else if (!flag26 || ((entry4.m_Layer == Layer.Mount) && flag28))
                                                            {
                                                                notoriety = Hues.GetItemHue(entry4.m_Item.ID, entry4.m_Item.Hue);
                                                                flag34 = false;
                                                            }
                                                            else if (flag26)
                                                            {
                                                                notoriety = hue9;
                                                                flag34 = true;
                                                            }
                                                            else
                                                            {
                                                                flag34 = false;
                                                            }
                                                            int num116 = Engine.m_Animations.GetFrameCount(num110, num111, direction);
                                                            if (num116 == 0)
                                                            {
                                                                num112 = 0;
                                                            }
                                                            else
                                                            {
                                                                num112 = num112 % num116;
                                                            }
                                                            frame2 = Engine.m_Animations.GetFrame(entry4.m_Item, num110, num111, direction, num112, num89, num90, notoriety, ref num95, ref num96, flag34);
                                                            if ((frame2.Image != null) && !frame2.Image.IsEmpty())
                                                            {
                                                                if (!ghost)
                                                                {
                                                                    if (m_AlphaEnable != flag35)
                                                                    {
                                                                        SetAlphaEnablePrecalc(flag35);
                                                                    }
                                                                    SetAlpha(num113);
                                                                }
                                                                else
                                                                {
                                                                    if (!m_AlphaEnable)
                                                                    {
                                                                        SetAlphaEnablePrecalc(true);
                                                                    }
                                                                    SetAlpha(0.5f);
                                                                }
                                                                if (m_FilterEnable)
                                                                {
                                                                    SetFilterEnablePrecalc(false);
                                                                }
                                                                AlphaTestEnable = true;
                                                                if (flag36)
                                                                {
                                                                    SetBlendType(DrawBlendType.BlackTransparency);
                                                                }
                                                                entry4.DrawGame(frame2.Image, num95, num96);
                                                                if (flag36)
                                                                {
                                                                    SetBlendType(DrawBlendType.Normal);
                                                                }
                                                            }
                                                        }
                                                    Label_3A65:
                                                        if ((entry4.m_Layer == Layer.Mount) && flag29)
                                                        {
                                                            if (m_AlphaEnable != alphaEnable)
                                                            {
                                                                SetAlphaEnablePrecalc(alphaEnable);
                                                            }
                                                            if (alphaEnable)
                                                            {
                                                                SetAlpha(fAlpha);
                                                            }
                                                            if (flag31)
                                                            {
                                                                SetBlendType(DrawBlendType.BlackTransparency);
                                                            }
                                                            if (flag24 && (mobile4 != null))
                                                            {
                                                                mobile4.DrawGame(t, num98, num99);
                                                            }
                                                            else
                                                            {
                                                                t.DrawGame(num98, num99);
                                                            }
                                                            if (flag31)
                                                            {
                                                                SetBlendType(DrawBlendType.Normal);
                                                            }
                                                        }
                                                    }
                                                }
                                                if (flag24 && (mobile4 != null))
                                                {
                                                    if ((mobile4 == Engine.m_LastHarmTarget) && (player.Warmode || ((Engine.TargetHandler is ServerTargetHandler) && ((((ServerTargetHandler)Engine.TargetHandler).Flags & ServerTargetFlags.Harmful) != ServerTargetFlags.None))))
                                                    {
                                                        DrawColoredIcon(num89, num90, 0xff2200, false, true);
                                                    }
                                                    else if ((mobile4 == Engine.m_LastBenTarget) && (player.Warmode || ((Engine.TargetHandler is ServerTargetHandler) && ((((ServerTargetHandler)Engine.TargetHandler).Flags & ServerTargetFlags.Beneficial) != ServerTargetFlags.None))))
                                                    {
                                                        DrawColoredIcon(num89, num90, 0xffff, false, true);
                                                    }
                                                    else if ((mobile4 == Engine.m_LastTarget) && (player.Warmode || ((Engine.TargetHandler is ServerTargetHandler) && ((((ServerTargetHandler)Engine.TargetHandler).Flags & (ServerTargetFlags.Beneficial | ServerTargetFlags.Harmful)) == ServerTargetFlags.None))))
                                                    {
                                                        DrawColoredIcon(num89, num90, 0xcccccc, false, true);
                                                    }
                                                }
                                                if ((flag24 && (mobile4 != null)) && World.CharData.Halos)
                                                {
                                                    int color = -1;
                                                    bool formation = false;
                                                    if (mobile4.m_IsFriend)
                                                    {
                                                        color = 0xff22;
                                                    }
                                                    else if (!mobile4.Player && player.Warmode)
                                                    {
                                                        bool flag38 = false;
                                                        if (mobile4.Notoriety == Notoriety.Innocent)
                                                        {
                                                            int num118 = mobile4.X - package.CellX;
                                                            int num119 = mobile4.Y - package.CellY;
                                                            if ((((num118 >= 0) && (num118 < cellWidth)) && ((num119 >= 0) && (num119 < cellHeight))) && package.landTiles[num118, num119].m_Guarded)
                                                            {
                                                                flag38 = true;
                                                            }
                                                        }
                                                        if (!flag38 && mobile4.Human)
                                                        {
                                                            if (((mobile == null) || (mobile4.Serial < mobile.Serial)) && player.InSquareRange(mobile4, 12))
                                                            {
                                                                mobile = mobile4;
                                                            }
                                                            color = 0xff2200;
                                                            formation = mobile4 == m_LastEnemy;
                                                        }
                                                    }
                                                    if (color != -1)
                                                    {
                                                        DrawColoredIcon(num89, num96 - 10, color, formation, false);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (((multiPreview && (x >= num28)) && ((x <= num30) && (y >= num29))) && (y <= num31))
                                {
                                    int num120 = bx + 0x16;
                                    int num121 = by + 0x2b;
                                    if (m_vMultiPool == null)
                                    {
                                        m_vMultiPool = VertexConstructor.Create();
                                    }
                                    for (int num122 = 0; num122 < num27; num122++)
                                    {
                                        MultiItem item7 = (MultiItem)Engine.m_MultiList[num122];
                                        if (((item7.X == (x - num23)) && (item7.Y == (y - num24))) && (item7.Z == 0))
                                        {
                                            num26 = num122 + 1;
                                            int num123 = item7.ItemID & 0x3fff;
                                            AnimData data2 = Map.GetAnim(num123);
                                            Client.Texture texture5 = null;
                                            if ((data2.frameCount == 0) || !Map.m_ItemFlags[num123][TileFlag.Animation])
                                            {
                                                texture5 = hue.GetItem(item7.ItemID);
                                            }
                                            else
                                            {
                                                texture5 = hue.GetItem(item7.ItemID + data2[(m_Frames / (data2.frameInterval + 1)) % data2.frameCount]);
                                            }
                                            if ((texture5 != null) && !texture5.IsEmpty())
                                            {
                                                if (m_AlphaEnable)
                                                {
                                                    m_AlphaEnable = false;
                                                }
                                                if (!m_AlphaTestEnable)
                                                {
                                                    m_AlphaTestEnable = true;
                                                }
                                                if (m_FilterEnable)
                                                {
                                                    SetFilterEnablePrecalc(false);
                                                }
                                                texture5.DrawGame(num120 - (texture5.Width >> 1), (num121 - ((num25 + item7.Z) << 2)) - texture5.Height, m_vMultiPool);
                                            }
                                        }
                                        else if (((item7.X + num23) > x) || (((item7.X + num23) == x) && ((item7.Y + num24) >= y)))
                                        {
                                            break;
                                        }
                                    }
                                }
                                for (int num124 = 0; num124 < list2.Count; num124++)
                                {
                                    DrawQueueEntry entry5 = (DrawQueueEntry)list2[num124];
                                    if ((entry5.m_TileX == x) && (entry5.m_TileY == y))
                                    {
                                        list2.RemoveAt(num124);
                                        num124--;
                                        entry5.m_Texture.Flip = entry5.m_Flip;
                                        Clipper clipper = new Clipper(Engine.GameX, by - 0x2e, Engine.GameWidth, (Engine.GameHeight - by) + 0x2e);
                                        if (m_AlphaEnable != entry5.m_bAlpha)
                                        {
                                            SetAlphaEnablePrecalc(entry5.m_bAlpha);
                                        }
                                        if (entry5.m_bAlpha)
                                        {
                                            SetAlpha(entry5.m_fAlpha);
                                        }
                                        if (!m_AlphaTestEnable)
                                        {
                                            m_AlphaTestEnable = true;
                                        }
                                        if (m_FilterEnable)
                                        {
                                            SetFilterEnablePrecalc(false);
                                        }
                                        entry5.m_Texture.DrawClipped(entry5.m_DrawX, entry5.m_DrawY, clipper);
                                    }
                                }
                                x++;
                                y--;
                            }
                        }
                        if (m_FilterEnable)
                        {
                            SetFilterEnablePrecalc(false);
                        }
                        if (m_Transparency)
                        {
                            if (m_vTransDrawPool == null)
                            {
                                m_vTransDrawPool = VertexConstructor.Create();
                            }
                            while (transDrawQueue.Count > 0)
                            {
                                TransparentDraw draw = (TransparentDraw)transDrawQueue.Dequeue();
                                if (m_AlphaEnable != draw.m_bAlpha)
                                {
                                    SetAlphaEnablePrecalc(draw.m_bAlpha);
                                }
                                if (draw.m_bAlpha)
                                {
                                    SetAlpha(draw.m_fAlpha);
                                }
                                AlphaTestEnable = true;
                                draw.m_Texture.DrawGame(draw.m_X, draw.m_Y, m_vTransDrawPool);
                                if (draw.m_Double)
                                {
                                    draw.m_Texture.DrawGame(draw.m_X + 5, draw.m_Y + 5, m_vTransDrawPool);
                                }
                                draw.Dispose();
                            }
                        }
                        SetTexture(null);
                        if (miniHealthQueue.Count > 0)
                        {
                            while (miniHealthQueue.Count > 0)
                            {
                                int num127;
                                int num128;
                                MiniHealthEntry entry6 = (MiniHealthEntry)miniHealthQueue.Dequeue();
                                Mobile mobile5 = entry6.m_Mobile;
                                if (m_AlphaEnable)
                                {
                                    SetAlphaEnablePrecalc(false);
                                }
                                AlphaTestEnable = false;
                                TransparentRect(0, entry6.m_X - 0x10, entry6.m_Y + 8, 0x20, 7);
                                double num125 = ((double)mobile5.HPCur) / ((double)mobile5.HPMax);
                                if (num125 == double.NaN)
                                {
                                    num125 = 0.0;
                                }
                                else if (num125 < 0.0)
                                {
                                    num125 = 0.0;
                                }
                                else if (num125 > 1.0)
                                {
                                    num125 = 1.0;
                                }
                                int width = (int)((30.0 * num125) + 0.5);
                                if (!m_AlphaEnable)
                                {
                                    SetAlphaEnablePrecalc(true);
                                }
                                SetAlpha(0.6f);
                                MobileFlags flags2 = mobile5.Flags;
                                if (flags2[MobileFlag.Poisoned])
                                {
                                    num127 = 0xff00;
                                    num128 = 0x8000;
                                }
                                else if (flags2[MobileFlag.YellowHits])
                                {
                                    num127 = 0xffc000;
                                    num128 = 0x806000;
                                }
                                else
                                {
                                    num127 = 0x20c0ff;
                                    num128 = 0x106080;
                                }
                                GradientRect(num127, num128, entry6.m_X - 15, entry6.m_Y + 9, width, 5);
                                GradientRect(0xc80000, 0x640000, (entry6.m_X - 15) + width, entry6.m_Y + 9, 30 - width, 5);
                                AlphaTestEnable = true;
                                entry6.Dispose();
                            }
                        }
                        if (Engine.m_Ingame)
                        {
                            if (m_AlphaEnable)
                            {
                                SetAlphaEnablePrecalc(false);
                            }
                            AlphaTestEnable = true;
                            Engine.Effects.Draw();
                            if ((eOffsetX != 0) || (eOffsetY != 0))
                            {
                                Engine.Effects.Offset(eOffsetX, eOffsetY);
                            }
                        }
                        Map.Unlock();
                        SetViewport(0, 0, Engine.ScreenWidth, Engine.ScreenHeight);
                    }
                    if (!Engine.m_Loading)
                    {
                        if (m_AlphaEnable)
                        {
                            SetAlphaEnablePrecalc(false);
                        }
                        AlphaTestEnable = true;
                        MessageManager.BeginRender();
                        if (Engine.m_Ingame && (m_TextSurface != null))
                        {
                            SetTexture(null);
                            AlphaTestEnable = false;
                            if (!m_AlphaEnable)
                            {
                                SetAlphaEnablePrecalc(true);
                            }
                            SetAlpha(0.5f);
                            if (((player != null) && player.OpenedStatus) && (player.StatusBar == null))
                            {
                                SolidRect(0, Engine.GameX + 2, (Engine.GameY + Engine.GameHeight) - 0x15, Engine.GameWidth - 0x2e, 0x13);
                                SetAlphaEnablePrecalc(false);
                                int num129 = (Engine.GameX + Engine.GameWidth) - 0x2c;
                                int num130 = (Engine.GameY + Engine.GameHeight) - 0x15;
                                SolidRect(0, num129, num130, 0x2a, 0x13);
                                num129++;
                                num130++;
                                if (player.Ghost)
                                {
                                    GradientRect(0xc0c0c0, 0x606060, num129, num130, 40, 5);
                                    num130 += 6;
                                    GradientRect(0xc0c0c0, 0x606060, num129, num130, 40, 5);
                                    num130 += 6;
                                    GradientRect(0xc0c0c0, 0x606060, num129, num130, 40, 5);
                                }
                                else
                                {
                                    int num132;
                                    int num133;
                                    int num131 = (int)((((double)player.HPCur) / ((double)player.HPMax)) * 40.0);
                                    if (num131 > 40)
                                    {
                                        num131 = 40;
                                    }
                                    else if (num131 < 0)
                                    {
                                        num131 = 0;
                                    }
                                    MobileFlags flags3 = player.Flags;
                                    if (flags3[MobileFlag.Poisoned])
                                    {
                                        num132 = 0xff00;
                                        num133 = 0x8000;
                                    }
                                    else if (flags3[MobileFlag.YellowHits])
                                    {
                                        num132 = 0xffc000;
                                        num133 = 0x806000;
                                    }
                                    else
                                    {
                                        num132 = 0x20c0ff;
                                        num133 = 0x106080;
                                    }
                                    GradientRect(num132, num133, num129, num130, num131, 5);
                                    GradientRect(0xff0000, 0x800000, num129 + num131, num130, 40 - num131, 5);
                                    num130 += 6;
                                    num131 = (int)((((double)player.ManaCur) / ((double)player.ManaMax)) * 40.0);
                                    if (num131 > 40)
                                    {
                                        num131 = 40;
                                    }
                                    else if (num131 < 0)
                                    {
                                        num131 = 0;
                                    }
                                    GradientRect(0x20c0ff, 0x106080, num129, num130, 40, 5);
                                    GradientRect(0xff0000, 0x800000, num129 + num131, num130, 40 - num131, 5);
                                    num130 += 6;
                                    num131 = (int)((((double)player.StamCur) / ((double)player.StamMax)) * 40.0);
                                    if (num131 > 40)
                                    {
                                        num131 = 40;
                                    }
                                    else if (num131 < 0)
                                    {
                                        num131 = 0;
                                    }
                                    GradientRect(0x20c0ff, 0x106080, num129, num130, 40, 5);
                                    GradientRect(0xff0000, 0x800000, num129 + num131, num130, 40 - num131, 5);
                                }
                            }
                            else
                            {
                                SolidRect(0, Engine.GameX + 2, (Engine.GameY + Engine.GameHeight) - 0x15, Engine.GameWidth - 4, 0x13);
                            }
                            if (m_AlphaEnable)
                            {
                                SetAlphaEnablePrecalc(false);
                            }
                            AlphaTestEnable = true;
                            m_vTextCache.Draw(m_TextSurface, Engine.GameX + 2, ((Engine.GameY + Engine.GameHeight) - 2) - m_TextSurface.Height);
                        }
                        Gumps.Draw();
                    }
                    if (Engine.m_Ingame)
                    {
                        int num134 = (Engine.GameY + Engine.GameHeight) - ((m_TextSurface == null) ? 2 : (m_TextSurface.Height + 4));
                        ArrayList rectsList = m_RectsList;
                        if (rectsList == null)
                        {
                            rectsList = m_RectsList = new ArrayList();
                        }
                        else if (rectsList.Count > 0)
                        {
                            rectsList.Clear();
                        }
                        World.DrawAllMessages();
                        textToDrawList.Sort();
                        int num135 = textToDrawList.Count;
                        for (int num136 = 0; num136 < num135; num136++)
                        {
                            TextMessage message = (TextMessage)textToDrawList[num136];
                            int num137 = message.X + Engine.GameX;
                            int num138 = message.Y + Engine.GameY;
                            if (num137 < (Engine.GameX + 2))
                            {
                                num137 = Engine.GameX + 2;
                            }
                            else if ((num137 + message.Image.Width) >= ((Engine.GameX + Engine.GameWidth) - 2))
                            {
                                num137 = ((Engine.GameX + Engine.GameWidth) - message.Image.Width) - 2;
                            }
                            if (num138 < (Engine.GameY + 2))
                            {
                                num138 = Engine.GameY + 2;
                            }
                            else if ((num138 + message.Image.Height) >= ((Engine.GameY + Engine.GameHeight) - 2))
                            {
                                num138 = ((Engine.GameY + Engine.GameHeight) - message.Image.Height) - 2;
                            }
                            rectsList.Add(new Rectangle(num137, num138, message.Image.Width, message.Image.Height));
                        }
                        for (int num139 = 0; num139 < num135; num139++)
                        {
                            TextMessage message2 = (TextMessage)textToDrawList[num139];
                            Rectangle rect = (Rectangle)rectsList[num139];
                            float num140 = 1f;
                            int num141 = rectsList.Count;
                            for (int num142 = num139 + 1; num142 < num141; num142++)
                            {
                                Rectangle rectangle3 = (Rectangle)rectsList[num142];
                                if (rectangle3.IntersectsWith(rect))
                                {
                                    num140 += ((TextMessage)textToDrawList[num142]).Alpha;
                                }
                            }
                            if ((num140 == 1f) && !message2.Disposing)
                            {
                                if (m_AlphaEnable)
                                {
                                    SetAlphaEnablePrecalc(false);
                                }
                            }
                            else
                            {
                                if (!m_AlphaEnable)
                                {
                                    SetAlphaEnablePrecalc(true);
                                }
                                if (message2.Disposing)
                                {
                                    SetAlpha((1f / num140) * message2.Alpha);
                                }
                                else
                                {
                                    SetAlpha(1f / num140);
                                }
                            }
                            AlphaTestEnable = true;
                            message2.Draw(rect.X, rect.Y);
                        }
                        if ((eOffsetX != 0) || (eOffsetY != 0))
                        {
                            World.Offset(eOffsetX, eOffsetY);
                        }
                    }
                    if (!Engine.m_Loading)
                    {
                        if (m_AlphaEnable)
                        {
                            SetAlphaEnablePrecalc(false);
                        }
                        AlphaTestEnable = true;
                        Client.Cursor.Draw();
                    }
                    PushAll();
                }
                catch (Exception exception)
                {
                    Debug.Trace("Draw Exception:");
                    Debug.Error(exception);
                }
                finally
                {
                    Engine.m_Device.EndScene();
                }
                Engine.m_Device.Present();
                m_Count = 0;
                m_LastEnemy = mobile;
                if (m_ScreenShot != null)
                {
                    Save(m_ScreenShot);
                }
                m_ActFrames++;
                while (toUpdateQueue.Count > 0)
                {
                    Mobile mobile6 = (Mobile)toUpdateQueue.Dequeue();
                    mobile6.MovedTiles++;
                    mobile6.Update();
                }
                Map.Unlock();
            }
        }

        public static ICell FindTileFromXY(int mx, int my, ref short TileX, ref short TileY)
        {
            return FindTileFromXY(mx, my, ref TileX, ref TileY, false);
        }

        public static ICell FindTileFromXY(int mx, int my, ref short TileX, ref short TileY, bool onlyMobs)
        {
            if (World.Serial == 0)
            {
                return null;
            }
            m_MousePoint.X = mx;
            m_MousePoint.Y = my;
            Mobile player = World.Player;
            int x = 0;
            int y = 0;
            int z = 0;
            if (player != null)
            {
                x = player.X;
                y = player.Y;
                z = player.Z;
            }
            if (((!Engine.m_Ingame || (mx < Engine.GameX)) || ((my < Engine.GameY) || (mx >= (Engine.GameX + Engine.GameWidth)))) || (my >= (Engine.GameY + Engine.GameHeight)))
            {
                goto Label_1ED4;
            }
            MapPackage package = Map.GetMap((x >> 3) - (blockWidth >> 1), (y >> 3) - (blockHeight >> 1), blockWidth, blockHeight);
            int num4 = ((x >> 3) - (blockWidth >> 1)) << 3;
            int num5 = ((y >> 3) - (blockHeight >> 1)) << 3;
            ArrayList[,] cells = package.cells;
            LandTile[,] landTiles = package.landTiles;
            int num6 = x & 7;
            int num7 = y & 7;
            int num8 = ((blockWidth / 2) * 8) + num6;
            int num9 = ((blockHeight / 2) * 8) + num7;
            int num10 = 0;
            int num11 = 0;
            num10 = Engine.GameWidth >> 1;
            num10 -= 0x16;
            num10 += (4 - num6) * 0x16;
            num10 -= (4 - num7) * 0x16;
            num11 += (4 - num6) * 0x16;
            num11 += (4 - num7) * 0x16;
            num11 += z << 2;
            num11 += ((((Engine.GameHeight >> 1) - ((num8 + num9) * 0x16)) - ((4 - num7) * 0x16)) - ((4 - num6) * 0x16)) - 0x16;
            num10--;
            num11--;
            num10 += Engine.GameX;
            num11 += Engine.GameY;
            int num12 = 0x7fffffff;
            int num13 = 0x7fffffff;
            int count = cells[num8 + 1, num9 + 1].Count;
            for (int i = 0; i < count; i++)
            {
                ICell cell = (ICell)cells[num8 + 1, num9 + 1][i];
                System.Type cellType = cell.CellType;
                if ((cellType == tStaticItem) || (cellType == tDynamicItem))
                {
                    ITile tile = (ITile)cell;
                    if ((Map.m_ItemFlags[tile.ID & 0x3fff][TileFlag.Roof] && (tile.Z >= (z + 15))) && (tile.Z < num13))
                    {
                        num13 = tile.Z;
                    }
                }
            }
            count = cells[num8, num9].Count;
            for (int j = 0; j < count; j++)
            {
                ICell cell2 = (ICell)cells[num8, num9][j];
                System.Type type2 = cell2.CellType;
                if (((type2 == tStaticItem) || (type2 == tDynamicItem)) || (type2 == tLandTile))
                {
                    ITile tile2 = (ITile)cell2;
                    if (!Map.GetTileFlags(tile2.ID)[TileFlag.Roof])
                    {
                        int num17 = (type2 == tLandTile) ? tile2.SortZ : tile2.Z;
                        if (num17 >= (z + 15))
                        {
                            if (type2 == tLandTile)
                            {
                                if (num17 < num13)
                                {
                                    num13 = num17;
                                }
                                if ((z + 0x10) < num12)
                                {
                                    num12 = z + 0x10;
                                }
                            }
                            else if (num17 < num13)
                            {
                                num13 = num17;
                            }
                        }
                    }
                }
            }
            ICell lastFind = m_LastFind;
            if (((lastFind != null) && (xwLast == x)) && ((ywLast == y) && (zwLast == z)))
            {
                System.Type type3 = lastFind.CellType;
                if (onlyMobs ? (type3 == tMobileCell) : true)
                {
                    int num18 = (xLast - yLast) * 0x16;
                    int num19 = (xLast + yLast) * 0x16;
                    if (type3 != tMobileCell)
                    {
                        if ((type3 != tStaticItem) && (type3 != tDynamicItem))
                        {
                            if (type3 == tLandTile)
                            {
                                LandTile tile3 = (LandTile)lastFind;
                                int num45 = tile3.m_Z;
                                if (((num12 >= 0x7fffffff) || (tile3.SortZ < num12)) && (tile3.m_ID != 2))
                                {
                                    int num46 = num18 + num10;
                                    int num47 = num19 + num11;
                                    if ((mx >= num46) && (mx < (num46 + 0x2c)))
                                    {
                                        m_PointPool[0].X = num46 + 0x16;
                                        m_PointPool[0].Y = num47 - (num45 << 2);
                                        m_PointPool[1].X = num46 + 0x2c;
                                        m_PointPool[1].Y = (num47 + 0x16) - (landTiles[xLast + 1, yLast + 1].m_Z << 2);
                                        m_PointPool[2].X = num46 + 0x16;
                                        m_PointPool[2].Y = (num47 + 0x2c) - (landTiles[xLast + 1, yLast + 1].m_Z << 2);
                                        m_PointPool[3].X = num46;
                                        m_PointPool[3].Y = (num47 + 0x16) - (landTiles[xLast, yLast + 1].m_Z << 2);
                                        if (LandTileHitTest(m_PointPool, m_MousePoint))
                                        {
                                            TileX = (short)(num4 + xLast);
                                            TileY = (short)(num5 + yLast);
                                            return lastFind;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            IItem item = (IItem)lastFind;
                            if (((((((item.ID != 0x4001) && (item.ID != 0x5796)) && ((item.ID != 0x61a4) && (item.ID != 0x6198))) && (item.ID != 0x61bc)) && (item.ID != 0x6199)) && ((num13 >= 0x7fffffff) || ((lastFind.Z < num13) && !Map.m_ItemFlags[item.ID & 0x3fff][TileFlag.Roof]))) && (((!Map.m_ItemFlags[item.ID & 0x3fff][TileFlag.Foliage] || (xLast < num8)) || ((yLast < num9) || (xLast >= (num8 + 8)))) || (yLast >= (num8 + 8))))
                            {
                                Client.Texture texture;
                                if (item.CellType == tDynamicItem)
                                {
                                    DynamicItem item2 = (DynamicItem)item;
                                    Item owner = item2.m_Item;
                                    if ((owner != null) && (owner.ID == 0x2006))
                                    {
                                        int amount = owner.Amount;
                                        CorpseTableEntry entry = (CorpseTableEntry)CorpseTable.m_Entries[amount];
                                        if ((entry != null) && (BodyConverter.GetFileSet(amount) == 1))
                                        {
                                            amount = entry.m_OldID;
                                        }
                                        int animDirection = Engine.GetAnimDirection(owner.Direction);
                                        int actionID = Engine.m_Animations.ConvertAction(amount, owner.Serial, owner.X, owner.Y, animDirection, GenericAction.Die, null);
                                        int num35 = Engine.m_Animations.GetFrameCount(amount, actionID, animDirection);
                                        int xCenter = num18 + 0x17;
                                        int yCenter = (num19 - (owner.Z << 2)) + 20;
                                        IHue h = Hues.Default;
                                        int textureX = 0;
                                        int textureY = 0;
                                        Frame frame2 = Engine.m_Animations.GetFrame(owner, amount, actionID, animDirection, num35 - 1, xCenter, yCenter, h, ref textureX, ref textureY, true);
                                        textureX += num10;
                                        textureY += num11;
                                        int num40 = textureX;
                                        int num41 = textureY;
                                        if (((mx < num40) || (my < num41)) || ((mx >= (num40 + frame2.Image.Width)) || (my >= (num41 + frame2.Image.Height))))
                                        {
                                            goto Label_156B;
                                        }
                                        if (frame2.Image.Flip)
                                        {
                                            if (frame2.Image.HitTest(-(mx - num40), my - num41))
                                            {
                                                TileX = owner.X;
                                                TileY = owner.Y;
                                                return lastFind;
                                            }
                                            goto Label_156B;
                                        }
                                        if (!frame2.Image.HitTest(mx - num40, my - num41))
                                        {
                                            goto Label_156B;
                                        }
                                        TileX = owner.X;
                                        TileY = owner.Y;
                                        return lastFind;
                                    }
                                }
                                int id = item.ID & 0x3fff;
                                bool xDouble = false;
                                if (type3 == tStaticItem)
                                {
                                    id = Map.GetDispID(id, 0, ref xDouble);
                                }
                                else
                                {
                                    Item item4 = ((DynamicItem)lastFind).m_Item;
                                    if (item4 == null)
                                    {
                                        id = Map.GetDispID(id, 0, ref xDouble);
                                    }
                                    else
                                    {
                                        id = Map.GetDispID(id, item4.Amount, ref xDouble);
                                    }
                                }
                                AnimData anim = Map.GetAnim(id);
                                if ((anim.frameCount == 0) || !Map.m_ItemFlags[id][TileFlag.Animation])
                                {
                                    texture = Hues.Default.GetItem(id);
                                }
                                else
                                {
                                    texture = Hues.Default.GetItem(id + anim[(m_Frames / (anim.frameInterval + 1)) % anim.frameCount]);
                                }
                                if ((texture != null) && !texture.IsEmpty())
                                {
                                    int num43 = num18 + 0x16;
                                    int num44 = (num19 - (lastFind.Z << 2)) + 0x2b;
                                    num43 -= texture.Width >> 1;
                                    num44 -= texture.Height;
                                    num43 += num10;
                                    num44 += num11;
                                    if (((!xDouble || (mx < num43)) || ((my < num44) || (mx >= ((num43 + texture.Width) + 5)))) || (my >= ((num44 + texture.Height) + 5)))
                                    {
                                        if (((!xDouble && (mx >= num43)) && ((my >= num44) && (mx < (num43 + texture.Width)))) && ((my < (num44 + texture.Height)) && texture.HitTest(mx - num43, my - num44)))
                                        {
                                            TileX = (short)(num4 + xLast);
                                            TileY = (short)(num5 + yLast);
                                            return lastFind;
                                        }
                                    }
                                    else
                                    {
                                        mx -= num43;
                                        my -= num44;
                                        if ((((mx < texture.Width) && (my < texture.Height)) && texture.HitTest(mx, my)) || (((mx >= 5) && (my >= 5)) && texture.HitTest(mx - 5, my - 5)))
                                        {
                                            TileX = (short)(num4 + xLast);
                                            TileY = (short)(num5 + yLast);
                                            return lastFind;
                                        }
                                        mx += num43;
                                        my += num44;
                                    }
                                }
                            }
                        }
                    }
                    else if ((num13 >= 0x7fffffff) || (lastFind.Z < num13))
                    {
                        IAnimatedCell cell4 = (IAnimatedCell)lastFind;
                        int body = 0;
                        int direction = 0;
                        int num22 = 0;
                        int action = 0;
                        int num24 = 0;
                        cell4.GetPackage(ref body, ref action, ref direction, ref num24, ref num22);
                        int num25 = num18 + 0x16;
                        int num26 = (num19 - (cell4.Z << 2)) + 0x16;
                        num25++;
                        num26 -= 2;
                        Mobile mobile = ((MobileCell)lastFind).m_Mobile;
                        if (mobile != null)
                        {
                            IHue grayscale;
                            if (mobile.Flags[MobileFlag.Hidden])
                            {
                                grayscale = Hues.Grayscale;
                            }
                            else if (Engine.m_Highlight == mobile)
                            {
                                grayscale = Hues.GetNotoriety(mobile.Notoriety);
                            }
                            else
                            {
                                grayscale = Hues.Load(num22);
                            }
                            int num27 = 0;
                            int num28 = 0;
                            Frame frame = Engine.m_Animations.GetFrame(mobile, body, action, direction, num24, num25, num26, grayscale, ref num27, ref num28, false);
                            if ((frame.Image != null) && !frame.Image.IsEmpty())
                            {
                                num27 += num10;
                                num28 += num11;
                                int num29 = num27;
                                int num30 = num28;
                                if (((mx >= num29) && (my >= num30)) && ((mx < (num29 + frame.Image.Width)) && (my < (num30 + frame.Image.Height))))
                                {
                                    if (!frame.Image.Flip)
                                    {
                                        if (frame.Image.HitTest(mx - num29, my - num30))
                                        {
                                            TileX = mobile.X;
                                            TileY = mobile.Y;
                                            return lastFind;
                                        }
                                    }
                                    else if (frame.Image.HitTest(-(mx - num29), my - num30))
                                    {
                                        TileX = mobile.X;
                                        TileY = mobile.Y;
                                        return lastFind;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int k = xLast + 6; k >= (xLast - 6); k--)
                    {
                        for (int m = yLast + 6; m >= (yLast - 6); m--)
                        {
                            if ((((k >= 0) && (m >= 0)) && (k < (cellWidth - 1))) && (m < (cellHeight - 1)))
                            {
                                int num50 = (k - m) * 0x16;
                                int num51 = (k + m) * 0x16;
                                for (int n = cells[k, m].Count - 1; n >= 0; n--)
                                {
                                    ICell cell5 = (ICell)cells[k, m][n];
                                    System.Type type4 = cell5.CellType;
                                    if (type4 == tMobileCell)
                                    {
                                        if ((num13 >= 0x7fffffff) || (cell5.Z < num13))
                                        {
                                            IHue notoriety;
                                            IAnimatedCell cell6 = (IAnimatedCell)cell5;
                                            int num54 = 0;
                                            int num55 = 0;
                                            int hue = 0;
                                            int num57 = 0;
                                            int num58 = 0;
                                            cell6.GetPackage(ref num54, ref num57, ref num55, ref num58, ref hue);
                                            int num59 = num50 + 0x16;
                                            int num60 = (num51 - (cell6.Z << 2)) + 0x16;
                                            num59++;
                                            num60 -= 2;
                                            Mobile mobile3 = ((MobileCell)cell5).m_Mobile;
                                            if (mobile3.Flags[MobileFlag.Hidden])
                                            {
                                                notoriety = Hues.Grayscale;
                                            }
                                            else if (Engine.m_Highlight == mobile3)
                                            {
                                                notoriety = Hues.GetNotoriety(mobile3.Notoriety);
                                            }
                                            else
                                            {
                                                notoriety = Hues.Load(hue);
                                            }
                                            int num61 = 0;
                                            int num62 = 0;
                                            Frame frame3 = Engine.m_Animations.GetFrame(mobile3, num54, num57, num55, num58, num59, num60, notoriety, ref num61, ref num62, false);
                                            if ((frame3.Image != null) && !frame3.Image.IsEmpty())
                                            {
                                                num61 += num10;
                                                num62 += num11;
                                                int num63 = num61;
                                                int num64 = num62;
                                                if (((mx >= num63) && (my >= num64)) && ((mx < (num63 + frame3.Image.Width)) && (my < (num64 + frame3.Image.Height))))
                                                {
                                                    if (!frame3.Image.Flip)
                                                    {
                                                        if (frame3.Image.HitTest(mx - num63, my - num64))
                                                        {
                                                            TileX = (short)(num4 + k);
                                                            TileY = (short)(num5 + m);
                                                            m_LastFind = cell5;
                                                            xLast = k;
                                                            yLast = m;
                                                            return cell5;
                                                        }
                                                    }
                                                    else if (frame3.Image.HitTest(-(mx - num63), my - num64))
                                                    {
                                                        TileX = (short)(num4 + k);
                                                        TileY = (short)(num5 + m);
                                                        m_LastFind = cell5;
                                                        xLast = k;
                                                        yLast = m;
                                                        return cell5;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if ((type4 == tStaticItem) || (type4 == tDynamicItem))
                                        {
                                            IItem item5 = (IItem)cell5;
                                            if (((((((item5.ID != 0x4001) && (item5.ID != 0x5796)) && ((item5.ID != 0x61a4) && (item5.ID != 0x6198))) && (item5.ID != 0x61bc)) && (item5.ID != 0x6199)) && ((num13 >= 0x7fffffff) || ((cell5.Z < num13) && !Map.m_ItemFlags[item5.ID & 0x3fff][TileFlag.Roof]))) && (((!Map.m_ItemFlags[item5.ID & 0x3fff][TileFlag.Foliage] || (k < num8)) || ((m < num9) || (k >= (num8 + 8)))) || (m >= (num8 + 8))))
                                            {
                                                Client.Texture texture2;
                                                if (item5.CellType == tDynamicItem)
                                                {
                                                    DynamicItem item6 = (DynamicItem)item5;
                                                    Item item7 = item6.m_Item;
                                                    if ((item7 != null) && (item7.ID == 0x2006))
                                                    {
                                                        int bodyID = item7.Amount;
                                                        CorpseTableEntry entry2 = (CorpseTableEntry)CorpseTable.m_Entries[bodyID];
                                                        if ((entry2 != null) && (BodyConverter.GetFileSet(bodyID) == 1))
                                                        {
                                                            bodyID = entry2.m_OldID;
                                                        }
                                                        int num67 = Engine.GetAnimDirection(item7.Direction);
                                                        int num68 = Engine.m_Animations.ConvertAction(bodyID, item7.Serial, item7.X, item7.Y, num67, GenericAction.Die, null);
                                                        int num69 = Engine.m_Animations.GetFrameCount(bodyID, num68, num67);
                                                        int num70 = num50 + 0x17;
                                                        int num71 = (num51 - (item7.Z << 2)) + 20;
                                                        IHue hue4 = Hues.Default;
                                                        int num72 = 0;
                                                        int num73 = 0;
                                                        Frame frame4 = Engine.m_Animations.GetFrame(item7, bodyID, num68, num67, num69 - 1, num70, num71, hue4, ref num72, ref num73, true);
                                                        num72 += num10;
                                                        num73 += num11;
                                                        int num74 = num72;
                                                        int num75 = num73;
                                                        if (((mx < num74) || (my < num75)) || ((mx >= (num74 + frame4.Image.Width)) || (my >= (num75 + frame4.Image.Height))))
                                                        {
                                                            goto Label_1535;
                                                        }
                                                        if (frame4.Image.Flip)
                                                        {
                                                            if (frame4.Image.HitTest(-(mx - num74), my - num75))
                                                            {
                                                                TileX = item7.X;
                                                                TileY = item7.Y;
                                                                m_LastFind = cell5;
                                                                xLast = k;
                                                                yLast = m;
                                                                return cell5;
                                                            }
                                                            goto Label_1535;
                                                        }
                                                        if (!frame4.Image.HitTest(mx - num74, my - num75))
                                                        {
                                                            goto Label_1535;
                                                        }
                                                        TileX = item7.X;
                                                        TileY = item7.Y;
                                                        m_LastFind = cell5;
                                                        xLast = k;
                                                        yLast = m;
                                                        return cell5;
                                                    }
                                                }
                                                int num76 = item5.ID & 0x3fff;
                                                bool flag4 = false;
                                                if (type4 == tStaticItem)
                                                {
                                                    num76 = Map.GetDispID(num76, 0, ref flag4);
                                                }
                                                else
                                                {
                                                    Item item8 = ((DynamicItem)cell5).m_Item;
                                                    if (item8 == null)
                                                    {
                                                        num76 = Map.GetDispID(num76, 0, ref flag4);
                                                    }
                                                    else
                                                    {
                                                        num76 = Map.GetDispID(num76, item8.Amount, ref flag4);
                                                    }
                                                }
                                                AnimData data2 = Map.GetAnim(num76);
                                                if ((data2.frameCount == 0) || !Map.m_ItemFlags[num76][TileFlag.Animation])
                                                {
                                                    texture2 = Hues.Default.GetItem(num76);
                                                }
                                                else
                                                {
                                                    texture2 = Hues.Default.GetItem(num76 + data2[(m_Frames / (data2.frameInterval + 1)) % data2.frameCount]);
                                                }
                                                if ((texture2 != null) && !texture2.IsEmpty())
                                                {
                                                    int num77 = num50 + 0x16;
                                                    int num78 = (num51 - (cell5.Z << 2)) + 0x2b;
                                                    num77 -= texture2.Width >> 1;
                                                    num78 -= texture2.Height;
                                                    num77 += num10;
                                                    num78 += num11;
                                                    if (((!flag4 || (mx < num77)) || ((my < num78) || (mx >= ((num77 + texture2.Width) + 5)))) || (my >= ((num78 + texture2.Height) + 5)))
                                                    {
                                                        if (((!flag4 && (mx >= num77)) && ((my >= num78) && (mx < (num77 + texture2.Width)))) && ((my < (num78 + texture2.Height)) && texture2.HitTest(mx - num77, my - num78)))
                                                        {
                                                            TileX = (short)(num4 + k);
                                                            TileY = (short)(num5 + m);
                                                            m_LastFind = cell5;
                                                            xLast = k;
                                                            yLast = m;
                                                            return cell5;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        mx -= num77;
                                                        my -= num78;
                                                        if ((((mx < texture2.Width) && (my < texture2.Height)) && texture2.HitTest(mx, my)) || (((mx >= 5) && (my >= 5)) && texture2.HitTest(mx - 5, my - 5)))
                                                        {
                                                            TileX = (short)(num4 + k);
                                                            TileY = (short)(num5 + m);
                                                            m_LastFind = cell5;
                                                            xLast = k;
                                                            yLast = m;
                                                            return cell5;
                                                        }
                                                        mx += num77;
                                                        my += num78;
                                                    }
                                                }
                                            }
                                        }
                                        else if (type4 == tLandTile)
                                        {
                                            LandTile tile4 = (LandTile)cell5;
                                            int num79 = tile4.m_Z;
                                            if ((tile4.m_ID != 2) && ((num12 >= 0x7fffffff) || (tile4.SortZ < num12)))
                                            {
                                                int num80 = num50 + num10;
                                                int num81 = num51 + num11;
                                                if ((mx >= num80) && (mx < (num80 + 0x2c)))
                                                {
                                                    m_PointPool[0].X = num80 + 0x16;
                                                    m_PointPool[0].Y = num81 - (num79 << 2);
                                                    m_PointPool[1].X = num80 + 0x2c;
                                                    m_PointPool[1].Y = (num81 + 0x16) - (landTiles[k + 1, m].m_Z << 2);
                                                    m_PointPool[2].X = num80 + 0x16;
                                                    m_PointPool[2].Y = (num81 + 0x2c) - (landTiles[k + 1, m + 1].m_Z << 2);
                                                    m_PointPool[3].X = num80;
                                                    m_PointPool[3].Y = (num81 + 0x16) - (landTiles[k, m + 1].m_Z << 2);
                                                    if (LandTileHitTest(m_PointPool, m_MousePoint))
                                                    {
                                                        TileX = (short)(num4 + k);
                                                        TileY = (short)(num5 + m);
                                                        m_LastFind = cell5;
                                                        xLast = k;
                                                        yLast = m;
                                                        return cell5;
                                                    }
                                                }
                                            }
                                        }
                                    Label_1535:;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        Label_156B:
            m_LastFind = null;
            xLast = -100;
            yLast = -100;
            xwLast = x;
            ywLast = y;
            zwLast = z;
            try
            {
                for (int num82 = cellWidth - 2; num82 >= 0; num82--)
                {
                    for (int num83 = cellHeight - 2; num83 >= 0; num83--)
                    {
                        int num84 = (num82 - num83) * 0x16;
                        int num85 = (num82 + num83) * 0x16;
                        for (int num87 = cells[num82, num83].Count - 1; num87 >= 0; num87--)
                        {
                            ICell cell7 = (ICell)cells[num82, num83][num87];
                            System.Type type5 = cell7.CellType;
                            if (type5 == tMobileCell)
                            {
                                if ((num13 >= 0x7fffffff) || (cell7.Z < num13))
                                {
                                    IHue hue5;
                                    IAnimatedCell cell8 = (IAnimatedCell)cell7;
                                    int num88 = 0;
                                    int num89 = 0;
                                    int num90 = 0;
                                    int num91 = 0;
                                    int num92 = 0;
                                    cell8.GetPackage(ref num88, ref num91, ref num89, ref num92, ref num90);
                                    int num93 = num84 + 0x16;
                                    int num94 = (num85 - (cell8.Z << 2)) + 0x16;
                                    num93++;
                                    num94 -= 2;
                                    Mobile mobile4 = ((MobileCell)cell7).m_Mobile;
                                    if (mobile4.Flags[MobileFlag.Hidden])
                                    {
                                        hue5 = Hues.Grayscale;
                                    }
                                    else if (Engine.m_Highlight == mobile4)
                                    {
                                        hue5 = Hues.GetNotoriety(mobile4.Notoriety);
                                    }
                                    else
                                    {
                                        hue5 = Hues.Load(num90);
                                    }
                                    int num95 = 0;
                                    int num96 = 0;
                                    Frame frame5 = Engine.m_Animations.GetFrame(mobile4, num88, num91, num89, num92, num93, num94, hue5, ref num95, ref num96, false);
                                    if ((frame5.Image != null) && !frame5.Image.IsEmpty())
                                    {
                                        num95 += num10;
                                        num96 += num11;
                                        int num97 = num95;
                                        int num98 = num96;
                                        if (((mx >= num97) && (my >= num98)) && ((mx < (num97 + frame5.Image.Width)) && (my < (num98 + frame5.Image.Height))))
                                        {
                                            if (!frame5.Image.Flip)
                                            {
                                                if (frame5.Image.HitTest(mx - num97, my - num98))
                                                {
                                                    TileX = (short)(num4 + num82);
                                                    TileY = (short)(num5 + num83);
                                                    m_LastFind = cell7;
                                                    xLast = num82;
                                                    yLast = num83;
                                                    return cell7;
                                                }
                                            }
                                            else if (frame5.Image.HitTest(-(mx - num97), my - num98))
                                            {
                                                TileX = (short)(num4 + num82);
                                                TileY = (short)(num5 + num83);
                                                m_LastFind = cell7;
                                                xLast = num82;
                                                yLast = num83;
                                                return cell7;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if ((type5 == tStaticItem) || (type5 == tDynamicItem))
                                {
                                    IItem item9 = (IItem)cell7;
                                    if (((((((item9.ID != 0x4001) && (item9.ID != 0x5796)) && ((item9.ID != 0x61a4) && (item9.ID != 0x6198))) && (item9.ID != 0x61bc)) && (item9.ID != 0x6199)) && ((num13 >= 0x7fffffff) || ((cell7.Z < num13) && !Map.m_ItemFlags[item9.ID & 0x3fff][TileFlag.Roof]))) && (((!Map.m_ItemFlags[item9.ID & 0x3fff][TileFlag.Foliage] || (num82 < num8)) || ((num83 < num9) || (num82 >= (num8 + 8)))) || (num83 >= (num8 + 8))))
                                    {
                                        Client.Texture texture3;
                                        if (item9.CellType == tDynamicItem)
                                        {
                                            DynamicItem item10 = (DynamicItem)item9;
                                            Item item11 = item10.m_Item;
                                            if ((item11 != null) && (item11.ID == 0x2006))
                                            {
                                                int oldID = item11.Amount;
                                                CorpseTableEntry entry3 = (CorpseTableEntry)CorpseTable.m_Entries[oldID];
                                                if ((entry3 != null) && (BodyConverter.GetFileSet(oldID) == 1))
                                                {
                                                    oldID = entry3.m_OldID;
                                                }
                                                int num101 = Engine.GetAnimDirection(item11.Direction);
                                                int num102 = Engine.m_Animations.ConvertAction(oldID, item11.Serial, item11.X, item11.Y, num101, GenericAction.Die, null);
                                                int num103 = Engine.m_Animations.GetFrameCount(oldID, num102, num101);
                                                int num104 = num84 + 0x17;
                                                int num105 = (num85 - (item11.Z << 2)) + 20;
                                                IHue hue6 = Hues.Default;
                                                int num106 = 0;
                                                int num107 = 0;
                                                Frame frame6 = Engine.m_Animations.GetFrame(item11, oldID, num102, num101, num103 - 1, num104, num105, hue6, ref num106, ref num107, true);
                                                num106 += num10;
                                                num107 += num11;
                                                int num108 = num106;
                                                int num109 = num107;
                                                if (((mx < num108) || (my < num109)) || ((mx >= (num108 + frame6.Image.Width)) || (my >= (num109 + frame6.Image.Height))))
                                                {
                                                    goto Label_1EA5;
                                                }
                                                if (frame6.Image.Flip)
                                                {
                                                    if (frame6.Image.HitTest(-(mx - num108), my - num109))
                                                    {
                                                        TileX = item11.X;
                                                        TileY = item11.Y;
                                                        m_LastFind = cell7;
                                                        xLast = num82;
                                                        yLast = num83;
                                                        return cell7;
                                                    }
                                                    goto Label_1EA5;
                                                }
                                                if (!frame6.Image.HitTest(mx - num108, my - num109))
                                                {
                                                    goto Label_1EA5;
                                                }
                                                TileX = item11.X;
                                                TileY = item11.Y;
                                                m_LastFind = cell7;
                                                xLast = num82;
                                                yLast = num83;
                                                return cell7;
                                            }
                                        }
                                        int num110 = item9.ID & 0x3fff;
                                        bool flag6 = false;
                                        if (type5 == tStaticItem)
                                        {
                                            num110 = Map.GetDispID(num110, 0, ref flag6);
                                        }
                                        else
                                        {
                                            Item item12 = ((DynamicItem)cell7).m_Item;
                                            if (item12 == null)
                                            {
                                                num110 = Map.GetDispID(num110, 0, ref flag6);
                                            }
                                            else
                                            {
                                                num110 = Map.GetDispID(num110, item12.Amount, ref flag6);
                                            }
                                        }
                                        AnimData data3 = Map.GetAnim(num110);
                                        if ((data3.frameCount == 0) || !Map.m_ItemFlags[num110][TileFlag.Animation])
                                        {
                                            texture3 = Hues.Default.GetItem(num110);
                                        }
                                        else
                                        {
                                            texture3 = Hues.Default.GetItem(num110 + data3[(m_Frames / (data3.frameInterval + 1)) % data3.frameCount]);
                                        }
                                        if ((texture3 != null) && !texture3.IsEmpty())
                                        {
                                            int num111 = num84 + 0x16;
                                            int num112 = (num85 - (cell7.Z << 2)) + 0x2b;
                                            num111 -= texture3.Width >> 1;
                                            num112 -= texture3.Height;
                                            num111 += num10;
                                            num112 += num11;
                                            if (((!flag6 || (mx < num111)) || ((my < num112) || (mx >= ((num111 + texture3.Width) + 5)))) || (my >= ((num112 + texture3.Height) + 5)))
                                            {
                                                if (((!flag6 && (mx >= num111)) && ((my >= num112) && (mx < (num111 + texture3.Width)))) && ((my < (num112 + texture3.Height)) && texture3.HitTest(mx - num111, my - num112)))
                                                {
                                                    TileX = (short)(num4 + num82);
                                                    TileY = (short)(num5 + num83);
                                                    m_LastFind = cell7;
                                                    xLast = num82;
                                                    yLast = num83;
                                                    return cell7;
                                                }
                                            }
                                            else
                                            {
                                                mx -= num111;
                                                my -= num112;
                                                if ((((mx < texture3.Width) && (my < texture3.Height)) && texture3.HitTest(mx, my)) || (((mx >= 5) && (my >= 5)) && texture3.HitTest(mx - 5, my - 5)))
                                                {
                                                    TileX = (short)(num4 + num82);
                                                    TileY = (short)(num5 + num83);
                                                    m_LastFind = cell7;
                                                    xLast = num82;
                                                    yLast = num83;
                                                    return cell7;
                                                }
                                                mx += num111;
                                                my += num112;
                                            }
                                        }
                                    }
                                }
                                else if (type5 == tLandTile)
                                {
                                    LandTile tile5 = (LandTile)cell7;
                                    int num113 = tile5.m_Z;
                                    if ((tile5.m_ID != 2) && ((num12 >= 0x7fffffff) || (tile5.SortZ <= num12)))
                                    {
                                        int num114 = num84 + num10;
                                        int num115 = num85 + num11;
                                        if ((mx >= num114) && (mx < (num114 + 0x2c)))
                                        {
                                            m_PointPool[0].X = num114 + 0x16;
                                            m_PointPool[0].Y = num115 - (num113 << 2);
                                            m_PointPool[1].X = num114 + 0x2c;
                                            m_PointPool[1].Y = (num115 + 0x16) - (landTiles[num82 + 1, num83].m_Z << 2);
                                            m_PointPool[2].X = num114 + 0x16;
                                            m_PointPool[2].Y = (num115 + 0x2c) - (landTiles[num82 + 1, num83 + 1].m_Z << 2);
                                            m_PointPool[3].X = num114;
                                            m_PointPool[3].Y = (num115 + 0x16) - (landTiles[num82, num83 + 1].m_Z << 2);
                                            if (LandTileHitTest(m_PointPool, m_MousePoint))
                                            {
                                                TileX = (short)(num4 + num82);
                                                TileY = (short)(num5 + num83);
                                                m_LastFind = cell7;
                                                xLast = num82;
                                                yLast = num83;
                                                return cell7;
                                            }
                                        }
                                    }
                                }
                            Label_1EA5:;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        Label_1ED4:
            TileX = -1;
            TileY = -1;
            m_LastFind = null;
            xLast = -1;
            yLast = -1;
            return null;
        }

        private static void Fix(ref int v, int cap)
        {
            if (v < 0)
            {
                v = 0;
            }
            else if (v > cap)
            {
                v = cap;
            }
        }

        private static CustomVertex.TransformedColoredTextured[] GeoPool(int count)
        {
            if ((m_GeoPool == null) || (m_GeoPool.Length < count))
            {
                m_GeoPool = new CustomVertex.TransformedColoredTextured[count];
                for (int i = 0; i < count; i++)
                {
                    m_GeoPool[i].Rhw = 1f;
                    m_GeoPool[i].Color = GetQuadColor(0);
                }
            }
            else
            {
                for (int j = 0; j < count; j++)
                {
                    m_GeoPool[j].Color = GetQuadColor(0);
                }
            }
            return m_GeoPool;
        }

        public static void GetColors(ref int c0, ref int c1)
        {
            if (!m_AlphaEnable)
            {
                c0 |= -16777216;
                c1 |= -16777216;
            }
            else
            {
                int alpha = m_Alpha;
                c0 &= 0xffffff;
                c0 |= alpha;
                c1 &= 0xffffff;
                c1 |= alpha;
            }
        }

        public static void GetColors(ref int c0, ref int c1, ref int c2, ref int c3)
        {
            if (!m_AlphaEnable)
            {
                c0 |= -16777216;
                c1 |= -16777216;
                c2 |= -16777216;
                c3 |= -16777216;
            }
            else
            {
                int alpha = m_Alpha;
                c0 &= 0xffffff;
                c0 |= alpha;
                c1 &= 0xffffff;
                c1 |= alpha;
                c2 &= 0xffffff;
                c2 |= alpha;
                c3 &= 0xffffff;
                c3 |= alpha;
            }
        }

        [DllImport("user32")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        public static int GetQuadColor(int Color)
        {
            if (!m_AlphaEnable)
            {
                Color |= -16777216;
                return Color;
            }
            Color &= 0xffffff;
            Color |= m_Alpha;
            return Color;
        }

        public static unsafe void GradientRect(int Color, int Color2, int X, int Y, int Width, int Height)
        {
            if ((Width > 0) && (Height > 0))
            {
                GetColors(ref Color, ref Color2);
                float num = -0.5f + X;
                float num2 = -0.5f + Y;
                CustomVertex.TransformedColoredTextured[] texturedArray = GeoPool(4);
                texturedArray[0].Color = texturedArray[2].Color = Color2;
                texturedArray[1].Color = texturedArray[3].Color = Color;
                texturedArray[0].X = texturedArray[1].X = num + Width;
                texturedArray[0].Y = texturedArray[2].Y = num2 + Height;
                texturedArray[1].Y = texturedArray[3].Y = num2;
                texturedArray[2].X = texturedArray[3].X = num;
                fixed (CustomVertex.TransformedColoredTextured* texturedRef = texturedArray)
                {
                    PushQuad(texturedRef);
                }
            }
        }

        public static unsafe void GradientRect4(int c00, int c10, int c11, int c01, int X, int Y, int Width, int Height)
        {
            GetColors(ref c00, ref c10, ref c11, ref c01);
            float num = -0.5f + X;
            float num2 = -0.5f + Y;
            CustomVertex.TransformedColoredTextured[] texturedArray = GeoPool(4);
            texturedArray[0].Color = c11;
            texturedArray[1].Color = c10;
            texturedArray[2].Color = c01;
            texturedArray[3].Color = c00;
            texturedArray[0].X = texturedArray[1].X = num + Width;
            texturedArray[0].Y = texturedArray[2].Y = num2 + Height;
            texturedArray[1].Y = texturedArray[3].Y = num2;
            texturedArray[2].X = texturedArray[3].X = num;
            fixed (CustomVertex.TransformedColoredTextured* texturedRef = texturedArray)
            {
                PushQuad(texturedRef);
            }
        }

        public static unsafe void GradientRectLR(int Color, int Color2, int X, int Y, int Width, int Height)
        {
            GetColors(ref Color, ref Color2);
            float num = -0.5f + X;
            float num2 = -0.5f + Y;
            CustomVertex.TransformedColoredTextured[] texturedArray = GeoPool(4);
            texturedArray[0].Color = texturedArray[1].Color = Color2;
            texturedArray[2].Color = texturedArray[3].Color = Color;
            texturedArray[0].X = texturedArray[1].X = num + Width;
            texturedArray[0].Y = texturedArray[2].Y = num2 + Height;
            texturedArray[1].Y = texturedArray[3].Y = num2;
            texturedArray[2].X = texturedArray[3].X = num;
            fixed (CustomVertex.TransformedColoredTextured* texturedRef = texturedArray)
            {
                PushQuad(texturedRef);
            }
        }

        private static unsafe void Grid(LandTile lt, LandTile[,] landTiles, int x, int y, int bx, int by)
        {
            if (((bx + 0x2c) > Engine.GameX) && (bx < (Engine.GameX + Engine.GameWidth)))
            {
                CustomVertex.TransformedColoredTextured[] v = GeoPool(5);
                v[0].Color = v[1].Color = v[2].Color = v[3].Color = 0x4080ff;
                v[0].X = bx + 0x16;
                v[0].Y = by - (lt.m_Z << 2);
                v[1].Y = (by + 0x16) - (landTiles[x + 1, y].m_Z << 2);
                v[1].X = bx + 0x2c;
                v[2].Y = (by + 0x2c) - (landTiles[x + 1, y + 1].m_Z << 2);
                v[2].X = bx + 0x16;
                v[3].Y = (by + 0x16) - (landTiles[x, y + 1].m_Z << 2);
                v[3].X = bx;
                v[4] = v[0];
                SetTexture(null);
                if (m_AlphaEnable)
                {
                    SetAlphaEnablePrecalc(false);
                }
                if (m_FilterEnable)
                {
                    SetFilterEnablePrecalc(false);
                }
                AlphaTestEnable = false;
                DrawLines(v);
                int num = x & 7;
                if ((y & 7) == 0)
                {
                    fixed (CustomVertex.TransformedColoredTextured* texturedRef = v)
                    {
                        texturedRef->Color = texturedRef[1].Color = GetQuadColor(0xff2080);
                        PushLineStrip(texturedRef, 2);
                    }
                }
                if (num == 0)
                {
                    fixed (CustomVertex.TransformedColoredTextured* texturedRef2 = v)
                    {
                        texturedRef2[3].Color = texturedRef2[4].Color = GetQuadColor(0xff2080);
                        PushLineStrip(texturedRef2 + 3, 2);
                    }
                }
            }
        }

        public static void Init(Caps Caps)
        {
            if (m_VertexStream != null)
            {
                m_VertexStream.Unlock();
            }
            m_VertexStream = null;
            m_CanAAEdges = false;
            m_CanAADependent = false;
            m_CanAAIndependent = false;
            m_CanCullNone = Caps.PrimitiveMiscCaps.SupportsCullNone;
            m_CanCullCW = Caps.PrimitiveMiscCaps.SupportsCullClockwise;
            m_CanAntiAlias = m_CanAADependent || m_CanAAIndependent;
            m_AAEnable = false;
            m_EdgeAAEnable = false;
            m_AlphaTestEnable = false;
            m_CullEnable = true;
            m_AlphaEnable = false;
            Engine.m_Device.VertexFormat = VertexFormats.Texture1 | VertexFormats.Diffuse | VertexFormats.Transformed;
            SamplerStateManager sampler = Engine.m_Device.SamplerState[0];
            sampler.AddressU = (TextureAddress.Clamp);
            sampler.AddressV = (TextureAddress.Clamp);
            sampler.MinFilter = (TextureFilter.Point);
            sampler.MagFilter = (TextureFilter.Point);
            Engine.m_Device.RenderState.ZBufferEnable = (true);
            Engine.m_Device.RenderState.ZBufferWriteEnable = (true);
        }

        public static void InsertAlphaState(bool lineStrip, bool alphaTest, DrawBlendType blend, Client.Texture texture, byte[] buffer, int length, int count)
        {
            AlphaState state = null;
            if ((m_AlphaStateCount > 0) && ((m_AlphaStateCount - 1) < m_AlphaStates.Count))
            {
                state = (AlphaState)m_AlphaStates[m_AlphaStateCount - 1];
            }
            bool flag = state == null;
            if (!flag)
            {
                flag = (((state.m_LineStrip != lineStrip) || (state.m_AlphaTest != alphaTest)) || (state.m_BlendType != blend)) || (state.m_Texture != texture);
            }
            if (flag)
            {
                if (m_AlphaStateCount < m_AlphaStates.Count)
                {
                    state = (AlphaState)m_AlphaStates[m_AlphaStateCount];
                }
                else
                {
                    state = new AlphaState();
                    state.m_TextureVB = new TextureVB();
                    m_AlphaStates.Add(state);
                }
                state.m_LineStrip = lineStrip;
                state.m_AlphaTest = alphaTest;
                state.m_BlendType = blend;
                state.m_Texture = texture;
                state.m_TextureVB.m_Count = 0;
                state.m_TextureVB.m_Frame = -1;
                state.m_TextureVB.m_Stream.Seek(0L, SeekOrigin.Begin);
                m_AlphaStateCount++;
            }
            state.m_TextureVB.m_Count += count;
            state.m_TextureVB.m_Frame = m_ActFrames;
            state.m_TextureVB.m_Stream.Write(buffer, 0, length);
        }

        public static void Invalidate()
        {
            m_Invalidate = true;
        }

        public static bool LandTileHitTest(System.Drawing.Point[] points, System.Drawing.Point check)
        {
            int y = points[0].Y;
            int num2 = points[2].Y;
            if ((check.Y >= points[0].Y) && (check.Y <= points[2].Y))
            {
                int num4;
                int num5;
                int num3 = check.X - points[3].X;
                if ((num3 >= 0) && (num3 < 0x16))
                {
                    double num6 = 0.047619047619047616 * num3;
                    num4 = points[3].Y + ((int)((points[0].Y - points[3].Y) * num6));
                    num5 = points[3].Y + ((int)((points[2].Y - points[3].Y) * num6));
                }
                else if ((num3 >= 0x16) && (num3 < 0x2c))
                {
                    double num7 = 0.047619047619047616 * (num3 - 0x16);
                    num4 = points[0].Y + ((int)((points[1].Y - points[0].Y) * num7));
                    num5 = points[2].Y + ((int)((points[1].Y - points[2].Y) * num7));
                }
                else
                {
                    return false;
                }
                return ((check.Y >= num4) && (check.Y <= num5));
            }
            return false;
        }

        public static void PushAll()
        {
            PushAll(false, false, true, 0);
            PushAll(false, false, false, 0);
            PushAll(false, true, false, 0);
            PushAlphaStates();
        }

        private static void PushAll(bool alphaEnable, bool lineStrip, bool alphaTest, int alpha)
        {
            int index = 0;
            if (alphaTest)
            {
                index |= 1;
            }
            if (lineStrip)
            {
                index |= 2;
            }
            if (alphaEnable)
            {
                index |= 4;
            }
            ArrayList list = m_Lists[index];
            if (((m_Counts[index] != 0) && (list != null)) && (list.Count != 0))
            {
                Device device = Engine.m_Device;
                if (alphaTest != m_CurAlphaTest)
                {
                    m_CurAlphaTest = alphaTest;
                    device.RenderState.AlphaTestEnable = (alphaTest);
                }
                int num3 = lineStrip ? 2 : 4;
                int num4 = num3 * 0x1c;
                if (m_VertexStream == null)
                {
                    m_VertexStream = new BufferedVertexStream(Engine.m_VertexBuffer, 0x8000, 0x1c);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    Client.Texture texture = (Client.Texture)list[i];
                    TextureVB evb = texture.GetVB(alphaEnable, lineStrip, alphaTest, alpha);
                    if ((evb.m_Count > 0) && (evb.m_Frame == m_ActFrames))
                    {
                        int startVertex = m_VertexStream.Push(evb.m_Stream.GetBuffer(), evb.m_Count * num3, false);
                        if (startVertex >= 0)
                        {
                            device.SetTexture(0, texture.Surface);
                            if (lineStrip)
                            {
                                device.DrawPrimitives(PrimitiveType.LineList, startVertex, evb.m_Count);
                            }
                            else
                            {
                                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, startVertex, 0, evb.m_Count * 4, 0, evb.m_Count * 2);
                            }
                            evb.m_Frame = -1;
                        }
                    }
                }
                list.Clear();
                m_Counts[index] = 0;
            }
        }

        private static void PushAlphaStates()
        {
            if (m_AlphaStateCount != 0)
            {
                if (m_VertexStream == null)
                {
                    m_VertexStream = new BufferedVertexStream(Engine.m_VertexBuffer, 0x8000, 0x1c);
                }
                Device device = Engine.m_Device;
                device.RenderState.ZBufferWriteEnable = (false);
                device.RenderState.AlphaBlendEnable = (true);
                for (int i = 0; i < m_AlphaStateCount; i++)
                {
                    AlphaState state = (AlphaState)m_AlphaStates[i];
                    Client.Texture texture = state.m_Texture;
                    TextureVB textureVB = state.m_TextureVB;
                    if ((textureVB.m_Count > 0) && (textureVB.m_Frame == m_ActFrames))
                    {
                        if (state.m_AlphaTest != m_CurAlphaTest)
                        {
                            m_CurAlphaTest = state.m_AlphaTest;
                            device.RenderState.AlphaTestEnable = (m_CurAlphaTest);
                        }
                        if (state.m_BlendType != m_CurBlendType)
                        {
                            m_CurBlendType = state.m_BlendType;
                            RenderStateManager states = device.RenderState;
                            switch (m_CurBlendType)
                            {
                                case DrawBlendType.Normal:
                                    states.SourceBlend = (Blend.SourceAlpha);
                                    states.DestinationBlend = (Blend.InvSourceAlpha);
                                    break;

                                case DrawBlendType.Additive:
                                    states.SourceBlend = (Blend.One);
                                    states.DestinationBlend = (Blend.One);
                                    break;

                                case DrawBlendType.BlackTransparency:
                                    states.SourceBlend = (Blend.Zero);
                                    states.DestinationBlend = (Blend.SourceColor);
                                    states.BlendOperation = (BlendOperation.Add);
                                    break;
                            }
                        }
                        int num2 = state.m_LineStrip ? 2 : 4;
                        int num3 = num2 * 0x1c;
                        int vertexCount = textureVB.m_Count * num2;
                        int startVertex = m_VertexStream.Push(textureVB.m_Stream.GetBuffer(), vertexCount, false);
                        if (startVertex >= 0)
                        {
                            device.SetTexture(0, texture.Surface);
                            if (state.m_LineStrip)
                            {
                                device.DrawPrimitives(PrimitiveType.LineList, startVertex, textureVB.m_Count);
                            }
                            else
                            {
                                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, startVertex, 0, vertexCount, 0, 2 * textureVB.m_Count);
                            }
                        }
                    }
                }
                device.RenderState.AlphaBlendEnable = (false);
                device.RenderState.ZBufferWriteEnable = (true);
                m_AlphaStateCount = 0;
            }
        }

        private static unsafe void PushLineStrip(CustomVertex.TransformedColoredTextured* pVertex, int count)
        {
            Client.Texture empty = m_Texture;
            if (empty == null)
            {
                empty = Client.Texture.Empty;
            }
            if (m_LineByteBuffer == null)
            {
                m_LineByteBuffer = new byte[8 * sizeof(CustomVertex.TransformedColoredTextured)];
            }
            int index = 2;
            if (m_AlphaTestEnable)
            {
                index |= 1;
            }
            if (m_AlphaEnable)
            {
                index |= 4;
            }
            fixed (byte* numRef = m_LineByteBuffer)
            {
                float num2 = ((float)(0x2000 - m_Count)) / 8192f;
                m_Count++;
                CustomVertex.TransformedColoredTextured* texturedPtr = (CustomVertex.TransformedColoredTextured*)numRef;
                CustomVertex.TransformedColoredTextured* texturedPtr2 = texturedPtr + ((count - 1) * 2);
                while (texturedPtr < texturedPtr2)
                {
                    pVertex++;
                    texturedPtr[0] = pVertex[0];
                    texturedPtr->Z = num2;
                    texturedPtr[1] = pVertex[0];
                    texturedPtr[1].Z = num2;
                    texturedPtr += 2;
                }
            }
            if (m_AlphaEnable)
            {
                InsertAlphaState(true, m_AlphaTestEnable, m_BlendType, empty, m_LineByteBuffer, ((count - 1) * 2) * sizeof(CustomVertex.TransformedColoredTextured), count - 1);
            }
            else
            {
                TextureVB evb = empty.GetVB(m_AlphaEnable, true, m_AlphaTestEnable, (m_Alpha >> 0x18) & 0xff);
                if (evb.m_Frame != m_ActFrames)
                {
                    if (evb.m_Count > 0)
                    {
                        evb.m_Stream.Seek(0L, SeekOrigin.Begin);
                        evb.m_Count = 0;
                    }
                    evb.m_Frame = m_ActFrames;
                    ArrayList list = m_Lists[index];
                    if (list == null)
                    {
                        m_Lists[index] = list = new ArrayList();
                    }
                    list.Add(empty);
                }
                evb.m_Stream.Write(m_LineByteBuffer, 0, ((count - 1) * 2) * sizeof(CustomVertex.TransformedColoredTextured));
                evb.m_Count += count - 1;
                m_Counts[index] += count - 1;
            }
        }

        private static unsafe void PushPointList(CustomVertex.TransformedColoredTextured* pVertex, int count)
        {
        }

        private static unsafe void PushQuad(CustomVertex.TransformedColoredTextured* pVertex)
        {
            Client.Texture empty = m_Texture;
            if (empty == null)
            {
                empty = Client.Texture.Empty;
            }
            if (m_QuadByteBuffer == null)
            {
                m_QuadByteBuffer = new byte[4 * sizeof(CustomVertex.TransformedColoredTextured)];
            }
            int index = 0;
            if (m_AlphaTestEnable)
            {
                index |= 1;
            }
            if (m_AlphaEnable)
            {
                index |= 4;
            }
            fixed (byte* numRef = m_QuadByteBuffer)
            {
                CustomVertex.TransformedColoredTextured* texturedPtr = (CustomVertex.TransformedColoredTextured*)numRef;
                texturedPtr[0] = pVertex[0];
                texturedPtr[1] = pVertex[1];
                texturedPtr[2] = pVertex[2];
                texturedPtr[3] = pVertex[3];
                float num2 = ((float)(0x2000 - m_Count)) / 8192f;
                m_Count++;
                texturedPtr->Z = num2;
                texturedPtr[1].Z = num2;
                texturedPtr[2].Z = num2;
                texturedPtr[3].Z = num2;
            }
            if (m_AlphaEnable)
            {
                InsertAlphaState(false, m_AlphaTestEnable, m_BlendType, empty, m_QuadByteBuffer, m_QuadByteBuffer.Length, 1);
            }
            else
            {
                TextureVB evb = empty.GetVB(m_AlphaEnable, false, m_AlphaTestEnable, (m_Alpha >> 0x18) & 0xff);
                if (evb.m_Frame != m_ActFrames)
                {
                    if (evb.m_Count > 0)
                    {
                        evb.m_Stream.Seek(0L, SeekOrigin.Begin);
                        evb.m_Count = 0;
                    }
                    evb.m_Frame = m_ActFrames;
                    ArrayList list = m_Lists[index];
                    if (list == null)
                    {
                        m_Lists[index] = list = new ArrayList();
                    }
                    list.Add(empty);
                }
                evb.m_Stream.Write(m_QuadByteBuffer, 0, m_QuadByteBuffer.Length);
                evb.m_Count++;
                m_Counts[index]++;
            }
        }

        [DllImport("user32")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        public static void ResetHitTest()
        {
            m_LastFind = null;
            xwLast = 0;
            ywLast = 0;
            zwLast = 0;
            xLast = 0;
            yLast = 0;
        }

        private static void Save(string title)
        {
            Form display = Engine.m_Display;
            Size size = new Size(Engine.ScreenWidth, Engine.ScreenHeight);
            IntPtr handle = display.Handle;
            IntPtr dC = GetDC(handle);
            Bitmap image = new Bitmap(size.Width, size.Height, PixelFormat.Format24bppRgb);
            Graphics graphics = Graphics.FromImage(image);
            IntPtr hdc = graphics.GetHdc();
            BitBlt(hdc, 0, 0, size.Width, size.Height, dC, 0, 0, 0xcc0020);
            graphics.ReleaseHdc(hdc);
            ReleaseDC(handle, dC);
            for (int i = 1; i <= 0x3e8; i++)
            {
                string path = Engine.FileManager.BasePath(string.Format("Screenshots/{0}_{1}.png", title, i));
                if (!File.Exists(path))
                {
                    MemoryStream stream = new MemoryStream();
                    image.Save(stream, ImageFormat.Png);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(Renderer.SaveStream), new object[] { stream, path });
                    break;
                }
            }
            image.Dispose();
            graphics.Dispose();
        }

        private static void SaveStream(object state)
        {
            object[] objArray = (object[])state;
            MemoryStream stream = (MemoryStream)objArray[0];
            FileStream stream2 = new FileStream((string)objArray[1], FileMode.Create, FileAccess.Write, FileShare.None);
            stream.WriteTo(stream2);
            stream2.Close();
            stream.Close();
        }

        public static void ScreenShot(string title)
        {
            if (!Engine.GMPrivs)
            {
                m_ScreenShot = title;
                Draw();
                m_ScreenShot = null;
            }
        }

        public static void SetAlpha(float alpha)
        {
            if (m_fAlpha != alpha)
            {
                m_fAlpha = alpha;
                m_Alpha = (int)(alpha * 255f);
                if (m_Alpha < 0)
                {
                    m_Alpha = 0;
                }
                else if (m_Alpha > 0xff)
                {
                    m_Alpha = -16777216;
                }
                else
                {
                    m_Alpha = m_Alpha << 0x18;
                }
            }
        }

        public static void SetAlphaEnable(bool enable)
        {
            if (m_AlphaEnable != enable)
            {
                m_AlphaEnable = enable;
            }
        }

        public static void SetAlphaEnablePrecalc(bool enable)
        {
            m_AlphaEnable = enable;
        }

        public static void SetBlendType(DrawBlendType type)
        {
            m_BlendType = type;
        }

        public static void SetFilterEnable(bool ShouldEnable)
        {
        }

        public static void SetFilterEnablePrecalc(bool shouldEnable)
        {
        }

        public static void SetText(string text)
        {
            int emoteHue;
            text = Engine.Encode(text);
            if (text.StartsWith("--") && UOAM.Connected)
            {
                text = "Chat: " + text.Substring(2) + "_";
                emoteHue = 0x59;
            }
            else if (text.StartsWith(": "))
            {
                text = "Emote: " + text.Substring(2) + "_";
                emoteHue = World.CharData.EmoteHue;
            }
            else if (text.StartsWith("; "))
            {
                text = "Whisper: " + text.Substring(2) + "_";
                emoteHue = World.CharData.WhisperHue;
            }
            else if (text.StartsWith("! "))
            {
                text = "Yell: " + text.Substring(2) + "_";
                emoteHue = World.CharData.YellHue;
            }
            else if (text.StartsWith(". "))
            {
                text = "Command: " + text.Substring(". ".Length) + "_";
                emoteHue = World.CharData.TextHue;
            }
            else if (text.StartsWith(@"\ "))
            {
                text = "<OOC> " + text.Substring(2) + "_";
                emoteHue = World.CharData.TextHue;
            }
            else if (text.StartsWith("/"))
            {
                string s = text;
                text = null;
                emoteHue = World.CharData.TextHue;
                if (((Party.State == PartyState.Joined) && (s.Length >= 2)) && char.IsDigit(s, 1))
                {
                    try
                    {
                        int index = Convert.ToInt32(s.Substring(1, 1)) - 1;
                        if (((index >= 0) && (index < Party.Members.Length)) && (index != Party.Index))
                        {
                            string str2;
                            Mobile mobile = Party.Members[index];
                            if (((mobile == null) || ((str2 = mobile.Name) == null)) || ((str2 = str2.Trim()).Length <= 0))
                            {
                                str2 = "Someone";
                            }
                            text = str2 + ": " + s.Substring(2);
                            emoteHue = World.CharData.WhisperHue;
                        }
                    }
                    catch
                    {
                    }
                }
                if (text == null)
                {
                    text = "Party: " + s.Substring(1) + "_";
                }
            }
            else
            {
                text = text + "_";
                emoteHue = World.CharData.TextHue;
            }
            if (m_vTextCache == null)
            {
                m_vTextCache = new Client.VertexCache();
            }
            else
            {
                m_vTextCache.Invalidate();
            }
            m_TextSurface = Engine.GetUniFont(3).GetString(text, Hues.Load(emoteHue));
        }

        public static void SetTexture(Client.Texture texture)
        {
            m_Texture = texture;
        }

        public static bool SetViewport(int x, int y, int w, int h)
        {
            PushAll();
            Viewport viewport = new Viewport();
            viewport.MinZ = 0f;
            viewport.MaxZ = 1f;
            int v = x;
            int num2 = y;
            int num3 = x + w;
            int num4 = y + h;
            Fix(ref v, Engine.ScreenWidth);
            Fix(ref num2, Engine.ScreenHeight);
            Fix(ref num3, Engine.ScreenWidth);
            Fix(ref num4, Engine.ScreenHeight);
            viewport.X = v;
            viewport.Y = num2;
            viewport.Width = num3 - v;
            viewport.Height = num4 - num2;
            if ((viewport.Width == 0) || (viewport.Height == 0))
            {
                return false;
            }
            Engine.m_Device.Viewport = viewport;
            return true;
        }

        public static unsafe void SolidQuad(int Color, Client.Point[] pts)
        {
            CustomVertex.TransformedColoredTextured[] texturedArray = GeoPool(4);
            texturedArray[0].Color = texturedArray[1].Color = texturedArray[2].Color = texturedArray[3].Color = GetQuadColor(Color);
            texturedArray[3].X = pts[0].X - 0.5f;
            texturedArray[3].Y = pts[0].Y - 0.5f;
            texturedArray[1].X = pts[1].X - 0.5f;
            texturedArray[1].Y = pts[1].Y - 0.5f;
            texturedArray[0].X = pts[2].X - 0.5f;
            texturedArray[0].Y = pts[2].Y - 0.5f;
            texturedArray[2].X = pts[3].X - 0.5f;
            texturedArray[2].Y = pts[3].Y - 0.5f;
            fixed (CustomVertex.TransformedColoredTextured* texturedRef = texturedArray)
            {
                PushQuad(texturedRef);
            }
        }

        public static unsafe void SolidRect(int Color, int X, int Y, int Width, int Height)
        {
            if ((Width > 0) && (Height > 0))
            {
                float num = -0.5f + X;
                float num2 = -0.5f + Y;
                CustomVertex.TransformedColoredTextured[] texturedArray = GeoPool(4);
                texturedArray[0].Color = texturedArray[1].Color = texturedArray[2].Color = texturedArray[3].Color = GetQuadColor(Color);
                texturedArray[0].X = texturedArray[1].X = num + Width;
                texturedArray[0].Y = texturedArray[2].Y = num2 + Height;
                texturedArray[1].Y = texturedArray[3].Y = num2;
                texturedArray[2].X = texturedArray[3].X = num;
                fixed (CustomVertex.TransformedColoredTextured* texturedRef = texturedArray)
                {
                    PushQuad(texturedRef);
                }
            }
        }

        public static unsafe void SolidRect(int Color, int X, int Y, int Width, int Height, Clipper c)
        {
            if ((Width > 0) && (Height > 0))
            {
                float num = -0.5f + X;
                float num2 = -0.5f + Y;
                CustomVertex.TransformedColoredTextured[] vertices = GeoPool(4);
                vertices[0].Color = vertices[1].Color = vertices[2].Color = vertices[3].Color = GetQuadColor(Color);
                vertices[0].X = vertices[1].X = num + Width;
                vertices[0].Y = vertices[2].Y = num2 + Height;
                vertices[1].Y = vertices[3].Y = num2;
                vertices[2].X = vertices[3].X = num;
                if (c.Clip(X, Y, Width, Height, vertices))
                {
                    fixed (CustomVertex.TransformedColoredTextured* texturedRef = vertices)
                    {
                        PushQuad(texturedRef);
                    }
                }
            }
        }

        public static unsafe void TransparentRect(int Color, int X, int Y, int Width, int Height)
        {
            Width--;
            Height--;
            float num = X;
            float num2 = Y;
            CustomVertex.TransformedColoredTextured[] texturedArray = GeoPool(5);
            texturedArray[0].Color = texturedArray[1].Color = texturedArray[2].Color = texturedArray[3].Color = GetQuadColor(Color);
            texturedArray[0].X = num;
            texturedArray[0].Y = num2;
            texturedArray[1].X = num + Width;
            texturedArray[1].Y = num2;
            texturedArray[2].X = num + Width;
            texturedArray[2].Y = num2 + Height;
            texturedArray[3].X = num;
            texturedArray[3].Y = num2 + Height;
            texturedArray[4] = texturedArray[0];
            fixed (CustomVertex.TransformedColoredTextured* texturedRef = texturedArray)
            {
                PushLineStrip(texturedRef, 5);
            }
        }

        public static void TransparentRect(int Color, int X, int Y, int Width, int Height, Clipper c)
        {
            SolidRect(Color, X, Y, 1, Height, c);
            SolidRect(Color, (X + Width) - 1, Y, 1, Height, c);
            SolidRect(Color, X + 1, Y, Width - 2, 1, c);
            SolidRect(Color, X + 1, (Y + Height) - 1, Width - 2, 1, c);
        }

        private static bool Validate()
        {
            bool flag;
            do
            {
                flag = false;
                try
                {
                    Engine.m_Device.TestCooperativeLevel();
                }
                catch (DeviceLostException)
                {
                    return false;
                }
                catch (DeviceNotResetException)
                {
                    Engine.m_Device.Reset(Engine.m_PresentParams);
                    GC.Collect();
                    return true;
                }
                catch
                {
                    Application.DoEvents();
                    flag = true;
                }
            }
            while (flag);
            return true;
        }

        public static bool AAEnable
        {
            get
            {
                return m_AAEnable;
            }
            set
            {
                if ((m_AAEnable != value) && m_CanAntiAlias)
                {
                    m_AAEnable = value;
                }
            }
        }

        public static bool AlphaTestEnable
        {
            get
            {
                return m_AlphaTestEnable;
            }
            set
            {
                if (m_AlphaTestEnable != value)
                {
                    m_AlphaTestEnable = value;
                }
            }
        }

        public static int AlwaysHighlight
        {
            get
            {
                return m_AlwaysHighlight;
            }
            set
            {
                m_AlwaysHighlight = value;
            }
        }

        public static bool ColorAlphaEnable
        {
            get
            {
                return m_ColorAlphaEnable;
            }
            set
            {
                if (m_ColorAlphaEnable != value)
                {
                    if (!value)
                    {
                    }
                    m_ColorAlphaEnable = value;
                }
            }
        }

        public static bool CullEnable
        {
            get
            {
                return m_CullEnable;
            }
            set
            {
                if (((m_CullEnable != value) && (!value || m_CanCullCW)) && (value || m_CanCullNone))
                {
                    m_CullEnable = value;
                }
            }
        }

        public static bool DrawFPS
        {
            get
            {
                return m_DrawFPS;
            }
            set
            {
                m_DrawFPS = value;
            }
        }

        public static bool DrawGrid
        {
            get
            {
                return m_DrawGrid;
            }
            set
            {
                m_DrawGrid = Engine.GMPrivs && value;
            }
        }

        public static bool DrawPCount
        {
            get
            {
                return m_DrawPCount;
            }
            set
            {
                m_DrawPCount = value;
            }
        }

        public static bool DrawPing
        {
            get
            {
                return m_DrawPing;
            }
            set
            {
                m_DrawPing = value;
            }
        }

        public static bool EdgeAAEnable
        {
            get
            {
                return m_EdgeAAEnable;
            }
            set
            {
                if ((m_EdgeAAEnable != value) && m_CanAAEdges)
                {
                    m_EdgeAAEnable = value;
                }
            }
        }

        public static bool MiniHealth
        {
            get
            {
                return m_MiniHealth;
            }
            set
            {
                m_MiniHealth = value;
            }
        }

        public static Rectangle ServerBoundary
        {
            set
            {
                m_xServerStart = value.X;
                m_yServerStart = value.Y;
                m_xServerEnd = value.Right;
                m_yServerEnd = value.Bottom;
            }
        }

        public static bool Transparency
        {
            get
            {
                return m_Transparency;
            }
            set
            {
                m_Transparency = value;
            }
        }

        private class AlphaState
        {
            public bool m_AlphaTest;
            public DrawBlendType m_BlendType;
            public bool m_LineStrip;
            public Client.Texture m_Texture;
            public TextureVB m_TextureVB;
        }

        private class DrawQueueEntry
        {
            public bool m_bAlpha;
            public int m_DrawX;
            public int m_DrawY;
            public float m_fAlpha;
            public bool m_Flip;
            public Client.Texture m_Texture;
            public int m_TileX;
            public int m_TileY;

            public DrawQueueEntry(Client.Texture tex, int tx, int ty, int dx, int dy)
            {
                this.m_Texture = tex;
                this.m_TileX = tx;
                this.m_TileY = ty;
                this.m_DrawX = dx;
                this.m_DrawY = dy;
                this.m_Flip = this.m_Texture.Flip;
                this.m_fAlpha = Renderer.m_fAlpha;
                this.m_bAlpha = Renderer.m_AlphaEnable;
            }
        }
    }
}