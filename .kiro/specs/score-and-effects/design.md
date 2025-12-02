# 設計ドキュメント

## 概要

このドキュメントは、Super Kiro Worldゲームにスコア履歴保存機能とビジュアルエフェクトシステムを追加するための設計を定義します。主な機能は以下の通りです：

1. **スコア履歴システム** - ハイスコアの永続化と表示
2. **パーティクルシステム** - 汎用的なパーティクルエンジン
3. **トレイルエフェクト** - プレイヤーの動きに追従する軌跡
4. **衝突エフェクト** - プラットフォームとの衝突時の爆発
5. **スパークルエフェクト** - 障害物を飛び越えた時の演出
6. **紙吹雪エフェクト** - 新記録達成時の祝福演出

これらの機能は既存のMonoGameアーキテクチャに統合され、ゲームプレイをより魅力的で報酬感のあるものにします。

## アーキテクチャ

### 既存システムとの統合

現在のゲームは`Game1.cs`に全てのロジックが含まれています。新機能は以下のように統合されます：

```
Game1.cs (既存)
├── GameState (拡張)
│   └── HighScore プロパティ追加
├── ScoreManager (新規)
│   ├── SaveHighScore()
│   ├── LoadHighScore()
│   └── CheckAndUpdateHighScore()
├── ParticleSystem (新規)
│   ├── Particle クラス
│   ├── ParticleEmitter クラス
│   └── ParticleManager クラス
└── EffectManager (新規)
    ├── TrailEffect
    ├── ExplosionEffect
    ├── SparkleEffect
    └── ConfettiEffect
```

### レイヤー構造

1. **データ層** - スコアの永続化（ファイルI/O）
2. **ゲームロジック層** - パーティクル生成とライフサイクル管理
3. **レンダリング層** - パーティクルとエフェクトの描画

### 設計原則

- **単一責任の原則**: 各クラスは明確な責任を持つ
- **既存コードへの最小限の変更**: Game1.csへの変更を最小限に抑える
- **パフォーマンス重視**: 60 FPSを維持するため、オブジェクトプーリングを使用
- **拡張性**: 新しいエフェクトタイプを簡単に追加できる設計

## コンポーネントとインターフェース

### 1. ScoreManager

ハイスコアの保存と読み込みを管理します。

```csharp
public class ScoreManager
{
    private const string SaveFileName = "highscore.dat";
    private string SaveFilePath { get; set; }
    
    public int HighScore { get; private set; }
    
    // ハイスコアをファイルから読み込む
    public void LoadHighScore();
    
    // ハイスコアをファイルに保存
    public void SaveHighScore(int score);
    
    // 現在のスコアがハイスコアを超えているかチェックして更新
    public bool CheckAndUpdateHighScore(int currentScore);
}
```

**責任**:
- ファイルシステムへのアクセス
- スコアデータのシリアライズ/デシリアライズ
- ハイスコアの検証と更新

**統合ポイント**:
- `Game1.Initialize()`でLoadHighScore()を呼び出し
- `Game1.LevelComplete()`でCheckAndUpdateHighScore()を呼び出し
- `Game1.DrawUI()`でハイスコアを表示

### 2. Particle クラス

個々のパーティクルを表現します。

```csharp
public class Particle
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Color Color { get; set; }
    public float Life { get; set; }          // 残り寿命（秒）
    public float MaxLife { get; set; }       // 最大寿命
    public float Size { get; set; }
    public float Rotation { get; set; }
    public float RotationSpeed { get; set; }
    public bool IsAlive => Life > 0;
    
    public void Update(float deltaTime);
}
```

### 3. ParticleEmitter クラス

パーティクルの生成と管理を行います。

```csharp
public class ParticleEmitter
{
    private List<Particle> _particles;
    private int _maxParticles;
    
    public Vector2 Position { get; set; }
    public bool IsActive { get; set; }
    
    // パーティクルを生成
    public void Emit(ParticleConfig config);
    
    // 全パーティクルを更新
    public void Update(float deltaTime);
    
    // 全パーティクルを描画
    public void Draw(SpriteBatch spriteBatch, Texture2D texture);
    
    // 全パーティクルをクリア
    public void Clear();
}
```

### 4. ParticleConfig 構造体

パーティクル生成の設定を定義します。

