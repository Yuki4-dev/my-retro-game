# Implementation Plan

## スコア履歴とビジュアルエフェクトの実装タスク

- [x] 1. ScoreManagerクラスの実装





  - SuperKiroWorld/ScoreManager.csファイルを作成
  - ハイスコアの保存・読み込みメソッドを実装（SaveHighScore, LoadHighScore）
  - ファイルI/Oのエラーハンドリングを実装（存在しないファイル、破損ファイル、無効な値）
  - CheckAndUpdateHighScoreメソッドを実装（新記録の場合trueを返す）
  - 保存先: ゲーム実行ディレクトリの"highscore.dat"ファイル
  - _Requirements: 1.1, 1.2, 1.3, 1.4_

- [ ]* 1.1 ScoreManagerのプロパティベーステストを作成
  - **Property 1: スコア保存のラウンドトリップ**
  - **Property 2: ハイスコア単調性**
  - **Property 3: 初期化の冪等性**
  - **Validates: Requirements 1.1, 1.2, 1.4**

- [x] 2. GameStateの拡張とScoreManagerの統合






  - GameStateクラスにHighScoreプロパティを追加
  - GameStateクラスにIsNewHighScoreプロパティを追加
  - Game1.csにScoreManagerのインスタンスフィールドを追加
  - Initialize()メソッドでScoreManagerを初期化してハイスコアを読み込み
  - LevelComplete()メソッドでCheckAndUpdateHighScore()を呼び出し、新記録フラグを設定
  - DrawUI()メソッドでハイスコアを表示（現在のスコアと並べて）
  - _Requirements: 1.2, 1.3, 1.5_

- [ ]* 2.1 スコア保存システムのユニットテストを作成
  - ハイスコア表示のテスト
  - レベルクリア時の更新ロジックのテスト
  - _Requirements: 1.5_

- [x] 3. パーティクルシステムの基礎実装





  - SuperKiroWorld/Particle.csファイルを作成
  - 位置、速度、色、寿命（Life, MaxLife）、サイズ、回転、回転速度のプロパティを定義
  - Update(float deltaTime)メソッドで寿命減少、位置更新、回転更新を実装
  - IsAliveプロパティを実装（Life > 0）
  - 重力適用のオプション機能を追加
  - _Requirements: 2.3, 3.5, 4.4, 5.4_

- [ ]* 3.1 Particleクラスのプロパティベーステストを作成
  - **Property 4: パーティクルライフサイクル**
  - **Validates: Requirements 2.3, 3.5, 4.4, 5.4**

- [x] 4. ParticleConfig構造体の実装





  - SuperKiroWorld/ParticleConfig.csファイルを作成
  - 生成数、速度範囲（Min/Max）、色配列、寿命範囲、サイズ範囲、回転速度範囲、重力フラグを定義
  - 各エフェクトタイプ用の静的プリセットメソッドを作成:
    - CreateTrailConfig() - Kiro紫、短寿命、小サイズ
    - CreateExplosionConfig() - 紫と白、放射状、中寿命
    - CreateSparkleConfig() - ゴールドと白、上方向バイアス、中寿命
    - CreateConfettiConfig() - 複数色、重力あり、回転あり
  - _Requirements: 2.1, 2.2, 2.3, 2.4, 3.1, 3.2, 3.3, 3.4, 4.1, 4.2, 4.3, 5.1, 5.2, 5.3_

- [x] 5. ParticleEmitterクラスの実装





  - SuperKiroWorld/ParticleEmitter.csファイルを作成
  - パーティクルリスト（List<Particle>）と最大数（MaxParticles）の管理を実装
  - Emit(Vector2 position, ParticleConfig config)メソッドを実装
  - Update(float deltaTime)メソッドで全パーティクルを更新し、死んだパーティクルを削除
  - Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 cameraOffset)メソッドを実装
  - 加算ブレンド（BlendState.Additive）を使用して光る効果を実現
  - Clear()メソッドで全パーティクルをクリア
  - _Requirements: 2.1, 2.2, 3.1, 3.2, 4.1, 5.1_

- [ ]* 5.1 ParticleEmitterのプロパティベーステストを作成
  - **Property 5: パーティクル数の上限**
  - **Validates: Requirements 2.1, 2.2, 3.1, 3.2, 4.1, 5.1**
- [x] 6. EffectManagerクラスの実装




