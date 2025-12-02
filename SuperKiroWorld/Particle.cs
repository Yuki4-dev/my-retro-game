using Microsoft.Xna.Framework;

namespace SuperKiroWorld
{
    /// <summary>
    /// ビジュアルエフェクトを作成するための個々のパーティクル
    /// </summary>
    public class Particle
    {
        /// <summary>
        /// パーティクルの位置
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// パーティクルの速度
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// パーティクルの色
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// 残り寿命（秒）
        /// </summary>
        public float Life { get; set; }

        /// <summary>
        /// 最大寿命（秒）
        /// </summary>
        public float MaxLife { get; set; }

        /// <summary>
        /// パーティクルのサイズ（ピクセル）
        /// </summary>
        public float Size { get; set; }

        /// <summary>
        /// パーティクルの回転角度（ラジアン）
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// パーティクルの回転速度（ラジアン/秒）
        /// </summary>
        public float RotationSpeed { get; set; }

        /// <summary>
        /// 重力を適用するかどうか
        /// </summary>
        public bool UseGravity { get; set; }

        /// <summary>
        /// 重力の強さ（ピクセル/秒²）
        /// </summary>
        private const float GravityStrength = 200f;

        /// <summary>
        /// パーティクルが生きているかどうか
        /// </summary>
        public bool IsAlive => Life > 0;

        /// <summary>
        /// パーティクルを更新
        /// </summary>
        /// <param name="deltaTime">前フレームからの経過時間（秒）</param>
        public void Update(float deltaTime)
        {
            // 寿命を減少
            Life -= deltaTime;

            // 重力を適用
            if (UseGravity)
            {
                Velocity = new Vector2(Velocity.X, Velocity.Y + GravityStrength * deltaTime);
            }

            // 位置を更新
            Position += Velocity * deltaTime;

            // 回転を更新
            Rotation += RotationSpeed * deltaTime;
        }
    }
}
