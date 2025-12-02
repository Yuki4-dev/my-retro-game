using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace SuperKiroWorld
{
    /// <summary>
    /// パーティクルの生成と管理を行うクラス
    /// </summary>
    public class ParticleEmitter
    {
        private List<Particle> _particles;
        private int _maxParticles;
        private Random _random;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="maxParticles">最大パーティクル数</param>
        public ParticleEmitter(int maxParticles = 1000)
        {
            _maxParticles = maxParticles;
            _particles = new List<Particle>(maxParticles);
            _random = new Random();
        }

        /// <summary>
        /// パーティクルを生成
        /// </summary>
        /// <param name="position">生成位置</param>
        /// <param name="config">パーティクル設定</param>
        public void Emit(Vector2 position, ParticleConfig config)
        {
            for (int i = 0; i < config.Count; i++)
            {
                // 最大数に達している場合は古いパーティクルを削除
                if (_particles.Count >= _maxParticles)
                {
                    _particles.RemoveAt(0);
                }

                // ランダムな速度を生成
                float velocityX = Lerp(config.VelocityMin.X, config.VelocityMax.X, (float)_random.NextDouble());
                float velocityY = Lerp(config.VelocityMin.Y, config.VelocityMax.Y, (float)_random.NextDouble());

                // ランダムな色を選択
                Color color = config.Colors[_random.Next(config.Colors.Length)];

                // ランダムな寿命を生成
                float life = Lerp(config.LifeMin, config.LifeMax, (float)_random.NextDouble());

                // ランダムなサイズを生成
                float size = Lerp(config.SizeMin, config.SizeMax, (float)_random.NextDouble());

                // ランダムな回転速度を生成
                float rotationSpeed = Lerp(config.RotationSpeedMin, config.RotationSpeedMax, (float)_random.NextDouble());

                // パーティクルを作成
                Particle particle = new Particle
                {
                    Position = position,
                    Velocity = new Vector2(velocityX, velocityY),
                    Color = color,
                    Life = life,
                    MaxLife = life,
                    Size = size,
                    Rotation = (float)(_random.NextDouble() * Math.PI * 2),
                    RotationSpeed = rotationSpeed,
                    UseGravity = config.UseGravity
                };

                _particles.Add(particle);
            }
        }

        /// <summary>
        /// 全パーティクルを更新
        /// </summary>
        /// <param name="deltaTime">前フレームからの経過時間（秒）</param>
        public void Update(float deltaTime)
        {
            // 全パーティクルを更新
            foreach (var particle in _particles)
            {
                particle.Update(deltaTime);
            }

            // 死んだパーティクルを削除
            _particles.RemoveAll(p => !p.IsAlive);
        }

        /// <summary>
        /// 全パーティクルを描画
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch</param>
        /// <param name="texture">パーティクル用のテクスチャ</param>
        /// <param name="cameraOffset">カメラオフセット</param>
        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 cameraOffset)
        {
            // 加算ブレンドで光る効果を実現
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.Additive,
                SamplerState.PointClamp,
                null, null, null, null
            );

            foreach (var particle in _particles)
            {
                // アルファ値をライフに基づいて計算（フェードアウト）
                float alpha = particle.Life / particle.MaxLife;
                Color color = particle.Color * alpha;

                // カメラオフセットを適用した描画位置
                Vector2 drawPosition = particle.Position - cameraOffset;

                // パーティクルを描画
                spriteBatch.Draw(
                    texture,
                    drawPosition,
                    null,
                    color,
                    particle.Rotation,
                    new Vector2(texture.Width / 2, texture.Height / 2),
                    particle.Size / texture.Width,
                    SpriteEffects.None,
                    0f
                );
            }

            spriteBatch.End();
        }

        /// <summary>
        /// 全パーティクルをクリア
        /// </summary>
        public void Clear()
        {
            _particles.Clear();
        }

        /// <summary>
        /// 線形補間
        /// </summary>
        private float Lerp(float min, float max, float t)
        {
            return min + (max - min) * t;
        }
    }
}
