using Microsoft.Xna.Framework;

namespace SuperKiroWorld
{
    /// <summary>
    /// パーティクル生成の設定を定義する構造体
    /// </summary>
    public struct ParticleConfig
    {
        // 生成数
        public int Count { get; set; }
        
        // 速度範囲
        public Vector2 VelocityMin { get; set; }
        public Vector2 VelocityMax { get; set; }
        
        // 色配列
        public Color[] Colors { get; set; }
        
        // 寿命範囲（秒）
        public float LifeMin { get; set; }
        public float LifeMax { get; set; }
        
        // サイズ範囲（ピクセル）
        public float SizeMin { get; set; }
        public float SizeMax { get; set; }
        
        // 回転速度範囲（ラジアン/秒）
        public float RotationSpeedMin { get; set; }
        public float RotationSpeedMax { get; set; }
        
        // 重力フラグ
        public bool UseGravity { get; set; }
        
        /// <summary>
        /// トレイルエフェクト用の設定を作成
        /// Kiro紫、短寿命、小サイズ
        /// </summary>
        public static ParticleConfig CreateTrailConfig()
        {
            return new ParticleConfig
            {
                Count = 2,
                VelocityMin = new Vector2(-1f, -1f),
                VelocityMax = new Vector2(1f, 1f),
                Colors = new Color[]
                {
                    new Color(121, 14, 203, 255),  // Kiro紫
                    new Color(121, 14, 203, 200),  // 少し透明
                },
                LifeMin = 0.3f,
                LifeMax = 0.5f,
                SizeMin = 3f,
                SizeMax = 8f,
                RotationSpeedMin = 0f,
                RotationSpeedMax = 0f,
                UseGravity = false
            };
        }

        /// <summary>
        /// 爆発エフェクト用の設定を作成
        /// 紫と白、放射状、中寿命
        /// </summary>
        public static ParticleConfig CreateExplosionConfig()
        {
            return new ParticleConfig
            {
                Count = 20,
                VelocityMin = new Vector2(-8f, -8f),
                VelocityMax = new Vector2(8f, 8f),
                Colors = new Color[]
                {
                    new Color(121, 14, 203, 255),  // Kiro紫
                    new Color(150, 50, 220, 255),  // 明るい紫
                    new Color(255, 255, 255, 255), // 白
                    new Color(200, 150, 255, 255), // 薄紫
                },
                LifeMin = 0.4f,
                LifeMax = 0.7f,
                SizeMin = 4f,
                SizeMax = 10f,
                RotationSpeedMin = -2f,
                RotationSpeedMax = 2f,
                UseGravity = false
            };
        }

        /// <summary>
        /// スパークルエフェクト用の設定を作成
        /// ゴールドと白、上方向バイアス、中寿命
        /// </summary>
        public static ParticleConfig CreateSparkleConfig()
        {
            return new ParticleConfig
            {
                Count = 8,
                VelocityMin = new Vector2(-3f, -6f),  // 上方向バイアス
                VelocityMax = new Vector2(3f, -2f),
                Colors = new Color[]
                {
                    new Color(255, 215, 0, 255),   // ゴールド
                    new Color(255, 255, 255, 255), // 白
                    new Color(255, 255, 150, 255), // 明るい黄色
                },
                LifeMin = 0.5f,
                LifeMax = 0.8f,
                SizeMin = 4f,
                SizeMax = 12f,
                RotationSpeedMin = -3f,
                RotationSpeedMax = 3f,
                UseGravity = false
            };
        }

        /// <summary>
        /// 紙吹雪エフェクト用の設定を作成
        /// 複数色、重力あり、回転あり
        /// </summary>
        public static ParticleConfig CreateConfettiConfig()
        {
            return new ParticleConfig
            {
                Count = 75,
                VelocityMin = new Vector2(-4f, -2f),
                VelocityMax = new Vector2(4f, 2f),
                Colors = new Color[]
                {
                    new Color(121, 14, 203, 255),  // Kiro紫
                    new Color(255, 215, 0, 255),   // ゴールド
                    new Color(255, 255, 255, 255), // 白
                    new Color(255, 105, 180, 255), // ピンク
                    new Color(150, 50, 220, 255),  // 明るい紫
                },
                LifeMin = 3f,
                LifeMax = 5f,
                SizeMin = 6f,
                SizeMax = 14f,
                RotationSpeedMin = -5f,
                RotationSpeedMax = 5f,
                UseGravity = true
            };
        }
    }
}