```csharp
public struct ParticleConfig
{
    public int Count;                    // 生成数
    public Vector2 VelocityMin;          // 最小速度
    public Vector2 VelocityMax;          // 最大速度
    public Color[] Colors;               // 使用する色の配列
    public float LifeMin;                // 最小寿命
    public float LifeMax;                // 最大寿命
    public float SizeMin;                // 最小サイズ
    public float SizeMax;                // 最大サイズ
    public float RotationSpeedMin;       // 最小回転速度
    public float RotationSpeedMax;       // 最大回転速度
    public bool UseGravity;              // 重力を適用するか
}
```

### 5. EffectManager クラス

各種エフェクトを管理し、適切なタイミングで生成します。

```csharp
public class EffectManager
{
    private ParticleEmitter _trailEmitter;
    private ParticleEmitter _explosionEmitter;
    private ParticleEmitter _sparkleEmitter;
    private ParticleEmitter _confettiEmitter;
    
    // トレイルエフェクトを生成
    public void CreateTrailEffect(Vector2 position, Vector2 velocity);
    
    // 爆発エフェクトを生成
    public void CreateExplosionEffect(Vector2 position);
    
    // スパークルエフェクトを生成
    public void CreateSparkleEffect(Vector2 position);
    
    // 紙吹雪エフェクトを生成
    public void CreateConfettiEffect();
    
    // 全エフェクトを更新
    public void Update(float deltaTime);
    
    // 全エフェクトを描画
    public void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 cameraOffset);
}
```

## データモデル

### GameState の拡張

既存の`GameState`クラスに以下のプロパティを追加：

```csharp
public class GameState
{
    // 既存のプロパティ...
    public int Score { get; set; }
    public int Lives { get; set; }
    public int CoinsCollected { get; set; }
    public int TotalCoins { get; set; }
    public bool IsGameOver { get; set; }
    public bool IsLevelComplete { get; set; }
    public Vector2 Camera { get; set; }
    
    // 新規追加
    public int HighScore { get; set; }
    public bool IsNewHighScore { get; set; }
}
```

### スコアファイル形式

ハイスコアは単純なテキストファイルとして保存されます：

```
ファイル名: highscore.dat
場所: ゲーム実行ディレクトリ
形式: プレーンテキスト（整数値のみ）
例: "15000"
```

**エラーハンドリング**:
- ファイルが存在しない場合: ハイスコアを0として初期化
- ファイルが破損している場合: ハイスコアを0として初期化し、警告をログ出力
- 書き込み失敗の場合: エラーをログ出力するが、ゲームは続行

## 正確性プロパティ

*プロパティとは、システムの全ての有効な実行において真であるべき特性や動作のことです。本質的には、システムが何をすべきかについての形式的な記述です。プロパティは、人間が読める仕様と機械で検証可能な正確性保証の橋渡しとなります。*

### スコア保存システムのプロパティ

**Property 1: スコア保存のラウンドトリップ**
*任意の*有効なスコア値に対して、SaveHighScore()してからLoadHighScore()すると、同じ値が返される
**Validates: Requirements 1.1, 1.2**

**Property 2: ハイスコア単調性**
*任意の*現在のハイスコアと新しいスコアに対して、新しいスコアがハイスコアより大きい場合のみ、ハイスコアが更新される
**Validates: Requirements 1.2**

**Property 3: 初期化の冪等性**
*任意の*破損または存在しないファイルに対して、LoadHighScore()を呼び出すと、ハイスコアは0に初期化され、ゲームは正常に続行される
**Validates: Requirements 1.4**

### パーティクルシステムのプロパティ

**Property 4: パーティクルライフサイクル**
*任意の*パーティクルに対して、生成されてからMaxLife秒経過すると、IsAliveはfalseになる
**Validates: Requirements 2.3, 3.5, 4.4, 5.4**

**Property 5: パーティクル数の上限**
*任意の*時点において、アクティブなパーティクルの総数は、設定された最大数を超えない
**Validates: Requirements 2.1, 2.2, 3.1, 3.2, 4.1, 5.1**

### トレイルエフェクトのプロパティ

**Property 6: 移動時のトレイル生成**
*任意の*フレームにおいて、プレイヤーの速度が閾値を超えている場合、トレイルパーティクルが生成される
**Validates: Requirements 2.1, 2.2**

**Property 7: 静止時のトレイル停止**
*任意の*フレームにおいて、プレイヤーの速度が閾値未満の場合、トレイルパーティクルの生成が停止または減少する
**Validates: Requirements 2.5**

**Property 8: トレイル色の一貫性**
*全ての*トレイルパーティクルは、Kiroブランドの紫色(121, 14, 203)を使用する
**Validates: Requirements 2.4**

### 衝突エフェクトのプロパティ

