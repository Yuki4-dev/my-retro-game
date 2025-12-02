using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SuperKiroWorld
{
    /// <summary>
    /// 各種エフェクトを管理するクラス
    /// </summary>
    public class EffectManager
    {
        // 各エフェクトタイプ用のエミッター
        private ParticleEmitter _trailEmitter;
        private ParticleEmitter _explosionEmitter;
        private ParticleEmitter _sparkleEmitter;
        private ParticleEmitter _confettiEmitter;

        private Random _random;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EffectManager()
        {
            _random = new Random();
        }

        /// <summary>
        /// エフェクトマネージャーを初期化
        /// </summary>
        public void Initialize()
        {
            // 各エミッターを初期化（最大パーティクル数を設定）
            _trailEmitter = new ParticleEmitter(500);
            _explosionEmitter = new ParticleEmitter(500);
            _sparkleEmitter = new ParticleEmitter(300);
            _confettiEmitter = new ParticleEmitter(1000);
        }

        /// <summary>
        /// トレイルエフェクトを生成
        /// </summary>
        /// <param name="position">生成位置</param>
        /// <param name="velocity">プレイヤーの速度（未使用だが将来の拡張用）</param>
        public void CreateTrailEffect(Vector2 position, Vector2 velocity)
        {
            ParticleConfig config = ParticleConfig.CreateTrailConfig();
            _trailEmitter.Emit(position, config);
        }

        /// <summary>
        /// 爆発エフェクトを生成（360度放射パターン）
        /// </summary>
        /// <param name="position">生成位置</param>
        public void CreateExplosionEffect(Vector2 position)
        {
            // 基本設定を取得
            ParticleConfig config = ParticleConfig.CreateExplosionConfig();
            
            // 360度放射パターンを実現するため、速度を角度ベースで設定
            // 各パーティクルを個別に生成して放射状に配置
            int particleCount = config.Count;
            float angleStep = (float)(2 * Math.PI / particleCount);
            
            for (int i = 0; i < particleCount; i++)
            {
                float angle = angleStep * i;
                float speed = 3f + (float)_random.NextDouble() * 5f; // 3-8ピクセル/フレーム
                
                // 角度から速度ベクトルを計算
                float velocityX = (float)Math.Cos(angle) * speed * 60f; // 60 FPSを考慮
                float velocityY = (float)Math.Sin(angle) * speed * 60f;
                
                // 個別のパーティクル設定を作成
                ParticleConfig singleConfig = new ParticleConfig
                {
                    Count = 1,
                    VelocityMin = new Vector2(velocityX, velocityY),
                    VelocityMax = new Vector2(velocityX, velocityY),
                    Colors = config.Colors,
                    LifeMin = config.LifeMin,
                    LifeMax = config.LifeMax,
                    SizeMin = config.SizeMin,
                    SizeMax = config.SizeMax,
                    RotationSpeedMin = config.RotationSpeedMin,
                    RotationSpeedMax = config.RotationSpeedMax,
                    UseGravity = config.UseGravity
                };
                
                _explosionEmitter.Emit(position, singleConfig);
            }
        }

        /// <summary>
        /// スパークルエフェクトを生成（上方向バイアス）
        /// </summary>
        /// <param name="position">生成位置</param>
        public void CreateSparkleEffect(Vector2 position)
        {
            ParticleConfig config = ParticleConfig.CreateSparkleConfig();
            
            // 上方向バイアスを強化（設定は既にCreateSparkleConfigで定義済み）
            // 速度範囲: X: -3 to 3, Y: -6 to -2 (上方向)
            _sparkleEmitter.Emit(position, config);
        }

        /// <summary>
        /// 紙吹雪エフェクトを生成（画面上部全体から生成）
        /// </summary>
        /// <param name="screenWidth">画面の幅</param>
        public void CreateConfettiEffect(int screenWidth)
        {
            ParticleConfig config = ParticleConfig.CreateConfettiConfig();
            
            // 画面上部全体に分散して生成
            int particlesPerBatch = config.Count / 5; // 5つのバッチに分割
            
            for (int batch = 0; batch < 5; batch++)
            {
                // 画面上部のランダムな位置
                float x = (float)_random.NextDouble() * screenWidth;
                float y = -20f; // 画面上部の少し上
                
                Vector2 spawnPosition = new Vector2(x, y);
                
                // バッチごとに生成
                ParticleConfig batchConfig = new ParticleConfig
                {
                    Count = particlesPerBatch,
                    VelocityMin = config.VelocityMin,
                    VelocityMax = config.VelocityMax,
                    Colors = config.Colors,
                    LifeMin = config.LifeMin,
                    LifeMax = config.LifeMax,
                    SizeMin = config.SizeMin,
                    SizeMax = config.SizeMax,
                    RotationSpeedMin = config.RotationSpeedMin,
                    RotationSpeedMax = config.RotationSpeedMax,
                    UseGravity = config.UseGravity
                };
                
                _confettiEmitter.Emit(spawnPosition, batchConfig);
            }
        }

        /// <summary>
        /// 全エミッターを更新
        /// </summary>
        /// <param name="deltaTime">前フレームからの経過時間（秒）</param>
        public void Update(float deltaTime)
        {
            _trailEmitter.Update(deltaTime);
            _explosionEmitter.Update(deltaTime);
            _sparkleEmitter.Update(deltaTime);
            _confettiEmitter.Update(deltaTime);
        }

        /// <summary>
        /// 全エミッターを描画
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch</param>
        /// <param name="texture">パーティクル用のテクスチャ</param>
        /// <param name="cameraOffset">カメラオフセット</param>
        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 cameraOffset)
        {
            _trailEmitter.Draw(spriteBatch, texture, cameraOffset);
            _explosionEmitter.Draw(spriteBatch, texture, cameraOffset);
            _sparkleEmitter.Draw(spriteBatch, texture, cameraOffset);
            _confettiEmitter.Draw(spriteBatch, texture, cameraOffset);
        }
    }
}
