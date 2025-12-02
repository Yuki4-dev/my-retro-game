using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SuperKiroWorld;

// ゲーム状態
public class GameState
{
    public int Score { get; set; }
    public int Lives { get; set; } = 3;
    public int CoinsCollected { get; set; }
    public int TotalCoins { get; set; }
    public bool IsGameOver { get; set; }
    public bool IsLevelComplete { get; set; }
    public Vector2 Camera { get; set; }
    public int HighScore { get; set; }
    public bool IsNewHighScore { get; set; }
}

// プレイヤー
public class Player
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 Size { get; set; } = new Vector2(50, 50);
    public float Speed { get; set; } = 5f;
    public float JumpPower { get; set; } = 12f;
    public float Gravity { get; set; } = 0.5f;
    public float Friction { get; set; } = 0.8f;
    public bool IsOnGround { get; set; }
}

// プラットフォーム
public struct Platform
{
    public Rectangle Bounds { get; set; }
    public Color Color { get; set; }
}

// コイン
public class Coin
{
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; } = new Vector2(25, 25);
    public bool Collected { get; set; }
}

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _playerTexture;
    private Texture2D _pixelTexture;
    private SpriteFont _font;

    private GameState _gameState;
    private Player _player;
    private List<Platform> _platforms;
    private List<Coin> _coins;
    private Rectangle _goal;
    private ScoreManager _scoreManager;

    private KeyboardState _previousKeyboardState;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        // ウィンドウサイズ設定
        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 600;
    }

    protected override void Initialize()
    {
        // ゲーム状態初期化
        _gameState = new GameState();
        
        // ScoreManager初期化とハイスコア読み込み
        _scoreManager = new ScoreManager();
        _scoreManager.LoadHighScore();
        _gameState.HighScore = _scoreManager.HighScore;
        
        // プレイヤー初期化
        _player = new Player
        {
            Position = new Vector2(100, 100)
        };

        // プラットフォーム初期化
        InitializePlatforms();
        
        // コイン初期化
        InitializeCoins();
        
        // ゴール設定
        _goal = new Rectangle(2550, 480, 50, 70);

        base.Initialize();
    }

    private void InitializePlatforms()
    {
        _platforms = new List<Platform>();
        var purple = new Color(121, 14, 203);
        var purpleLight = new Color(154, 62, 224);

        // 地面
        _platforms.Add(new Platform { Bounds = new Rectangle(0, 550, 400, 50), Color = purple });
        _platforms.Add(new Platform { Bounds = new Rectangle(500, 550, 400, 50), Color = purple });
        _platforms.Add(new Platform { Bounds = new Rectangle(1000, 550, 400, 50), Color = purple });
        _platforms.Add(new Platform { Bounds = new Rectangle(1500, 550, 400, 50), Color = purple });
        _platforms.Add(new Platform { Bounds = new Rectangle(2000, 550, 600, 50), Color = purple });

        // 浮遊プラットフォーム
        _platforms.Add(new Platform { Bounds = new Rectangle(450, 450, 100, 20), Color = purpleLight });
        _platforms.Add(new Platform { Bounds = new Rectangle(650, 380, 100, 20), Color = purpleLight });
        _platforms.Add(new Platform { Bounds = new Rectangle(850, 320, 120, 20), Color = purpleLight });
        _platforms.Add(new Platform { Bounds = new Rectangle(1100, 400, 150, 20), Color = purpleLight });
        _platforms.Add(new Platform { Bounds = new Rectangle(1350, 350, 100, 20), Color = purpleLight });
        _platforms.Add(new Platform { Bounds = new Rectangle(1600, 450, 120, 20), Color = purpleLight });
        _platforms.Add(new Platform { Bounds = new Rectangle(1850, 380, 100, 20), Color = purpleLight });
        _platforms.Add(new Platform { Bounds = new Rectangle(2200, 450, 150, 20), Color = purpleLight });
        _platforms.Add(new Platform { Bounds = new Rectangle(2450, 550, 200, 50), Color = purple });
    }

    private void InitializeCoins()
    {
        _coins = new List<Coin>
        {
            new Coin { Position = new Vector2(250, 500) },
            new Coin { Position = new Vector2(480, 410) },
            new Coin { Position = new Vector2(680, 340) },
            new Coin { Position = new Vector2(880, 280) },
            new Coin { Position = new Vector2(1150, 360) },
            new Coin { Position = new Vector2(1380, 310) },
            new Coin { Position = new Vector2(1630, 410) },
            new Coin { Position = new Vector2(1880, 340) },
            new Coin { Position = new Vector2(2250, 410) },
            new Coin { Position = new Vector2(2500, 500) }
        };
        
        _gameState.TotalCoins = _coins.Count;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // プレイヤーテクスチャ読み込み（エラー時は白い四角）
        try
        {
            _playerTexture = Content.Load<Texture2D>("kiro-logo");
        }
        catch
        {
            _playerTexture = CreateColorTexture(50, 50, new Color(121, 14, 203));
        }

        // ピクセルテクスチャ作成（図形描画用）
        _pixelTexture = CreateColorTexture(1, 1, Color.White);
    }

    private Texture2D CreateColorTexture(int width, int height, Color color)
    {
        var texture = new Texture2D(GraphicsDevice, width, height);
        var data = new Color[width * height];
        for (int i = 0; i < data.Length; i++)
            data[i] = color;
        texture.SetData(data);
        return texture;
    }

    protected override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        
        if (keyboardState.IsKeyDown(Keys.Escape))
            Exit();

        // Rキーでリスタート
        if (keyboardState.IsKeyDown(Keys.R) && _previousKeyboardState.IsKeyUp(Keys.R))
        {
            RestartGame();
        }

        if (!_gameState.IsGameOver && !_gameState.IsLevelComplete)
        {
            UpdatePlayer(keyboardState);
            UpdateCamera();
        }

        _previousKeyboardState = keyboardState;
        base.Update(gameTime);
    }

    private void UpdatePlayer(KeyboardState keyboardState)
    {
        // 左右移動
        if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
        {
            _player.Velocity = new Vector2(-_player.Speed, _player.Velocity.Y);
        }
        else if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
        {
            _player.Velocity = new Vector2(_player.Speed, _player.Velocity.Y);
        }
        else
        {
            _player.Velocity = new Vector2(_player.Velocity.X * _player.Friction, _player.Velocity.Y);
        }

        // ジャンプ
        if ((keyboardState.IsKeyDown(Keys.Space) || keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) 
            && _player.IsOnGround)
        {
            _player.Velocity = new Vector2(_player.Velocity.X, -_player.JumpPower);
            _player.IsOnGround = false;
        }

        // 重力適用
        _player.Velocity = new Vector2(_player.Velocity.X, _player.Velocity.Y + _player.Gravity);

        // 位置更新
        _player.Position += _player.Velocity;

        // 地面判定リセット
        _player.IsOnGround = false;

        // プラットフォーム衝突判定
        var playerRect = new Rectangle((int)_player.Position.X, (int)_player.Position.Y, 
                                       (int)_player.Size.X, (int)_player.Size.Y);

        foreach (var platform in _platforms)
        {
            if (playerRect.Intersects(platform.Bounds))
            {
                // 上から着地
                if (_player.Velocity.Y > 0 && _player.Position.Y + _player.Size.Y - _player.Velocity.Y <= platform.Bounds.Y + 5)
                {
                    _player.Position = new Vector2(_player.Position.X, platform.Bounds.Y - _player.Size.Y);
                    _player.Velocity = new Vector2(_player.Velocity.X, 0);
                    _player.IsOnGround = true;
                }
                // 下から衝突
                else if (_player.Velocity.Y < 0 && _player.Position.Y - _player.Velocity.Y >= platform.Bounds.Y + platform.Bounds.Height)
                {
                    _player.Position = new Vector2(_player.Position.X, platform.Bounds.Y + platform.Bounds.Height);
                    _player.Velocity = new Vector2(_player.Velocity.X, 0);
                }
                // 横から衝突
                else
                {
                    if (_player.Velocity.X > 0)
                        _player.Position = new Vector2(platform.Bounds.X - _player.Size.X, _player.Position.Y);
                    else if (_player.Velocity.X < 0)
                        _player.Position = new Vector2(platform.Bounds.X + platform.Bounds.Width, _player.Position.Y);
                    _player.Velocity = new Vector2(0, _player.Velocity.Y);
                }
            }
        }

        // 画面外に落ちた場合
        if (_player.Position.Y > 700)
        {
            LoseLife();
        }

        // コイン収集
        foreach (var coin in _coins)
        {
            if (!coin.Collected)
            {
                var coinRect = new Rectangle((int)coin.Position.X, (int)coin.Position.Y, 
                                            (int)coin.Size.X, (int)coin.Size.Y);
                if (playerRect.Intersects(coinRect))
                {
                    coin.Collected = true;
                    _gameState.CoinsCollected++;
                    _gameState.Score += 100;
                }
            }
        }

        // ゴール判定
        if (playerRect.Intersects(_goal))
        {
            LevelComplete();
        }
    }

    private void UpdateCamera()
    {
        float targetX = _player.Position.X - 800 / 3f;
        _gameState.Camera = new Vector2(
            _gameState.Camera.X + (targetX - _gameState.Camera.X) * 0.1f,
            0
        );

        // カメラ範囲制限
        _gameState.Camera = new Vector2(
            Math.Max(0, Math.Min(_gameState.Camera.X, 2650 - 800)),
            0
        );
    }

    private void LoseLife()
    {
        _gameState.Lives--;
        
        if (_gameState.Lives <= 0)
        {
            _gameState.IsGameOver = true;
        }
        else
        {
            ResetPlayerPosition();
        }
    }

    private void ResetPlayerPosition()
    {
        _player.Position = new Vector2(100, 100);
        _player.Velocity = Vector2.Zero;
        _gameState.Camera = Vector2.Zero;
    }

    private void LevelComplete()
    {
        _gameState.IsLevelComplete = true;
        _gameState.Score += _gameState.Lives * 500;
        
        // ハイスコアチェックと更新
        bool isNewHighScore = _scoreManager.CheckAndUpdateHighScore(_gameState.Score);
        if (isNewHighScore)
        {
            _gameState.IsNewHighScore = true;
            _gameState.HighScore = _scoreManager.HighScore;
        }
    }

    private void RestartGame()
    {
        _gameState.Score = 0;
        _gameState.Lives = 3;
        _gameState.CoinsCollected = 0;
        _gameState.IsGameOver = false;
        _gameState.IsLevelComplete = false;
        _gameState.IsNewHighScore = false;

        foreach (var coin in _coins)
            coin.Collected = false;

        ResetPlayerPosition();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(26, 26, 26));

        _spriteBatch.Begin();

        // カメラ変換を適用して描画
        DrawGame();
        
        // UI描画（カメラ影響なし）
        DrawUI();

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawGame()
    {
        var cameraOffset = new Vector2(-_gameState.Camera.X, 0);

        // プラットフォーム描画
        foreach (var platform in _platforms)
        {
            var drawRect = new Rectangle(
                platform.Bounds.X + (int)cameraOffset.X,
                platform.Bounds.Y,
                platform.Bounds.Width,
                platform.Bounds.Height
            );
            _spriteBatch.Draw(_pixelTexture, drawRect, platform.Color);
        }

        // コイン描画
        foreach (var coin in _coins)
        {
            if (!coin.Collected)
            {
                var coinPos = coin.Position + cameraOffset;
                var coinRect = new Rectangle((int)coinPos.X, (int)coinPos.Y, (int)coin.Size.X, (int)coin.Size.Y);
                _spriteBatch.Draw(_pixelTexture, coinRect, Color.Gold);
            }
        }

        // ゴール描画
        var goalRect = new Rectangle(_goal.X + (int)cameraOffset.X, _goal.Y, _goal.Width, _goal.Height);
        _spriteBatch.Draw(_pixelTexture, goalRect, Color.LimeGreen);

        // プレイヤー描画
        var playerPos = _player.Position + cameraOffset;
        var playerRect = new Rectangle((int)playerPos.X, (int)playerPos.Y, (int)_player.Size.X, (int)_player.Size.Y);
        _spriteBatch.Draw(_playerTexture, playerRect, Color.White);
    }

    private void DrawUI()
    {
        // 簡易テキスト表示（フォントなしバージョン）
        string scoreText = $"スコア: {_gameState.Score}";
        string highScoreText = $"ハイスコア: {_gameState.HighScore}";
        string livesText = $"ライフ: {_gameState.Lives}";
        string coinsText = $"コイン: {_gameState.CoinsCollected}/{_gameState.TotalCoins}";

        // 背景バー
        _spriteBatch.Draw(_pixelTexture, new Rectangle(0, 0, 800, 40), new Color(42, 42, 42));

        if (_gameState.IsGameOver)
        {
            string message = "ゲームオーバー！ Rキーでリスタート";
            // 中央に表示
            _spriteBatch.Draw(_pixelTexture, new Rectangle(200, 250, 400, 100), new Color(121, 14, 203));
        }
        else if (_gameState.IsLevelComplete)
        {
            string message = $"レベルクリア！ 最終スコア: {_gameState.Score}";
            _spriteBatch.Draw(_pixelTexture, new Rectangle(200, 250, 400, 100), new Color(121, 14, 203));
            
            // 新記録の場合は追加メッセージ
            if (_gameState.IsNewHighScore)
            {
                string newRecordMessage = "新記録！";
                _spriteBatch.Draw(_pixelTexture, new Rectangle(250, 370, 300, 50), Color.Gold);
            }
        }
    }
}