**Property 9: 衝突時の爆発生成**
*任意の*プレイヤーとプラットフォームの衝突イベントに対して、衝突点で爆発パーティクルが生成される
**Validates: Requirements 3.1, 3.2**

**Property 10: 爆発の放射パターン**
*任意の*爆発エフェクトに対して、パーティクルは衝突点から複数の方向に放射される
**Validates: Requirements 3.3**

### スパークルエフェクトのプロパティ

**Property 11: ジャンプ成功時のスパークル**
*任意の*プラットフォームを飛び越える成功したジャンプに対して、スパークルパーティクルが生成される
**Validates: Requirements 4.1**

**Property 12: 着地時のスパークル停止**
*任意の*着地イベントに対して、そのジャンプに関連するスパークルパーティクルの生成が停止する
**Validates: Requirements 4.5**

### 紙吹雪エフェクトのプロパティ

**Property 13: 新記録時の紙吹雪トリガー**
*任意の*スコアがハイスコアを超えた場合、紙吹雪エフェクトが画面全体に生成される
**Validates: Requirements 5.1**

**Property 14: 紙吹雪の重力適用**
*全ての*紙吹雪パーティクルは、重力の影響を受けて下方向に加速する
**Validates: Requirements 5.3**

**Property 15: 画面外パーティクルの削除**
*任意の*紙吹雪パーティクルが画面外に出た場合、そのパーティクルは削除される
**Validates: Requirements 5.4**

## エラーハンドリング

### ファイルI/Oエラー

**シナリオ**: ハイスコアファイルの読み込み/書き込みに失敗

**対応**:
1. try-catchブロックでIOExceptionをキャッチ
2. エラーをログ出力（Console.WriteLine）
3. ハイスコアを0として初期化
4. ゲームは正常に続行

```csharp
try
{
    // ファイル読み込み
}
catch (IOException ex)
{
    Console.WriteLine($"ハイスコア読み込みエラー: {ex.Message}");
    HighScore = 0;
}
```

### パーティクル生成の過負荷

**シナリオ**: 短時間に大量のパーティクルが生成される

**対応**:
1. 各エミッターに最大パーティクル数を設定
2. 上限に達した場合、古いパーティクルを削除して新しいものを生成
3. フレームレートが低下した場合、パーティクル生成レートを動的に調整

### 無効なスコア値

**シナリオ**: ファイルから読み込んだスコアが負の値または異常に大きい

**対応**:
1. スコア値の妥当性チェック（0 <= score <= int.MaxValue）
2. 無効な場合は0として初期化
3. 警告をログ出力

## テスト戦略

### ユニットテスト

MonoGameのテストフレームワークまたはxUnitを使用して、以下のコンポーネントをテストします：

**ScoreManagerのテスト**:
- ハイスコアの保存と読み込み
- 存在しないファイルの処理
- 破損したファイルの処理
- ハイスコア更新ロジック

**Particleクラスのテスト**:
- ライフサイクル管理（Update呼び出し後の寿命減少）
- IsAliveプロパティの正確性
- 位置と速度の更新

**ParticleEmitterのテスト**:
- パーティクル生成数の正確性
- 最大パーティクル数の制限
- 死んだパーティクルの削除

### プロパティベーステスト

C#のプロパティベーステストライブラリ**FsCheck**を使用します。各テストは最低100回の反復を実行します。

**設定**:
```csharp
// FsCheckをNuGetからインストール
// dotnet add package FsCheck
// dotnet add package FsCheck.Xunit

using FsCheck;
using FsCheck.Xunit;

[Property(MaxTest = 100)]
public Property PropertyName() { ... }
```

**テスト対象のプロパティ**:

1. **スコア保存のラウンドトリップ** (Property 1)
   - ランダムなスコア値を生成
   - 保存してから読み込み
   - 元の値と一致することを検証

2. **ハイスコア単調性** (Property 2)
   - ランダムな現在のハイスコアと新しいスコアを生成
   - 更新ロジックを実行
   - 新しいスコアが大きい場合のみ更新されることを検証

3. **パーティクルライフサイクル** (Property 4)
   - ランダムなMaxLifeを持つパーティクルを生成
   - MaxLife秒分のUpdateを実行
   - IsAliveがfalseになることを検証

4. **トレイル色の一貫性** (Property 8)
   - ランダムな位置と速度でトレイルを生成
   - 全てのパーティクルの色がKiro紫であることを検証

5. **紙吹雪の重力適用** (Property 14)
   - ランダムな初期速度で紙吹雪を生成
   - 複数フレーム更新
   - Y速度が増加（下向き）していることを検証