- [ ] 6. EffectManagerクラスの実装

  - SuperKiroWorld/EffectManager.csファイルを作成
  - 各エフェクトタイプ用のParticleEmitterフィールドを定義（trail, explosion, sparkle, confetti）
  - Initialize(GraphicsDevice)メソッドで各エミッターを初期化
  - CreateTrailEffect(Vector2 position, Vector2 velocity)メソッドを実装
  - CreateExplosionEffect(Vector2 position)メソッドを実装（360度放射パターン）
  - CreateSparkleEffect(Vector2 position)メソッドを実装（上方向バイアス）
  - CreateConfettiEffect(int screenWidth)メソッドを実装（画面上部全体から生成）
  - Update(float deltaTime)メソッドで全エミッターを更新
  - Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 cameraOffset)メソッドで全エミッターを描画
  - _Requirements: 2.1, 2.2, 2.5, 3.1, 3.2, 3.3, 4.1, 5.1, 5.3_

- [x] 7. トレイルエフェクトの統合





  - Game1.csにEffectManagerのインスタンスフィールドを追加
  - Initialize()メソッドでEffectManagerを初期化
  - UpdatePlayer()メソッドにトレイル生成ロジックを追加（速度閾値: 2.0以上）
  - プレイヤーの速度が閾値を超えている場合にCreateTrailEffect()を呼び出し
  - Update()メソッドでEffectManager.Update()を呼び出し
  - Draw()メソッドでEffectManager.Draw()を呼び出し（ゲームオブジェクト描画後、UI描画前）
  - _Requirements: 2.1, 2.2, 2.4, 2.5_

- [ ]* 7.1 トレイルエフェクトのプロパティベーステストを作成
  - **Property 6: 移動時のトレイル生成**
  - **Property 7: 静止時のトレイル停止**
  - **Property 8: トレイル色の一貫性**
  - **Validates: Requirements 2.1, 2.2, 2.4, 2.5**

- [x] 8. 衝突エフェクトの統合





  - UpdatePlayer()メソッドの衝突検出部分に爆発エフェクト生成を追加
  - 横からの衝突時: 衝突点でCreateExplosionEffect()を呼び出し
  - 下からの衝突時: 衝突点でCreateExplosionEffect()を呼び出し
  - 衝突点の座標を正確に計算（プレイヤーとプラットフォームの接触点）
  - _Requirements: 3.1, 3.2, 3.3, 3.4_

- [ ]* 8.1 衝突エフェクトのプロパティベーステストを作成
  - **Property 9: 衝突時の爆発生成**
  - **Property 10: 爆発の放射パターン**
  - **Validates: Requirements 3.1, 3.2, 3.3**
- [x] 9. スパークルエフェクトの統合




- [ ] 9. スパークルエフェクトの統合

  - Playerクラスにジャンプ追跡用のフィールドを追加（IsJumping, WasOverPlatform）
  - UpdatePlayer()メソッドにジャンプ成功検出ロジックを追加
  - プラットフォームを飛び越えた時（空中 && プラットフォーム上空通過）にCreateSparkleEffect()を呼び出し
  - 着地時（IsOnGround == true）にスパークル生成フラグをリセット
  - ゴールドと白色を使用
  - _Requirements: 4.1, 4.2, 4.3, 4.5_

- [ ]* 9.1 スパークルエフェクトのプロパティベーステストを作成
  - **Property 11: ジャンプ成功時のスパークル**
  - **Property 12: 着地時のスパークル停止**
  - **Validates: Requirements 4.1, 4.5**
-

- [x] 10. 紙吹雪エフェクトの統合




  - LevelComplete()メソッドでCheckAndUpdateHighScore()の結果を確認
  - 新記録の場合、CreateConfettiEffect()を呼び出し（画面幅800を渡す）
  - GameState.IsNewHighScoreフラグを設定
  - DrawUI()メソッドで新記録時に「新記録！」テキストを表示
  - 紙吹雪は重力と回転アニメーションで自然に落下
  - 複数の明るい色を使用（紫、ゴールド、白、ピンク）
  - _Requirements: 5.1, 5.2, 5.3, 5.4, 5.5_

- [ ]* 10.1 紙吹雪エフェクトのプロパティベーステストを作成
  - **Property 13: 新記録時の紙吹雪トリガー**
  - **Property 14: 紙吹雪の重力適用**
  - **Property 15: 画面外パーティクルの削除**
  - **Validates: Requirements 5.1, 5.3, 5.4**
-

- [x] 11. チェックポイント - 全機能の動作確認




  - ゲームを実行してハイスコア保存が正常に動作することを確認
  - 各エフェクト（トレイル、爆発、スパークル、紙吹雪）が正しく表示されることを確認
  - 60 FPSが維持されていることを確認
  - 問題があればユーザーに質問します