**各プロパティベーステストには、以下の形式でコメントを付けます**:
```csharp
// Feature: score-and-effects, Property 1: スコア保存のラウンドトリップ
[Property(MaxTest = 100)]
public Property ScoreSaveLoadRoundTrip() { ... }
```

### 統合テスト

実際のゲームシナリオをシミュレートします：

- プレイヤーがレベルをクリアし、ハイスコアが更新される
- プレイヤーが移動し、トレイルエフェクトが生成される
- プレイヤーがプラットフォームに衝突し、爆発エフェクトが表示される
- 新記録達成時に紙吹雪エフェクトが表示される

### パフォーマンステスト

- 1000個のパーティクルが同時にアクティブな状態で60 FPSを維持
- エフェクト生成時のフレームレート低下を測定
- メモリ使用量の監視（パーティクルプーリングの効果確認）

## 実装の詳細

### パーティクルの描画最適化

**SpriteBatch.Begin()のパラメータ**:
```csharp
_spriteBatch.Begin(
    SpriteSortMode.Deferred,
    BlendState.Additive,  // 加算ブレンドで光る効果
    SamplerState.PointClamp,
    null, null, null, null
);
```

### エフェクト設定の詳細

**トレイルエフェクト**:
- 生成頻度: 移動速度に応じて調整（速いほど多く）
- パーティクル寿命: 0.3〜0.5秒
- サイズ: 3〜8ピクセル
- 色: Kiro紫 (121, 14, 203) with alpha fade

**爆発エフェクト**:
- 生成数: 15〜25個
- 放射角度: 360度全方向
- 初速: 3〜8ピクセル/フレーム
- 色: 紫の濃淡 + 白
- 寿命: 0.4〜0.7秒

**スパークルエフェクト**:
- 生成数: 5〜10個
- 動き: 上方向バイアス + ランダム
- 色: ゴールド、白、明るい黄色
- 寿命: 0.5〜0.8秒
- 特殊効果: サイズが脈動（twinkling）

**紙吹雪エフェクト**:
- 生成数: 50〜100個
- 初期位置: 画面上部全体
- 色: 紫、ゴールド、白、ピンク
- 重力: 0.2ピクセル/フレーム²
- 回転: -5〜5度/フレーム
- 寿命: 画面外に出るまで

### Game1.csへの統合ポイント

**Initialize()メソッド**:
```csharp
_scoreManager = new ScoreManager();
_scoreManager.LoadHighScore();
_gameState.HighScore = _scoreManager.HighScore;

_effectManager = new EffectManager();
```

**Update()メソッド**:
```csharp
// プレイヤー更新後
if (プレイヤーが移動中)
    _effectManager.CreateTrailEffect(_player.Position, _player.Velocity);

// 衝突検出後
if (衝突発生)
    _effectManager.CreateExplosionEffect(衝突点);

// エフェクト更新
_effectManager.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
```

**Draw()メソッド**:
```csharp
// ゲームオブジェクト描画後、UI描画前
_effectManager.Draw(_spriteBatch, _pixelTexture, new Vector2(-_gameState.Camera.X, 0));
```

**LevelComplete()メソッド**:
```csharp
bool isNewHighScore = _scoreManager.CheckAndUpdateHighScore(_gameState.Score);
if (isNewHighScore)
{
    _gameState.IsNewHighScore = true;
    _effectManager.CreateConfettiEffect();
}
```

## 将来の拡張性

### 追加可能なエフェクト

- **ダッシュエフェクト**: 高速移動時の残像
- **着地エフェクト**: 地面に着地した時の衝撃波
- **コイン収集エフェクト**: コインを取った時のキラキラ
- **ダメージエフェクト**: ライフを失った時の画面フラッシュ

### パフォーマンス改善

- **オブジェクトプーリング**: パーティクルオブジェクトの再利用
- **空間分割**: 画面外のパーティクルの早期カリング
- **LOD**: 遠くのパーティクルの簡略化

### データ拡張

- **スコアボード**: 上位10件のスコアを保存
- **統計情報**: プレイ時間、総コイン数、総ジャンプ数
- **実績システム**: 特定条件達成時のバッジ

## まとめ

この設計は、既存のSuper Kiro Worldゲームに対して、最小限の変更で最大限の視覚的インパクトを与えることを目指しています。パーティクルシステムは汎用的で拡張可能な設計となっており、将来的な機能追加も容易です。スコア保存システムはシンプルで信頼性が高く、プレイヤーの進捗を確実に追跡します。
